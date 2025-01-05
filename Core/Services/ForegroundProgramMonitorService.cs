using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Helper;
using Core.Common;
using Core.Enums;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Accessibility;
using Core.Extensions.Interfaces;
using Core.Event;
using Core.Services.Interfaces;

namespace Core.Services;
public class ForegroundProgramMonitorService : ISingletonService, IDisposable, IForegroundProgramMonitorService
{
    public event OnForegroundProgramChangedEventHandler? OnForegroundProgramChanged;

    private IMECodeMonitorService codeMonitorService;

    private GCHandle callBackHandle;
    private HWINEVENTHOOK hWINEVENTHOOK = new();

    public ForegroundProgramMonitorService(IMECodeMonitorService codeMonitorService)
    {
        this.codeMonitorService = codeMonitorService;
    }

    // https://github.com/walterlv/Walterlv.ForegroundWindowMonitor

    /// <summary>
    /// 开始监听前景窗口切换
    /// </summary>
    public void Start()
    {
        try
        {
            // https://learn.microsoft.com/zh-cn/dotnet/api/system.runtime.interopservices.gchandle?view=net-8.0
            var callBack = new WINEVENTPROC(WinEventCallBack);
            callBackHandle = GCHandle.Alloc(callBack);
            // 监听系统的前台窗口变化。
            hWINEVENTHOOK = PInvoke.SetWinEventHook(PInvoke.EVENT_SYSTEM_FOREGROUND,
                                                     PInvoke.EVENT_SYSTEM_FOREGROUND,
                                                     (HMODULE)nint.Zero,
                                                     callBack,
                                                     0,
                                                     0,
                                                     PInvoke.WINEVENT_OUTOFCONTEXT | PInvoke.WINEVENT_SKIPOWNPROCESS);

            // 开启消息循环，以便 WinEventProc 能够被调用。
            if (PInvoke.GetMessage(out var lpMsg, default, default, default))
            {
                PInvoke.TranslateMessage(in lpMsg);
                PInvoke.DispatchMessage(in lpMsg);
            }
        }
        catch (Exception)
        {
            Stop();
            throw;
        }
    }

    public void Stop()
    {
        if (callBackHandle.IsAllocated) callBackHandle.Free();
        if (hWINEVENTHOOK != 0) PInvoke.UnhookWinEvent(hWINEVENTHOOK);
    }

    private string? lastProgressName;
    /// <summary>
    /// 当前台窗口变化时
    /// </summary>
    /// <param name="hWinEventHook"></param>
    /// <param name="event"></param>
    /// <param name="hwnd"></param>
    /// <param name="idObject"></param>
    /// <param name="idChild"></param>
    /// <param name="idEventThread"></param>
    /// <param name="dwmsEventTime"></param>
    private void WinEventCallBack(HWINEVENTHOOK hWinEventHook,
                                  uint @event,
                                  HWND hwnd,
                                  int idObject,
                                  int idChild,
                                  uint idEventThread,
                                  uint dwmsEventTime)
    {
        Win32Window? window1, window2;

        codeMonitorService.Stop();
        try
        {
            window1 = Win32Helper.GetForegroundWindowInfo();
            Thread.Sleep(200);
            window2 = Win32Helper.GetForegroundWindowInfo();
        }
        catch
        {
            return;
        }
        finally
        {
            codeMonitorService.Start();
        }

        if (window1.ProcessName != window2.ProcessName || window2.ProcessName == lastProgressName)
        {
            return;
        }
        IMECode currentIMECode = Win32Helper.GetIMECode(window2.IMEHandle);

        lastProgressName = window2.ProcessName;

        OnForegroundProgramChanged?.Invoke(window2, lastProgressName);
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}
