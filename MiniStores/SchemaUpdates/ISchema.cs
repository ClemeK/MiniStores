namespace MiniStores.SchemaUpdates
{
    public interface ISchema
    {
        static int ThisUpdate { get; }

        void UpdateSchema(int currentVersion);
    }
}
