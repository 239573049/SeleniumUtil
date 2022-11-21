using Atata.WebDriverSetup;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using SeleniumUtil.Entitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace SeleniumUtil
{
    /// <summary>
    /// 简易封装selenium工具包
    /// </summary>
    public class CrawlerMain
    {
        public EdgeDriverService? EdgeDriverService { get; protected set; }
        public EdgeOptions? EdgeOptions { get; protected set; }
        public EdgeDriver? EdgeDriver { get; protected set; }
        public ChromeDriverService? ChromeDriverService { get; protected set; }
        public ChromeOptions? ChromeOptions { get; protected set; }
        public ChromeDriver? ChromeDriver { get; protected set; }
        public FirefoxDriverService? FirefoxDriverService { get; protected set; }
        public FirefoxOptions? FirefoxOptions { get; protected set; }
        public FirefoxDriver? FirefoxDriver { get; protected set; }
        public BrowserEnum? BrowserEnum { get; protected set; }

        /// <summary>
        /// 获取driver
        /// </summary>
        /// <returns>itme1枚举浏览器类型|itme2 driver</returns>
        public Tuple<BrowserEnum, object> GetDriver()
        {
            BrowserEnum browser = default;
            object? driver = null;
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    browser = Entitys.BrowserEnum.Chrome;
                    driver = ChromeDriver;
                    break;
                case Entitys.BrowserEnum.Edge:
                    browser = Entitys.BrowserEnum.Edge;
                    driver = EdgeDriver;
                    break;
                case Entitys.BrowserEnum.Firefox:
                    browser = Entitys.BrowserEnum.Firefox;
                    driver = FirefoxDriver;
                    break;
            }

            return new Tuple<BrowserEnum, object>(browser, driver!);
        }


        /// <summary>
        /// 获取DriverService
        /// </summary>
        /// <returns>itme1枚举浏览器类型|itme2 DriverService</returns>
        public Tuple<BrowserEnum, object> GetDriverService()
        {
            BrowserEnum browser = default;
            object? driverService = null;
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    browser = Entitys.BrowserEnum.Chrome;
                    driverService = ChromeDriverService;
                    break;
                case Entitys.BrowserEnum.Edge:
                    browser = Entitys.BrowserEnum.Edge;
                    driverService = EdgeDriverService;
                    break;
                case Entitys.BrowserEnum.Firefox:
                    browser = Entitys.BrowserEnum.Firefox;
                    driverService = FirefoxDriverService;
                    break;
            }

            return new Tuple<BrowserEnum, object>(browser, driverService!);
        }

        /// <summary>
        /// 获取Options
        /// </summary>
        /// <returns>itme1枚举浏览器类型|itme2 Options</returns>
        public Tuple<BrowserEnum, object> GetOptions()
        {
            BrowserEnum browser = default;
            object? options = null;
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    browser = Entitys.BrowserEnum.Chrome;
                    options = ChromeOptions!;
                    break;
                case Entitys.BrowserEnum.Edge:
                    browser = Entitys.BrowserEnum.Edge;
                    options = EdgeOptions!;
                    break;
                case Entitys.BrowserEnum.Firefox:
                    browser = Entitys.BrowserEnum.Firefox;
                    options = FirefoxOptions!;
                    break;
            }

            return new Tuple<BrowserEnum, object>(browser, options!);
        }

        /// <summary>
        /// 获取当前窗口数量
        /// </summary>
        public int GetWindowCount
        {
            get
            {
                return BrowserEnum switch
                {
                    Entitys.BrowserEnum.Firefox => FirefoxDriver!.WindowHandles.Count,
                    Entitys.BrowserEnum.Chrome => ChromeDriver!.WindowHandles.Count,
                    Entitys.BrowserEnum.Edge => EdgeDriver!.WindowHandles.Count,
                    _ => throw new NullReferenceException("不存在浏览器适配"),
                };
            }
        }

        /// <summary>
        /// 是否启动了JavaScript监听
        /// </summary>
        private bool isJavaScriptMonitor = false;

        /// <summary>
        /// 获取所有窗口句柄
        /// </summary>
        public List<string> WindowHandles
        {
            get
            {
                return BrowserEnum switch
                {
                    Entitys.BrowserEnum.Firefox => FirefoxDriver!.WindowHandles.ToList(),
                    Entitys.BrowserEnum.Chrome => ChromeDriver!.WindowHandles.ToList(),
                    Entitys.BrowserEnum.Edge => EdgeDriver!.WindowHandles.ToList(),
                    _ => throw new NullReferenceException("不存在浏览器适配"),
                };
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="browser">选择浏览器</param>
        /// <param name="hideCommandPromptWindow">是否隐藏命令窗口</param>
        /// <param name="pageLoadStrategy">加载策略</param>
        /// <param name="isEnableVerboseLogging">是否启动日志详细(firefox不生效)</param>
        /// <param name="isShowBrowser">是否显示浏览器</param>
        /// <param name="isGpu">是否启用gpu加速</param>
        /// <param name="argument">设置浏览器启动参数</param>
        /// <param name="size">浏览器显示大小</param>
        /// <exception cref="NullReferenceException"></exception>
        public CrawlerMain(BrowserEnum browser,
            Size? size = null,
            bool hideCommandPromptWindow = false,
            PageLoadStrategy pageLoadStrategy = PageLoadStrategy.Normal,
            bool isEnableVerboseLogging = false,
            bool isGpu = false,
            bool isShowBrowser = true,
            string[]? argument = null
        )
        {
            BrowserEnum = browser;
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    DriverSetup.ConfigureChrome()
                        .WithAutoVersion()
                        .SetUp();
                    ChromeDriverService = ChromeDriverService.CreateDefaultService();
                    ChromeDriverService.HideCommandPromptWindow = hideCommandPromptWindow;
                    if (isEnableVerboseLogging)
                    {
                        ChromeDriverService.LogPath = AppDomain.CurrentDomain.BaseDirectory + "chromedriver.log";
                        ChromeDriverService.EnableVerboseLogging = true;
                    }

                    ChromeOptions = new ChromeOptions
                    {
                        PageLoadStrategy = pageLoadStrategy
                    };
                    if (!isShowBrowser)
                    {
                        ChromeOptions.AddArgument("--headless"); //隐藏浏览器
                    }
                    else
                    {
                        if (size == null) size = new Size(800, 1200);
                        ChromeOptions.AddArgument($"--window-size={size.Width},{size.Height}");
                    }

                    if (!isGpu)
                    {
                        ChromeOptions.AddArgument("--disable-gpu");
                    }

                    if (argument != null)
                    {
                        foreach (var arg in argument)
                        {
                            ChromeOptions.AddArgument(arg);
                        }
                    }

                    ChromeDriver = new ChromeDriver(ChromeDriverService, ChromeOptions);
                    break;
                case Entitys.BrowserEnum.Edge:
                    DriverSetup.ConfigureEdge()
                        .WithAutoVersion()
                        .SetUp();
                    EdgeDriverService = EdgeDriverService.CreateDefaultService();
                    EdgeDriverService.HideCommandPromptWindow = hideCommandPromptWindow;
                    if (isEnableVerboseLogging)
                    {
                        EdgeDriverService.LogPath = AppDomain.CurrentDomain.BaseDirectory + "edgedriver.log";
                        EdgeDriverService.EnableVerboseLogging = true;
                    }

                    EdgeOptions = new EdgeOptions
                    {
                        PageLoadStrategy = pageLoadStrategy
                    };
                    if (!isShowBrowser)
                    {
                        EdgeOptions.AddArgument("--headless"); //隐藏浏览器
                    }
                    else
                    {
                        if (size == null) size = new Size(500, 1200);
                        EdgeOptions.AddArgument($"--window-size={size.Width},{size.Height}");
                    }

                    if (!isGpu)
                    {
                        EdgeOptions.AddArgument("--disable-gpu"); //是否启动gpu加速
                    }

                    if (argument != null)
                    {
                        foreach (var arg in argument)
                        {
                            EdgeOptions.AddArgument(arg);
                        }
                    }

                    EdgeDriver = new EdgeDriver(EdgeDriverService, EdgeOptions);
                    break;
                case Entitys.BrowserEnum.Firefox:
                    DriverSetup.ConfigureFirefox()
                        .WithAutoVersion()
                        .SetUp();
                    FirefoxDriverService = FirefoxDriverService.CreateDefaultService();
                    FirefoxDriverService.HideCommandPromptWindow = hideCommandPromptWindow;
                    FirefoxOptions = new FirefoxOptions
                    {
                        PageLoadStrategy = pageLoadStrategy
                    };
                    if (!isShowBrowser)
                    {
                        FirefoxOptions.AddArgument("--headless"); //隐藏浏览器
                    }
                    else
                    {
                        if (size == null) size = new Entitys.Size(500, 1200);
                        FirefoxOptions.AddArgument($"--window-size={size.Width},{size.Height}");
                    }

                    if (!isGpu)
                    {
                        FirefoxOptions.AddArgument("--disable-gpu");
                    }

                    if (argument != null)
                    {
                        foreach (var arg in argument)
                        {
                            FirefoxOptions.AddArgument(arg);
                        }
                    }

                    FirefoxDriver = new FirefoxDriver(FirefoxDriverService, FirefoxOptions);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 默认创建Edge
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public static CrawlerMain NewCrawlerMain(BrowserEnum browser = Entitys.BrowserEnum.Edge
        )
        {
            return new CrawlerMain(browser);
        }

        /// <summary>
        /// 访问地址
        /// </summary>
        /// <param name="url"></param>
        public void GoToUrl(string url)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Navigate().GoToUrl(url);
                    break;
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Navigate().GoToUrl(url);
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Navigate().GoToUrl(url);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 访问地址
        /// </summary>
        /// <param name="url"></param>
        public void GoToUrl(Uri url)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Navigate().GoToUrl(url);
                    break;
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Navigate().GoToUrl(url);
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Navigate().GoToUrl(url);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 在浏览器的历史记录中向后移动一个条目。  
        /// </summary>
        public void Back()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Navigate().Back();
                    break;
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Navigate().Back();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Navigate().Back();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 在浏览器的历史记录中向前移动一个“项目”。  
        /// </summary>
        public void Forward()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Navigate().Forward();
                    break;
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Navigate().Forward();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Navigate().Forward();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 刷新当前页面  
        /// </summary>
        public void Refresh()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Navigate().Refresh();
                    break;
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Navigate().Refresh();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Navigate().Refresh();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 查找与提供的类名匹配的元素列表  
        /// </summary>
        /// <param name="className">元素的CSS类名</param>
        /// <returns>IWebElement对象的ReadOnlyCollection，这样你就可以与它们交互了 </returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByClassNames(string className)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.ClassName(className)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.ClassName(className)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.ClassName(className)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与指定CSS选择器匹配的所有元素。  
        /// </summary>
        /// <param name="cssSelector">要匹配的CSS选择器。</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByCssSelectors(string cssSelector)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.CssSelector(cssSelector)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.CssSelector(cssSelector)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.CssSelector(cssSelector)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找页面中与提供的ID匹配的第一个元素  
        /// </summary>
        /// <param name="id">元素的ID</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByIds(string id)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.Id(id)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.Id(id)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.Id(id)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与所提供的链接文本匹配的元素列表      
        /// </summary>
        /// <param name="linkText">元素的链接文本</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByLinkTexts(string linkText)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.LinkText(linkText)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.LinkText(linkText)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.LinkText(linkText)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与提供的类名匹配的元素列表  
        /// </summary>
        /// <param name="partialLinkText">部分链接文本</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkTexts(string partialLinkText)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.PartialLinkText(partialLinkText)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.PartialLinkText(partialLinkText)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.PartialLinkText(partialLinkText)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与所提供的DOM标记匹配的元素列表  
        /// </summary>
        /// <param name="tagName">DOM标记被搜索的元素名</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByTagNames(string tagName)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.TagName(tagName)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.TagName(tagName)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.TagName(tagName)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找匹配链接文本XPath的元素列表  
        /// </summary>
        /// <param name="xpath">Xpath到元素</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<IWebElement> FindElementsByXPaths(string xpath)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElements(By.XPath(xpath)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElements(By.XPath(xpath)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElements(By.XPath(xpath)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与提供的类名匹配的元素列表  
        /// </summary>
        /// <param name="className">元素的CSS类名</param>
        /// <returns>IWebElement对象的ReadOnlyCollection，这样你就可以与它们交互了 </returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsByClassName(string className)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.ClassName(className)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.ClassName(className)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.ClassName(className)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与指定CSS选择器匹配的所有元素。  
        /// </summary>
        /// <param name="cssSelector">要匹配的CSS选择器。</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsByCssSelector(string cssSelector)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.CssSelector(cssSelector)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.CssSelector(cssSelector)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.CssSelector(cssSelector)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找页面中与提供的ID匹配的第一个元素  
        /// </summary>
        /// <param name="id">元素的ID</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsById(string id)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.Id(id)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.Id(id)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.Id(id)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与所提供的链接文本匹配的元素列表      
        /// </summary>
        /// <param name="linkText">元素的链接文本</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsByLinkText(string linkText)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.LinkText(linkText)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.LinkText(linkText)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.LinkText(linkText)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与提供的类名匹配的元素列表  
        /// </summary>
        /// <param name="partialLinkText">部分链接文本</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsByPartialLinkText(string partialLinkText)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.PartialLinkText(partialLinkText)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.PartialLinkText(partialLinkText)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.PartialLinkText(partialLinkText)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找与所提供的DOM标记匹配的元素列表  
        /// </summary>
        /// <param name="tagName">DOM标记被搜索的元素名</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsByTagName(string tagName)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.TagName(tagName)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.TagName(tagName)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.TagName(tagName)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 查找匹配链接文本XPath的元素列表  
        /// </summary>
        /// <param name="xpath">Xpath到元素</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement FindElementsByXPath(string xpath)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.FindElement(By.XPath(xpath)),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.FindElement(By.XPath(xpath)),
                Entitys.BrowserEnum.Edge => EdgeDriver!.FindElement(By.XPath(xpath)),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 获取一个对象来设置速度
        /// </summary>
        /// <returns>返回一个IOptions对象，允许驱动程序设置速度和cookie，获取cookie  </returns>
        /// <exception cref="NullReferenceException"></exception>
        public IOptions Manage()
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.Manage(),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.Manage(),
                Entitys.BrowserEnum.Edge => EdgeDriver!.Manage(),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 关闭浏览器
        /// </summary>
        public void CloseBrowser()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Close();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Close();
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Close();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 方法使您能够访问开关框架和窗口  
        /// </summary>
        /// <returns>返回一个对象，该对象允许您切换帧和窗口  </returns>
        /// <exception cref="NullReferenceException"></exception>
        public ITargetLocator SwitchTo()
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.SwitchTo(),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.SwitchTo(),
                Entitys.BrowserEnum.Edge => EdgeDriver!.SwitchTo(),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 在当前选择的框架或窗口的上下文中执行JavaScript  
        /// </summary>
        /// <param name="script">要执行的JavaScript代码。</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>脚本返回的值。</returns>
        public object ExecuteScript(string script, params object[] args)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.ExecuteScript(script, args),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.ExecuteScript(script, args),
                Entitys.BrowserEnum.Edge => EdgeDriver!.ExecuteScript(script, args),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 在当前选择的框架或窗口的上下文中异步执行JavaScript。  
        /// </summary>
        /// <param name="script">要执行的JavaScript代码。</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>脚本返回的值。</returns>
        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.ExecuteAsyncScript(script, args),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.ExecuteAsyncScript(script, args),
                Entitys.BrowserEnum.Edge => EdgeDriver!.ExecuteAsyncScript(script, args),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 重置操作执行程序的输入状态。  
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void ResetInputState()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.ResetInputState();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.ResetInputState();
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.ResetInputState();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 使用此操作执行程序执行指定的操作列表。  
        /// </summary>
        /// <param name="actionSequenceList">要执行的动作序列列表。</param>
        /// <exception cref="NullReferenceException"></exception>
        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.PerformActions(actionSequenceList);
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.PerformActions(actionSequenceList);
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.PerformActions(actionSequenceList);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 获取一个 OpenQA.Selenium.Screenshot对象，该对象表示屏幕上页面的图像。  
        /// </summary>
        /// <returns>包含 OpenQA.Selenium.Screenshot 对象。  </returns>
        /// <exception cref="NullReferenceException"></exception>
        public Screenshot GetScreenshot()
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.GetScreenshot(),
                Entitys.BrowserEnum.Chrome => ChromeDriver!.GetScreenshot(),
                Entitys.BrowserEnum.Edge => EdgeDriver!.GetScreenshot(),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 关闭浏览器关闭驱动
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Dispose()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Dispose();
                    ChromeDriverService!.Dispose();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Dispose();
                    EdgeDriverService!.Dispose();
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Dispose();
                    FirefoxDriverService!.Dispose();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 监听JavaScript异常启动
        /// 使用示例
        /// await JavaScriptConsoleApiCalled((a,s) =>
        ///    {
        ///     Console.WriteLine($"a:{a},s:{s.MessageContent}");
        ///  });
        /// </summary>
        /// <param name="javaScriptExceptionListening"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task JavaScriptConsoleApiCalledAsync(
            EventHandler<JavaScriptConsoleApiCalledEventArgs> javaScriptExceptionListening)
        {
            if (isJavaScriptMonitor) return;
            IJavaScriptEngine? monitor = null;
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    monitor = new JavaScriptEngine(ChromeDriver);
                    break;
                case Entitys.BrowserEnum.Edge:
                    monitor = new JavaScriptEngine(EdgeDriver);
                    break;
                case Entitys.BrowserEnum.Firefox:
                    monitor = new JavaScriptEngine(FirefoxDriver);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }

            monitor!.JavaScriptConsoleApiCalled += javaScriptExceptionListening;
            await monitor.StartEventMonitoring();
            isJavaScriptMonitor = true;
        }

        /// <summary>
        /// 关闭JavaScript监听
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void JavaScriptConsoleStop()
        {
            if (isJavaScriptMonitor!) return;
            IJavaScriptEngine? monitor;
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    monitor = new JavaScriptEngine(ChromeDriver);
                    break;
                case Entitys.BrowserEnum.Edge:
                    monitor = new JavaScriptEngine(EdgeDriver);
                    break;
                case Entitys.BrowserEnum.Firefox:
                    monitor = new JavaScriptEngine(FirefoxDriver);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }

            monitor.StopEventMonitoring();
        }

        /// <summary>
        /// 切换到Frame
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebDriver Frame(string data)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.SwitchTo().Frame(data),
                Entitys.BrowserEnum.Edge => EdgeDriver!.SwitchTo().Frame(data),
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.SwitchTo().Frame(data),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 切换到Frame
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebDriver Frame(IWebElement data)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.SwitchTo().Frame(data),
                Entitys.BrowserEnum.Edge => EdgeDriver!.SwitchTo().Frame(data),
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.SwitchTo().Frame(data),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 切换到Frame
        /// </summary>
        /// <param name="data">第几个Frame</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception> 
        public IWebDriver Frame(int data)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.SwitchTo().Frame(data),
                Entitys.BrowserEnum.Edge => EdgeDriver!.SwitchTo().Frame(data),
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.SwitchTo().Frame(data),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 回到顶层退出Frame
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebDriver DefaultContent()
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.SwitchTo().DefaultContent(),
                Entitys.BrowserEnum.Edge => EdgeDriver!.SwitchTo().DefaultContent(),
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.SwitchTo().DefaultContent(),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddCookie(string key, string value)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Manage().Cookies.AddCookie(new Cookie(key, value));
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Manage().Cookies.AddCookie(new Cookie(key, value));
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Manage().Cookies.AddCookie(new Cookie(key, value));
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void AddCookie(Dictionary<string, string> cookies)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    foreach (var d in cookies)
                    {
                        ChromeDriver!.Manage().Cookies.AddCookie(new Cookie(d.Key, d.Value));
                    }

                    break;
                case Entitys.BrowserEnum.Edge:
                    foreach (var d in cookies)
                    {
                        EdgeDriver!.Manage().Cookies.AddCookie(new Cookie(d.Key, d.Value));
                    }

                    break;
                case Entitys.BrowserEnum.Firefox:
                    foreach (var d in cookies)
                    {
                        FirefoxDriver!.Manage().Cookies.AddCookie(new Cookie(d.Key, d.Value));
                    }

                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="name">Key</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public Cookie GetCookie(string name)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.Manage().Cookies.GetCookieNamed(name),
                Entitys.BrowserEnum.Edge => EdgeDriver!.Manage().Cookies.GetCookieNamed(name),
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.Manage().Cookies.GetCookieNamed(name),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void DeleteCookie(string key)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Manage().Cookies.DeleteCookieNamed(key);
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Manage().Cookies.DeleteCookieNamed(key);
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Manage().Cookies.DeleteCookieNamed(key);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void DeleteCookie(Cookie cookie)
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Manage().Cookies.DeleteCookie(cookie);
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Manage().Cookies.DeleteCookie(cookie);
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Manage().Cookies.DeleteCookie(cookie);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 获取所有Cookie
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public ReadOnlyCollection<Cookie> GetCookiesAll()
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.Manage().Cookies.AllCookies,
                Entitys.BrowserEnum.Edge => EdgeDriver!.Manage().Cookies.AllCookies,
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.Manage().Cookies.AllCookies,
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 删除所有Cookie
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void DeleteCookieAll()
        {
            switch (BrowserEnum)
            {
                case Entitys.BrowserEnum.Chrome:
                    ChromeDriver!.Manage().Cookies.DeleteAllCookies();
                    break;
                case Entitys.BrowserEnum.Edge:
                    EdgeDriver!.Manage().Cookies.DeleteAllCookies();
                    break;
                case Entitys.BrowserEnum.Firefox:
                    FirefoxDriver!.Manage().Cookies.DeleteAllCookies();
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }

        /// <summary>
        /// 切换Window界面
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebDriver SwitchoverWindow(string windowName)
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.SwitchTo().Window(windowName),
                Entitys.BrowserEnum.Edge => EdgeDriver!.SwitchTo().Window(windowName),
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.SwitchTo().Window(windowName),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            return BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => ChromeDriver!.Title,
                Entitys.BrowserEnum.Edge => EdgeDriver!.Title,
                Entitys.BrowserEnum.Firefox => FirefoxDriver!.Title,
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

        /// <summary>
        /// 等待某一个元素加载完成
        /// </summary>
        /// <param name="by"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IWebElement Wait(By by, TimeSpan timeSpan)
        {
            var wait = BrowserEnum switch
            {
                Entitys.BrowserEnum.Chrome => new WebDriverWait(ChromeDriver, timeSpan),
                Entitys.BrowserEnum.Edge => new WebDriverWait(EdgeDriver, timeSpan),
                Entitys.BrowserEnum.Firefox => new WebDriverWait(FirefoxDriver, timeSpan),
                _ => throw new NullReferenceException("不存在浏览器适配")
            };

            return wait.Until(p => p.FindElement(by));
        }
    }
}