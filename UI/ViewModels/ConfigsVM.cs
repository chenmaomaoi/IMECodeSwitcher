using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Extensions.Interfaces;
using Core.Services;
using UI.Models;

namespace UI.ViewModels;
public partial class ConfigsVM : ConfigsM, ITransientService
{
    public ConfigsVM(AppConfigService configService)
    {
        this.IsStartUp = Core.Common.Helper.ShortcutUtilities.IsStartup();
        this.RefreshDelay = configService.GetConfigs().RefreshDelay;
    }
}
