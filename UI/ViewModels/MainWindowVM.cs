using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Extensions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using UI.Models;
using UI.Views;

namespace UI.ViewModels;
public partial class MainWindowVM : MainWindowM, ITransientService
{
    public MainWindowVM(IServiceProvider provider)
    {
        PageList = provider.GetServices<Page>().ToList();
        if (PageList.Count > 0)
        {
            SelectedPage = PageList[0];
        }
    }
}
