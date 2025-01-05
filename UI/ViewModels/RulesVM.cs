using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Helper;
using Core.DB;
using Core.Extensions.Interfaces;
using Core.Models.DB;
using Core.Services;
using MaterialDesignThemes.Wpf;
using UI.Models;
using UI.Views;

namespace UI.ViewModels;
public partial class RulesVM : RulesM, ITransientService
{
    private RulesService rulesService;
    private AppConfigService configService;

    public RulesVM(RulesService rulesService, AppConfigService configService)
    {
        this.rulesService = rulesService;
        this.configService = configService;

        this.DeleteUnlockRules = this.configService.GetConfigs().DeleteUnlockRules;

        Rules = rulesService.Finds().OrderBy(p => p.Id).ThenBy(p => p.Lock).ToList();
    }

    [RelayCommand]
    private void Remove(DataGrid dataGrid)
    {
        if (((RulesModel)dataGrid.SelectedItem).Id == 1)
        {
            MessageDialog messageDialog = new() { Message = { Text = "默认规则不允许删除！" } };
            DialogHost.Show(messageDialog, "RootDialog");

            return;
        }

        Rules.Remove((RulesModel)dataGrid.SelectedItem);
        Application.Current.Dispatcher.Invoke(dataGrid.Items.Refresh);
    }

    [RelayCommand]
    private void Add(DataGrid dataGrid)
    {
        Rules.Add(new RulesModel());
        Application.Current.Dispatcher.Invoke(dataGrid.Items.Refresh);
    }

    [RelayCommand]
    private void Refresh(DataGrid dataGrid)
    {
        Rules = rulesService.Finds().OrderBy(p => p.Id).ThenBy(p => p.Lock).ToList();
        Application.Current.Dispatcher.Invoke(dataGrid.Items.Refresh);
    }

    [RelayCommand]
    private void Save(DataGrid dataGrid)
    {
        configService.ConfigModel.DeleteUnlockRules = this.DeleteUnlockRules;
        configService.Save();

        var db = rulesService.Finds();

        foreach (RulesModel item in db)
        {
            if (!Rules.Any(p => p.Id == item.Id))
            {
                rulesService.Delete(item);
            }
        }

        foreach (RulesModel item in Rules)
        {
            rulesService.AddAndUpdate(item);
        }

        //提示信息
        MessageDialog messageDialog = new() { Message = { Text ="成功！" } };
        DialogHost.Show(messageDialog, "RootDialog");

        Refresh(dataGrid);
    }
}
