using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Extensions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using UI.Views;

namespace UI.Models;

public partial class MainWindowM : ObservableObject, ITransientService
{
    [ObservableProperty]
    private List<Page> pageList;

    [ObservableProperty]
    private Page selectedPage;
}
