# N2NEdgeGUI

EdgeGUI 是一个用于管理和运行 edge.exe 的图形用户界面程序。

## 开发环境

- .NET 8.0

## 编译说明

1. 确保您的系统已安装 .NET 8.0 SDK。

2. 在项目根目录下运行以下命令进行编译：

   ```
   dotnet build
   ```

3. 编译完成后，请确保将 `edge.exe` 文件复制到输出目录中。通常，输出目录位于 `bin/Debug/net8.0-windows/` 或 `bin/Release/net8.0-windows/`，具体取决于您的构建配置。

## 使用说明

1. 运行编译后生成的 EdgeGUI.exe 文件。程序将自动请求管理员权限。

2. 在界面中填写以下信息：
   - 名称 (-c)
   - 密钥 (-k)
   - 服务器地址 (-l)
   - 本地地址 (-a)

3. 点击"运行"按钮启动 edge.exe。

4. 使用"停止"按钮可以终止正在运行的 edge.exe 进程。

5. 程序会自动保存您的设置，下次启动时会自动加载。

## 注意事项

- 确保 edge.exe 文件与 EdgeGUI.exe 位于同一目录下，或者在系统的 PATH 环境变量中。
- 如果程序无法找到 edge.exe，会提示您手动选择文件位置。
- 本程序需要管理员权限才能正常运行。

## 项目结构

- `MainWindow.xaml` 和 `MainWindow.xaml.cs`: 主窗口界面和逻辑
- `App.xaml` 和 `App.xaml.cs`: 应用程序入口和全局设置
- `EdgeGUI.csproj`: 项目文件
- `app.manifest`: 应用程序清单文件，用于请求管理员权限

## 配置文件

程序使用 `settings.json` 文件保存配置，位于程序运行目录下。示例内容如下：

```json
{
  "Name": "your_name",
  "Key": "your_key",
  "Server": "server_address:port",
  "Local": "local_address",
  "EdgeExePath": "edge.exe"
}
```

请注意：实际使用时请替换为您的真实配置信息。
>>>>>>> 1698e6d (docs: update readme)
