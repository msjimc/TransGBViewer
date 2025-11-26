using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TransGBViewer
{
    internal class DomianIdentifier
    {

        private static readonly HttpClient client = new HttpClient();
        private static readonly WebClient webClient = new WebClient();
        private bool quit = false;

        public Dictionary<string, string> GetProteinIDsFrommRNAID(List<string> mRNAID, Dictionary<string, string> known, mRNADomainSearchUpdate owner)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            bool retrievedData = false;
            try
            {
                foreach (string id in mRNAID)
                {
                    if (known.ContainsKey(id))
                    {
                        results[id] = known[id];
                        owner.AddStatusText(id + " aready linked to " + results[id]);
                        retrievedData = true;
                    }
                    else
                    {
                        string url = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/elink.fcgi?dbfrom=nuccore&db=protein&id=" + id;
                        int count = 0;
                        string resultPage = "";
                        while (count < 5 && quit == false)
                        {
                            try
                            {
                                owner.AddStatusText("Searching for: " + id + " (Try " + (count + 1).ToString() + " of 5)");
                                resultPage = client.GetStringAsync(url).Result;
                                Thread.Sleep(500);
                                owner.AddStatusText("Response retrived");
                                count = 5;
                            }
                            catch { owner.AddStatusText("Failed"); count++; }
                        }
                        if (quit == true) { return new Dictionary<string, string>(); }

                        XmlDocument elinkData = new XmlDocument();
                        elinkData.LoadXml(resultPage);

                        XmlNode proteinIDNode = elinkData.SelectSingleNode("//LinkSetDb/Link/Id");
                        if (proteinIDNode == null)
                        {
                            XmlNode test = elinkData.SelectSingleNode("//LinkSet/IdList/Id");
                            if (test != null)
                            { owner.AddStatusText("Returned data incorrectly formed."); }
                        }
                        if (proteinIDNode != null)
                        {
                            string proteinID = proteinIDNode.InnerText;
                            owner.AddStatusText("Searching with protein id" + proteinID + "\n");
                            string eFetchURL = $"https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=protein&id={proteinID}&rettype=gb&retmode=text";
                            count = 0;
                            string eFetcgaAnswer = "";
                            while (count < 5 && quit == false)
                            {
                                try
                                {
                                    owner.AddStatusText("Searching for: " + id + " / " + proteinID + " (Try " + (count + 1).ToString() + " of 5)");
                                    eFetcgaAnswer = client.GetStringAsync(eFetchURL).Result;
                                    owner.AddStatusText("Response retrived\n");
                                    count = 5;
                                }
                                catch { owner.AddStatusText("Failed\r\n"); count++; }
                            }
                            if (quit == true) { return new Dictionary<string, string>(); }

                            string NPAccessionID = ExtractVersionLine(eFetcgaAnswer);
                            results[id] = NPAccessionID; retrievedData = true;
                            owner.AddStatusText(id + " linked to " + NPAccessionID + "\n");
                        }
                        else
                        {
                            results[id] = "Not found";
                            owner.AddStatusText("Not found\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                owner.AddStatusText("An error occured: " + ex.Message);
                results.Clear();
            }
            if (retrievedData == false)
            { return new Dictionary<string, string>(); }
            else { return results; }
        }

        private string ExtractVersionLine(string responseString)
        {
            foreach (var line in responseString.Split('\n'))
            {
                if (line.StartsWith("VERSION"))
                {
                    return line.Replace("VERSION", "").Trim();
                }
            }
            return "No VERSION line found";
        }

        public string SubmitMappingJob(List<string> refSeqProteinIdList)
        {
            try
            {
                string refSeqProteinId = string.Join(",", refSeqProteinIdList);
                var form = new MultipartFormDataContent
        {
            { new StringContent("RefSeq_Protein"), "from" },
            { new StringContent("UniProtKB"), "to" },
            { new StringContent(refSeqProteinId), "ids" }
        };

                var response = client.PostAsync("https://rest.uniprot.org/idmapping/run", form).Result;
                response.EnsureSuccessStatusCode();

                var jobJson = response.Content.ReadAsStringAsync().Result;
                var jobId = JObject.Parse(jobJson)["jobId"].ToString();
                return jobId;
            }
            catch
            {
                return "";
            }            

        }

        public Dictionary <string, List<string>> GetMappingResults(string jobId, mRNADomainSearchUpdate mRNADSU)
        {
            Dictionary<string, List<string>> resultsPairs = new Dictionary<string, List<string>>();
            try
            {
                var statusUrl = $"https://rest.uniprot.org/idmapping/status/{jobId}";
                var resultUrl = $"https://rest.uniprot.org/idmapping/results/{jobId}";

                while (quit == false)
                {
                    string statusJson = client.GetStringAsync(statusUrl).Result;
                    string answer = statusJson.PadRight(20);
                    System.Diagnostics.Debug.WriteLine(answer.Substring(0, 20));
                    if (statusJson.Contains("RUNNIN") == false)
                    { break; }
                    else if (statusJson.Contains("Error") == true )
                    { 
                        mRNADSU.AddStatusText("Error message received: stopping"); 
                        return resultsPairs; 
                    }
                    mRNADSU.AddStatusText("Not ready, wait 5 sec and before trying again");
                    Thread.Sleep(5000); // Wait 5 seconds before retrying
                }
                mRNADSU.AddStatusText("Received results");
                var resultsJson = client.GetStringAsync(resultUrl).Result;
                var root = JObject.Parse(resultsJson);
                var results = root["results"];

                List<string> mappings = new List<string>();

                foreach (var entry in results)
                {
                    var fromId = entry["from"]?.ToString();
                    var toId = entry["to"]?.ToString();
                    if (!string.IsNullOrEmpty(fromId) && !string.IsNullOrEmpty(toId))
                    {
                        if (mappings.Contains(toId) == false)
                        { mappings.Add(toId); }
                        if (resultsPairs.ContainsKey(toId) == false)
                        {
                            List<string> list = new List<string>();
                            list.Add(fromId);
                            resultsPairs.Add(toId, list);
                        }
                        else { resultsPairs[toId].Add(fromId); }

                    }
                }
                mRNADSU.AddStatusText("Received domains: " + string.Join(" ", mappings));
                foreach (string key in resultsPairs.Keys)
                {
                    mRNADSU.AddStatusText(key + " -> " + string.Join(" ", resultsPairs[key]));
                }
            }
            catch (Exception ex)
            {
                mRNADSU.AddStatusText("An error occured during mapping: " + ex.Message);
                resultsPairs.Clear();
            }
            return resultsPairs;
        }

        public string GettingIDFromAA(string sequence)
        {
            var postData = $"CMD=Put&PROGRAM=blastp&DATABASE=nr&QUERY={Uri.EscapeDataString(sequence)}";
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            string response = webClient.UploadString("https://blast.ncbi.nlm.nih.gov/Blast.cgi", postData);
            var ridMatch = Regex.Match(response, @"RID = (\S+)");
            string ID = "";
            if (ridMatch.Success == true)
            { ID = ridMatch.Groups[1].Value; }
            else
            { return ""; }

            string status = "";
            bool gotData = false;
            for (int i = 0; i < 60; i++) // Max 60 attempts
            {
                string checkUrl = $"https://blast.ncbi.nlm.nih.gov/Blast.cgi?CMD=Get&RID={ID}&FORMAT_OBJECT=SearchInfo";
                status = webClient.DownloadString(checkUrl);

                if (status.Contains("Status=READY") && !status.Contains("ThereAreHits=no"))
                {
                    gotData = true;
                    break;
                }
                Thread.Sleep(5000); // Wait 5 seconds
            }

            if (gotData == true)
            {
                string resultUrl = $"https://blast.ncbi.nlm.nih.gov/Blast.cgi?CMD=Get&RID={ID}&FORMAT_TYPE=XML";
                string xml = webClient.DownloadString(resultUrl);

                var match = Regex.Match(xml, @"<Hit_accession>NP(\S+)</Hit_accession>");
                string answer = "";
                if (match.Success == true)
                {
                    answer = "NP_" + match.Groups[1].Value;
                }
                else
                {
                    match = Regex.Match(xml, @"<Hit_accession>(\S+)</Hit_accession>");
                    if (match.Success == true)
                    { answer = match.Groups[1].Value; }
                    else return "Not found";
                }
                return answer;
            }
            else { return "Not found"; }
        }

        public static string getSequenceFromEBIResult(string result)
        {
            string answer = "";
            int sequenceStart = result.IndexOf("\"sequence\":\"");
            if (sequenceStart == -1) { return ""; }
            else { sequenceStart += 12; }
            int sequenceEnd = result.IndexOf("\"", sequenceStart + 1);
            if (sequenceEnd == -1) { return ""; }
            else
            { answer = result.Substring(sequenceStart, sequenceEnd - sequenceStart); }
            return answer;
        }

        public bool Quit { get { return quit; } set { quit = value; } }

    }
}
