using MiniStores.Models;
using System;

namespace MiniStores.SchemaUpdates
{
    internal class DBSchema3 : ISchema
    {
        public int ThisUpdate = 3;

        public void UpdateSchema(int currentVersion)
        {
            if (ThisUpdate > currentVersion)
            {
                // ***** Demo Code *****

                //string cmd = "CREATE TABLE Persons " +
                //                "(PersonID int," +
                //                "LastName varchar(255)," +
                //                "FirstName varchar(255)," +
                //                "Address varchar(255)," +
                //                "City varchar(255));";

                //SQLiteDataAccess.UpdateSchema(cmd);

                //AddSchemaRecord();
            }
        }

        public void AddSchemaRecord()
        {
            // update Schema File
            DateTime now = DateTime.Now;
            SchemaModel model = new SchemaModel() { SVersion = ThisUpdate, SDateTime = now.ToString() };
            SQLiteDataAccess.SaveSchemaVersion(model);
        }

    }
}
