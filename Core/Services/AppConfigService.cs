using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions.Interfaces;
using Core.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.IO;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Core.Event;
using Core.Services.Interfaces;

namespace Core.Services;
public class AppConfigService : ISingletonService, IAppConfigService
{
    public event OnConfigSavedEventHandler? OnConfigSaved;

    public AppConfigModel ConfigModel { get; set; }

    public AppConfigService()
    {
        Load();
    }

    public void Load()
    {
        if (File.Exists(Constant.ConfigFileName))
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            try
            {
                ConfigModel = deserializer.Deserialize<AppConfigModel>(File.ReadAllText(Constant.ConfigFileName));
                return;
            }
            catch (Exception)
            {
                File.Delete(Constant.ConfigFileName);
            }
        }

        ConfigModel = new AppConfigModel();
    }

    public AppConfigModel GetConfigs()
    {
        Load();
        return ConfigModel;
    }

    public void Save()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        var yaml = serializer.Serialize(ConfigModel);

        File.WriteAllTextAsync(Constant.ConfigFileName, yaml);

        OnConfigSaved?.Invoke(GetConfigs());
    }
}
