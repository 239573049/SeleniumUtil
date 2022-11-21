using SeleniumUtil;
using System;
using System.Threading;
using OpenQA.Selenium;
using Xunit;

namespace Selenium.Test;
public class UnitTest1
{
    [Fact]
    public void Selenium()
    {
        var edge = new CrawlerMain(SeleniumUtil.Entitys.BrowserEnum.Edge,isShowBrowser:true,hideCommandPromptWindow:false,isEnableVerboseLogging:true,isGpu:true,size:new SeleniumUtil.Entitys.Size(1000,1000));
        edge.GoToUrl("https://www.baidu.com/"); //访问网站
        var wd = edge.FindElementsById("kw"); //获取到输入框
        wd.SendKeys("Selenium");//输入内容
        var su = edge.FindElementsById("su"); //获取搜索按钮
        su.Click();//点击
        // 等待10秒元素加载完成
        edge.Wait(By.Id("content_left"), TimeSpan.FromSeconds(10));
        
        var content_left = edge.FindElementsById("content_left"); //获取内容
        
        Console.WriteLine(content_left.Text);
    }
}