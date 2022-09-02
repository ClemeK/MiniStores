# MiniStores
Hobbyist inventory management application

Background
This is a Program is designed to be a simple Inventory Management application.  I was watching a YouTuber who does electronics during the pandemic, and he said he needed something to catalogue his components as he was doing it in a spreadsheet. I thought I have a look around for some suitable and all I could find was generic database programs, that seamed overly complicated to set up. So, I created this inventory application. Although I designed this around electronic components, it could just as easily be used by hobbyist of all kinds that have a large collection of small parts\things that need to be stored.
Set-up
The program come with some basic data that I used for testing, so the first thing you will probably want to do is delete all the sample data unless you want to use this for electronic components.
To do this go into each tab ((Part, Type, Manufacturer, Position, Location) I recommend doing them in the order listed) and delete the existing data by selecting the item in the ListBox and pressing the Delete button.
Once that is done You will need to set up your own versions of ((Type, Manufacturer, Location, Position,) I recommend doing them in the order listed).
Once all that is set-up you can start adding your own Parts.

Alternately …
I have added the ability to import from CSV files, there are some Sample Templates in a folder.
There is also the ability to export to a CVS file for Backup or you want to migrate your data to another application.

Multi-Lingual
I’ve added the ability to change the screen text to any language (currently only 3 languages are available). You can change the setting in the File\Setting screen and then you will need to re-start the application.
If you want to change any of the words\phrases you can edit the language files as there are csv files. You will find them in the “MiniStore\Language” folder. If you wish to create a new language file, I suggest you copy the Default.lang file, and change the filename to the correct language prefix, you can find that here (Link). Then you can edit the file with a text editor of your choice or spreadsheet of your choice. Change the text in the second column and the resave the file, you should then be able to change the Language setting to select the new file.
You can forward me any new language files, and I will add them in for inclusion. 

Third Party Produces
Dapper - https://github.com/DapperLib/Dapper
SQLite - https://www.sqlite.org/index.html
FontAwesome - https://fontawesome.com/
