namespace MiniStores.Models
{
    internal class PositionModel
    {
        public int PositionId { get; set; }
        public int LocationFK { get; set; }
        public string PositionName { get; set; }


        public override string ToString()
        {
            string output = "ID: " + PositionId
                + " Position: " + PositionName
                + " at Location: " + LocationFK;

            return output;
        }
    }
}