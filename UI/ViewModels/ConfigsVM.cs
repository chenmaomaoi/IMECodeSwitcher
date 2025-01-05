using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Extensions.Interfaces;
using Core.Services;
using MaterialDesignThemes.Wpf;
using UI.Models;
using UI.Views;

namespace UI.ViewModels;
public partial class ConfigsVM : ConfigsM, ITransientService
{
    private AppConfigService configService;

    public ConfigsVM(AppConfigService configService)
    {
        this.configService = configService;
        this.IsStartUp = Core.Common.Helper.ShortcutUtilities.IsStartup();
        this.RefreshDelay = configService.GetConfigs().RefreshDelay;
    }

    [RelayCommand]
    public void Save()
    {
        configService.ConfigModel.RefreshDelay = this.RefreshDelay;

        configService.Save();

        if (this.IsStartUp)
        {
            Core.Common.Helper.ShortcutUtilities.SetStartup();
        }
        else
        {
            Core.Common.Helper.ShortcutUtilities.UnSetStartup();
        }

        MessageDialog messageDialog = new() { Message = { Text = "成功！" } };
        DialogHost.Show(messageDialog, "RootDialog");
    }
}
