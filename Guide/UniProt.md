# Display of features linked to the transcribes by UniProt

The __Protein features from UniProt website__ panel (Figure 1) contains the controls required to obtain, select, format and display features linked to the transcripts by the UniProt website.

The __Protein features from UniProt website__ panel contains four buttons:
- The __Import__ button allows previously saved feature data from the Uniprot website to be reentered (blue line in Figure 1).
- the __Search__ button allows you to search the UniProt website for features linked to the transcripts (green line in Figure 1).
- The __Save__ button allows you to save the search results from UniProt for later use (red line in Figure 1).
- The __Create__ button allows you to select, format and display features retrieved from a UniProt search (grey line in Figure 1).

![Figre 1](images/figurefeature12.jpg)

Figure 1: The __Protein features from UniProt website__ panel (Figure 1) contains the controls required to obtain, select, format and display features linked to the transcripts by the UniProt website as described above.

---

## Retrieving a previously saved search result

Pressing the __Import__ button (blue line in Figure 1) allows you to select a file that contains a previously stored search result, that was saved using the __Save__ button (red line in Figure 1). If the data is successfully imported the __Create__ button (black line in Figure 1) will be come active allowing you to select and format UniProt features ([see Selecting and formatting Uniprot features](#selecting-and-formatting-uniprot-features)).

## Searching the UniProt website for features linked to the transcripts

Pressing the __Search__ button (green line in Figure 1) opens the __Domain Search__ window (Figure 2). This wind performs the searches, displaying the current search status in the large text area.

When performing the searches, this window will be locked while it awaits a response from the websites. 

The search consists of four steps:
- Collect the transcript's GenBank accession IDs and retrieve the linked NCBI sequence IDs from the NCBI site. (These are not the Genbank DNA or protein accession IDs).
- Submit the NCBI sequence IDs to the NCBI to retrieve the linked GenBank protein accession ID.
- Submit the GenBank protein accession IDs to UniProt, initiate a search and retrieve the search's job ID.
- Request the search results from UniProt using the job id. If the search has not finished the process waits 5 seconds before requesting the results.

Since the process requires both the NCBI and UniProt web services to be running the process can be slow or fail. When it works, the process is relatively quick, however, high server load at either the NCBI or UniProt may have a considerable impact. If it fails, resubmit the search straight away, if that fails you may have to wait several hours before it works correctly. 







## Selecting and formatting Uniprot features 