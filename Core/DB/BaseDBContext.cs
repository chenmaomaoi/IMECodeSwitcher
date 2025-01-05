using System.Data.Entity;
using Core.Extensions.Interfaces;
using Core.Models.DB;

namespace Core.DB;

[DbConfigurationType(typeof(SQLiteConfiguration))]
public class BaseDBContext : DbContext, ITransientService
{
    public BaseDBContext() : base("DBConnectStr")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        Database.SetInitializer(new BaseDBInitializer(modelBuilder));
    }

    //在此处添加实体
    public virtual DbSet<RulesModel> Rules { get; set; }
}
