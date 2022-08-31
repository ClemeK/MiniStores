using MiniStores.Models;
using MiniStores.SchemaUpdates;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


/*
* Title:    MiniStores
* Author:   Kelvin Clements
* Date:     23 June 2022
* Purpose:  Hobby Inventory Application
* 
* Developer's Note's
* ==================
* + Hungarian Notation is used for object in the UI.
* 
* Developer's Log
* ===============
* + 30-Aug-2022 - Added Schema update facility.
* + 29-Aug-2022 - Added Up\Down buttons to the Parts\Quantity fields (Now a Number spinner)
* + 29-Aug-2022 - Changed the Parts Screen to check that Quantity = Zero, and the delete if is.
* + 25-Aug-2022 - Updated the Comments throughout the program.
* + 21-Aug-2022 - Changed how the Lingual system works (easier for me).
* + 17-Aug-2022 - Added Setting screen.
* + 15-Aug-2022 - Made App Multi-Lingual.
* + 09-Aug-2022 - Added INI file to save the settings (INIFile.cs).
* + 01-Aug-2022 - Added About screen.
* + 22-Jul-2022 - Added Simple Error logging (JobLog.cs). 
* + 17-Jul-2022 - Added a Help Screen\File.
* + 27-Jul-2022 - Added SQLite as a DB as I can not get the MS-SQL to work on my machine.
* + 23-Jun-2022 - Applications Created.
*/

// TODO - Add Print button to the search screen.
// TODO - Add a Language editor screen.
// TODO - UPDATE THE HELP FILE!!!

namespace MiniStores
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Global Objects ...
        /// </summary>
        List<PartsModel> Parts = new List<PartsModel>();
        List<TypeModel> Types = new List<TypeModel>();
        List<ManufacturerModel> Manufacturers = new List<ManufacturerModel>();
        List<LocationModel> Locations = new List<LocationModel>();
        List<PositionModel> Positions = new List<PositionModel>();

        List<PositionModel> PosSubSet = new List<PositionModel>();

        Dictionary<string, string> ScreenText = new Dictionary<string, string>();


        Joblog errorlog = new Joblog("MiniStore", GlobalSetting.LogsToKeep);

        // ************************************************
        // ***      M A I N   W I N D O W
        public MainWindow()
        {
            GlobalSetting.InitiliasSettings();

            LanguageData AppLanguage = new LanguageData(errorlog, GlobalSetting.Language);

            ScreenText = AppLanguage.GetLanguage();
            //DataContext = ScreenText;

            InitializeComponent();

            CheckSchemaVersion();

            InitliseApp();

            SetScreenLanguage();

            errorlog.InformationMessage("Program Initialised");
        }
        /// <summary>
        /// Check to see if the Schema need an update
        /// </summary>
        private void CheckSchemaVersion()
        {
            // Additional Schema's to the list below 
            List<ISchema> Updates = new List<ISchema>()
            {
                { new SchemaUpdates.DBSchema2()},
                { new SchemaUpdates.DBSchema3()}
            };

            foreach (ISchema update in Updates)
            {
                update.UpdateSchema(SQLiteDataAccess.GetSchemaVersion());
            }


        }
        /// <summary>
        /// Load all the ComboBox's and ListBox's and set the Button On or Off
        /// </summary>
        private void InitliseApp()
        {
            // Refresh the different Lists
            RefreshTypesLB();
            RefreshManusLB();
            RefreshLocsLB();
            // Need to be installed after The Location
            UpdateLocPoss();
            RefreshPossLB();
            // Needs to be installed after EVERYTHING else
            RefreshPartsLB();

            // Set-up the different Tab Buttons
            btnAddType.IsEnabled = true;
            btnUpdateType.IsEnabled = false;
            btnDeleteType.IsEnabled = false;

            btnAddManu.IsEnabled = true;
            btnUpdateManu.IsEnabled = false;
            btnDeleteManu.IsEnabled = false;

            btnAddLoc.IsEnabled = true;
            btnUpdateLoc.IsEnabled = false;
            btnDeleteLoc.IsEnabled = false;

            btnAddPos.IsEnabled = true;
            btnUpdatePos.IsEnabled = false;
            btnDeletePos.IsEnabled = false;

            btnAddPart.IsEnabled = true;
            btnUpdatePart.IsEnabled = false;
            btnDeletePart.IsEnabled = false;
        }
        /// <summary>
        /// Translate the Main Screen into desired Language
        /// </summary>
        private void SetScreenLanguage()
        {
            // Yes I know this isn't the best way to do this, but it's easier for me to understand

            lblMainTitle.Content = LookUpTranslation("MiniStores");

            // Menu's
            MFile.Header = LookUpTranslation("File");
            MFSetting.Header = LookUpTranslation("Settings");
            MFExit.Header = LookUpTranslation("Exit");

            MImport.Header = LookUpTranslation("Import");
            MIParts.Header = LookUpTranslation("Parts");
            MITypes.Header = LookUpTranslation("Types");
            MIManufacturers.Header = LookUpTranslation("Manufacturers");
            MILocations.Header = LookUpTranslation("Location");
            MIPositions.Header = LookUpTranslation("Positions");

            MExport.Header = LookUpTranslation("Export");
            MEParts.Header = LookUpTranslation("Parts");
            METypes.Header = LookUpTranslation("Types");
            MEManufacturers.Header = LookUpTranslation("Manufacturers");
            MELocations.Header = LookUpTranslation("Location");
            MEPositions.Header = LookUpTranslation("Positions");

            MHelp.Header = LookUpTranslation("Help");
            MHView.Header = LookUpTranslation("ViewHelp");
            MHAbout.Header = LookUpTranslation("About");

            // Tab Names
            tiSearch.Header = LookUpTranslation("Search");
            tiPart.Header = LookUpTranslation("Part");
            tiType.Header = LookUpTranslation("Type");
            tiManu.Header = LookUpTranslation("Manufacturer");
            tiLocation.Header = LookUpTranslation("Location");
            tiPosition.Header = LookUpTranslation("Position");

            // Search Tab
            lblSPart.Content = LookUpTranslation("Part");
            lblSType.Content = LookUpTranslation("Type");
            lblSManufacturer.Content = LookUpTranslation("Manufacturer");
            lblSLocation.Content = LookUpTranslation("Location");
            lblSPosition.Content = LookUpTranslation("Position");

            PId.Header = LookUpTranslation("Id");
            PName.Header = LookUpTranslation("Part");
            PType.Header = LookUpTranslation("Type");
            PQty.Header = LookUpTranslation("Qty");
            PManu.Header = LookUpTranslation("Manufacturer");
            PLoc.Header = LookUpTranslation("Location");
            PPos.Header = LookUpTranslation("Position");

            // Part Tab
            lblPPartId.Content = LookUpTranslation("PartId");
            lblPPartName.Content = LookUpTranslation("PartName");
            lblPQuantity.Content = LookUpTranslation("Quantity");
            lblPType.Content = LookUpTranslation("Type:");
            lblPManufacturer.Content = LookUpTranslation("Manufacturer:");
            lblPLocation.Content = LookUpTranslation("Location:");
            lblPPosition.Content = LookUpTranslation("Position:");
            lblPPrice.Content = LookUpTranslation("Price");
            lblPComment.Content = LookUpTranslation("Comment");
            lblPParts.Content = LookUpTranslation("PartList");

            btnAddPart.Content = LookUpTranslation("Add");
            btnUpdatePart.Content = LookUpTranslation("Update");
            btnDeletePart.Content = LookUpTranslation("Delete");
            btnClearPart.Content = LookUpTranslation("Clear");
            btnRefreshList.Content = LookUpTranslation("Refresh");
            btnImportPart.Content = LookUpTranslation("Import");
            btnExportPart.Content = LookUpTranslation("Export");

            // Type Tab
            lblTTypeId.Content = LookUpTranslation("TypeId");
            lblTTypeName.Content = LookUpTranslation("TypeName");
            lblTTypes.Content = LookUpTranslation("TypeList");

            btnAddType.Content = LookUpTranslation("Add");
            btnUpdateType.Content = LookUpTranslation("Update");
            btnDeleteType.Content = LookUpTranslation("Delete");
            btnClearType.Content = LookUpTranslation("Clear");
            btnRefreshTypeList.Content = LookUpTranslation("Refresh");
            btnImportType.Content = LookUpTranslation("Import");
            btnExportType.Content = LookUpTranslation("Export");

            // Manufacturer Tab
            lblMManuId.Content = LookUpTranslation("ManufacturerId");
            lblMManuName.Content = LookUpTranslation("ManufacturerName");
            lblMManus.Content = LookUpTranslation("ManufacturerList");

            btnAddManu.Content = LookUpTranslation("Add");
            btnUpdateManu.Content = LookUpTranslation("Update");
            btnDeleteManu.Content = LookUpTranslation("Delete");
            btnClearManu.Content = LookUpTranslation("Clear");
            btnRefreshManuList.Content = LookUpTranslation("Refresh");
            btnImportManu.Content = LookUpTranslation("Import");
            btnExportManu.Content = LookUpTranslation("Export");
            // Location Tab
            lblLLocId.Content = LookUpTranslation("LocationId");
            lblLLocName.Content = LookUpTranslation("LocationName");
            lblLLocs.Content = LookUpTranslation("LocationList");

            btnAddLoc.Content = LookUpTranslation("Add");
            btnUpdateLoc.Content = LookUpTranslation("Update");
            btnDeleteLoc.Content = LookUpTranslation("Delete");
            btnClearLoc.Content = LookUpTranslation("Clear");
            btnRefreshLocList.Content = LookUpTranslation("Refresh");
            btnImportLoc.Content = LookUpTranslation("Import");
            btnExportLoc.Content = LookUpTranslation("Export");
            // Position Tab
            lblPPosId.Content = LookUpTranslation("PositionId");
            lblPLoc.Content = LookUpTranslation("Location");
            lblPPosName.Content = LookUpTranslation("PositionName");
            lblPPoss.Content = LookUpTranslation("PositionList");

            btnAddPos.Content = LookUpTranslation("Add");
            btnUpdatePos.Content = LookUpTranslation("Update");
            btnDeletePos.Content = LookUpTranslation("Delete");
            btnClearPos.Content = LookUpTranslation("Clear");
            btnRefreshPosList.Content = LookUpTranslation("Refresh");
            btnImportPos.Content = LookUpTranslation("Import");
            btnExportPos.Content = LookUpTranslation("Export");

        }
        // ************************************************
        // ***      S E A R C H
        /// <summary>
        /// Re-Build the Search query
        /// </summary>
        private void Search_Changed()
        {
            List<PartsModel> QueryResults = new List<PartsModel>();
            List<ColumnTable> query = new List<ColumnTable>();

            bool FirstConditionSet = false;
            string SearchQuery = "select * from Parts where";

            // Re-Build the Query String
            if (tbSearchPart.Text != "")
            {
                SearchQuery += " PartName like \'%" + tbSearchPart.Text + "%\'";
                FirstConditionSet = true;
            }

            if (cbSearchType.SelectedIndex > -1)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " TypeFK = \'" + FindType(cbSearchType.SelectedValue.ToString()).ToString() + "\'";
                FirstConditionSet = true;
            }


            if (cbSearchManu.SelectedIndex > -1)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " ManufacturerFK = \'" + FindManu(cbSearchManu.SelectedValue.ToString()).ToString() + "\'";
                FirstConditionSet = true;
            }

            if (cbSearchLoc.SelectedIndex > -1)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " LocationFK = \'" + FindLocation(cbSearchLoc.SelectedValue.ToString()).ToString() + "\'";
                FirstConditionSet = true;
            }

            if (cbSearchLoc.SelectedIndex > -1
                && cbSearchPos.SelectedIndex > -1)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " PositionFK = \'" +
                    FindPosition(FindLocation(cbSearchLoc.SelectedValue.ToString()),
                    cbSearchPos.SelectedValue.ToString()).ToString() + "\'";
            }

            if (SearchQuery != "select * from Parts where")
            {
                // Get the Search Results
                QueryResults = SQLiteDataAccess.SearchParts(SearchQuery);

                if (QueryResults.Count > 0)
                {
                    // convert Foreign Keys to Text
                    foreach (var item in QueryResults)
                    {
                        ColumnTable dt = new ColumnTable();

                        dt.PId = item.PartsId.ToString();
                        dt.PName = item.PartName;
                        dt.PType = LookupType(item.TypeFK);
                        dt.PQty = item.Quantity.ToString();
                        dt.PManu = LookupManu(item.ManufacturerFK);
                        dt.PLoc = LookupLocation(item.LocationFK);
                        dt.PPos = LookupPosition(item.LocationFK, item.PositionFK);

                        query.Add(dt);
                    }
                }
            }

            if (GlobalSetting.DebugApp == true)
            {
                lblSearchText.Content = SearchQuery + " (" + QueryResults.Count + ")";
            }

            // Read the list and add to the query to display in the DataGrid
            dgSearch.ItemsSource = query;
        }
        /// <summary>
        /// Change the Search query - Part Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSearchPart_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Search_Changed();
        }
        /// <summary>
        /// Change the Search query - Type Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSearchType_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Search_Changed();
        }
        /// <summary>
        /// Change the Search query - Manufacturer Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSearchManu_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Search_Changed();
        }
        /// <summary>
        /// Change the Search query - Position Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSearchPos_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Search_Changed();
        }
        /// <summary>
        /// Change the Search query - Location Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSearchLoc_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateSLocPoss();
            Search_Changed();
        }
        /// <summary>
        /// Refresh the Position based on the new Search-Location (Search Screen)
        /// </summary>
        private void UpdateSLocPoss()
        {
            int index = FindLocation((string)cbSearchLoc.SelectedValue);

            PosSubSet.Clear();

            if (index > 0)
            {
                PosSubSet = SQLiteDataAccess.GetPositions(index);

                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));
            }

            WireUpSPosCB();
        }
        /// <summary>
        /// Populate the Search Position ComboBox
        /// </summary>
        private void WireUpSPosCB()
        {
            cbSearchPos.Items.Clear();

            foreach (var p in PosSubSet)
            {
                cbSearchPos.Items.Add(p.PositionName);
            }
        }
        /// <summary>
        /// Do this if you Double Click the DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateGrid_Selected(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            int index = dgSearch.SelectedIndex;

            if (index >= -1)
            {
                DataGridRow row = (DataGridRow)dgSearch.ItemContainerGenerator.ContainerFromIndex(index);

                ColumnTable drv = (ColumnTable)dgSearch.SelectedItem;


                if (drv != null)
                {
                    tbMaster.SelectedItem = tiPart;

                    // Update Label with number of parts
                    lblPParts.Content = LookUpTranslation("PartList") + " (" + Parts.Count.ToString() + "): ";

                    if (GlobalSetting.DebugApp == true)
                    {
                        lblPParts.Content += drv.PId;
                    }

                    // move data into the screen
                    lblPartIdValue.Content = int.Parse(drv.PId);

                    tbPartName.Text = drv.PName;
                    tbPartQty.Text = drv.PQty.ToString();
                    cbType.SelectedValue = drv.PType;
                    cbManufacturer.SelectedValue = drv.PManu;
                    cbLocation.SelectedValue = drv.PLoc;

                    UpdateLocationPositions();
                    cbPosition.SelectedValue = drv.PPos;

                    int p = LookupPartPos(int.Parse(drv.PId));
                    tbPrice.Text = $"{Parts[p].Price:C2}";
                    tbComment.Text = Parts[p].Comment;

                    // reset the screen buttons
                    btnAddPart.IsEnabled = false;
                    btnUpdatePart.IsEnabled = true;
                    btnDeletePart.IsEnabled = true;


                }
            }
        }
        // ************************************************
        // ***      P A R T S
        /// <summary>
        /// Populate the Parts ListBox (Parts Screen)
        /// </summary>
        private void RefreshPartsLB()
        {
            // Get the parts
            Parts = SQLiteDataAccess.LoadParts();

            // Sort
            Parts.Sort((x, y) => x.PartName.CompareTo(y.PartName));

            // reset the add field on the display
            tbPartName.Text = "";
            tbPartQty.Text = "";

            RefreshTypesCB();
            cbType.SelectedIndex = 0;

            RefreshManufacturersCB();
            cbManufacturer.SelectedIndex = 0;

            RefreshLocationsCB();
            cbLocation.SelectedIndex = 0;

            // Must be refreshed after The Locations
            RefreshPositionsCB();
            cbPosition.SelectedIndex = 0;

            // Clear Informational Fields
            tbPrice.Text = $"{0.00m:C2}";
            tbComment.Text = "";

            // Display the parts in the ListBox
            // Must be Refreshed after EVERYTHING else
            WireUpPartsLB();

            // reset the display field on the display
            lblPParts.Content = LookUpTranslation("PartList") + " (" + Parts.Count.ToString() + "): ";

            lblPartIdValue.Content = "";

            errorlog.InformationMessage("Parts Refreshed");
        }
        /// <summary>
        /// Re-Load the Parts ListBox (Parts Screen)
        /// </summary>
        private void WireUpPartsLB()
        {
            lbPartsList.Items.Clear();

            foreach (var p in Parts)
            {
                string text = $"{p.PartName} ({LookupType(p.TypeFK)}) ";

                lbPartsList.Items.Add(text);
            }
        }
        /// <summary>
        /// Re-Load the Type ComboBox (Parts Screen)
        /// </summary>
        private void RefreshTypesCB()
        {
            Types = SQLiteDataAccess.LoadTypes();

            // Load a Blank one for the ComboBox's
            Types.Add(new TypeModel() { TypeId = 0, TypeName = " " });

            // Sort
            Types.Sort((x, y) => x.TypeName.CompareTo(y.TypeName));

            WireUpTypesCB();
        }
        /// <summary>
        /// Populate the Type ComboBox (Parts Screen)
        /// </summary>
        private void WireUpTypesCB()
        {
            cbType.Items.Clear();

            foreach (var t in Types)
            {
                cbType.Items.Add(t.TypeName);
                cbSearchType.Items.Add(t.TypeName);
            }
        }
        /// <summary>
        /// Re-Load the Manufacturer ComboBox (Parts Screen)
        /// </summary>
        private void RefreshManufacturersCB()
        {
            Manufacturers = SQLiteDataAccess.LoadManufacturers();

            // Load a Blank one for the ComboBox's
            Manufacturers.Add(new ManufacturerModel() { ManufacturerId = 0, ManufacturerName = " " });

            // Sort
            Manufacturers.Sort((x, y) => x.ManufacturerName.CompareTo(y.ManufacturerName));

            WireUpManufacturersCB();
        }
        /// <summary>
        /// Populate the Manufacturer ComboBox (Parts Screen)
        /// </summary>
        private void WireUpManufacturersCB()
        {
            cbManufacturer.Items.Clear();

            foreach (var m in Manufacturers)
            {
                cbManufacturer.Items.Add(m.ManufacturerName);
                cbSearchManu.Items.Add(m.ManufacturerName);
            }
        }
        /// <summary>
        /// Re-Load the Location ComboBox (Parts Screen)
        /// </summary>
        private void RefreshLocationsCB()
        {
            Locations = SQLiteDataAccess.LoadLocations();

            // Load a Blank one for the ComboBox's
            Locations.Add(new LocationModel() { LocationId = 0, LocationName = " " });

            // Sort
            Locations.Sort((x, y) => x.LocationName.CompareTo(y.LocationName));

            WireUpLocationsCB();
        }
        /// <summary>
        /// Populate the Location ComboBox (Parts Screen)
        /// </summary>
        private void WireUpLocationsCB()
        {
            cbLocation.Items.Clear();

            foreach (var l in Locations)
            {
                cbLocation.Items.Add(l.LocationName);
                cbSearchLoc.Items.Add(l.LocationName);
            }
        }
        /// <summary>
        /// Re-Load the Position ComboBox (Parts Screen)
        /// </summary>
        private void RefreshPositionsCB()
        {
            int index = FindLocation((string)cbLocation.SelectedValue);

            // Refresh ALL position list
            Positions.Clear();
            Positions = SQLiteDataAccess.LoadPositions();

            // Load a Blank one for the ComboBox's
            Positions.Add(new PositionModel() { PositionId = 0, PositionName = " " });

            // Sort
            Positions.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));

            // Rebuild the Drop-down List
            UpdateLocationPositions();
        }
        /// <summary>
        /// Populate the Position ComboBox (Parts Screen)
        /// </summary>
        private void WireUpPositionsCB()
        {
            cbPosition.Items.Clear();

            string text = "";

            foreach (var p in PosSubSet)
            {
                text = p.PositionName;

                cbPosition.Items.Add(text);
            }
        }
        /// <summary>
        /// Refresh the Parts ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshPartsButton(object sender, RoutedEventArgs e)
        {
            RefreshPartsLB();
        }
        /// <summary>
        /// Clear the Parts Screen of data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPartButton(object sender, RoutedEventArgs e)
        {
            lblPartIdValue.Content = "";
            tbPartName.Text = "";
            tbPartQty.Text = "";

            cbType.SelectedIndex = 0;
            cbManufacturer.SelectedIndex = 0;
            cbLocation.SelectedIndex = 0;
            cbPosition.SelectedIndex = 0;
            tbPrice.Text = $"{0.00m:C2}";
            tbComment.Text = "";
        }
        /// <summary>
        /// Validates the Parts screen
        /// </summary>
        /// <returns>True if passed</returns>
        private bool ValidatePartsSrceen()
        {
            bool output = true;

            if (tbPartName.Text == "") output = false;
            if (tbPartQty.Text == "") output = false;
            if (cbType.Text == "") output = false;
            if (cbManufacturer.Text == "") output = false;
            if (cbLocation.Text == "") output = false;
            if (cbPosition.Text == "") output = false;

            return output;
        }
        /// <summary>
        /// Add the new Part
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPartButton(object sender, RoutedEventArgs e)
        {
            bool Passed = ValidatePartsSrceen();

            if (Passed == true)
            {
                PartsModel p = new PartsModel();

                p.PartName = tbPartName.Text;
                p.Quantity = int.Parse(tbPartQty.Text);
                p.TypeFK = FindType((string)cbType.SelectedValue);
                p.ManufacturerFK = FindManu((string)cbManufacturer.SelectedValue);
                p.LocationFK = FindLocation((string)cbLocation.SelectedValue);
                p.PositionFK = FindPosition(p.LocationFK, (string)cbPosition.SelectedValue);

                SQLiteDataAccess.SavePart(p);

                // Refresh ListBox
                RefreshPartsLB();

                errorlog.InformationMessage("Part Added", p.ToString());
            }
            else
            {
                MessageBox.Show("One or more of the fields are blank!", "Parts Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// Update the selected Part
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatePartButton(object sender, RoutedEventArgs e)
        {
            bool Passed = ValidatePartsSrceen();

            if (Passed == true)
            {
                int qty = int.Parse(tbPartQty.Text);

                if (qty == 0)
                {
                    MessageBoxResult res = MessageBox.Show(LookUpTranslation("DeleteP"),
                                    LookUpTranslation("MiniStores"),
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Hand);

                    if (res == MessageBoxResult.Yes)
                    {
                        // Delete the part
                        DeletePart();
                    }

                    if (res == MessageBoxResult.No)
                    {
                        // update the part

                        int index = lbPartsList.SelectedIndex;

                        errorlog.InformationMessage("Part Updated", "From: " + Parts[index].ToString());

                        Parts[index].PartName = tbPartName.Text;
                        Parts[index].Quantity = int.Parse(tbPartQty.Text);
                        Parts[index].TypeFK = FindType((string)cbType.SelectedValue);
                        Parts[index].ManufacturerFK = FindManu((string)cbManufacturer.SelectedValue);
                        Parts[index].LocationFK = FindLocation((string)cbLocation.SelectedValue);
                        Parts[index].PositionFK = FindPosition(Parts[index].LocationFK, (string)cbPosition.SelectedValue);

                        if (tbPrice.Text != "")
                        {
                            Decimal.TryParse(tbPrice.Text, out decimal num);
                            Parts[index].Price = num;
                        }
                        else
                        {
                            Parts[index].Price = 0.00m;
                        }

                        Parts[index].Comment = tbComment.Text;

                        errorlog.InformationMessage("", "To:" + Parts[index].ToString());

                        SQLiteDataAccess.UpdatePart(Parts[index]);

                        // Refresh ListBox
                        RefreshPartsLB();
                    }
                }
            }
        }
        /// <summary>
        /// Retrieve the Part data that is selected in the ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PartSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbPartsList.SelectedIndex;

            if (index != -1)
            {
                // Update Label with number of parts
                lblPParts.Content = LookUpTranslation("PartList") + " (" + Parts.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblPParts.Content += index.ToString();
                }

                // move data into the screen
                lblPartIdValue.Content = Parts[index].PartsId;

                tbPartName.Text = Parts[index].PartName;
                tbPartQty.Text = Parts[index].Quantity.ToString();
                cbType.SelectedValue = LookupType(Parts[index].TypeFK);
                cbManufacturer.SelectedValue = LookupManu(Parts[index].ManufacturerFK);
                cbLocation.SelectedValue = LookupLocation(Parts[index].LocationFK);

                UpdateLocationPositions();
                cbPosition.SelectedValue = LookupPosition(Parts[index].LocationFK, Parts[index].PositionFK);

                tbPrice.Text = $"{Parts[index].Price:C2}";
                tbComment.Text = Parts[index].Comment;

                // reset the screen buttons
                btnAddPart.IsEnabled = false;
                btnUpdatePart.IsEnabled = true;
                btnDeletePart.IsEnabled = true;

            }
            else
            {
                btnAddPart.IsEnabled = true;
                btnUpdatePart.IsEnabled = false;
                btnDeletePart.IsEnabled = false;
            }
        }
        /// <summary>
        /// Delete the select Part
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePartButton(object sender, RoutedEventArgs e)
        {
            DeletePart();
        }
        /// <summary>
        /// Delete a Part
        /// </summary>
        private void DeletePart()
        {
            int index = lbPartsList.SelectedIndex;
            lblPParts.Content = "Parts List : " + index.ToString();

            errorlog.InformationMessage("Part Deleted", Parts[index].ToString());

            SQLiteDataAccess.DeletePart(Parts[index].PartsId);

            RefreshPartsLB();
        }
        /// <summary>
        /// Import the Parts from a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportPartButton(object sender, RoutedEventArgs e)
        {
            List<ColumnModel8> importParts = new List<ColumnModel8>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importParts = CSVFile.ReadImportFile6(fileName, "PartName");

                int count = 0;

                foreach (var item in importParts)
                {
                    if (FindPart(item.First) == -1)
                    {
                        count++;
                        PartsModel p = new PartsModel();

                        p.PartName = item.First;
                        p.TypeFK = FindType(item.Second);
                        p.Quantity = int.Parse(item.Thrid);
                        p.ManufacturerFK = FindManu(item.Fourth);
                        p.LocationFK = FindLocation(item.Fifth);
                        p.PositionFK = FindPosition(p.LocationFK, item.Sixth);

                        if (item.Seventh != "")
                        {
                            Decimal.TryParse(item.Seventh, out decimal num);
                            p.Price = num;
                        }
                        else
                        {
                            p.Price = 0.00m;
                        }

                        p.Comment = item.Eighth;

                        SQLiteDataAccess.SavePart(p);
                    }
                }

                RefreshPartsLB();

                MessageBox.Show("Import of Parts Completed Successful", "Import of Parts", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Import of Parts Completed Successful", "Parts Imported: " + count);
            }
            else
            {
                MessageBox.Show("Import of Parts Failed to open file", "Import of Parts", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Import of Parts Failed to open file");
            }

        }
        /// <summary>
        /// Export the Parts to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportPartButton(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            bool worked = CSVFile.GetExportFileName(out fileName);
            string[] contents = new string[Parts.Count + 1];

            if (worked)
            {
                int i = 0;

                contents[i] = "PartId,PartName,Type,Quantity,Manufacturer,Location,Position,Price,Comment";
                i++;

                foreach (var item in Parts)
                {
                    contents[i] = item.PartsId + ","
                        + item.PartName + ","
                        + LookupType(item.TypeFK) + ","
                        + item.Quantity + ","
                        + LookupManu(item.ManufacturerFK) + ","
                        + LookupLocation(item.LocationFK) + ","
                        + LookupPosition(item.LocationFK, item.PositionFK) + ","
                        + item.Price + ","
                        + item.Comment + ",";

                    i++;
                }

                System.IO.File.WriteAllLines(fileName, contents);

                MessageBox.Show("Export of Parts Completed Successful", "Export of Parts", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Export of Parts Completed Successful", "Parts Exported: " + Parts.Count);
            }
            else
            {
                MessageBox.Show("Export of Parts Failed to open file", "Export of Parts", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Export of Parts Failed to open file");
            }

        }
        /// <summary>
        /// Increase the Quantity in the Part screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQtyUp_Click(object sender, RoutedEventArgs e)
        {
            int Qty = int.Parse(tbPartQty.Text);

            Qty++;

            tbPartQty.Text = Qty.ToString();
        }
        /// <summary>
        /// Decrease the Quantity in the Part screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQtyDown_Click(object sender, RoutedEventArgs e)
        {
            int Qty = int.Parse(tbPartQty.Text);

            Qty--;

            tbPartQty.Text = Qty.ToString();
        }
        /// <summary>
        /// Populate the Type ComboBox (Parts screen)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblPType.Content = LookUpTranslation("Type:") + " " + cbType.SelectedIndex.ToString();
            }
        }
        /// <summary>
        /// Populate the Manufacturer ComboBox (Parts screen)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManufacturerCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblPManufacturer.Content = LookUpTranslation("Manufacturer:") + " " + cbManufacturer.SelectedIndex.ToString();
            }
        }
        /// <summary>
        /// Populate the Location ComboBox (Parts screen)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocationCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblPLocation.Content = LookUpTranslation("Location:") + " " + cbLocation.SelectedIndex.ToString();
            }

            UpdateLocationPositions();
        }
        /// <summary>
        /// Populate the Position ComboBox (Parts screen)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblPPosition.Content = LookUpTranslation("Position:") + " " + cbPosition.SelectedIndex.ToString();
            }
        }
        /// <summary>
        /// Updates the Possible Position for a Given Location
        /// </summary>
        private void UpdateLocationPositions()
        {
            int index = FindLocation((string)cbLocation.SelectedValue);

            PosSubSet.Clear();

            if (index > 0)
            {
                PosSubSet = SQLiteDataAccess.GetPositions(index);
            }

            WireUpPositionsCB();
        }
        // ************************************************
        // ***      T Y P E S
        /// <summary>
        /// Re-Load the Types ListBox
        /// </summary>
        private void RefreshTypesLB()
        {
            // Get the parts
            Types = SQLiteDataAccess.LoadTypes();

            Types.Sort((x, y) => x.TypeName.CompareTo(y.TypeName));

            // Display the parts in the ListBox
            WireUpTypesLB();

            // reset the add field on the display
            tbTypeName.Text = "";

            // reset the display field on the display
            lblTTypes.Content = LookUpTranslation("TypeList") + " (" + Types.Count.ToString() + "): ";

            lblTTypeIdValue.Content = "";

            errorlog.InformationMessage("Types Refreshed");
        }
        /// <summary>
        /// Populate the Type ListBox
        /// </summary>
        private void WireUpTypesLB()
        {
            lbTypesList.Items.Clear();

            foreach (var t in Types)
            {
                lbTypesList.Items.Add(t.TypeName);
            }
        }
        /// <summary>
        /// Refresh the Type ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTypesButton(object sender, RoutedEventArgs e)
        {
            RefreshTypesLB();
        }
        /// <summary>
        /// Clear the Type screen data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearTypeButton(object sender, RoutedEventArgs e)
        {
            tbTypeName.Text = "";
            lblTTypeIdValue.Content = "";
        }
        /// <summary>
        /// Add the New Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTypeButton(object sender, RoutedEventArgs e)
        {
            if (tbTypeName.Text != "")
            {
                TypeModel t = new TypeModel();

                t.TypeName = tbTypeName.Text;

                SQLiteDataAccess.SaveType(t);

                // Refresh ListBox
                RefreshTypesLB();

                errorlog.InformationMessage("Type Added", t.ToString());

            }
            else
            {
                MessageBox.Show("Type Name field is blank!", "Types Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Update the selected type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTypeButton(object sender, RoutedEventArgs e)
        {
            if (tbTypeName.Text != "")
            {
                int index = lbTypesList.SelectedIndex;

                errorlog.InformationMessage("Types Updated", "From: " + Types[index].ToString());

                Types[index].TypeName = tbTypeName.Text;

                errorlog.InformationMessage("", "To: " + Types[index].ToString());

                SQLiteDataAccess.UpdateType(Types[index]);

                // Refresh ListBox
                RefreshTypesLB();
            }
        }
        /// <summary>
        /// Retrieve the Type data that is selected in the ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbTypesList.SelectedIndex;

            if (index != -1)
            {
                lblTTypes.Content = LookUpTranslation("TypeList") + " (" + Types.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblTTypes.Content += index.ToString();
                }

                lblTTypeIdValue.Content = Types[index].TypeId;

                tbTypeName.Text = Types[index].TypeName;

                btnAddType.IsEnabled = false;
                btnUpdateType.IsEnabled = true;
                btnDeleteType.IsEnabled = true;
            }
            else
            {
                btnAddType.IsEnabled = true;
                btnUpdateType.IsEnabled = false;
                btnDeleteType.IsEnabled = false;
            }
        }
        /// <summary>
        /// Delete the selected Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTypeButton(object sender, RoutedEventArgs e)
        {
            int index = lbTypesList.SelectedIndex;
            lblTTypes.Content = LookUpTranslation("TypeList") + " " + index.ToString();

            errorlog.InformationMessage("Types Deleted", Types[index].ToString());

            SQLiteDataAccess.DeleteType(Types[index].TypeId);

            RefreshTypesLB();
        }
        /// <summary>
        /// Import the Types from a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportTypeButton(object sender, RoutedEventArgs e)
        {
            List<string> importTypes = new List<string>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importTypes = CSVFile.ReadImportFile1(fileName, "TypeName");

                int count = 0;

                foreach (var item in importTypes)
                {
                    if (FindType(item) == -1)
                    {
                        count++;
                        TypeModel t = new TypeModel();

                        t.TypeName = item;

                        SQLiteDataAccess.SaveType(t);
                    }
                }

                RefreshTypesLB();

                RefreshTypesCB();

                MessageBox.Show("Import of Types Completed Successful", "Import of Types", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Import of Types Completed Successful", "Types Imported: " + count);

            }
            else
            {
                MessageBox.Show("Import of Types Failed to open file", "Import of Types", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Import of Types Failed to open file");
            }


        }
        /// <summary>
        /// Export the Types to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportTypeButton(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            bool worked = CSVFile.GetExportFileName(out fileName);
            string[] contents = new string[Types.Count + 1];

            if (worked)
            {
                int i = 0;

                contents[i] = "TypeId,TypeName";
                i++;

                foreach (var item in Types)
                {
                    contents[i] = item.TypeId + "," + item.TypeName + ",";
                    i++;
                }

                System.IO.File.WriteAllLines(fileName, contents);

                MessageBox.Show("Export of Types Completed Successful", "Export of Types", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Export of Types Completed Successful", "Types Exported: " + Types.Count);
            }
            else
            {
                MessageBox.Show("Export of Types Failed to open file", "Export of Types", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Export of Types Failed to open file");
            }
        }
        // ************************************************
        // ***      M A N U F A C T U R E R S
        /// <summary>
        /// Re-Load the Manufacturer ListBox
        /// </summary>
        private void RefreshManusLB()
        {
            // Get the Manufacturers
            Manufacturers = SQLiteDataAccess.LoadManufacturers();

            Manufacturers.Sort((x, y) => x.ManufacturerName.CompareTo(y.ManufacturerName));

            // Display the parts in the ListBox
            WireUpManuLB();

            // reset the add field on the display
            tbManuName.Text = "";

            // reset the display field on the display
            lblMManus.Content = LookUpTranslation("ManufacturerList") + " (" + Manufacturers.Count.ToString() + "): ";

            lblMManuIdValue.Content = "";

            errorlog.InformationMessage("Manufacturers Refreshed");

        }
        /// <summary>
        /// Populate the Manufacturer ListBox
        /// </summary>
        private void WireUpManuLB()
        {
            lbManusList.Items.Clear();

            foreach (var m in Manufacturers)
            {
                lbManusList.Items.Add(m.ManufacturerName);
            }
        }
        /// <summary>
        /// Clear the Manufacturer screen data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearManuButton(object sender, RoutedEventArgs e)
        {
            lblMManuIdValue.Content = "";
            tbManuName.Text = "";
        }
        /// <summary>
        /// Refresh the Manufacturer ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshManusButton(object sender, RoutedEventArgs e)
        {
            RefreshManusLB();
        }
        /// <summary>
        /// Add the New Manufacturer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddManuButton(object sender, RoutedEventArgs e)
        {
            if (tbManuName.Text == "")
            {
                ManufacturerModel m = new ManufacturerModel();

                m.ManufacturerName = tbManuName.Text;

                SQLiteDataAccess.SaveManufacturer(m);

                // Refresh ListBox
                RefreshManusLB();

                errorlog.InformationMessage("Manufacturers Added", m.ToString());
            }
            else
            {
                MessageBox.Show("Manufacturer Name field is blank!", "Manufacturers Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Update the selected Manufacturer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateManuButton(object sender, RoutedEventArgs e)
        {
            if (tbManuName.Text != "")
            {
                int index = lbManusList.SelectedIndex;

                errorlog.InformationMessage("Manufacturers Updated", "From: " + Manufacturers[index].ToString());
                Manufacturers[index].ManufacturerName = tbManuName.Text;
                errorlog.InformationMessage("", "To: " + Manufacturers[index].ToString());

                SQLiteDataAccess.UpdateManufacturer(Manufacturers[index]);

                // Refresh ListBox
                RefreshManusLB();
            }
        }
        /// <summary>
        /// Retrieve the Manufacturer data that is selected in the ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManuSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbManusList.SelectedIndex;

            if (index != -1)
            {
                lblMManus.Content = lblMManus.Content = LookUpTranslation("ManufacturerList") + " (" + Manufacturers.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblMManus.Content += index.ToString();
                }

                lblMManuIdValue.Content = Manufacturers[index].ManufacturerId;

                tbManuName.Text = Manufacturers[index].ManufacturerName;

                btnAddManu.IsEnabled = false;
                btnUpdateManu.IsEnabled = true;
                btnDeleteManu.IsEnabled = true;
            }
            else
            {
                btnAddManu.IsEnabled = true;
                btnUpdateManu.IsEnabled = false;
                btnDeleteManu.IsEnabled = false;
            }
        }
        /// <summary>
        /// Delete the select Manufacturer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteManuButton(object sender, RoutedEventArgs e)
        {
            int index = lbManusList.SelectedIndex;
            lblMManus.Content = LookUpTranslation("ManufacturerList") + " " + index.ToString();

            errorlog.InformationMessage("Manufacturers Deleted", Manufacturers[index].ToString());

            SQLiteDataAccess.DeleteManufacturer(Manufacturers[index].ManufacturerId);

            RefreshManusLB();
        }
        /// <summary>
        /// Import the Manufacturers to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportManuButton(object sender, RoutedEventArgs e)
        {
            List<string> importManufacturers = new List<string>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importManufacturers = CSVFile.ReadImportFile1(fileName, "ManufacturerName");

                int count = 0;
                foreach (var item in importManufacturers)
                {
                    if (FindManu(item) == -1)
                    {
                        count++;
                        ManufacturerModel m = new ManufacturerModel();

                        m.ManufacturerName = item;

                        SQLiteDataAccess.SaveManufacturer(m);
                    }
                }

                RefreshManusLB();

                WireUpManufacturersCB();

                MessageBox.Show("Import of Manufacturers Completed Successful", "Import of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Import of Manufacturers Completed Successful", "Manufacturers Imported: " + count);

            }
            else
            {
                MessageBox.Show("Import of Manufacturers Failed to open file", "Import of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Import of Manufacturers Failed to open file");
            }
        }
        /// <summary>
        /// Export the Manufacturers to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportManuButton(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            bool worked = CSVFile.GetExportFileName(out fileName);
            string[] contents = new string[Manufacturers.Count + 1];

            if (worked)
            {
                int i = 0;

                contents[i] = "ManufacturerId,ManufacturerName";
                i++;

                foreach (var item in Manufacturers)
                {
                    contents[i] = item.ManufacturerId + "," + item.ManufacturerName + ",";
                    i++;
                }

                System.IO.File.WriteAllLines(fileName, contents);

                MessageBox.Show("Export of Manufacturers Completed Successful", "Export of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Export of Manufacturers Completed Successful", "Manufacturers Exported: " + Manufacturers.Count);
            }
            else
            {
                MessageBox.Show("Export of Manufacturers Failed to open file", "Export of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Export of Manufacturers Failed to open file");
            }
        }
        // ************************************************
        // ***      L O C A T I O N S
        /// <summary>
        /// Re-Load the Locations from the DB
        /// </summary>
        private void RefreshLocsLB()
        {
            // Get the Locations
            Locations = SQLiteDataAccess.LoadLocations();

            Locations.Sort((x, y) => x.LocationName.CompareTo(y.LocationName));

            // Display the parts in the ListBox
            WireUpLocLB();

            // reset the add field on the display
            tbLocName.Text = "";

            // reset the display field on the display
            lblLLocs.Content = LookUpTranslation("LocationList") + "(" + Locations.Count.ToString() + "): ";

            lblLLocIdValue.Content = "";

            errorlog.InformationMessage("Locations Refreshed");
        }
        /// <summary>
        /// Populate the location ComboBox in the Locations Screen
        /// </summary>
        private void WireUpLocLB()
        {
            lbLocsList.Items.Clear();

            foreach (var l in Locations)
            {
                lbLocsList.Items.Add(l.LocationName);
            }
        }
        /// <summary>
        /// Clear the Location screen data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearLocButton(object sender, RoutedEventArgs e)
        {
            lblLLocIdValue.Content = "";
            tbLocName.Text = "";
        }
        /// <summary>
        /// Refresh the Locations ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshLocsButton(object sender, RoutedEventArgs e)
        {
            RefreshLocsLB();
        }
        /// <summary>
        /// Add the New Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLocButton(object sender, RoutedEventArgs e)
        {

            if (tbLocName.Text == "")
            {
                LocationModel l = new LocationModel();

                l.LocationName = tbLocName.Text;

                SQLiteDataAccess.SaveLocation(l);

                // Refresh ListBox
                RefreshLocsLB();

                errorlog.InformationMessage("Locations Added", l.ToString());
            }
            else
            {
                MessageBox.Show("Location Name field is blank!", "Locations Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Update the selected Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLocButton(object sender, RoutedEventArgs e)
        {
            if (tbLocName.Text != "")
            {
                int index = lbLocsList.SelectedIndex;

                errorlog.InformationMessage("Locations Updated", "From: " + Locations[index].ToString());
                Locations[index].LocationName = tbLocName.Text;
                errorlog.InformationMessage("", "To: " + Locations[index].ToString());

                SQLiteDataAccess.UpdateLocation(Locations[index]);

                // Refresh ListBox
                RefreshLocsLB();
            }
        }
        /// <summary>
        /// Retrieve the Location data that is selected in the ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbLocsList.SelectedIndex;

            if (index != -1)
            {
                lblLLocs.Content = LookUpTranslation("LocationList") + " (" + Locations.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblLLocs.Content += index.ToString();
                }

                lblLLocIdValue.Content = Locations[index].LocationId;

                tbLocName.Text = Locations[index].LocationName;

                btnAddLoc.IsEnabled = false;
                btnUpdateLoc.IsEnabled = true;
                btnDeleteLoc.IsEnabled = true;
            }
            else
            {
                btnAddLoc.IsEnabled = true;
                btnUpdateLoc.IsEnabled = false;
                btnDeleteLoc.IsEnabled = false;
            }
        }
        /// <summary>
        /// Export the Locations to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLocButton(object sender, RoutedEventArgs e)
        {
            int index = lbLocsList.SelectedIndex;
            lblLLocs.Content = LookUpTranslation("LocationList") + " " + index.ToString();

            errorlog.InformationMessage("Locations Deleted", Locations[index].ToString());

            SQLiteDataAccess.DeleteLocation(Locations[index].LocationId);

            RefreshLocsLB();
        }
        /// <summary>
        /// Import the Locations from a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportLocButton(object sender, RoutedEventArgs e)
        {
            List<string> importLocations = new List<string>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importLocations = CSVFile.ReadImportFile1(fileName, "LocationName");

                int count = 0;
                foreach (var item in importLocations)
                {
                    if (FindLocation(item) == -1)
                    {
                        count++;
                        LocationModel l = new LocationModel();

                        l.LocationName = item;

                        SQLiteDataAccess.SaveLocation(l);
                    }
                }

                RefreshLocsLB();

                RefreshLocationsCB();

                MessageBox.Show("Import of Locations Completed Successful", "Import of Locations", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Import of Locations Completed Successful", "Locations Imported" + count);
            }
            else
            {
                MessageBox.Show("Import of Locations Failed to open file", "Import of Locations", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Import of Locations Failed to open file");
            }
        }
        /// <summary>
        /// Export the Location to a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportLocButton(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            bool worked = CSVFile.GetExportFileName(out fileName);
            string[] contents = new string[Locations.Count + 1];

            if (worked)
            {
                int i = 0;

                contents[i] = "LocationId,LocationName";
                i++;

                foreach (var item in Locations)
                {
                    contents[i] = item.LocationId + "," + item.LocationName + ",";
                    i++;
                }

                System.IO.File.WriteAllLines(fileName, contents);

                MessageBox.Show("Export of Locations Completed Successful", "Export of Locations", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Export of Locations Completed Successful", "Locations Exported: " + Locations.Count);
            }
            else
            {
                MessageBox.Show("Export of Locations Failed to open file", "Export of Locations", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Export of Locations Failed to open file");
            }

        }
        // ************************************************
        // ***      P O S I T I O N S
        /// <summary>
        /// Re-Load the Position List from the DB
        /// </summary>
        private void RefreshPossLB()
        {
            // Get the Locations
            Positions = SQLiteDataAccess.LoadPositions();
            PosSubSet = SQLiteDataAccess.GetPositions(cbLoc.SelectedIndex);

            // Only need to sort the "Displayed" version
            if (PosSubSet.Count > 0)
            {
                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));
            }

            // Display the Locations in the ComboBox
            cbLoc.Items.Clear();
            WireUpLocCB();
            cbLoc.SelectedIndex = 0;
            // Display the parts in the ListBox
            WireUpPosLB();

            // reset the add field on the display
            tbPosName.Text = "";

            // reset the display field on the display
            lblPPoss.Content = LookUpTranslation("PositionList") + " (" + PosSubSet.Count.ToString() + "): ";

            lblPPosIdValue.Content = "";

            errorlog.InformationMessage("Positions Refreshed");

        }
        /// <summary>
        /// Populate the Position ComboBox in the Position Screen
        /// </summary>
        private void WireUpPosLB()
        {
            lbPossList.Items.Clear();

            foreach (var p in PosSubSet)
            {
                lbPossList.Items.Add(p.PositionName);
            }
        }
        /// <summary>
        /// Populate the Location ComboBox in the Position Screen
        /// </summary>
        private void WireUpLocCB()
        {
            cbLoc.Items.Clear();

            foreach (var l in Locations)
            {
                cbLoc.Items.Add(l.LocationName);
            }
        }
        /// <summary>
        /// Clear the Position Screen data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPosButton(object sender, RoutedEventArgs e)
        {
            lblPPosIdValue.Content = "";
            cbLoc.Text = "";
            tbPosName.Text = "";
        }
        /// <summary>
        /// Refresh the Position ListBox data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshPossButton(object sender, RoutedEventArgs e)
        {
            RefreshPossLB();
        }
        /// <summary>
        /// Add the New Position to the DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPosButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (tbPosName.Text == "") error = true;
            if (cbLoc.Text == "") error = true;

            if (error == true)
            {
                PositionModel p = new PositionModel();

                p.LocationFK = FindLocation((string)cbLoc.SelectedValue);

                p.PositionName = tbPosName.Text;

                SQLiteDataAccess.SavePosition(p);

                // Refresh ListBox
                RefreshPossLB();

                errorlog.InformationMessage("Positions Added", p.ToString());
            }
            else
            {
                MessageBox.Show("One or more fields are blank!", "Positions Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// Update the Selected Position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatePosButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (tbPosName.Text == "") error = true;
            if (cbLoc.Text == "") error = true;

            if (error == true)
            {
                int index = lbPossList.SelectedIndex;

                int selectedID = PosSubSet[index].PositionId;

                errorlog.InformationMessage("Positions Updated", "From: " + PosSubSet[index].ToString());
                PosSubSet[index].PositionName = tbPosName.Text;
                errorlog.InformationMessage("", "To: " + PosSubSet[index].ToString());

                SQLiteDataAccess.UpdatePosition(PosSubSet[index]);

                // Refresh ListBox
                RefreshPossLB();
            }
        }
        /// <summary>
        /// Retrieve the Position data that is selected in the ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PosSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbPossList.SelectedIndex;

            if (index != -1)
            {
                lblPPoss.Content = LookUpTranslation("PositionList") + " (" + PosSubSet.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblPPoss.Content += index.ToString();
                }

                lblPPosIdValue.Content = PosSubSet[index].PositionId;

                tbPosName.Text = PosSubSet[index].PositionName;

                btnAddPos.IsEnabled = false;
                btnUpdatePos.IsEnabled = true;
                btnDeletePos.IsEnabled = true;
            }
            else
            {
                btnAddPos.IsEnabled = true;
                btnUpdatePos.IsEnabled = false;
                btnDeletePos.IsEnabled = false;
            }
        }
        /// <summary>
        /// Delete a select Position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePosButton(object sender, RoutedEventArgs e)
        {
            int index = lbPossList.SelectedIndex;
            lblPPoss.Content = LookUpTranslation("PositionList") + " " + index.ToString();

            errorlog.InformationMessage("Positions Deleted", PosSubSet[index].ToString());

            SQLiteDataAccess.DeletePosition(PosSubSet[index].PositionId);

            RefreshPossLB();
        }
        /// <summary>
        ///  Imports the Positions from a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportPosButton(object sender, RoutedEventArgs e)
        {
            List<ColumnModel2> importPositions = new List<ColumnModel2>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importPositions = CSVFile.ReadImportFile2(fileName, "PositionName");

                int count = 0;
                foreach (var item in importPositions)
                {
                    if (FindLocation(item.First) > -1 && FindPosition(FindLocation(item.First), item.Second) == -1)
                    {
                        count++;
                        PositionModel p = new PositionModel();

                        p.LocationFK = FindLocation(item.First);
                        p.PositionName = item.Second;

                        SQLiteDataAccess.SavePosition(p);
                    }
                }

                RefreshPossLB();

                RefreshPositionsCB();

                MessageBox.Show("Import of Positions Completed Successful", "Import of Positions", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Import of Positions Completed Successful", "Position Imported: " + count);
            }
            else
            {
                MessageBox.Show("Import of Positions Failed to open file", "Import of Positions", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Import of Positions Failed to open file");
            }
        }
        /// <summary>
        /// Exports the Positions to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportPosButton(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            bool worked = CSVFile.GetExportFileName(out fileName);
            string[] contents = new string[Positions.Count + 1];

            if (worked)
            {
                int i = 0;

                contents[i] = "PositionId,LocationName,PositionName";
                i++;

                foreach (var item in Positions)
                {
                    contents[i] = item.PositionId + ","
                        + LookupLocation(item.LocationFK) + ","
                        + item.PositionName + ",";
                    i++;
                }

                System.IO.File.WriteAllLines(fileName, contents);

                MessageBox.Show("Export of Positions Completed Successful", "Export of Positions", MessageBoxButton.OK, MessageBoxImage.Information);
                errorlog.InformationMessage("Export of Positions Completed Successful", "Positions Exported: " + Positions.Count);
            }
            else
            {
                MessageBox.Show("Export of Positions Failed to open file", "Export of Positions", MessageBoxButton.OK, MessageBoxImage.Warning);
                errorlog.WarningMessage("Export of Positions Failed to open file");
            }

        }
        /// <summary>
        /// Update the Positions when the Location Drop-down changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblPLoc.Content = "Location: " + cbLoc.SelectedIndex.ToString();
            }

            UpdateLocPoss();
        }
        /// <summary>
        /// Updates the Possible Position for a Given Location
        /// </summary>
        private void UpdateLocPoss()
        {
            int index = FindLocation((string)cbLoc.SelectedValue);

            PosSubSet.Clear();

            if (index > 0)
            {
                PosSubSet = SQLiteDataAccess.GetPositions(index);

                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));

                lblPPoss.Content = LookUpTranslation("PositionList") + " (" + PosSubSet.Count.ToString() + "): ";
            }

            WireUpPosLB();
        }
        // ************************************************
        // // ****     M e n u
        /// <summary>
        /// Open the File\Setting Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingButton(object sender, RoutedEventArgs e)
        {
            new FileSettings(errorlog).Show();
        }
        /// <summary>
        /// Exit the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton(object sender, RoutedEventArgs e)
        {
            errorlog.InformationMessage("Application Ending ...", "Someone pressed the Exit options.");

            Application.Current.Shutdown();
        }
        /// <summary>
        /// Open the Help\Help Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton(object sender, RoutedEventArgs e)
        {
            new AboutHelp(errorlog).Show();
        }
        /// <summary>
        /// Open the Help\About Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutButton(object sender, RoutedEventArgs e)
        {
            new AboutAbout(errorlog).Show();
        }
        // ************************************************
        // ****     G E N E R A L
        /// <summary>
        /// Lookup the Part based on the Part Name
        /// </summary>
        /// <param name="l">Part Name</param>
        /// <returns>Part Id</returns>
        private int FindPart(string p)
        {
            foreach (var part in Parts)
            {
                if (part.PartName == p)
                {
                    return part.PartsId;
                }
            }

            return -1;
        }
        /// <summary>
        /// Lookup the Part based on the Part Id
        /// </summary>
        /// <param name="id">Part Id</param>
        /// <returns>Part Name</returns>
        private string LookupPart(int id)
        {
            foreach (var part in Parts)
            {
                if (part.PartsId == id)
                {
                    return part.PartName;
                }
            }

            return "";
        }
        /// <summary>
        /// Lookup the Part Index based on the Part Id
        /// </summary>
        /// <param name="id">Part Id</param>
        /// <returns>Part index</returns>
        private int LookupPartPos(int id)
        {
            for (int i = 0; i < Parts.Count; i++)
            {
                if (Parts[i].PartsId == id)
                {
                    return i;
                }
            }

            return -1;
        }
        /// <summary>
        /// Lookup the Type based on the Type Name
        /// </summary>
        /// <param name="l">Type Name</param>
        /// <returns>Type Id</returns>
        public int FindType(string t)
        {
            foreach (var type in Types)
            {
                if (type.TypeName == t)
                {
                    return type.TypeId;
                }
            }

            return -1;
        }
        /// <summary>
        /// Lookup the Type based on the Type Id
        /// </summary>
        /// <param name="id">Type Id</param>
        /// <returns>Type Name</returns>
        private string LookupType(int id)
        {
            foreach (var type in Types)
            {
                if (type.TypeId == id)
                {
                    return type.TypeName;
                }
            }

            return "";
        }
        /// <summary>
        /// Lookup the Manufacturer based on the Manufacturer Name
        /// </summary>
        /// <param name="l">Manufacturer Name</param>
        /// <returns>Manufacturer Id</returns>
        private int FindManu(string m)
        {
            foreach (var manu in Manufacturers)
            {
                if (manu.ManufacturerName == m)
                {
                    return manu.ManufacturerId;
                }
            }

            return -1;
        }
        /// <summary>
        /// Lookup the Manufacturer based on the Manufacturer Id
        /// </summary>
        /// <param name="id">Manufacturer Id</param>
        /// <returns>Manufacturer Name</returns>
        private string LookupManu(int id)
        {
            foreach (var manu in Manufacturers)
            {
                if (manu.ManufacturerId == id)
                {
                    return manu.ManufacturerName;
                }
            }

            return "";
        }
        /// <summary>
        /// Lookup the Location based on the Location Name
        /// </summary>
        /// <param name="l">Location Name</param>
        /// <returns>Location Id</returns>
        private int FindLocation(string l)
        {
            foreach (var loc in Locations)
            {
                if (loc.LocationName == l)
                {
                    return loc.LocationId;
                }
            }

            return -1;
        }
        // ****
        /// <summary>
        /// Lookup the Location based on the Location Id
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <returns>Location Name</returns>
        private string LookupLocation(int id)
        {
            foreach (var loc in Locations)
            {
                if (loc.LocationId == id)
                {
                    return loc.LocationName;
                }
            }

            return "";
        }
        // ****
        /// <summary>
        /// Find the OPosition Id based on the Location Id and the Position Name
        /// </summary>
        /// <param name="l">Location Id</param>
        /// <param name="p">Position Name</param>
        /// <returns>Position Id</returns>
        private int FindPosition(int l, string p)
        {
            PosSubSet.Clear();

            if (l > 0)
            {
                PosSubSet = SQLiteDataAccess.GetPositions(l);

                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));
            }

            foreach (var pos in PosSubSet)
            {
                if (pos.PositionName == p)
                {
                    return pos.PositionId;
                }
            }

            return -1;
        }
        /// <summary>
        /// Looks up the Position based on the Location and Position Id's
        /// </summary>
        /// <param name="Lid">Location Id</param>
        /// <param name="Pid">Position Id</param>
        /// <returns>Position Name</returns>
        private string LookupPosition(int Lid, int Pid)
        {
            foreach (var pos in Positions)
            {
                if (pos.LocationFK == Lid && pos.PositionId == Pid)
                {
                    return pos.PositionName;
                }
            }

            return "";
        }
        /// <summary>
        /// Fetch's the correct Language Phrase from the Dictionary
        /// </summary>
        /// <param name="dic">Dictionary to use</param>
        /// <param name="lookUpKey">Key to look up</param>
        /// <returns></returns>
        private string LookUpTranslation(string lookUpKey)
        {
            bool worked = ScreenText.TryGetValue(lookUpKey, out string? output);

            if (worked)
            {
                return output;
            }
            else
            {
                return "Unknown (" + lookUpKey + ")";
            }
        }
        /// <summary>
        /// Validate the Number only fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRetainValidation(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Only allow numbers in the textbox
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}