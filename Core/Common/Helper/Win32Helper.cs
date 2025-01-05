using Core.Enums;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace Core.Common.Helper;

public static class Win32Helper
{
    #region 窗口
    /// <summary>
    /// 获取前台窗口信息
    /// </summary>
    /// <returns></returns>
    public static Win32Window GetForegroundWindowInfo()
    {
        var current = PInvoke.GetForegroundWindow();
        var w = new Win32Window(current);
        return w;
    }
    #endregion

    #region 输入法

    /// <summary>
    /// 获取大小写锁定状态
    /// </summary>
    /// <param name="hWND"></param>
    /// <returns></returns>
    internal static bool GetCapsLockState()
    {
        return (PInvoke.GetKeyState(VK_CAPITAL) & 0x0001) == 1;
    }

    /// <summary>
    /// 设置IMECode
    /// </summary>
    /// <param name="imeHWND"></param>
    /// <param name="imeCode"></param>
    internal static void SetIMECode(HWND imeHWND, IMECode imeCode)
    {
        if (imeHWND.IsNull || imeHWND.Value == 0) return;
        PInvoke.SendMessage(imeHWND, PInvoke.WM_IME_CONTROL, PInvoke.IMC_SETCONVERSIONMODE, (int)imeCode);
    }

    /// <summary>
    /// 获取IMECode
    /// </summary>
    /// <param name="imeHWND"></param>
    /// <returns></returns>
    internal static IMECode GetIMECode(HWND imeHWND)
    {
        var mode = GetIMEConversionMode(imeHWND);
        if ((mode & (long)IMECode.Native) != 0)
            return IMECode.Native;

        //todo 其余IMECode处理
        return IMECode.AlphaNumeric;
    }

    /// <summary>
    /// 获取输入法转换模式
    /// </summary>
    /// <param name="imeHWND"></param>
    /// <returns></returns>
    private static LRESULT GetIMEConversionMode(HWND imeHWND)
    {
        if (imeHWND.IsNull || imeHWND.Value == 0) return (LRESULT)0;
        return PInvoke.SendMessage(imeHWND, PInvoke.WM_IME_CONTROL, IMC_GETCONVERSIONMODE, nint.Zero);
    }

    /// <summary>
    /// 获取窗口 IME句柄
    /// </summary>
    /// <param name="windowHWND"></param>
    /// <returns></returns>
    internal static HWND GetIMEHWND(HWND windowHWND)
    {
        var hWND = PInvoke.ImmGetDefaultIMEWnd(windowHWND);
        if (hWND == nint.Zero) return HWND.Null;
        return hWND;
    }

    /// <summary>
    /// 大写锁定键
    /// </summary>
    internal const int VK_CAPITAL = 0x14;

    /// <summary>
    /// 输入法管理器命令
    /// </summary>
    internal const int IMC_GETCONVERSIONMODE = 0x001;

    #endregion
}
