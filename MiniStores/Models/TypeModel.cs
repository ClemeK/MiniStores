namespace MiniStores.Models
{
    internal class TypeModel
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public override string ToString()
        {
            string output = "ID: " + TypeId + " Type: " + TypeName;

            return output;
        }
    }
}
