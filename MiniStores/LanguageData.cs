using MiniStores.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MiniStores
{
    public class LanguageData
    {
        static Dictionary<string, string> CurrentLanguage =
            new Dictionary<string, string>();

        CultureInfo ci = CultureInfo.InstalledUICulture;

        private string _path = @"Language\";
        private string _fileName = "Default.lang";


        /// <summary>
        /// Sets up the Language file for the program
        /// </summary>
        /// <param name="errorlog">Joblog to write messages too</param>
        /// <param name="Lang">Language file to read</param>
        public LanguageData(Joblog errorlog, string Lang)
        {
            _fileName = "Default.lang";
            string fullpathandfile = _path + _fileName;

            // Initialise Dictionary
            InitialiseLanguage();

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
                LoadLanguage(errorlog, fullpathandfile, ci.Name);
            }
            else
            {
                // else load the default language
                _fileName = "Default.lang";
                fullpathandfile = _path + _fileName;
                LoadLanguage(errorlog, fullpathandfile, ci.Name);
            }

            errorlog.InformationMessage("Loaded Language", $"Loaded {_fileName} as a Language.");

        }
        // *****************************
        /// <summary>
        /// Initialises the Dictionary with all the correct Keys
        /// </summary>
        private void InitialiseLanguage()
        {
            // List of the Dictionary Key's
            string[] temp = new string[] {
                "MiniStores", "Search", "Part",  "Parts", "Type",
                "Types", "Manufacturer", "Manufacturers", "Location", "Locations",
                "Position", "Positions", "Id", "Qty", "PartId",
                "PartName", "TypeM", "ManufacturerM", "LocationM",
                "PositionM", "Quantity", "Price", "Comment", "PartList",
                "TypeId", "TypeName", "TypeList", "ManufacturerId", "ManufacturerName",
                "ManufacturerList", "LocationId", "LocationName", "LocationList", "PositionId",
                "PositionName", "PositionList", "Add", "Update", "Delete",
                "Clear", "Import", "Export", "File", "Exit",
                "Help", "ViewHelp", "AboutMiniStores", "About", "Title",
                "Description", "Product", "Copyright", "Version",  "Settings",
                "Language", "RetainP", "Debug", "Save", "Cancel","Setting",
                "On","Off", "HelpSubP","ReloadP"
            };

            // Sort the array before adding to the Dictionary
            temp = ArraySort(temp);

            // Add the Key's to the Dictionary
            for (int row = 0; row < temp.Length; row++)
            {
                // Check to see if the Key exist, and if not add it
                bool worked = CurrentLanguage.TryGetValue(temp[row], out string? output);

                if (worked == false)
                {
                    CurrentLanguage.Add(temp[row], temp[row]);
                }
            }
        }
        // *****************************
        /// <summary>
        /// Add the Default (UK-English) language to the Dictionary
        /// </summary>
        private void AddDefaultLanguage()
        {
            string[,] temp = new string[,] {
            {"MiniStores", "Mini-Stores"},
            {"Search","Search" },
            {"Part" ,"Part"},
            {"Parts","Parts" },
            {"Type","Type" },
            {"Types","Types" },
            {"Manufacturer","Manufacturer" },
            {"Manufacturers","Manufacturers" },
            {"Location","Location" },
            {"Locations","Locations" },
            {"Position","Position" },
            {"Positions","Positions" },
            {"Id","Id" },
            {"Qty","Qty" },
            {"PartId","Part Id:" },
            {"PartName","Part Name:" },
            {"Type:","Type:" },
            {"ManufacturerM","Manufacturer:" },
            {"Location:","Location:" },
            {"Position:","Position:" },
            {"Quantity:","Quantity:" },
            {"Price:","Price:" },
            {"Comment:","Comment:" },
            {"PartList","Part List:" },
            {"TypeId","Type Id:" },
            {"TypeName","Type Name:" },
            {"TypeList","Type List:" },
            {"ManufacturerId","Manufacturer Id:" },
            {"ManufacturerName","Manufacturer Name:" },
            {"ManufacturerList","Manufacturer List:" },
            {"LocationId","Location Id:" },
            {"LocationName","Location Name:" },
            {"LocationList","Location List:" },
            {"PositionId","Position Id:" },
            {"PositionName","Position Name:" },
            {"PositionList","Position List:" },
            {"Add","Add" },
            {"Update","Update" },
            {"Delete","Delete" },
            {"Clear","Clear" },
            {"Refresh","Refresh" },
            {"Import","Import" },
            {"Export","Export" },
            {"File","File" },
            {"Exit","Exit" },
            {"Help","Help" },
            {"ViewHelp","View Help" },
            {"AboutMiniStores","About MiniStores" },
            {"About","About" },
            {"Title:","Title:" },
            {"Description:","Description:" },
            {"Product:","Product:" },
            {"Copyright:","Copyright:" },
            {"Version:","Version:" },
            {"Settings","Settings" },
            {"Language:","Language:" },
            {"RetainP","Days to Retain Joblog's:" },
            {"Debug:","Debug:" },
            {"Save","Save" },
            {"Cancel","Cancel" },
            {"Setting", "Setting" },
            {"On", "On" },
            {"Off", "Off" },
            {"HelpSubP", "Sorry, the help is only in English currently." },
            {"ReloadP", "Re-Start Required for the Language change to take place." }
            };

            // Take the List and put it into the Dictionary
            for (int row = 0; row < (temp.Length / 2); row++)
            {
                string myKey = temp[row, 0];
                CurrentLanguage[myKey] = temp[row, 1];
            }
        }
        // ****
        /// <summary>
        /// Save a Dictionary file to disk.
        /// </summary>
        /// <param name="el">Joblog to write messages too</param>
        /// <param name="file">File to write to</param>
        /// <param name="lang">Language the file is writing</param>
        private void SaveLanguage(Joblog el, string file, string lang)
        {
            string[] contents = new string[CurrentLanguage.Count + 1];

            int i = 0;

            contents[i] = "Key,Phrase";
            i++;

            foreach (var item in CurrentLanguage)
            {
                contents[i] = item.Key + "," + item.Value;
                i++;
            }

            System.IO.File.WriteAllLines(file, contents);

            el.InformationMessage($"{lang} Language file Created Successful", "Language file contains " + CurrentLanguage.Count + " words or phrases.");
        }
        // ****
        /// <summary>
        /// Loads a Language file from disk.
        /// </summary>
        /// <param name="el">Joblog to write messages too</param>
        /// <param name="fileToLoad">File to read from</param>
        /// <param name="ciName">Language the file is reading</param>
        private void LoadLanguage(Joblog el, string fileToLoad, string ciName)
        {
            List<ColumnModel2> inputFile = new List<ColumnModel2>();

            inputFile = CSVFile.ReadImportFile2(fileToLoad, "Phrase");

            int count = 0;

            if (inputFile.Count > -1)
            {
                // Clear the Language Dictionary and re-Initialise it
                CurrentLanguage.Clear();
                InitialiseLanguage();

                foreach (var set in inputFile)
                {
                    CurrentLanguage[set.First] = set.Second;

                    count++;
                }
            }

            // Check all phrases exist...Add if not
            if (count != CurrentLanguage.Count)
            {
                SaveLanguage(el, fileToLoad, ciName);
            }
        }
        // ****
        /// <summary>
        /// Creates a list to the available languages in the disk folder
        /// </summary>
        /// <returns>Returns a List of Filename (en-UK.lang)</returns>
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
        /// <summary>
        /// Returns the name of the current language
        /// </summary>
        /// <returns>language name</returns>
        public Dictionary<string, string> GetLanguage()
        {
            return CurrentLanguage;
        }
        // ****
        /// <summary>
        /// Sorts a string array into assenting order.
        /// </summary>
        /// <param name="stringList">array to sort</param>
        /// <returns>string array</returns>
        static string[] ArraySort(string[] stringList)
        {
            string[] output = stringList;
            bool swaped = false;

            do
            {
                swaped = false;

                for (int i = 1; i < output.Length; i++)
                {

                    int comparison = String.Compare(output[i - 1], output[i], comparisonType: StringComparison.OrdinalIgnoreCase);

                    // use < for largest to Smallest
                    // use > for smallest to largest

                    if (comparison > 0)
                    {
                        string temp = output[i];
                        output[i] = output[i - 1];
                        output[i - 1] = temp;

                        swaped = true;
                    }
                }

            } while (swaped == true);

            return output;
        }
        // ****
    }
}
