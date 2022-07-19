using Microsoft.Win32;
using MiniStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MiniStores
{
    internal class CSVFile
    {
        public static bool GetImportFileName(out string filename)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                return true;
            }
            else
            {
                filename = "";
                return false;
            }
        }

        public static bool GetExportFileName(out string filename)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text file (*.csv)|*.csv|All file (*.*)|*.*";

            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == true)
            {
                filename = saveFileDialog.FileName;
                return true;
            }
            else
            {
                filename = "";
                return false;
            }
        }

        public static List<string> ReadImportFile1(string path, string header)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            int freq = lines[0].Count(f => (f == ','));

            List<string> data = new List<string> { };

            // read through the Lines of the file 
            for (int j = 0; j < lines.Length; j++)
            {
                string[] columns = lines[j].Split(',');

                columns[0] = columns[0].Trim();

                if (columns[0] != header)
                {
                    data.Add(columns[0]);
                }
            }

            return data;
        }

        public static List<ColumnModel2> ReadImportFile2(string path, string header)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            int freq = lines[0].Count(f => (f == ','));

            List<ColumnModel2> data = new List<ColumnModel2> { };

            // read through the Lines of the file 
            for (int j = 0; j < lines.Length; j++)
            {
                string[] columns = lines[j].Split(',');

                if (columns[1] != header)
                {
                    ColumnModel2 c = new ColumnModel2();
                    c.First = columns[0].Trim();  // Location
                    c.Second = columns[1].Trim(); // Position

                    data.Add(c);
                }
            }

            return data;
        }

        public static List<ColumnModel6> ReadImportFile6(string path, string header)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            int freq = lines[0].Count(f => (f == ','));

            List<ColumnModel6> data = new List<ColumnModel6> { };

            // read through the Lines of the file 
            for (int j = 0; j < lines.Length; j++)
            {
                string[] columns = lines[j].Split(',');

                if (columns[0] != header)
                {
                    ColumnModel6 c = new ColumnModel6();
                    c.First = columns[0].Trim();  // Part Name
                    c.Second = columns[1].Trim(); // Type
                    c.Thrid = columns[2].Trim();  // Quantity
                    c.Fourth = columns[3].Trim(); // Manufacturer
                    c.Fifth = columns[4].Trim();  // Location
                    c.Sixth = columns[5].Trim();  // Position

                    data.Add(c);
                }
            }

            return data;
        }

    }
}
