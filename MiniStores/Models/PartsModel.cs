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
    }
}