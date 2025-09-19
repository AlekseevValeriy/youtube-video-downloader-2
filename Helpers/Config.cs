using Newtonsoft.Json.Linq;

namespace Cherub.Helpers;

public static class Config
{
    public static void LoadConfig()
    {
        JObject ConfigData = JObject.Parse(File.ReadAllText(AppPaths.ConfigFilePath));

        String? field;

        field = GetParameter<String>(ConfigData, nameof(AppSettings.DownloadPath), (p) => p != "");
        if (field is not null)
        {
            AppSettings.DownloadPath = field;
        }

        field = GetParameter<String>(ConfigData, nameof(AppSettings.DownloadToolPath), (p) => p != "");
        if (field is not null)
        {
            AppSettings.DownloadToolPath = field;
        }

        field = GetParameter<String>(ConfigData, nameof(AppSettings.ConvertToolPath), (p) => p != "");
        if (field is not null)
        {
            AppSettings.ConvertToolPath = field;
        }
    }

    public static void Save()
    {
        JObject ConfigData = JObject.Parse(File.ReadAllText(AppPaths.ConfigFilePath));

        ConfigData[nameof(AppSettings.DownloadPath)] = AppSettings.DownloadPath;
        ConfigData[nameof(AppSettings.DownloadToolPath)] = AppSettings.DownloadToolPath;
        ConfigData[nameof(AppSettings.ConvertToolPath)] = AppSettings.ConvertToolPath;

        File.WriteAllText(AppPaths.ConfigFilePath, ConfigData.ToString());
    }

    public static T? GetParameter<T>(String name, Predicate<T>? condition = null)
    {
        JObject ConfigData = JObject.Parse(File.ReadAllText(AppPaths.ConfigFilePath));

        if (ConfigData.ContainsKey(name))
        {
            T parameter = ConfigData[name]!.Value<T>()!;
            if (condition is not null && condition(parameter))
            {
                return parameter;
            }
        }
        return default(T);
    }

    public static T? GetParameter<T>(JObject configData, String name, Predicate<T>? condition = null)
    {
        if (configData.ContainsKey(name))
        {
            T parameter = configData[name]!.Value<T>()!;
            if (condition is not null && condition(parameter))
            {
                return parameter;
            }
        }
        return default(T);
    }

    public static void SaveParameter<T>(String name, T parameter)
    {
        JObject ConfigData = JObject.Parse(File.ReadAllText(AppPaths.ConfigFilePath));

        ConfigData[name] = parameter?.ToString() ?? "";

        File.WriteAllText(AppPaths.ConfigFilePath, ConfigData.ToString());
    }

    public static void SaveParameter<T>(JObject configData, String name, T parameter)
    {
        configData[name] = parameter?.ToString() ?? "";
    }
}
