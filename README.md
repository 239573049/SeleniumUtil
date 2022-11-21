# SeleniumUtil

[![NuGet](https://img.shields.io/nuget/dt/SeleniumUtil.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/SeleniumUtil/)
[![NuGet](https://img.shields.io/nuget/v/SeleniumUtil.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/SeleniumUtil/)

文档语言 : [English](README.en.md) | [简体中文](README.md)

#### Description

Selenium简化工具包，包含三个主流浏览器的一些基本操作

#### 安装插件

在 工具=》NuGet包管理器=》程序包管理器控制台

```
Install-Package SeleniumUtil
```

#### 使用SeleniumUtil访问baidu

```
using SeleniumUtil;


var data = new CrawlerMain(SeleniumUtil.Entitys.BrowserEnum.Edge,isShowBrowser:true,hideCommandPromptWindow:false,isEnableVerboseLogging:true,isGpu:true,size:new SeleniumUtil.Entitys.Size(1000,1000));
data.GoToUrl("https://www.baidu.com/"); //访问网站
var wd = edge.FindElementsById("kw"); //获取到输入框
wd.SendKeys("Selenium");//输入内容
var su = data.FindElementsById("su"); //获取搜索按钮
su.Click();//点击
// 等待10秒元素加载完成
data.Wait(By.Id("content_left"), TimeSpan.FromSeconds(10));
        
var content_left = data.FindElementsById("content_left"); //获取内容
        
Console.WriteLine(content_left.Text);

```

