using MiniStores.Models;
using System;

namespace MiniStores.SchemaUpdates
{
    public class DBSchema2 : ISchema
    {
        private int ThisUpdate = 2;

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

        private void AddSchemaRecord()
        {
            // update Schema File
            DateTime now = DateTime.Now;
            SchemaModel model = new SchemaModel() { SVersion = ThisUpdate, SDateTime = now.ToString() };
            SQLiteDataAccess.SaveSchemaVersion(model);
        }

    }
}
