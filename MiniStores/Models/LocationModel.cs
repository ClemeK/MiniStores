namespace MiniStores.Models
{
    internal class LocationModel
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }


        public override string ToString()
        {
            string output = "ID: " + LocationId + " Location: " + LocationName;

            return output;
        }

    }
}