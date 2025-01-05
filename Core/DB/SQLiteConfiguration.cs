using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace Core.DB;

public class SQLiteConfiguration : DbConfiguration, IDbConnectionFactory
{
    public SQLiteConfiguration()
    {
        SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
        SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

        var providerServices = (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices));

        SetProviderServices("System.Data.SQLite", providerServices);
        SetProviderServices("System.Data.SQLite.EF6", providerServices);

        SetDefaultConnectionFactory(this);
    }

    public DbConnection CreateConnection(string connectionString)
        => new SQLiteConnection(connectionString);
}
