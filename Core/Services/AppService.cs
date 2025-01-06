using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Common;
using Core.Common.Helper;
using Core.DB;
using Core.Enums;
using Core.Extensions.Interfaces;
using Core.Models.DB;

namespace Core.Services;
public class AppService : ISingletonService
{
    private RulesService rulesService;
    private AppConfigService appConfigService;
    private IMECodeMonitorService codeMonitorService;
    private ForegroundProgramMonitorService programMonitorService;

    public AppService(RulesService rulesService, AppConfigService appConfigService, IMECodeMonitorService codeMonitorService, ForegroundProgramMonitorService programMonitorService)
    {
        this.rulesService = rulesService;
        this.appConfigService = appConfigService;
        this.codeMonitorService = codeMonitorService;
        this.programMonitorService = programMonitorService;

        this.appConfigService.OnConfigSaved += AppConfigService_OnConfigSaved;
        this.codeMonitorService.OnProgramIMECodeChanged += CodeMonitorService_OnProgramIMECodeChanged;
        this.programMonitorService.OnForegroundProgramChanged += ProgramMonitorService_OnForegroundProgramChanged;

        this.codeMonitorService.Start();
        this.programMonitorService.Start();

        //删除没被锁定的规则
        if (this.appConfigService.ConfigModel.DeleteUnlockRules)
        {
            this.rulesService.DeleteUnlockRules();
        }
    }

    private void AppConfigService_OnConfigSaved(Models.AppConfigModel config)
    {
        this.codeMonitorService.ResetAndUpdateInterval();
    }

    private void CodeMonitorService_OnProgramIMECodeChanged(string programName, IMECode imeCode)
    {
        if (rulesService.IsExist(p => p.ProgressName == programName))
        {
            var record = rulesService.FirstOrDefault(p => p.ProgressName == programName);
            //库里有
            if (record.MonitIMECodeChanges && record.IMECode != imeCode)
            {
                //与存的不符，改库
                record.IMECode = imeCode;
                rulesService.AddAndUpdate(record);
            }
        }
        else
        {
            //库里没有
            //看默认设置
            var defaultRule = rulesService.GetDefault();
            if (defaultRule.MonitIMECodeChanges)
            {
                //按默认设置新增
                RulesModel record = new()
                {
                    ProgressName = programName,
                    MonitIMECodeChanges = defaultRule.MonitIMECodeChanges,
                    IMECode = imeCode,
                    Lock = defaultRule.Lock
                };

                rulesService.AddAndUpdate(record);
            }
        }
    }

    private void ProgramMonitorService_OnForegroundProgramChanged(Win32Window window, string programName)
    {
        var record = rulesService.FirstOrDefault(p => p.ProgressName == programName);

        if (record != default && record.IMECode != IMECode.Ignore)
        {
            Win32Helper.SetIMECode(window.IMEHandle, record.IMECode);
        }
    }
}
