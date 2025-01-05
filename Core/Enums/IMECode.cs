namespace Core.Enums;

public enum IMECode
{
    /// <summary>
    /// 什么都不做
    /// </summary>
    Ignore = -1,

    /// <summary>
    /// 字母数字模式
    /// </summary>
    AlphaNumeric = 0x00,

    /// <summary>
    /// 本地语言输入
    /// </summary>
    Native = 0x01
}