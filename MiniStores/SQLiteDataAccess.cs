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
        // ***
        /// <summary>
        /// Retrives the connection string from the Config Manager
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string = connection name</returns>
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        // ************************************************
        // ************************************************
        /// <summary>
        /// Loads all Parts
        /// </summary>
        /// <returns>Returns a List of the Parts</returns>
        public static List<PartsModel> LoadParts()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>("select * from Parts", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Updates a Part in the DB
        /// </summary>
        /// <param name="part">Part to update</param>
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
                    ", Price = " + part.Price + " " +
                    ", Comment = \"" + part.Comment + "\" " +
                    "where PartsId = " + part.PartsId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Saves a Part to the DB
        /// </summary>
        /// <param name="part">Part to save</param>
        public static void SavePart(PartsModel part)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Parts (PartName, TypeFK, Quantity, ManufacturerFK, LocationFK, PositionFK, Price, Comment) " +
                    "values (@PartName, @TypeFK, @Quantity, @ManufacturerFK, @LocationFK, @PositionFK, @Price, @Comment)", part);
            }
        }
        // ************************************************
        /// <summary>
        /// Fetch's a Part from the DB
        /// </summary>
        /// <param name="id">Part ID to Fetch</param>
        /// <returns>Part as a List</returns>
        public static List<PartsModel> GetPart(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>("select * from Parts where PartsId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes a Part from the DB
        /// </summary>
        /// <param name="id">Part ID to delete</param>
        public static void DeletePart(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>("delete from Parts where PartsId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Search's the DB
        /// </summary>
        /// <param name="SearchQurey">Query for the search</param>
        /// <returns>List of the Parts found from the Query</returns>
        public static List<PartsModel> SearchParts(string SearchQurey)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PartsModel>(SearchQurey, new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        // ************************************************
        /// <summary>
        /// Load All Type's from the DB
        /// </summary>
        /// <returns>List of the Type's</returns>
        public static List<TypeModel> LoadTypes()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TypeModel>("select * from Type", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Update a Type in the DB
        /// </summary>
        /// <param name="type">Type to update</param>
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
        /// <summary>
        /// Save a Type to the DB
        /// </summary>
        /// <param name="type">Type to Save</param>
        public static void SaveType(TypeModel type)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Type (TypeName) values (@TypeName)", type);
            }
        }
        // ************************************************
        /// <summary>
        /// Fetch a Type from the DB
        /// </summary>
        /// <param name="id">Type ID to fetch</param>
        /// <returns>List of the Type</returns>
        public static List<TypeModel> GetType(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TypeModel>("select * from Type where TypeId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Delete a Type from the DB
        /// </summary>
        /// <param name="id">Type ID to delete</param>
        public static void DeleteType(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TypeModel>("delete from Type where TypeId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        /// <summary>
        /// Load All the Manufacturer's from the DB
        /// </summary>
        /// <returns>List of Manufacturer</returns>
        public static List<ManufacturerModel> LoadManufacturers()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ManufacturerModel>("select * from Manufacturer", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Update a Manufacturer in the DB
        /// </summary>
        /// <param name="manu">Manufacturer to Update</param>
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
        /// <summary>
        /// Save a Manufacturer to the DB
        /// </summary>
        /// <param name="manu">Manufacturer to save</param>
        public static void SaveManufacturer(ManufacturerModel manu)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Manufacturer (ManufacturerName) values (@ManufacturerName)", manu);
            }
        }
        // ************************************************
        /// <summary>
        /// Fetch a Manufacturer from the DB
        /// </summary>
        /// <param name="id">Manufacturer ID to fetch</param>
        /// <returns>List of Manufacturer</returns>
        public static List<ManufacturerModel> GetManufacturer(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ManufacturerModel>("select * from Manufacturer where ManufacturerId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Delete a Manufacturer from the db
        /// </summary>
        /// <param name="id">Manufacturer ID to delete</param>
        public static void DeleteManufacturer(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ManufacturerModel>("delete from Manufacturer where ManufacturerId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        /// <summary>
        /// Load All Location's from the DB
        /// </summary>
        /// <returns>List of Location's</returns>
        public static List<LocationModel> LoadLocations()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LocationModel>("select * from Location", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Update a Location in the DB
        /// </summary>
        /// <param name="loc">Location to update</param>
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
        /// <summary>
        /// Save a Location to DB
        /// </summary>
        /// <param name="loc">Location to save</param>
        public static void SaveLocation(LocationModel loc)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Location (LocationName) values (@LocationName)", loc);
            }
        }
        // ************************************************
        /// <summary>
        /// Fetch a Location from the DB
        /// </summary>
        /// <param name="id">Location Id to fetch</param>
        /// <returns>List of Location's</returns>
        public static List<LocationModel> GetLocation(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LocationModel>("select * from Location where LocationId = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Delete a Location from the DB
        /// </summary>
        /// <param name="id">Location ID to delete</param>
        public static void DeleteLocation(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LocationModel>("delete from Location where LocationId = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        // ************************************************
        /// <summary>
        /// Load All Position's in the DB
        /// </summary>
        /// <returns>List of Position's</returns>
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
        /// <summary>
        /// Update a Position in the DB
        /// </summary>
        /// <param name="pos">Position to update</param>
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
        /// <summary>
        /// Save a Position to the DB
        /// </summary>
        /// <param name="pos">Position to save</param>
        public static void SavePosition(PositionModel pos)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Position (LocationFK, PositionName) values (@LocationFK, @PositionName)", pos);
            }
        }
        // ************************************************
        /// <summary>
        /// Load a Set of Position based on the Location from the DB
        /// </summary>
        /// <param name="location">Location where the Position's are connected</param>
        /// <returns>List of Position's</returns>
        public static List<PositionModel> GetPositions(int location)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PositionModel>("select * from Position where LocationFK = " + location.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Delete a Position from the DB
        /// </summary>
        /// <param name="id">Position ID to delete</param>
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
