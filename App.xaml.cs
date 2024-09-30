using System.Windows;
using System.Security.Principal;
using System.Diagnostics;

namespace EdgeGUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (!IsRunAsAdministrator())
        {
            RestartAsAdministrator();
        }
    }

    private bool IsRunAsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }

    private void RestartAsAdministrator()
    {
        try
        {
            var exeName = Process.GetCurrentProcess().MainModule?.FileName;
            if (exeName != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
                Shutdown();
            }
            else
            {
                MessageBox.Show("无法获取当前进程的文件名", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"无法以管理员权限重新启动程序：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

