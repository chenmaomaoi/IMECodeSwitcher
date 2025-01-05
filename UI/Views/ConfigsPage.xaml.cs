﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core.Extensions.Interfaces;
using UI.ViewModels;

namespace UI.Views;
/// <summary>
/// SettingsPage.xaml 的交互逻辑
/// </summary>
public partial class ConfigsPage : Page, ITransientService
{
    public ConfigsPage(ConfigsVM configsVM)
    {
        InitializeComponent();
        this.DataContext = configsVM;
    }
}