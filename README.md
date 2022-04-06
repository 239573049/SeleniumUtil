# SeleniumUtil

## Description

Selenium简化工具包，包含三个主流浏览器的一些基本操作

[工具示例网站](https://thinghelp.cn/docs/docs/selenium-uitl/selenium)

## 安装插件

在 工具=》NuGet包管理器=》程序包管理器控制台

```csharp
Install-Package SeleniumUtil -Version 1.2.1
```

## 使用SeleniumUtil访问baidu

```csharp
using SeleniumUtil;

var data=new CrawlerMain(browser:BrowserEnum.Edge,isEnableVerboseLogging:true);
data.GoToUrl("https://www.baidu.com/");//访问网站
var wd = data.FindElementsById("kw");//获取到输入框
wd.SendKeys("Selenium");//输入内容
var su = data.FindElementsById("su");//获取搜索按钮
su.Click();//点击
Thread.Sleep(500);//等待界面加载完成
var content_left = data.FindElementsById("content_left");//获取内容
Console.WriteLine(content_left.Text);
Console.ReadKey();
```

