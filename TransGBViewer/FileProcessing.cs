using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransGBViewer
{
    internal class FileProcessing
    {
        string lastLine = "";
        bool representLastLine= false;
        private System.IO.FileStream fileStream = null;
        private System.IO.Compression.GZipStream gzipStream = null;
        StreamReader reader = null;
        public FileProcessing(string FileName)
        {

            if (FileName.ToLower().Substring(FileName.Length - 3, 3).ToLower() == ".gz")
            {
                fileStream = File.OpenRead(FileName);
                gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
                reader = new StreamReader(gzipStream);
            }
            else 
            {
                reader = new StreamReader(FileName);
            }
        }

        public int Peek() { return reader.Peek(); }

        public string ReadLine()
        {
            if (representLastLine == true)
            {
                representLastLine = false;
                return lastLine;
            }

            if (reader == null) return null;
            lastLine = reader.ReadLine();
            return lastLine;
        }

        public void RePresentLastLine()
        { representLastLine = true; }

        public void Close()
        {
            dispose();
        }
        public void dispose()
        {
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                reader = null;
            }
            if (gzipStream != null)
            {
                gzipStream.Close();
                gzipStream.Dispose();
                gzipStream = null;
            }
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;
            }
        }
    }
}
