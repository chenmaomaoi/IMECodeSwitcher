using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Windows.Forms;
using System.Threading;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace UI;
public class TaskBarNotifyIcon : IHostedService
{
    //通知栏图标
    private NotifyIcon notifyIcon;

    //设置窗口
    private MainWindow? mainWindow;

    private AppService appService;

    private IServiceProvider serviceProvider;

    public TaskBarNotifyIcon(AppService appService, IServiceProvider serviceProvider)
    {
        this.appService = appService;
        this.serviceProvider = serviceProvider;

        //设置托盘的各个属性
        notifyIcon = new NotifyIcon
        {
            Text = Core.Constant.Name,
            Icon = Resource.tq,
            Visible = true
        };
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        //绑定鼠标点击事件
        notifyIcon.MouseClick += new MouseEventHandler(_notifyIcon_MouseClick);

        var menuContext = new ContextMenuStrip();
        menuContext.Items.Add("设置", null, (sender, e) =>
        {
            if (mainWindow == null || mainWindow.IsClosed)
            {
                mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            }
            mainWindow.Show();
            mainWindow.Activate();
        });
        menuContext.Items.Add("退出", null, notifyIcon_exit);

        notifyIcon.ContextMenuStrip = menuContext;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        notifyIcon_exit(this, new EventArgs());
        return Task.CompletedTask;
    }

    private void _notifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        //显示设置窗口
        if (e.Button == MouseButtons.Left)
        {
            if (mainWindow == null || mainWindow.IsClosed)
            {
                mainWindow = serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                mainWindow.Activate();
            }
            else
            {
                mainWindow.Close();
                mainWindow = null;
            }
        }
    }

    private void notifyIcon_exit(object? sender, EventArgs e)
    {
        notifyIcon.Dispose();
        System.Windows.Application.Current.Shutdown();
    }
}
