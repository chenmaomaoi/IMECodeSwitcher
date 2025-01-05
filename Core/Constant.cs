using System;
using System.IO;

namespace Core;
public static class Constant
{
    public const string Name = "输入法切换器";

    public static readonly string ConfigFileName = Path.Combine(Environment.CurrentDirectory, "Configs.yaml");
}
