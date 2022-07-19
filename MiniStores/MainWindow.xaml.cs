using MiniStores.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MiniStores
{
    public partial class MainWindow : Window
    {
        List<PartsModel> Parts = new List<PartsModel>();
        List<TypeModel> Types = new List<TypeModel>();
        List<ManufacturerModel> Manufacturers = new List<ManufacturerModel>();
        List<LocationModel> Locations = new List<LocationModel>();
        List<PositionModel> Positions = new List<PositionModel>();
        List<PositionModel> PosSubSet = new List<PositionModel>();

        public MainWindow()
        {
            InitializeComponent();

            RefreshPartsLB();
            AddPart.IsEnabled = true;
            UpdatePart.IsEnabled = false;
            DeletePart.IsEnabled = false;

            RefreshTypesLB();
            AddType.IsEnabled = true;
            UpdateType.IsEnabled = false;
            DeleteType.IsEnabled = false;

            RefreshManusLB();
            AddManu.IsEnabled = true;
            UpdateManu.IsEnabled = false;
            DeleteManu.IsEnabled = false;

            RefreshLocsLB();
            AddLoc.IsEnabled = true;
            UpdateLoc.IsEnabled = false;
            DeleteLoc.IsEnabled = false;

            UpdateLocPoss();
            RefreshPossLB();
            AddPos.IsEnabled = true;
            UpdatePos.IsEnabled = false;
            DeletePos.IsEnabled = false;
        }
        // ************************************************
        // ***      P A R T S
        private void RefreshPartsLB()
        {
            // Get the parts
            Parts = SQLiteDataAccess.LoadParts();

            // Display the parts in the ListBox
            WireUpPartsLB();

            // reset the add field on the display
            PartName.Text = "";
            PartQty.Text = "";

            RefreshTypesCB();
            TypeCB.SelectedIndex = 0;

            RefreshManufacturersCB();
            ManufacturerCB.SelectedIndex = 0;

            RefreshLocationsCB();
            LocationCB.SelectedIndex = 0;

            RefreshPositionsCB();
            PositionCB.SelectedIndex = 0;

            // reset the display field on the display
            PartsL.Content = "Parts List (" + Parts.Count.ToString() + "): ";

            PartIdL.Content = "";
        }

        private void WireUpPartsLB()
        {
            PartsListLB.Items.Clear();

            foreach (var p in Parts)
            {
                string text = $"{p.PartName} ({LookupType(p.TypeFK)}) ";

                PartsListLB.Items.Add(text);
            }
        }

        private void RefreshTypesCB()
        {
            Types = SQLiteDataAccess.LoadTypes();

            WireUpTypesCB();
        }

        private void WireUpTypesCB()
        {
            TypeCB.Items.Clear();

            foreach (var t in Types)
            {
                TypeCB.Items.Add(t.TypeName);
            }
        }

        private void RefreshManufacturersCB()
        {
            Manufacturers = SQLiteDataAccess.LoadManufacturers();

            WireUpManufacturersCB();
        }

        private void WireUpManufacturersCB()
        {
            ManufacturerCB.Items.Clear();

            foreach (var m in Manufacturers)
            {
                ManufacturerCB.Items.Add(m.ManufacturerName);
            }
        }

        private void RefreshLocationsCB()
        {
            Locations = SQLiteDataAccess.LoadLocations();

            WireUpLocationsCB();
        }

        private void WireUpLocationsCB()
        {
            LocationCB.Items.Clear();

            foreach (var l in Locations)
            {
                LocationCB.Items.Add(l.LocationName);
            }
        }

        private void RefreshPositionsCB()
        {
            int index = FindLocation((string)LocationCB.SelectedValue);

            // Refresh ALL position list
            Positions.Clear();
            Positions = SQLiteDataAccess.LoadPositions();

            // Rebuild the Drop-down List
            UpdateLocationPositions();
        }

        private void WireUpPositionsCB()
        {
            PositionCB.Items.Clear();
            string text = "";

            foreach (var p in PosSubSet)
            {
                text = p.PositionName;

                PositionCB.Items.Add(text);
            }
        }

        private void RefreshPartsButton(object sender, RoutedEventArgs e)
        {
            RefreshPartsLB();
        }

        private void AddPartButton(object sender, RoutedEventArgs e)
        {
            PartsModel p = new PartsModel();

            p.PartName = PartName.Text;
            p.Quantity = int.Parse(PartQty.Text);
            p.TypeFK = FindType((string)TypeCB.SelectedValue);
            p.ManufacturerFK = FindManu((string)ManufacturerCB.SelectedValue);
            p.LocationFK = FindLocation((string)LocationCB.SelectedValue);
            p.PositionFK = FindPosition(p.LocationFK, (string)PositionCB.SelectedValue);


            SQLiteDataAccess.SavePart(p);

            // Refresh ListBox
            RefreshPartsLB();
        }

        private void UpdatePartButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (PartName.Text == "") error = true;
            if (PartQty.Text == "") error = true;
            if (TypeCB.Text == "") error = true;
            if (ManufacturerCB.Text == "") error = true;
            if (LocationCB.Text == "") error = true;
            if (PositionCB.Text == "") error = true;

            if (!error)
            {
                int index = PartsListLB.SelectedIndex;

                Parts[index].PartName = PartName.Text;
                Parts[index].Quantity = int.Parse(PartQty.Text);
                Parts[index].TypeFK = FindType((string)TypeCB.SelectedValue);
                Parts[index].ManufacturerFK = FindManu((string)ManufacturerCB.SelectedValue);
                Parts[index].LocationFK = FindLocation((string)LocationCB.SelectedValue);
                Parts[index].PositionFK = FindPosition(Parts[index].LocationFK, (string)PositionCB.SelectedValue);

                SQLiteDataAccess.UpdatePart(Parts[index]);

                // Refresh ListBox
                RefreshPartsLB();
            }
        }

        private void PartSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = PartsListLB.SelectedIndex;

            if (index != -1)
            {
                PartsL.Content = "Parts List (" + Parts.Count.ToString() + "): " + index.ToString();

                PartIdL.Content = Parts[index].PartsId;

                PartName.Text = Parts[index].PartName;
                PartQty.Text = Parts[index].Quantity.ToString();
                TypeCB.SelectedValue = LookupType(Parts[index].TypeFK);
                ManufacturerCB.SelectedValue = LookupManu(Parts[index].ManufacturerFK);
                LocationCB.SelectedValue = LookupLocation(Parts[index].LocationFK);

                UpdateLocationPositions();
                PositionCB.SelectedValue = LookupPosition(Parts[index].LocationFK, Parts[index].PositionFK);

                AddPart.IsEnabled = false;
                UpdatePart.IsEnabled = true;
                DeletePart.IsEnabled = true;
            }
            else
            {
                AddPart.IsEnabled = true;
                UpdatePart.IsEnabled = false;
                DeletePart.IsEnabled = false;
            }
        }

        private void DeletePartButton(object sender, RoutedEventArgs e)
        {
            int index = PartsListLB.SelectedIndex;
            PartsL.Content = "Parts List : " + index.ToString();

            SQLiteDataAccess.DeletePart(Parts[index].PartsId);

            RefreshPartsLB();
        }
        private void ImportPartButton(object sender, RoutedEventArgs e)
        {
            List<ColumnModel6> importParts = new List<ColumnModel6>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importParts = CSVFile.ReadImportFile6(fileName, "PartName");

                foreach (var item in importParts)
                {
                    if (FindPart(item.First) == -1)
                    {
                        PartsModel p = new PartsModel();

                        p.PartName = item.First;
                        p.TypeFK = FindType(item.Second);
                        p.Quantity = int.Parse(item.Thrid);
                        p.ManufacturerFK = FindManu(item.Fourth);
                        p.LocationFK = FindLocation(item.Fifth);
                        p.PositionFK = FindPosition(p.LocationFK, item.Sixth);

                        SQLiteDataAccess.SavePart(p);
                    }
                }

                RefreshPartsLB();

                MessageBox.Show("Import of Parts Completed Successfully", "Import of Parts", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Import of Parts Failed to open file", "Import of Parts", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void ExportPartButton(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            bool worked = CSVFile.GetExportFileName(out fileName);
            string[] contents = new string[Parts.Count + 1];

            if (worked)
            {
                int i = 0;

                contents[i] = "PartId,PartName,Type,Quantity,Manufacturer,Location,Position";
                i++;

                foreach (var item in Parts)
                {
                    contents[i] = item.PartsId + ","
                        + item.PartName + ","
                        + LookupType(item.TypeFK) + ","
                        + item.Quantity + ","
                        + LookupManu(item.ManufacturerFK) + ","
                        + LookupLocation(item.LocationFK) + ","
                        + LookupPosition(item.LocationFK, item.PositionFK) + ",";

                    i++;
                }

                System.IO.File.WriteAllLines(fileName, contents);

                MessageBox.Show("Export of Parts Completed Successfully", "Export of Parts", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export of Parts Failed to open file", "Export of Parts", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void TypeCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TypeL.Content = "Type: " + TypeCB.SelectedIndex.ToString();
        }

        private void ManufacturerCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ManufacturerL.Content = "Manufacturer: " + ManufacturerCB.SelectedIndex.ToString();
        }

        private void LocationCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LocationL.Content = "Location: " + LocationCB.SelectedIndex.ToString();

            UpdateLocationPositions();
        }

        private void PositionCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PositionL.Content = "Position: " + PositionCB.SelectedIndex.ToString();
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
            TypeName.Text = "";

            // reset the display field on the display
            TypesL.Content = "Types List (" + Types.Count.ToString() + "): ";

            TypeIdL.Content = "";
        }

        private void WireUpTypesLB()
        {
            TypesListLB.Items.Clear();

            foreach (var t in Types)
            {
                TypesListLB.Items.Add(t.TypeName);
            }
        }

        private void RefreshTypesButton(object sender, RoutedEventArgs e)
        {
            RefreshTypesLB();
        }

        private void AddTypeButton(object sender, RoutedEventArgs e)
        {
            TypeModel t = new TypeModel();

            t.TypeName = TypeName.Text;

            SQLiteDataAccess.SaveType(t);

            // Refresh ListBox
            RefreshTypesLB();
        }

        private void UpdateTypeButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (TypeName.Text == "") error = true;

            if (!error)
            {
                int index = TypesListLB.SelectedIndex;

                Types[index].TypeName = TypeName.Text;

                SQLiteDataAccess.UpdateType(Types[index]);

                // Refresh ListBox
                RefreshTypesLB();
            }
        }

        private void TypeSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = TypesListLB.SelectedIndex;

            if (index != -1)
            {
                TypesL.Content = "Types List (" + Types.Count.ToString() + "): " + index.ToString();

                TypeIdL.Content = Types[index].TypeId;

                TypeName.Text = Types[index].TypeName;

                AddType.IsEnabled = false;
                UpdateType.IsEnabled = true;
                DeleteType.IsEnabled = true;
            }
            else
            {
                AddType.IsEnabled = true;
                UpdateType.IsEnabled = false;
                DeleteType.IsEnabled = false;
            }
        }

        private void DeleteTypeButton(object sender, RoutedEventArgs e)
        {
            int index = TypesListLB.SelectedIndex;
            TypesL.Content = "Types List : " + index.ToString();

            SQLiteDataAccess.DeleteType(Types[index].TypeId);

            RefreshTypesLB();
        }

        private void ImportTypeButton(object sender, RoutedEventArgs e)
        {
            List<string> importTypes = new List<string>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importTypes = CSVFile.ReadImportFile1(fileName, "TypeName");

                foreach (var item in importTypes)
                {
                    if (FindType(item) == -1)
                    {
                        TypeModel t = new TypeModel();

                        t.TypeName = item;

                        SQLiteDataAccess.SaveType(t);
                    }
                }

                RefreshTypesLB();

                MessageBox.Show("Import of Types Completed Successfully", "Import of Types", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Import of Types Failed to open file", "Import of Types", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

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

                MessageBox.Show("Export of Types Completed Successfully", "Export of Types", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export of Types Failed to open file", "Export of Types", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            ManuName.Text = "";

            // reset the display field on the display
            ManusL.Content = "Manufacturers List (" + Manufacturers.Count.ToString() + "): ";

            ManuIdL.Content = "";
        }

        private void WireUpManuLB()
        {
            ManusListLB.Items.Clear();

            foreach (var m in Manufacturers)
            {
                ManusListLB.Items.Add(m.ManufacturerName);
            }
        }

        private void RefreshManusButton(object sender, RoutedEventArgs e)
        {
            RefreshManusLB();
        }

        private void AddManuButton(object sender, RoutedEventArgs e)
        {
            ManufacturerModel m = new ManufacturerModel();

            m.ManufacturerName = ManuName.Text;

            SQLiteDataAccess.SaveManufacturer(m);

            // Refresh ListBox
            RefreshManusLB();
        }

        private void UpdateManuButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (ManuName.Text == "") error = true;

            if (!error)
            {
                int index = ManusListLB.SelectedIndex;

                Manufacturers[index].ManufacturerName = ManuName.Text;

                SQLiteDataAccess.UpdateManufacturer(Manufacturers[index]);

                // Refresh ListBox
                RefreshManusLB();
            }
        }

        private void ManuSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = ManusListLB.SelectedIndex;

            if (index != -1)
            {
                ManusL.Content = ManusL.Content = "Manufacturers List (" + Manufacturers.Count.ToString() + "): " + index.ToString();

                ManuIdL.Content = Manufacturers[index].ManufacturerId;

                ManuName.Text = Manufacturers[index].ManufacturerName;

                AddManu.IsEnabled = false;
                UpdateManu.IsEnabled = true;
                DeleteManu.IsEnabled = true;
            }
            else
            {
                AddManu.IsEnabled = true;
                UpdateManu.IsEnabled = false;
                DeleteManu.IsEnabled = false;
            }
        }

        private void DeleteManuButton(object sender, RoutedEventArgs e)
        {
            int index = ManusListLB.SelectedIndex;
            ManusL.Content = "Manufacturers List : " + index.ToString();

            SQLiteDataAccess.DeleteManufacturer(Manufacturers[index].ManufacturerId);

            RefreshManusLB();
        }

        private void ImportManuButton(object sender, RoutedEventArgs e)
        {
            List<string> importManufacturers = new List<string>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importManufacturers = CSVFile.ReadImportFile1(fileName, "ManufacturerName");


                foreach (var item in importManufacturers)
                {
                    if (FindManu(item) == -1)
                    {
                        ManufacturerModel m = new ManufacturerModel();

                        m.ManufacturerName = item;

                        SQLiteDataAccess.SaveManufacturer(m);
                    }
                }

                RefreshManusLB();

                MessageBox.Show("Import of Manufacturers Completed Successfully", "Import of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Import of Manufacturers Failed to open file", "Import of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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

                MessageBox.Show("Export of Manufacturers Completed Successfully", "Export of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export of Manufacturers Failed to open file", "Export of Manufacturers", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            LocName.Text = "";

            // reset the display field on the display
            LocsL.Content = "Locations List(" + Locations.Count.ToString() + "): ";

            LocIdL.Content = "";
        }

        private void WireUpLocLB()
        {
            LocsListLB.Items.Clear();

            foreach (var l in Locations)
            {
                LocsListLB.Items.Add(l.LocationName);
            }
        }

        private void RefreshLocsButton(object sender, RoutedEventArgs e)
        {
            RefreshLocsLB();
        }

        private void AddLocButton(object sender, RoutedEventArgs e)
        {
            LocationModel l = new LocationModel();

            l.LocationName = LocName.Text;

            SQLiteDataAccess.SaveLocation(l);

            // Refresh ListBox
            RefreshLocsLB();
        }

        private void UpdateLocButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (LocName.Text == "") error = true;

            if (!error)
            {
                int index = LocsListLB.SelectedIndex;

                Locations[index].LocationName = LocName.Text;

                SQLiteDataAccess.UpdateLocation(Locations[index]);

                // Refresh ListBox
                RefreshLocsLB();
            }
        }

        private void LocSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = LocsListLB.SelectedIndex;

            if (index != -1)
            {
                LocsL.Content = "Locations List(" + Locations.Count.ToString() + "): " + index.ToString();

                LocIdL.Content = Locations[index].LocationId;

                LocName.Text = Locations[index].LocationName;

                AddLoc.IsEnabled = false;
                UpdateLoc.IsEnabled = true;
                DeleteLoc.IsEnabled = true;
            }
            else
            {
                AddLoc.IsEnabled = true;
                UpdateLoc.IsEnabled = false;
                DeleteLoc.IsEnabled = false;
            }
        }

        private void DeleteLocButton(object sender, RoutedEventArgs e)
        {
            int index = LocsListLB.SelectedIndex;
            LocsL.Content = "Location List : " + index.ToString();

            SQLiteDataAccess.DeleteLocation(Locations[index].LocationId);

            RefreshLocsLB();
        }

        private void ImportLocButton(object sender, RoutedEventArgs e)
        {
            List<string> importLocations = new List<string>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importLocations = CSVFile.ReadImportFile1(fileName, "LocationName");

                foreach (var item in importLocations)
                {
                    if (FindLocation(item) == -1)
                    {
                        LocationModel l = new LocationModel();

                        l.LocationName = item;

                        SQLiteDataAccess.SaveLocation(l);
                    }
                }

                RefreshLocsLB();

                MessageBox.Show("Import of Locations Completed Successfully", "Import of Locations", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Import of Locations Failed to open file", "Import of Locations", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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

                MessageBox.Show("Export of Locations Completed Successfully", "Export of Locations", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export of Locations Failed to open file", "Export of Locations", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        // ***      P O S I T I O N S
        private void RefreshPossLB()
        {
            // Get the Locations
            Positions = SQLiteDataAccess.LoadPositions();
            PosSubSet = SQLiteDataAccess.GetPositions(LocCB.SelectedIndex);

            // Only need to sort the "Displayed" version
            if (PosSubSet.Count > 0)
            {
                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));
            }

            // Display the Locations in the ComboBox
            LocCB.Items.Clear();
            WireUpLocCB();
            // Display the parts in the ListBox
            WireUpPosLB();

            // reset the add field on the display
            PosName.Text = "";

            // reset the display field on the display
            PossL.Content = "Positions List(" + PosSubSet.Count.ToString() + "): ";

            PosIdL.Content = "";
        }

        private void WireUpPosLB()
        {
            PossListLB.Items.Clear();

            foreach (var p in PosSubSet)
            {
                PossListLB.Items.Add(p.PositionName);
            }
        }

        private void WireUpLocCB()
        {
            LocCB.Items.Clear();

            foreach (var l in Locations)
            {
                LocCB.Items.Add(l.LocationName);
            }
        }

        private void RefreshPossButton(object sender, RoutedEventArgs e)
        {
            RefreshPossLB();
        }

        private void AddPosButton(object sender, RoutedEventArgs e)
        {
            PositionModel p = new PositionModel();

            p.LocationFK = FindLocation((string)LocCB.SelectedValue);

            p.PositionName = PosName.Text;

            SQLiteDataAccess.SavePosition(p);

            // Refresh ListBox
            RefreshPossLB();
        }

        private void UpdatePosButton(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (PosName.Text == "") error = true;
            if (LocCB.Text == "") error = true;

            if (!error)
            {
                int index = PossListLB.SelectedIndex;

                int selectedID = PosSubSet[index].PositionId;

                PosSubSet[index].PositionName = PosName.Text;

                SQLiteDataAccess.UpdatePosition(PosSubSet[index]);

                // Refresh ListBox
                RefreshPossLB();
            }
        }

        private void PosSelectedListBox(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = PossListLB.SelectedIndex;

            if (index != -1)
            {
                PossL.Content = "Positions List(" + PosSubSet.Count.ToString() + "): " + index.ToString();

                PosIdL.Content = PosSubSet[index].PositionId;

                PosName.Text = PosSubSet[index].PositionName;

                AddPos.IsEnabled = false;
                UpdatePos.IsEnabled = true;
                DeletePos.IsEnabled = true;
            }
            else
            {
                AddPos.IsEnabled = true;
                UpdatePos.IsEnabled = false;
                DeletePos.IsEnabled = false;
            }
        }

        private void DeletePosButton(object sender, RoutedEventArgs e)
        {
            int index = PossListLB.SelectedIndex;
            PossL.Content = "Position List : " + index.ToString();

            SQLiteDataAccess.DeletePosition(PosSubSet[index].PositionId);

            RefreshPossLB();
        }

        private void ImportPosButton(object sender, RoutedEventArgs e)
        {
            List<ColumnModel2> importPositions = new List<ColumnModel2>();

            string fileName = "";
            bool worked = CSVFile.GetImportFileName(out fileName);

            if (worked)
            {
                importPositions = CSVFile.ReadImportFile2(fileName, "PositionName");

                foreach (var item in importPositions)
                {
                    if (FindLocation(item.First) > -1 && FindPosition(FindLocation(item.First), item.Second) == -1)
                    {
                        PositionModel p = new PositionModel();

                        p.LocationFK = FindLocation(item.First);
                        p.PositionName = item.Second;

                        SQLiteDataAccess.SavePosition(p);
                    }
                }

                RefreshPossLB();

                MessageBox.Show("Import of Positions Completed Successfully", "Import of Positions", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Import of Positions Failed to open file", "Import of Positions", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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

                MessageBox.Show("Export of Positions Completed Successfully", "Export of Positions", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export of Positions Failed to open file", "Export of Positions", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void LocCB_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LocL.Content = "Location: " + LocCB.SelectedIndex.ToString();
            UpdateLocPoss();
        }
        // ************************************************
        // ************************************************
        private void UpdateLocationPositions()
        {
            int index = FindLocation((string)LocationCB.SelectedValue);

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
            int index = FindLocation((string)LocCB.SelectedValue);

            PosSubSet.Clear();

            if (index > 0)
            {
                PosSubSet = SQLiteDataAccess.GetPositions(index);

                PosSubSet.Sort((x, y) => x.PositionName.CompareTo(y.PositionName));

                PossL.Content = "Positions List(" + PosSubSet.Count.ToString() + "): ";
            }

            WireUpPosLB();
        }
        // ****

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HelpButton(object sender, RoutedEventArgs e)
        {
            new AboutHelp().Show();
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
    }
}