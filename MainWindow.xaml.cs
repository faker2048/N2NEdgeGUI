using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using Path = System.IO.Path;
using System.Security.Principal;
using Microsoft.Win32;
// 删除了错误的 using 语句

namespace EdgeGUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Process? currentProcess;
    private const string SettingsFileName = "settings.json";
    private string? edgeExePath; // 修改为可空字符串

    public MainWindow()
    {
        InitializeComponent();
        InitializeEdgeExePath(); // 确保在构造函数中调用
        LoadSettings();
    }

    private void InitializeEdgeExePath()
    {
        // 首先尝试在应用程序目录中查找 edge.exe
        edgeExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "edge.exe");
        
        // 如果在应用程序目录中找不到，则尝试在项目目录中查找（用于开发环境）
        if (!File.Exists(edgeExePath))
        {
            string projectDir = Directory.GetCurrentDirectory();
            edgeExePath = Path.Combine(projectDir, "edge.exe");
        }

        // 如果仍然找不到，使用默认值
        if (!File.Exists(edgeExePath))
        {
            edgeExePath = "edge.exe";
        }
    }

    private void LoadSettings()
    {
        string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);
        if (File.Exists(settingsPath))
        {
            try
            {
                string json = File.ReadAllText(settingsPath);
                var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (settings != null)
                {
                    NameTextBox.Text = settings.GetValueOrDefault("Name", string.Empty);
                    KeyTextBox.Text = settings.GetValueOrDefault("Key", string.Empty);
                    ServerTextBox.Text = settings.GetValueOrDefault("Server", string.Empty);
                    LocalTextBox.Text = settings.GetValueOrDefault("Local", string.Empty);
                    edgeExePath = settings.GetValueOrDefault("EdgeExePath", "edge.exe");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载设置时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void SaveSettings()
    {
        string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);
        var settings = new Dictionary<string, string>
        {
            { "Name", NameTextBox.Text ?? string.Empty },
            { "Key", KeyTextBox.Text ?? string.Empty },
            { "Server", ServerTextBox.Text ?? string.Empty },
            { "Local", LocalTextBox.Text ?? string.Empty },
            { "EdgeExePath", edgeExePath ?? string.Empty }
        };

        try
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsPath, json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存设置时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void RunButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();

        string name = NameTextBox.Text;
        string key = KeyTextBox.Text;
        string server = ServerTextBox.Text;
        string local = LocalTextBox.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(key) ||
            string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(local))
        {
            MessageBox.Show("请填写所有字段。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // 检查 edge.exe 是否存在
        if (string.IsNullOrEmpty(edgeExePath) || !File.Exists(edgeExePath))
        {
            MessageBox.Show("未找到 edge.exe 文件。请选择正确的文件位置。", "文件未找到", MessageBoxButton.OK, MessageBoxImage.Warning);
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "可执行文件 (*.exe)|*.exe",
                Title = "选择 edge.exe 文件"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                edgeExePath = openFileDialog.FileName;
            }
            else
            {
                return; // 用户取消选择文件，退出方法
            }
        }

        string arguments = $"-c {name} -k {key} -l {server} -a {local}";

        try
        {
            currentProcess = new Process();
            currentProcess.StartInfo.FileName = edgeExePath; // 使用存储的 edge.exe 路径
            currentProcess.StartInfo.Arguments = arguments;
            currentProcess.StartInfo.UseShellExecute = false;
            currentProcess.StartInfo.RedirectStandardOutput = true;
            currentProcess.StartInfo.RedirectStandardError = true;
            currentProcess.StartInfo.CreateNoWindow = true;

            currentProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Dispatcher.Invoke(() =>
                    {
                        OutputTextBox.AppendText(e.Data + Environment.NewLine);
                        OutputTextBox.ScrollToEnd();
                    });
                }
            };

            currentProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Dispatcher.Invoke(() =>
                    {
                        OutputTextBox.AppendText("错误: " + e.Data + Environment.NewLine);
                        OutputTextBox.ScrollToEnd();
                    });
                }
            };

            currentProcess.Start();
            currentProcess.BeginOutputReadLine();
            currentProcess.BeginErrorReadLine();

            RunButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            await currentProcess.WaitForExitAsync();

            RunButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            currentProcess = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"运行 edge.exe 时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        StopEdgeProcess();
        RunButton.IsEnabled = true;
        StopButton.IsEnabled = false;
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        SaveSettings();
        StopEdgeProcess();
        base.OnClosing(e);
    }

    private void StopEdgeProcess()
    {
        if (currentProcess != null && !currentProcess.HasExited)
        {
            try
            {
                currentProcess.Kill();
                currentProcess.WaitForExit(5000); // 等待最多5秒钟
                OutputTextBox.AppendText("已停止 edge.exe 进程。\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止 edge.exe 时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                currentProcess = null;
            }
        }
    }
}