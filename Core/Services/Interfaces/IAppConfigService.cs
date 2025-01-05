using Core.Event;
using Core.Models;

namespace Core.Services.Interfaces;
public interface IAppConfigService
{
    AppConfigModel ConfigModel { get; set; }

    event OnConfigSavedEventHandler? OnConfigSaved;

    AppConfigModel GetConfigs();
    void Load();
    void Save();
}