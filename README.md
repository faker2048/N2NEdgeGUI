# N2NEdgeGUI 🕹️✨

- 项目的初心是为了和不太懂电脑的小伙伴们一起愉快玩局域网游戏，毕竟命令行对普通用户来说有点“陌生感” 😅，所以诞生了这个简易的图形化界面！

- Edge 是 N2N 项目的客户端，公网服务端需要自己准备和搭建，但不难，不懂的请参考 [N2N 官方文档](https://github.com/ntop/n2n) 如何搭建`supernode`💪。

- 虽然这个项目的代码几乎是由 AI 生成的，但经过多次优化，现在已经比较可靠了，可以和小伙伴一起愉快玩局域网游戏 🎉！

- 使用了 GitHub Action 自动编译，省心省力，放心食用 🚀~


![34ecb68b3e61a8bf0a54adb9eb5c3aae](https://github.com/user-attachments/assets/f682e8e6-0896-4e7c-8ac0-2021c574a63d)

## 开发环境 🌐

- .NET 8.0.X

## 自行编译说明

1. 首先，确保你的系统已经安装了 .NET 8.0.X SDK。微软官方下载安装即可：[.NET 8.0.X 下载链接](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


2. 在项目根目录下运行以下命令进行编译：

   ```
   dotnet build
   ```

3. 编译完成后，请确保将 `edge.exe` 文件复制到输出目录中。通常，输出目录位于 `bin/Debug/net8.0-windows/` 或 `bin/Release/net8.0-windows/`，具体取决于您的构建配置。

## 使用说明

1. 运行生成的 EdgeGUI.exe 文件。程序将自动请求管理员权限，以确保一切顺利进行。

2. 在界面中填写以下信息：
   - 名称 (-c)
   - 密钥 (-k)
   - 服务器地址 (-l)
   - 本地地址 (-a)

3. 点击"运行"按钮启动 edge.exe。

4. 使用"停止"按钮可以终止正在运行的 edge.exe 进程。

5. 程序会自动保存您的设置，下次启动时会自动加载。

## 注意事项

- 请确保 `edge.exe` 文件和 `EdgeGUI.exe` 位于同一目录，或者将 `edge.exe` 添加到系统的 PATH 环境变量中。
- 如果程序无法找到 `edge.exe`，会提示您手动选择文件位置。
- 本程序需要管理员权限才能正常运行。

## 项目结构

- `MainWindow.xaml` 和 `MainWindow.xaml.cs`: 主窗口界面和逻辑
- `App.xaml` 和 `App.xaml.cs`: 应用程序的入口及全局设置都在这里。
- `EdgeGUI.csproj`: 项目文件，管理所有依赖和构建。
- `app.manifest`: 请求管理员权限的配置文件，确保高权限运行。

## 配置文件

程序会将你的设置保存在 settings.json 文件中，文件位于程序运行的目录下。一个示例配置如下：

```json
{
  "Name": "your_name",
  "Key": "your_key",
  "Server": "server_address:port",
  "Local": "local_address",
  "EdgeExePath": "edge.exe"
}
```

## 致谢

- 感谢 [N2N](https://github.com/ntop/n2n) 
- 感谢 [Dotnet](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 

## License

项目基于 MIT 协议开源，欢迎大家使用和改进！
