using Dapper;
using MiniStores.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace MiniStores
{
    internal class SQLiteDataAccess
    {
        // ************************************************
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        // ************************************************
        // ************************************************
        public static List<PartsModel> LoadParts()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>("select * from Parts", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        public static void UpdatePart(PartsModel part)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Parts set" +
                    " PartName = \"" + part.PartName + "\" " +
                    ", TypeFK = " + part.TypeFK.ToString() + " " +
                    ", Quantity = " + part.Quantity.ToString() + " " +
                    ", ManufacturerFK = " + part.ManufacturerFK.ToString() + " " +
                    ", LocationFK = " + part.LocationFK.ToString() + " " +
                    ", PositionFK = " + part.PositionFK.ToString() + " " +
                    "where PartsId = " + part.PartsId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        public static void SavePart(PartsModel part)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Parts (PartName, TypeFK, Quantity, ManufacturerFK, LocationFK, PositionFK) " +
                    "values (@PartName, @TypeFK, @Quantity, @ManufacturerFK, @LocationFK, @PositionFK)", part);
            }
        }
        // ************************************************
        public static List<PartsModel> GetPart(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>("select * from Parts where PartId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        public static void DeletePart(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>("delete from Parts where PartsId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        public static List<TypeModel> LoadTypes()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TypeModel>("select * from Type", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        public static void UpdateType(TypeModel type)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Type set" +
                    " TypeName = \"" + type.TypeName + "\" " +
                    "where TypeId = " + type.TypeId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        public static void SaveType(TypeModel type)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Type (TypeName) values (@TypeName)", type);
            }
        }
        // ************************************************
        public static List<TypeModel> GetType(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TypeModel>("select * from Type where TypeId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        public static void DeleteType(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TypeModel>("delete from Type where TypeId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        public static List<ManufacturerModel> LoadManufacturers()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ManufacturerModel>("select * from Manufacturer", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        public static void UpdateManufacturer(ManufacturerModel manu)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Manufacturer set" +
                    " ManufacturerName = \"" + manu.ManufacturerName + "\" " +
                    "where ManufacturerId = " + manu.ManufacturerId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        public static void SaveManufacturer(ManufacturerModel manu)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Manufacturer (ManufacturerName) values (@ManufacturerName)", manu);
            }
        }
        // ************************************************
        public static List<ManufacturerModel> GetManufacturer(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ManufacturerModel>("select * from Manufacturer where ManufacturerId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        public static void DeleteManufacturer(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ManufacturerModel>("delete from Manufacturer where ManufacturerId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        public static List<LocationModel> LoadLocations()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LocationModel>("select * from Location", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        public static void UpdateLocation(LocationModel loc)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Location set" +
                    " LocationName = \"" + loc.LocationName + "\" " +
                    "where LocationId = " + loc.LocationId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        public static void SaveLocation(LocationModel loc)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Location (LocationName) values (@LocationName)", loc);
            }
        }
        // ************************************************
        public static List<LocationModel> GetLocation(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LocationModel>("select * from Location where LocationId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        public static void DeleteLocation(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LocationModel>("delete from Location where LocationId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        public static List<PositionModel> LoadPositions()
        {
            // Not currently used, but here for completeness
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PositionModel>("select * from Position", new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        public static void UpdatePosition(PositionModel pos)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Position set" +
                    " PositionName = \"" + pos.PositionName + "\" " +
                    "where PositionId = " + pos.PositionId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        public static void SavePosition(PositionModel pos)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Position (LocationFK, PositionName) values (@LocationFK, @PositionName)", pos);
            }
        }
        // ************************************************
        public static List<PositionModel> GetPositions(int location)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PositionModel>("select * from Position where LocationFK = " + location.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        public static void DeletePosition(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PositionModel>("delete from Position where PositionId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
    }
}
