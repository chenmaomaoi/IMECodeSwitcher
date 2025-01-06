using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Core.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost host;

    public App()
    {
        //检查是否已经在运行
        if (IsRuned())
        {
            Environment.Exit(0);
        }

#if !DEBUG
        //绑定错误捕获
        DispatcherUnhandledException += App_DispatcherUnhandledException;
#endif

        var hostBuilder = Host.CreateDefaultBuilder();
        host = hostBuilder.ConfigureServices(services =>
        {
            services.RegisterAllServices();
            services.AddSerilog();
        }).UseSerilog((context, configuration) =>
        {
            configuration.MinimumLevel.Debug();
            configuration.WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day);
        }).Build();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        host.StartAsync();
    }

    #region 获取当前程序是否已运行
    /// <summary>
    /// 获取当前程序是否已运行
    /// </summary>
    private bool IsRuned()
    {
        string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        bool result = System.Diagnostics.Process.GetProcessesByName(strProcessName).Length > 1;
#if DEBUG
        result = false;
#endif
        return result;
    }
    #endregion

    #region 全局错误处理
    /// <summary>
    /// 全局错误处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Fatal(e.Exception.ToString());
        Log.CloseAndFlush();
    }
    #endregion
}
