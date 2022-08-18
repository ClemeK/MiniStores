namespace MiniStores.Models
{
    internal class PartsModel
    {
        public int PartsId { get; set; }
        public string PartName { get; set; }
        public int TypeFK { get; set; }
        public int Quantity { get; set; }
        public int ManufacturerFK { get; set; }
        public int LocationFK { get; set; }
        public int PositionFK { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }


        public override string ToString()
        {
            string output = " ID: " + PartsId
                + " Part: " + PartName
                + " Type:" + TypeFK
                + " Quantity:" + Quantity
                + " Manufacturer:" + ManufacturerFK
                + " Location:" + LocationFK
                + " Position:" + PositionFK;

            return output;
        }
    }
}