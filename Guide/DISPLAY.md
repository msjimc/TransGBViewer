# Adjusting how sequences are displayed

![Figure 1](images/figureDisplay1.jpg)

The __Display__ tab of the ___mRNA Display Options___ window provides controls for configuring how sequences are shown (Figure 1).

## Changing the colours used to draw the sequences

By default, coding sequences are drawn as green blocks and non-coding regions are drawn as grey rectangles, these colours can be modified using the __Sequence colour selection__ window which is opened by pressing the __Select__ button at the top of the __Display__ tab (Figure 2)

![Figure 2](images/figureDisplay2.jpg)

Figure 2: Pressing the __Select__ button opens the __Sequence colour selection__ window.

<hr />

To change the colour of one or more transcripts, first check the name of the transcripts in the check list on the right hand side (Blue box in Figure 3) and then select whether the change will affect the coding, non-coding or both types of sequence using the __Coding__ and __Non-coding__ check boxes on the left (red box in Figure 3)

![Figure 3](images/figureDisplay3.jpg)

Figure 3: To change the transcript colour scheme, first select the transcripts you wish to modify (blue box). Next select whether coding or non-coding sequences will be changed (black box).

<hr />

Once one transcripts and type of sequence of sequence have been selected the __Select__ button will be come active (grey light Figure 3). Pressing the __Select__ button will then display a colour selection window. If the __By Name__ option is selected the __Select sequence colour__ window is displayed (Figure 4a). If the __Colour picker__ option is selected the standard Windows __Colour Picker dialog__ window opens (Figure 4b).

The use of the __Select colour by name dialog__ and __Windows colour picker dialog__ boxes are described here: 

- [Using the Select colour by name dialog box](equenceColour.md)
- [Using the Windows colour picker dialog box](ColourPickerDialog.md)

Once the colour has been selected, pressing the __Accept___ button will accept the modifications and redraw the the image (Figure 4), while pressing __Cancel__ will discard the changes.

![Figure 4](images/figureDisplay7.jpg)

Figure 4: Pressing the __Accept__ button will close the window, save the new scheme and redraw the image.

<hr />

## Selecting a region to view

The __Display__ tab contains two number boxes that allow you to select a region view. By default all sequences are visible (Figure 5a), but by selecting a new start and end point it is possible to expand the image to show a region of interest (Figure 5b). 

![Figure 5a](images/figureDisplay8a.jpg)

Figure 5a: By default all sequences are visible, with the values in the __Limit region displayed__ number boxes set to 1 and the consensus sequence's length (blue box). 

<hr />

![Figure 5b](images/figureDisplay8b.jpg)

Figure 5b: Adjusting the values in the __Limit region displayed__ number boxes allows a specific region to be viewed (blue box).

<hr />

### Set the base pair interval value to start at 1 when zooming in

In Figure 5b a region of 148 to 1588 was select and the base pair position label started at 148 bp. Checking the __Start at 1bp__ option (blue box in Figure 6), renumbers the coordinates to start at 1 bp (red line in Figure 6).

![Figure 6](images/figureDisplay9.jpg)

Figure 6: Checking the __Start at 1bp__ option (blue line) renumber the base pair position axis (red line)

<hr />

## Highlighting coding sequences

By default coding sequences are drawn in as different coloured rectangles overlaying the non-coding sequences, however, unchecking the __Show__ check box (blue line in Figure 7) to the right of the __Highlight coding sequences__ label will hide the coding sequence rectangles.

![Figure 7](images/figureDisplay10.jpg)

Figure 7: Unchecking the __Show__ option (blue line) will stop the coding sequence been highlighted.

<hr />

## Drawing exons as boxes with or without rounded corners

By default, the boxes are drawn with rounded corners (red circle in Figure 8a), unchecking the __Round__ check box (blue line in Figure 8 a and b) to the right of the __Round the corners of the exons__ label will redraw the image without rounded corners (red circle in Figure 8b).

![Figure 8a](images/figureDisplay11a.jpg)

Figure 8a: By default all sequences are drawn with rounded corners (red circle). 

<hr />

![Figure 8b](images/figureDisplay11b.jpg)

Figure 8b: Unchecking the the __Round__ check box (blue line) redraws the images with square corners (red circle in Figure 8b).

<hr />

## Highlight coding and non-coding sequences by reducing the height of the non-coding sequence boxes

As well as drawing the coding and non-coding sequences in different colours, coding sequences can be highlighted by reducing the height of the rectangles representing the non-coding sequences. Unchecking the __Reduce__ check box to the right of the __Reduce the height of non-coding sequences__ will redraw the image with all the rectangles drawn with the same height (Figure 9a and 9b).

![Figure 9a](images/figureDisplay12a.jpg)

Figure 9a: By default all sequences are drawn with non-coding sequence represented by narrow rectangles (red box). 

<hr />

![Figure 9b](images/figureDisplay12b.jpg)

Figure 9b: Unchecking the the __Reduc__ check box (blue line) redraws the images with all the boxes the same height (red box in Figure 9b).

<hr />

## Highlighting a transcripts exon splice sites

Transcripts may share a common exon but differ functionally by utilizing alternative splice sites within that exon. To aid the visualisation of these situations it is possible to highlight exon boundaries for one or all the transcripts using the __Highlight splice sites of selected sequence__ option (Figure 10). When a transcript is selected from the dropdown list (blue line in Figure 10) it's splice sites are highlighted as a series of vertical dotted lines that run span the entire height of the image (excluding the base pair position). 

<b>Note:</b> Visualisation of alternative splice sites if aided if a small gap is inserted at the site of an intron ([described here](../Guide/FORMAT.md#adjust-size-of-gap-signifying-an-intron-option)) and the image is zoomed in to the splice site ([described here](../Guide/DISPLAY.md#selecting-a-region-to-view)).

![Figure 10](images/figureDisplay13.jpg)

Figure 10: Selecting a transcript's name from the drop down list box (blue line) to the right of the __Highlight splice sites of selected sequence__ label causes the the splice sites to be highted as a series of vertical dotted lines that allow the use of alternative splice sites to be visualised (black box). 

<hr />

## Changing the colour of the lines used to highlight the donor and/or acceptor splice sites

By default, the lines highlighting the splice sites are black, however, their colours can be changed by pressing the appropriate __Donor__ or __Acceptor__ button (blue line in Figure 11a). This will display a window called __Select the donor line colour__ or __Select the acceptor line colour__ that allows a colour to be selected by either its Windows system name (check the List of names option - black line in Figure 11a) or using the Windows colour picker dialog window (check the List of names option - green line in Figure 11) after pressing the __Colour__ button (red line in Figure 11).

- [Using the Select colour by name dialog](equenceColour.md)
- [Using the Windows colour picker dialog](ColourPickerDialog.md)

The new colour will then be displayed new to the __Colour__ button. Pressing the __Accept__ button will then accept the changes and change the blocks of colour next to the __Donor__ or __Acceptor__ buttons (blue lines in Figure 11b) and redraw the image (black box in Figure 11b)


![Figure 11a](images/figureDisplay14a.jpg)

Figure 11a: The colour of the lines used to highlight splice sites can be changed by pressing the __Donor__ or __Acceptor__ button (blue line) and selecting the colour using the displayed dialog box.

<hr />

![Figure 11b](images/figureDisplay14b.jpg)

Figure 11b: Once changed the colour of the acceptor and donor lines are shown next to the appropriate button (blue lines) will the lines are now colour coded (black box).

<hr />

## Highlighting a transcript's translational start and stop sites

A gene's transcripts may utilise different translational start and stop sites, to aid the visualisation of these instances it is possible for one or all the transcript's start and stop sites to be highlighted using the __Highlight the translational start and stop sites of selected sequence__ option (Figure 12). When a transcript is selected from the dropdown list (blue line in Figure 12) it's translational start and stop sites are highlighted as a series of vertical dotted lines that run span the entire height of the image (excluding the base pair position). 

<b>Note:</b> Visualisation of translational start and stop sites if the image is zoomed in to the splice site ([described here](../Guide/DISPLAY.md#selecting-a-region-to-view)).

![Figure 12](images/figureDisplay15a.jpg)

Figure 12: Selecting a transcript's name from the drop down list box (blue line) to the right of the __Highlight splice sites of selected sequence__ label causes the the start and stop sites to be shown as a series of vertical dotted lines that allow the use of alternative translation start and stop sites to be visualised (black boxes). 

<hr />

## Changing the colour of the lines used to highlight the translational start and stop sites

By default, the lines highlighting the splice sites are black, however, their colours can be changed by pressing the appropriate __Start__ or __Stop__ button (blue line in Figure 13a). This will display a window called __Select the start codon line colour__ or __Select the stop codon line colour__ that allows a colour to be selected by either its Windows system name (check the List of names option - black line in Figure 13a) or using the Windows colour picker dialog window (check the List of names option - green line in Figure 13a) after pressing the __Colour__ button (red line in Figure 13a).

- [Using the Select colour by name dialog](equenceColour.md)
- [Using the Windows colour picker dialog](ColourPickerDialog.md)

The new colour will then be displayed next to the __Colour__ button and pressing the __Accept__ button will accept the changes and change the blocks of colour next to the __Donor__ or __Acceptor__ buttons (blue lines in Figure 11b) and redraw the image (black box in Figure 11b)


![Figure 13a](images/figureDisplay16a.jpg)

Figure 13a: The colour of the lines used to highlight splice sites can be changed by pressing the __Donor__ or __Acceptor__ button (blue line) and selecting the colour using the displayed dialog box.

<hr />

![Figure 13b](images/figureDisplay16b.jpg)

Figure 13b:  Once changed the colour of the start and stop lines are shown next to the appropriate button (blue lines) will the lines are now colour coded (black boxes).

<hr />

## Adjusting the thickness of the lines showing splice sites and translational start and stop sites

By default, the lines showing the splice sites and translational start and stop sites are 1.5 pixels wide (on a 96 DPI image). While this thickness is easily seen, it may be difficult to identify the lines colour. Consequently, its possible to adjust the lines' thickness from 0.5 (Figure 14a) to 3 pixels (Figure 14b) (at 96 DPI) using the line thickness control (blue line in Figure 14a and 14b). 

![Figure 14a](images/figureDisplay17a.jpg)

Figure 14a: Setting the value of the lowest number control (blue line) to 0.5 draws the lines with a width of 0.5 pixels when drawn at 96 DPI (black lines).

<hr />

![Figure 14b](images/figureDisplay17b.jpg)

Figure 14b: Setting the value of the lowest number control (blue line) to 3.0 draws the lines with a width of 3.0 pixels when drawn at 96 DPI (black lines).
 
<hr />