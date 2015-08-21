using System.Data.Entity.SqlServer;

namespace JCModel
{
    internal static class MissingDllHack
    {
        private static SqlProviderServices instance = SqlProviderServices.Instance;
    }
}