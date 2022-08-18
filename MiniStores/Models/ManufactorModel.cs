namespace MiniStores.Models
{
    internal class ManufacturerModel
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }


        public override string ToString()
        {
            string output = "ID: " + ManufacturerId + " Manufacturer: " + ManufacturerName;

            return output;
        }

    }
}
