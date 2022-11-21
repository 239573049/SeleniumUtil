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
        edge.GoToUrl("https://www.baidu.com/"); //������վ
        var wd = edge.FindElementsById("kw"); //��ȡ�������
        wd.SendKeys("Selenium");//��������
        var su = edge.FindElementsById("su"); //��ȡ������ť
        su.Click();//���
        // �ȴ�10��Ԫ�ؼ������
        edge.Wait(By.Id("content_left"), TimeSpan.FromSeconds(10));
        
        var content_left = edge.FindElementsById("content_left"); //��ȡ����
        
        Console.WriteLine(content_left.Text);
    }
}