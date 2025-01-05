using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Extensions.Interfaces;
using Core.Models;
using Core.Models.DB;

namespace UI.Models;
public partial class RulesM : ObservableObject, ITransientService
{
    [ObservableProperty]
    private List<RulesModel> rules;

    [ObservableProperty]
    private bool deleteUnlockRules;
}
