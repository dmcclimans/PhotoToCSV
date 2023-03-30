[PhotoToCSV](https://github.com/dmcclimans/PhotoToCSV)
==========

PhotoToCSV is a Windows application to extract keyword data from image files and process
that data into CSV (comma separated variable) files.

PhotoToCSV is was created for a project that used trail cameras to monitor
wildlife for the U.S. Forest Service. PhotoToCSV is unlikely to be of use in other
projects because of the specific rules it applies to the photos and keywords.
But you might be able to adapt the source code to use in other projects with similar
processing.

## Contents
* [Requirements](#requirements)
* [Installation](#installation)
* [Usage](#usage)
* [License](#license)

## Requirements
* Requires Windows 10 or later.

## Installation
* Go to the PhotoToCSV
  [latest release](https://github.com/dmcclimans/PhotoToCSV/releases/latest)
  page and download `PhotoToCSV_x.y.zip` (where x.y is the version number).

* Unzip the files into a folder, and run `PhotoToCSV.exe`.

## Usage
![Screenshot_Main](Screenshot_Main.png)

The user interface consists of 3 input fields and a run button.

1. The **Input folder** is the folder where PhotoToCSV will search for images. It will
process all image files found in that folder. Image files include .jpg, .png, etc.

2. The **Output Photo CSV file** is the first file produced. It is produced by Exiftool,
a free and open source tool for reading and writing metadata to image files. It
contains one line per image file, with the file path and name, the date and time of the
photo, and a list of keywords. The file is sorted first by the camera number
("cam&lt;number&gt;") if that is present, and then by the date/time.

    This file is useful for error checking, as it is relatively easy to spot keyword
    errors.

3. The **Output Encounter CSV file** is the second file produced. It is in the format used
for Wildlife project database files.

4. The **Run** button initiates processing.

    Processing can take a few seconds, but even on input folders with hundreds of photos
    it generally doesn't take over 10 seconds. The status bar at the bottom of the
    application window will display "processing" while running, and "done" when complete.

## License
PhotoToCSV is licensed under the MIT license. You may use the PhotoToCSV
application in any way you like. You may copy, distribute and modify the PhotoToCSV
software provided you include the copyright notice and license in all copies of the
software.

PhotoToCSV links to a library that is also licensed under the MIT License.

PhotoToCSV uses, but does not link to, the program exiftool.exe. ExifTool is licensed under
the GNU General Public License (GPL) or the Artistic License. You may use the software in
any way you like. You may copy and distribute the software. You may modify the exiftool
software as long as you make the source code available.

See the [License.txt](License.txt) file for additional information.

