using MiniStores.Models;
using System;
using System.Collections.Generic;
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
* + 
* + 
* 
* Developer's Log
* ===============
* + 17-Aug-2022 - Added Setting screen.
* + 15-Aug-2022 - Made App Multi-Lingual
* + 09-Aug-2022 - Added INI file to save the settings (INIFile.cs).
* + 01-Aug-2022 - Added About screen.
* + 22-Jul-2022 - Added Simple Error logging (JobLog.cs). 
* + 17-Jul-2022 - Added a Help Screen\File.
* + 27-Jul-2022 - Added SQLite as a DB as I can not get the MS-SQL to work on my machine.
* + 23-Jun-2022 - Applications Created.
*/

namespace MiniStores
{
    public partial class MainWindow : Window
    {
        // Global Objects ...
        List<PartsModel> Parts = new List<PartsModel>();
        List<TypeModel> Types = new List<TypeModel>();
        List<ManufacturerModel> Manufacturers = new List<ManufacturerModel>();
        List<LocationModel> Locations = new List<LocationModel>();
        List<PositionModel> Positions = new List<PositionModel>();

        List<PositionModel> PosSubSet = new List<PositionModel>();

        Dictionary<string, PharseText> ScreenText = new Dictionary<string, PharseText>();
        GlobalSetting gs = new GlobalSetting();

        Joblog errorlog = new Joblog("MiniStore", GlobalSetting.LogsToKeep);
        // ************************************************
        // ***      M A I N   W I N D O W
        public MainWindow()
        {
            LanguageData AppLanguage = new LanguageData(errorlog, GlobalSetting.Language);

            ScreenText = AppLanguage.GetLanguage();
            DataContext = ScreenText;

            InitializeComponent();

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

            //Translate Search grid text
            PId.Header = LookUpTranslation("Id");
            PName.Header = LookUpTranslation("Part");
            PType.Header = LookUpTranslation("Type");
            PQty.Header = LookUpTranslation("Qty");
            PManu.Header = LookUpTranslation("Manufacturer");
            PLoc.Header = LookUpTranslation("Location");
            PPos.Header = LookUpTranslation("Position");

            errorlog.InformationMessage("Program Initialised");
        }
        // ************************************************
        // ***      S E A R C H
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

            if (cbSearchType.SelectedIndex > 0)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " TypeFK = \'" + FindType(cbSearchType.SelectedValue.ToString()).ToString() + "\'";
                FirstConditionSet = true;
            }


            if (cbSearchManu.SelectedIndex > 0)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " ManufacturerFK = \'" + FindManu(cbSearchManu.SelectedValue.ToString()).ToString() + "\'";
                FirstConditionSet = true;
            }

            if (cbSearchLoc.SelectedIndex > 0)
            {
                if (FirstConditionSet == true)
                {
                    SearchQuery += " and ";
                }
                SearchQuery += " LocationFK = \'" + FindLocation(cbSearchLoc.SelectedValue.ToString()).ToString() + "\'";
                FirstConditionSet = true;
            }

            if (cbSearchLoc.SelectedIndex > 0
                && cbSearchPos.SelectedIndex > 0)
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
        // ****
        private void tbSearchPart_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Search_Changed();
        }
        // ****
        private void cbSearchType_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Search_Changed();
        }
        // ****
        private void cbSearchManu_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Search_Changed();
        }
        // ****
        private void cbSearchPos_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Search_Changed();
        }
        // ****
        private void cbSearchLoc_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateSLocPoss();
            Search_Changed();
        }
        // ****
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
        // ****
        private void WireUpSPosCB()
        {
            cbSearchPos.Items.Clear();

            foreach (var p in PosSubSet)
            {
                cbSearchPos.Items.Add(p.PositionName);
            }
        }
        // ****
        private void DateGrid_Selected(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            int index = dgSearch.SelectedIndex;

            if (index >= -1)
            {
                DataGridRow row = (DataGridRow)dgSearch.ItemContainerGenerator.ContainerFromIndex(index);

                ColumnTable drv = (ColumnTable)dgSearch.SelectedItem;

                int selectId = int.Parse(drv.PId);

                MessageBox.Show($"you selected - {selectId}");
            }
        }
        // ************************************************
        // ***      P A R T S
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
            lblParts.Content = LookUpTranslation("PartList") + " (" + Parts.Count.ToString() + "): ";

            lblPartId.Content = "";

            errorlog.InformationMessage("Parts Refreshed");
        }
        // ****
        private void WireUpPartsLB()
        {
            lbPartsList.Items.Clear();

            foreach (var p in Parts)
            {
                string text = $"{p.PartName} ({LookupType(p.TypeFK)}) ";

                lbPartsList.Items.Add(text);
            }
        }
        // ****
        private void ClearTypeButton(object sender, RoutedEventArgs e)
        {
            tbTypeName.Text = "";
            lblTypeId.Content = "";
        }
        // ****
        private void RefreshTypesCB()
        {
            Types = SQLiteDataAccess.LoadTypes();

            // Load a Blank one for the ComboBox's
            Types.Add(new TypeModel() { TypeId = 0, TypeName = " " });

            // Sort
            Types.Sort((x, y) => x.TypeName.CompareTo(y.TypeName));

            WireUpTypesCB();
        }
        // ****
        private void WireUpTypesCB()
        {
            cbType.Items.Clear();

            foreach (var t in Types)
            {
                cbType.Items.Add(t.TypeName);
                cbSearchType.Items.Add(t.TypeName);
            }
        }
        // ****
        private void RefreshManufacturersCB()
        {
            Manufacturers = SQLiteDataAccess.LoadManufacturers();

            // Load a Blank one for the ComboBox's
            Manufacturers.Add(new ManufacturerModel() { ManufacturerId = 0, ManufacturerName = " " });

            // Sort
            Manufacturers.Sort((x, y) => x.ManufacturerName.CompareTo(y.ManufacturerName));

            WireUpManufacturersCB();
        }
        // ****
        private void WireUpManufacturersCB()
        {
            cbManufacturer.Items.Clear();

            foreach (var m in Manufacturers)
            {
                cbManufacturer.Items.Add(m.ManufacturerName);
                cbSearchManu.Items.Add(m.ManufacturerName);
            }
        }
        // ****
        private void RefreshLocationsCB()
        {
            Locations = SQLiteDataAccess.LoadLocations();

            // Load a Blank one for the ComboBox's
            Locations.Add(new LocationModel() { LocationId = 0, LocationName = " " });

            // Sort
            Locations.Sort((x, y) => x.LocationName.CompareTo(y.LocationName));

            WireUpLocationsCB();
        }
        // ****
        private void WireUpLocationsCB()
        {
            cbLocation.Items.Clear();

            foreach (var l in Locations)
            {
                cbLocation.Items.Add(l.LocationName);
                cbSearchLoc.Items.Add(l.LocationName);
            }
        }
        // ****
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
        // ****
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
        // ****
        private void RefreshPartsButton(object sender, RoutedEventArgs e)
        {
            RefreshPartsLB();
        }
        // ****
        private void ClearPartButton(object sender, RoutedEventArgs e)
        {
            lblPartId.Content = "";
            tbPartName.Text = "";
            tbPartQty.Text = "";

            cbType.SelectedIndex = 0;
            cbManufacturer.SelectedIndex = 0;
            cbLocation.SelectedIndex = 0;
            cbPosition.SelectedIndex = 0;
            tbPrice.Text = $"{0.00m:C2}";
            tbComment.Text = "";
        }
        // ****
        private void AddPartButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (tbPartName.Text == "") error = true;
            if (tbPartQty.Text == "") error = true;
            if (cbType.Text == "") error = true;
            if (cbManufacturer.Text == "") error = true;
            if (cbLocation.Text == "") error = true;
            if (cbPosition.Text == "") error = true;

            if (error == true)
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
        // ****
        private void UpdatePartButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (tbPartName.Text == "") error = true;
            if (tbPartQty.Text == "") error = true;
            if (cbType.Text == "") error = true;
            if (cbManufacturer.Text == "") error = true;
            if (cbLocation.Text == "") error = true;
            if (cbPosition.Text == "") error = true;

            if (error == false)
            {
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
        // ****
        private void PartSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbPartsList.SelectedIndex;

            if (index != -1)
            {

                lblParts.Content = LookUpTranslation("PartList") + " (" + Parts.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblParts.Content += index.ToString();
                }

                lblPartId.Content = Parts[index].PartsId;

                tbPartName.Text = Parts[index].PartName;
                tbPartQty.Text = Parts[index].Quantity.ToString();
                cbType.SelectedValue = LookupType(Parts[index].TypeFK);
                cbManufacturer.SelectedValue = LookupManu(Parts[index].ManufacturerFK);
                cbLocation.SelectedValue = LookupLocation(Parts[index].LocationFK);

                UpdateLocationPositions();
                cbPosition.SelectedValue = LookupPosition(Parts[index].LocationFK, Parts[index].PositionFK);

                tbPrice.Text = $"{Parts[index].Price:C2}";
                tbComment.Text = Parts[index].Comment;

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
        // ****
        private void DeletePartButton(object sender, RoutedEventArgs e)
        {
            int index = lbPartsList.SelectedIndex;
            lblParts.Content = "Parts List : " + index.ToString();

            errorlog.InformationMessage("Part Deleted", Parts[index].ToString());

            SQLiteDataAccess.DeletePart(Parts[index].PartsId);

            RefreshPartsLB();
        }
        // ****
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
        // ****
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
        // ****
        private void TypeCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblType.Content = LookUpTranslation("TypeM") + " " + cbType.SelectedIndex.ToString();
            }
        }
        // ****
        private void ManufacturerCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblManufacturer.Content = LookUpTranslation("ManufacturerM") + " " + cbManufacturer.SelectedIndex.ToString();
            }
        }
        // ****
        private void LocationCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblLocation.Content = LookUpTranslation("LocationM") + " " + cbLocation.SelectedIndex.ToString();
            }

            UpdateLocationPositions();
        }
        // ****
        private void PositionCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblPosition.Content = LookUpTranslation("PositionM") + " " + cbPosition.SelectedIndex.ToString();
            }
        }
        // ***      T Y P E S
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
            lblTypes.Content = LookUpTranslation("TypeList") + " (" + Types.Count.ToString() + "): ";

            lblTypeId.Content = "";

            errorlog.InformationMessage("Types Refreshed");
        }
        // ****
        private void WireUpTypesLB()
        {
            lbTypesList.Items.Clear();

            foreach (var t in Types)
            {
                lbTypesList.Items.Add(t.TypeName);
            }
        }
        // ****
        private void RefreshTypesButton(object sender, RoutedEventArgs e)
        {
            RefreshTypesLB();
        }
        // ****
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
        // ****
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
        // ****
        private void TypeSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbTypesList.SelectedIndex;

            if (index != -1)
            {
                lblTypes.Content = LookUpTranslation("TypeList") + " (" + Types.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblTypes.Content += index.ToString();
                }

                lblTypeId.Content = Types[index].TypeId;

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
        // ****
        private void DeleteTypeButton(object sender, RoutedEventArgs e)
        {
            int index = lbTypesList.SelectedIndex;
            lblTypes.Content = LookUpTranslation("TypeList") + " " + index.ToString();

            errorlog.InformationMessage("Types Deleted", Types[index].ToString());

            SQLiteDataAccess.DeleteType(Types[index].TypeId);

            RefreshTypesLB();
        }
        // ****
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
        // ****
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
        // ***      M A N U F A C T U R E R S
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
            lblManus.Content = LookUpTranslation("ManufacturerList") + " (" + Manufacturers.Count.ToString() + "): ";

            lblManuId.Content = "";

            errorlog.InformationMessage("Manufacturers Refreshed");

        }
        // ****
        private void WireUpManuLB()
        {
            lbManusList.Items.Clear();

            foreach (var m in Manufacturers)
            {
                lbManusList.Items.Add(m.ManufacturerName);
            }
        }
        // ****
        private void ClearManuButton(object sender, RoutedEventArgs e)
        {
            lblManuId.Content = "";
            tbManuName.Text = "";
        }
        // ****
        private void RefreshManusButton(object sender, RoutedEventArgs e)
        {
            RefreshManusLB();
        }
        // ****
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
        // ****
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
        // ****
        private void ManuSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbManusList.SelectedIndex;

            if (index != -1)
            {
                lblManus.Content = lblManus.Content = LookUpTranslation("ManufacturerList") + " (" + Manufacturers.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblManus.Content += index.ToString();
                }

                lblManuId.Content = Manufacturers[index].ManufacturerId;

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
        // ****
        private void DeleteManuButton(object sender, RoutedEventArgs e)
        {
            int index = lbManusList.SelectedIndex;
            lblManus.Content = LookUpTranslation("ManufacturerList") + " " + index.ToString();

            errorlog.InformationMessage("Manufacturers Deleted", Manufacturers[index].ToString());

            SQLiteDataAccess.DeleteManufacturer(Manufacturers[index].ManufacturerId);

            RefreshManusLB();
        }
        // ****
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
        // ****
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
        // ***      L O C A T I O N S
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
            lblLocs.Content = LookUpTranslation("LocationList") + "(" + Locations.Count.ToString() + "): ";

            lblLocId.Content = "";

            errorlog.InformationMessage("Locations Refreshed");
        }
        // ****
        private void WireUpLocLB()
        {
            lbLocsList.Items.Clear();

            foreach (var l in Locations)
            {
                lbLocsList.Items.Add(l.LocationName);
            }
        }
        // ****
        private void ClearLocButton(object sender, RoutedEventArgs e)
        {
            lblLocId.Content = "";
            tbLocName.Text = "";
        }
        // ****
        private void RefreshLocsButton(object sender, RoutedEventArgs e)
        {
            RefreshLocsLB();
        }
        // ****
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
        // ****
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
        // ****
        private void LocSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbLocsList.SelectedIndex;

            if (index != -1)
            {
                lblLocs.Content = LookUpTranslation("LocationList") + " (" + Locations.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblLocs.Content += index.ToString();
                }

                lblLocId.Content = Locations[index].LocationId;

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
        // ****
        private void DeleteLocButton(object sender, RoutedEventArgs e)
        {
            int index = lbLocsList.SelectedIndex;
            lblLocs.Content = LookUpTranslation("LocationList") + " " + index.ToString();

            errorlog.InformationMessage("Locations Deleted", Locations[index].ToString());

            SQLiteDataAccess.DeleteLocation(Locations[index].LocationId);

            RefreshLocsLB();
        }
        // ****
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
        // ****
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
        // ***      P O S I T I O N S
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
            lblPoss.Content = LookUpTranslation("PositionList") + " (" + PosSubSet.Count.ToString() + "): ";

            lblPosId.Content = "";

            errorlog.InformationMessage("Positions Refreshed");

        }
        // ****
        private void WireUpPosLB()
        {
            lbPossList.Items.Clear();

            foreach (var p in PosSubSet)
            {
                lbPossList.Items.Add(p.PositionName);
            }
        }
        // ****
        private void WireUpLocCB()
        {
            cbLoc.Items.Clear();

            foreach (var l in Locations)
            {
                cbLoc.Items.Add(l.LocationName);
            }
        }
        // ****
        private void ClearPosButton(object sender, RoutedEventArgs e)
        {
            lblPosId.Content = "";
            cbLoc.Text = "";
            tbPosName.Text = "";
        }
        // ****
        private void RefreshPossButton(object sender, RoutedEventArgs e)
        {
            RefreshPossLB();
        }
        // ****
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
        // ****
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
        // ****
        private void PosSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbPossList.SelectedIndex;

            if (index != -1)
            {
                lblPoss.Content = LookUpTranslation("PositionList") + " (" + PosSubSet.Count.ToString() + "): ";

                if (GlobalSetting.DebugApp == true)
                {
                    lblPoss.Content += index.ToString();
                }

                lblPosId.Content = PosSubSet[index].PositionId;

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
        // ****
        private void DeletePosButton(object sender, RoutedEventArgs e)
        {
            int index = lbPossList.SelectedIndex;
            lblPoss.Content = LookUpTranslation("PositionList") + " " + index.ToString();

            errorlog.InformationMessage("Positions Deleted", PosSubSet[index].ToString());

            SQLiteDataAccess.DeletePosition(PosSubSet[index].PositionId);

            RefreshPossLB();

        }
        // ****
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
        // ****
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
        // ****
        private void LocCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GlobalSetting.DebugApp == true)
            {
                lblLoc.Content = "Location: " + cbLoc.SelectedIndex.ToString();
            }

            UpdateLocPoss();
        }
        // ************************************************
        // ************************************************
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
        // ****
        private void UpdateLocPoss()
        {
            int index = FindLocation((string)cbLoc.SelectedValue);

            PosSubSet.Clear();

            if (index > 0)
            {
                PosSubSet = SQLiteDataAccess.GetPositions(index);

                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));

                lblPoss.Content = LookUpTranslation("PositionList") + " (" + PosSubSet.Count.ToString() + "): ";
            }

            WireUpPosLB();
        }
        // **** ****
        private void SettingButton(object sender, RoutedEventArgs e)
        {
            new FileSettings(errorlog, gs).Show();
        }
        // ****
        private void ExitButton(object sender, RoutedEventArgs e)
        {
            errorlog.InformationMessage("Application Ending ...", "Someone pressed the Exit options.");

            Application.Current.Shutdown();
        }
        // ****
        private void HelpButton(object sender, RoutedEventArgs e)
        {
            new AboutHelp().Show();
        }
        // ****
        private void AboutButton(object sender, RoutedEventArgs e)
        {
            new AboutAbout().Show();
        }
        // ****     G E N E R A L
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
        // ****
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
        // ****
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
        // ****
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
        // ****
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
        // ****
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
        // ****
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
        // ****
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
        // ****
        public string LookUpTranslation(string lookUpKey)
        {
            bool worked = ScreenText.TryGetValue(lookUpKey, out PharseText output);

            if (worked)
            {
                return output.LangText.ToString();
            }
            else
            {
                return "Unknown";
            }
        }
        // ****
    }
}