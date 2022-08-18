using System;
using System.Diagnostics;
using System.IO;

namespace MiniStores
{
    public class Joblog
    {
        // Private Properties
        private DateTime currentTime = new DateTime();
        private string fileName;
        private string fName;
        // Public Properties

        // Constructor
        public Joblog(string FileName, int daysToKeep)
        {
            // create a file name containing the date and time
            fName = FileName;
            currentTime = DateTime.Now;

            DeleteOldLogs(daysToKeep);

            CreateFileName(fName, currentTime);
        }
        //*********************************
        public void InformationMessage(string header = "", string detail = "")
        {
            AddMessage("Information", header, detail);
        }
        //*********************************
        public void WarningMessage(string header = "", string detail = "")
        {
            AddMessage("Warning", header, detail);
        }
        //*********************************
        public void ErrorMessage(string header = "", string detail = "")
        {
            AddMessage("Error", header, detail);
        }
        //*********************************
        //*********************************
        //*********************************
        private void CreateFileName(string f, DateTime dt)
        {
            // create a file name containing the date and time
            fileName = f + "-" + dt.ToString("yyyyMMddHHmm") + ".Log";
        }
        //*********************************
        private void AddMessage(string errType, string header, string detail)
        {
            // find out the current time
            currentTime = DateTime.Now;

            // add the current time to the log entry types and a
            string textHeader = "";
            string textDetail = "";

            // if there is a header entry add that to the log
            if (header != "")
            {
                textHeader = currentTime.ToLongTimeString() + " [" + errType + "] - ";
                textHeader += header;
            }

            // if there is a detail entry add that to the log
            if (detail != "")
            {
                textDetail = currentTime.ToLongTimeString() + " -- ";
                textDetail += detail;
            }

            PrintLogEntry(textHeader, textDetail);
        }
        //*********************************
        private void PrintLogEntry(string header, string detail)
        {
            // Print the log to a text file
            using (StreamWriter outputFile = new StreamWriter(fileName, append: true))
            {
                if (header != "")
                {
                    outputFile.WriteLine(header);
                }

                if (detail != "")
                {
                    outputFile.WriteLine(detail);
                }
            }
        }
        //*********************************
        private void DeleteOldLogs(int daysToKeep)
        {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string filter = "*.log";

            string[] files = Directory.GetFiles(folder, filter);

            DateTime keepDate = DateTime.Today.AddDays(-daysToKeep);

            foreach (var file in files)
            {
                string bit = file.Substring(file.Length - 16, 8);

                int Year = int.Parse(bit.Substring(0, 4));
                int Month = int.Parse(bit.Substring(4, 2));
                int Day = int.Parse(bit.Substring(6, 2));

                DateTime fileDate = new DateTime(Year, Month, Day, 0, 0, 0);

                if (fileDate < keepDate)
                {
                    File.Delete(file);
                    //Console.WriteLine($"File: {file} (DELETED)");
                }
            }
        }
    }
}