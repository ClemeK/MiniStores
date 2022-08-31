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
            else
            {
                File.Delete(fullpathandfile);
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
            // * Put a P at the end of key to indicate a Phrase
            string[] temp = new string[] {
                "About",
                "Add",
                "Cancel",
                "Clear",
                "Comment",
                "Copyright",
                "Debug",
                "Delete",
                "DeleteP",
                "Description",
                "Exit",
                "Export",
                "File",
                "Help",
                "HelpSubP",
                "Id",
                "Import",
                "Language",
                "Location",
                "Location:",
                "LocationId",
                "LocationList",
                "LocationName",
                "Locations",
                "Manufacturer",
                "Manufacturer:",
                "ManufacturerId",
                "ManufacturerList",
                "ManufacturerName",
                "Manufacturers",
                "MiniStores",
                "Off",
                "On",
                "Part",
                "PartId",
                "PartList",
                "PartName",
                "Parts",
                "Position",
                "Position:",
                "PositionId",
                "PositionList",
                "PositionName",
                "Positions",
                "Price",
                "Product",
                "Qty",
                "Quantity",
                "ReloadP",
                "RetainP",
                "Save",
                "Search",
                "Setting",
                "Settings",
                "Title",
                "Type",
                "Type:",
                "TypeId",
                "TypeList",
                "TypeName",
                "Types",
                "Update",
                "Version",
                "ViewHelp",
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
            {"About","About" },
            {"Add","Add" },
            {"Cancel","Cancel" },
            {"Clear","Clear" },
            {"Comment:","Comment:" },
            {"Copyright:","Copyright:" },
            {"Debug:","Debug:" },
            {"Delete","Delete" },
            {"DeleteP","Part Quantity is now Zero. Do you want me to delete it from the database?" },
            {"Description:","Description:" },
            {"Exit","Exit" },
            {"Export","Export" },
            {"File","File" },
            {"Help","Help" },
            {"HelpSubP", "Sorry. The help is only in English currently." },
            {"Id","Id" },
            {"Import","Import" },
            {"Language:","Language:" },
            {"Location","Location" },
            {"Location:","Location:" },
            {"LocationId","Location Id:" },
            {"LocationList","Location List" },
            {"LocationName","Location Name:" },
            {"Locations","Locations" },
            {"Manufacturer","Manufacturer" },
            {"Manufacturer:","Manufacturer:" },
            {"ManufacturerId","Manufacturer Id:" },
            {"ManufacturerList","Manufacturer List" },
            {"ManufacturerName","Manufacturer Name:" },
            {"Manufacturers","Manufacturers" },
            {"MiniStores", "Mini-Stores"},
            {"Off", "Off" },
            {"On", "On" },
            {"Part" ,"Part"},
            {"PartId","Part Id:" },
            {"PartList","Part List" },
            {"PartName","Part Name:" },
            {"Parts","Parts" },
            {"Position","Position" },
            {"Position:","Position:" },
            {"PositionId","Position Id:" },
            {"PositionList","Position List" },
            {"PositionName","Position Name:" },
            {"Positions","Positions" },
            {"Price:","Price:" },
            {"Product:","Product:" },
            {"Qty","Qty" },
            {"Quantity:","Quantity:" },
            {"Refresh","Refresh" },
            {"ReloadP", "Re-Start Required for the Language change to take place." },
            {"RetainP","Days to Retain Joblog's:" },
            {"Save","Save" },
            {"Search","Search" },
            {"Setting", "Setting" },
            {"Settings","Settings" },
            {"Title:","Title:" },
            {"Type","Type" },
            {"Type:","Type:" },
            {"TypeId","Type Id:" },
            {"TypeList","Type List" },
            {"TypeName","Type Name:" },
            {"Types","Types" },
            {"Update","Update" },
            {"Version:","Version:" },
            {"ViewHelp","View Help" },
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
