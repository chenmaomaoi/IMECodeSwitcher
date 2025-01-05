using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Extensions.Interfaces;

namespace UI.Models;
public partial class ConfigsM : ObservableObject, ITransientService
{
    [ObservableProperty]
    private bool isStartUp;

    [ObservableProperty]
    private int refreshDelay;

}
