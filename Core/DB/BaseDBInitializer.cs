using System.Data.Entity;
using Core.Enums;
using Core.Models.DB;
using SQLite.CodeFirst;

namespace Core.DB;

public class BaseDBInitializer : SqliteDropCreateDatabaseWhenModelChanges<BaseDBContext>
{
    public BaseDBInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
    {
    }

    protected override void Seed(BaseDBContext context)
    {
        RulesModel defaultRule = new()
        {
            Id = 1,
            ProgressName = "默认",
            MonitIMECodeChanges = true,
            IMECode = IMECode.Native,
            Lock = true
        };
        context.Set<RulesModel>().Add(defaultRule);
    }
}