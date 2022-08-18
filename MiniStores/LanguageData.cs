using MiniStores.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MiniStores
{
    public class PharseText
    {
        public string LangText { get; set; }
    }
    // *****************************************

    internal class LanguageData
    {
        static Dictionary<string, PharseText> CurrentLanguage =
            new Dictionary<string, PharseText>();

        CultureInfo ci = CultureInfo.InstalledUICulture;

        private string _path = @"Language\";
        private string _fileName = "Default.lang";

        public LanguageData(Joblog errorlog, string Lang)
        {
            _fileName = "Default.lang";
            string fullpathandfile = _path + _fileName;

            //Check if the Default Language file exist and if not create it
            if (File.Exists(fullpathandfile) == false)
            {
                AddDefaultLanguage();

                SaveLanguage(errorlog, fullpathandfile, "Default");
            }

            if (Lang != "")
            {
                // Get this apps Language setting 
                _fileName = Lang + ".Lang";
            }
            else
            {
                // check if system language exist and load it.
                _fileName = ci.Name + ".Lang";
            }

            fullpathandfile = _path + _fileName;
            if (File.Exists(fullpathandfile) == true)
            {
                LoadLanguage(errorlog, fullpathandfile);
            }
            else
            {
                // else load the default language
                _fileName = "Default.lang";
                fullpathandfile = _path + _fileName;
                LoadLanguage(errorlog, fullpathandfile);
            }

            errorlog.InformationMessage("Loaded Language", $"Loaded {_fileName} as a Language.");

        }
        // *****************************
        private void AddDefaultLanguage()
        {
            // Application Name
            CurrentLanguage.Add("MiniStores", new PharseText { LangText = "Mini-Stores" });
            // Tabs
            CurrentLanguage.Add("Search", new PharseText { LangText = "Search" });
            CurrentLanguage.Add("Part", new PharseText { LangText = "Part" });
            CurrentLanguage.Add("Parts", new PharseText { LangText = "Parts" });
            CurrentLanguage.Add("Type", new PharseText { LangText = "Type" });
            CurrentLanguage.Add("Types", new PharseText { LangText = "Types" });
            CurrentLanguage.Add("Manufacturer", new PharseText { LangText = "Manufacturer" });
            CurrentLanguage.Add("Manufacturers", new PharseText { LangText = "Manufacturers" });
            CurrentLanguage.Add("Location", new PharseText { LangText = "Location" });
            CurrentLanguage.Add("Locations", new PharseText { LangText = "Locations" });
            CurrentLanguage.Add("Position", new PharseText { LangText = "Position" });
            CurrentLanguage.Add("Positions", new PharseText { LangText = "Positions" });
            // Search Tab
            CurrentLanguage.Add("Id", new PharseText { LangText = "Id" });
            CurrentLanguage.Add("Qty", new PharseText { LangText = "Qty" });
            // Parts Tab
            CurrentLanguage.Add("PartId", new PharseText { LangText = "Part Id:" });
            CurrentLanguage.Add("PartName", new PharseText { LangText = "Part Name:" });
            CurrentLanguage.Add("TypeM", new PharseText { LangText = "Type:" });
            CurrentLanguage.Add("ManufacturerM", new PharseText { LangText = "Manufacturer:" });
            CurrentLanguage.Add("LocationM", new PharseText { LangText = "Location:" });
            CurrentLanguage.Add("PositionM", new PharseText { LangText = "Position:" });
            CurrentLanguage.Add("Quantity", new PharseText { LangText = "Quantity:" });
            CurrentLanguage.Add("Price", new PharseText { LangText = "Price:" });
            CurrentLanguage.Add("Comment", new PharseText { LangText = "Comment:" });
            CurrentLanguage.Add("PartList", new PharseText { LangText = "Part List:" });
            // Type Tab
            CurrentLanguage.Add("TypeId", new PharseText { LangText = "Type Id:" });
            CurrentLanguage.Add("TypeName", new PharseText { LangText = "Type Name:" });
            CurrentLanguage.Add("TypeList", new PharseText { LangText = "Type List:" });
            // Manufacturer Tab
            CurrentLanguage.Add("ManufacturerId", new PharseText { LangText = "Manufacturer Id:" });
            CurrentLanguage.Add("ManufacturerName", new PharseText { LangText = "Manufacturer Name:" });
            CurrentLanguage.Add("ManufacturerList", new PharseText { LangText = "Manufacturer List:" });
            // Location Tab
            CurrentLanguage.Add("LocationId", new PharseText { LangText = "Location Id:" });
            CurrentLanguage.Add("LocationName", new PharseText { LangText = "Location Name:" });
            CurrentLanguage.Add("LocationList", new PharseText { LangText = "Location List:" });
            // Position Tab
            CurrentLanguage.Add("PositionId", new PharseText { LangText = "Position Id:" });
            CurrentLanguage.Add("PositionName", new PharseText { LangText = "Position Name:" });
            CurrentLanguage.Add("PositionList", new PharseText { LangText = "Position List:" });
            // Buttons
            CurrentLanguage.Add("Add", new PharseText { LangText = "Add" });
            CurrentLanguage.Add("Update", new PharseText { LangText = "Update" });
            CurrentLanguage.Add("Delete", new PharseText { LangText = "Delete" });
            CurrentLanguage.Add("Clear", new PharseText { LangText = "Clear" });
            CurrentLanguage.Add("Refresh", new PharseText { LangText = "Refresh" });
            CurrentLanguage.Add("Import", new PharseText { LangText = "Import" });
            CurrentLanguage.Add("Export", new PharseText { LangText = "Export" });
            // Menu
            CurrentLanguage.Add("File", new PharseText { LangText = "File" });
            CurrentLanguage.Add("Exit", new PharseText { LangText = "Exit" });
            CurrentLanguage.Add("Help", new PharseText { LangText = "Help" });
            CurrentLanguage.Add("ViewHelp", new PharseText { LangText = "View Help" });
            CurrentLanguage.Add("AboutMiniStores", new PharseText { LangText = "About MiniStores" });
            // Help\About
            CurrentLanguage.Add("About", new PharseText { LangText = "About" });
            CurrentLanguage.Add("Title", new PharseText { LangText = "Title:" });
            CurrentLanguage.Add("Description", new PharseText { LangText = "Description:" });
            CurrentLanguage.Add("Product", new PharseText { LangText = "Product:" });
            CurrentLanguage.Add("Copyright", new PharseText { LangText = "Copyright:" });
            CurrentLanguage.Add("Version", new PharseText { LangText = "Version:" });
            // Help\Help
            CurrentLanguage.Add("Help", new PharseText { LangText = "Help" });
            // File\Settings
            CurrentLanguage.Add("Settings", new PharseText { LangText = "Settings" });
            CurrentLanguage.Add("Language", new PharseText { LangText = "Language:" });
            CurrentLanguage.Add("RetainP", new PharseText { LangText = "Days to Retain Joblog's:" });
            CurrentLanguage.Add("Debug", new PharseText { LangText = "Debug:" });
            CurrentLanguage.Add("Save", new PharseText { LangText = "Save" });
            CurrentLanguage.Add("Cancel", new PharseText { LangText = "Cancel" });
        }
        // ****
        private void SaveLanguage(Joblog el, string file, string lang)
        {
            string[] contents = new string[CurrentLanguage.Count + 1];

            int i = 0;

            contents[i] = "Key,Phrase";
            i++;

            foreach (var item in CurrentLanguage)
            {
                contents[i] = item.Key + "," + item.Value.LangText;
                i++;
            }

            System.IO.File.WriteAllLines(file, contents);

            el.InformationMessage($"{lang} Language file Created Successful", "Language file contains " + CurrentLanguage.Count + " words or phrases.");
        }
        // ****
        private void LoadLanguage(Joblog el, string fileToLoad)
        {
            List<ColumnModel2> inputFile = new List<ColumnModel2>();

            inputFile = CSVFile.ReadImportFile2(fileToLoad, "Phrase");

            if (inputFile.Count > -1)
            {
                CurrentLanguage.Clear();

                foreach (var set in inputFile)
                {
                    CurrentLanguage.Add(set.First, new PharseText { LangText = set.Second });
                }

            }
            // Check all phrases exist...Add if not
        }
        // ****
        public List<string> AvaibleLanguages()
        {
            List<string> output = new List<string>();

            // get a list of the files with extension .lang
            string[] files = Directory.GetFiles(_path, "*.lang");

            if (files.Length > 0)
            {
                for (int f = 0; f < files.Length; f++)
                {
                    int folderLength = 0;
                    int fileLength = 0;
                    string temp = files[f];
                    char letter = '\\';

                    // find out how big the path is
                    for (int j = temp.Length - 1; j > 0; j--)
                    {
                        char c = temp[j];

                        if (c == letter)
                        {
                            folderLength = j + 1;
                            fileLength = temp.Length - j - 1;
                            break;
                        }
                    }

                    // remove the path to leave a list of filenames
                    output.Add(files[f].Substring(folderLength, fileLength));
                }
            }


            return output;
        }
        // ****
        public Dictionary<string, PharseText> GetLanguage()
        {
            return CurrentLanguage;
        }
        // ****
    }
}
