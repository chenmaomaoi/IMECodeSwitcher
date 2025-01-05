using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Core.DB;
using Core.Extensions;
using Core.Extensions.Interfaces;
using System.Timers;
using Core.Enums;
using Core.Models.DB;
using Core.Common;
using Core.Common.Helper;
using Core.Event;
using Core.Services.Interfaces;

namespace Core.Services;
public class IMECodeMonitorService : ISingletonService, IIMECodeMonitorService
{
    public event OnProgramIMECodeChangedEventHandler? OnProgramIMECodeChanged;

    private AppConfigService appConfigService;

    private Timer refreshTimer;

    public IMECodeMonitorService(AppConfigService appConfig)
    {
        this.appConfigService = appConfig;

        refreshTimer = new Timer();
        refreshTimer.Elapsed += refreshTimer_Elapsed;
        refreshTimer.Interval = this.appConfigService.GetConfigs().RefreshDelay * 100;
    }

    private string? lastProgressName;
    private IMECode lastIMECode;
    private void refreshTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        //跟上次获取到的程序名称对比，不一样什么都不做
        Win32Window window = Win32Helper.GetForegroundWindowInfo();
        var currentProgramName = window.ProcessName;

        if (lastProgressName != currentProgramName)
        {
            lastProgressName = currentProgramName;
            return;
        }

        var currentIMECode = Win32Helper.GetIMECode(window.IMEHandle);

        //IMECode相比上次没变，直接返回
        if (lastIMECode == currentIMECode)
        {
            return;
        }

        OnProgramIMECodeChanged?.Invoke(currentProgramName, currentIMECode);

        lastProgressName = currentProgramName;
        lastIMECode = currentIMECode;
    }

    public void ResetAndUpdateInterval()
    {
        refreshTimer.Stop();
        refreshTimer.Interval = appConfigService.GetConfigs().RefreshDelay * 100;
        refreshTimer.Start();
    }

    public void Start()
    {
        refreshTimer.Interval = appConfigService.ConfigModel.RefreshDelay * 100;
        refreshTimer.Start();
    }

    public void Stop() => refreshTimer.Stop();
}
