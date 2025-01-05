using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Common;
using Core.DB;
using Core.Extensions.Interfaces;
using Core.Models.DB;

namespace Core.Services;
public class RulesService : ISingletonService
{
    private UnitWork UnitWork;

    public RulesService(UnitWork unitWork)
    {
        this.UnitWork = unitWork;
    }

    public IQueryable<RulesModel> Finds(Expression<Func<RulesModel, bool>> exp = null)
    {
        return UnitWork.Finds(exp);
    }

    public bool IsExist(Expression<Func<RulesModel, bool>> exp)
    {
        return UnitWork.IsExist(exp);
    }

    public RulesModel FirstOrDefault(Expression<Func<RulesModel, bool>> exp)
    {
        return UnitWork.FirstOrDefault(exp);
    }

    public RulesModel GetDefault()
    {
        return FirstOrDefault(p => p.Id == 1);
    }

    public void AddAndUpdate(RulesModel rulesModel)
    {
        if (string.IsNullOrWhiteSpace(rulesModel.ProgressName))
        {
            return;
        }

        if (rulesModel.Id == 0)
        {
            UnitWork.Add(rulesModel);
        }
        else
        {
            UnitWork.Update(rulesModel);
        }

        UnitWork.Save();
    }

    public void Delete(Expression<Func<RulesModel, bool>> exp)
    {
        UnitWork.Delete(exp);
        UnitWork.Save();
    }

    public void Delete(RulesModel rulesModel)
    {
        UnitWork.Delete(rulesModel);
        UnitWork.Save();
    }
}
