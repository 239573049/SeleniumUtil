# SeleniumUtil

[![NuGet](https://img.shields.io/nuget/dt/SeleniumUtil.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/SeleniumUtil/)
[![NuGet](https://img.shields.io/nuget/v/SeleniumUtil.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.System.Device.WiFi/)

document language : [English](README.en.md) | [简体中文](README.md)

## Description

Selenium Simplified toolkit that contains some basic operations for all three major browsers

[Sample tool site](https://tokengo.top/docs/selenium-uitl/selenium/) 

## Installing a plug-in

In the tool=》NuGet package manager  =》Package manager console

```csharp
Install-Package SeleniumUtil -Version 1.2.1
```

## Use SeleniumUtil to access Baidu  

```csharp
using SeleniumUtil;

var data=new CrawlerMain(browser:BrowserEnum.Edge,isEnableVerboseLogging:true);
data.GoToUrl("https://www.baidu.com/"); //access a website 
var wd = data.FindElementsById("kw"); //Get the input box
wd.SendKeys("Selenium"); //The input
var su = data.FindElementsById("su");//Get the search button
su.Click();//click
Thread.Sleep(500); //Wait for the interface to complete loading
var content_left = data.FindElementsById("content_left"); // Access to content
Console.WriteLine(content_left.Text);
Console.ReadKey();
```
