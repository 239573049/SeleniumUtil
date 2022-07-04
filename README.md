# SeleniumUtil
[![NuGet](https://img.shields.io/nuget/dt/SeleniumUtil.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/SeleniumUtil/)
[![NuGet](https://img.shields.io/nuget/v/SeleniumUtil.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/SeleniumUtil/)

文档语言 : [English](README.en.md) | [简体中文](README.md)
#### Description
Selenium简化工具包，包含三个主流浏览器的一些基本操作
#### 安装插件
在 工具=》NuGet包管理器=》程序包管理器控制台 

```
Install-Package SeleniumUtil -Version 1.2.1
```
#### 使用SeleniumUtil访问baidu

```
using SeleniumUtil;

var data=new CrawlerMain(browser:BrowserEnum.Edge,isEnableVerboseLogging:true);
data.GoToUrl("https://www.baidu.com/");
var wd = data.FindElementsById("kw");
wd.SendKeys("Selenium");
var su = data.FindElementsById("su");
su.Click();
Thread.Sleep(500);//等待界面加载完成
var content_left = data.FindElementsById("content_left");
Console.WriteLine(content_left.Text);
Console.ReadKey();

```

