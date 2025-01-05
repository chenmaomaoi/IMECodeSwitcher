using System;

namespace Core.Models;

[Serializable]
public class AppConfigModel
{
    /// <summary>
    /// 在程序启动时，删除所有没被锁定的
    /// </summary>
    public bool DeleteUnlockRules { get; set; } = true;

    /// <summary>
    /// 刷新IMECode延迟(ms)
    /// </summary>
    public int RefreshDelay { get; set; } = 2;
}
