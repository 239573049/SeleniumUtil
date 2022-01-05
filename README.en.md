# SeleniumUtil

#### Description
Selenium Simplified tool kit，Contains some basic operations for the three major browsers
####  Install Package
in tool=》NuGet Package Manager =》package manager console
Install-Package SeleniumUtil -Version 1.1.0
#### use SeleniumUtil call on baidu

```
using SeleniumUtil;

var data=new CrawlerMain(browser:BrowserEnum.Edge,isEnableVerboseLogging:true);
data.GoToUrl("https://www.baidu.com/");
var wd = data.FindElementsById("kw");
wd.SendKeys("Selenium");
var su = data.FindElementsById("su");
su.Click();
Thread.Sleep(500);//Wait for the interface to complete loading
var content_left = data.FindElementsById("content_left");
Console.WriteLine(content_left.Text);
Console.ReadKey();

```

