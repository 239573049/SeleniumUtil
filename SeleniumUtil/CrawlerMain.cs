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

namespace SeleniumUtil
{
    /// <summary>
    /// 简易封装seslenium工具包
    /// </summary>
    public class CrawlerMain
    {
        public EdgeDriverService? _edgeDriver { get; protected set; }
        public EdgeOptions? _edgeOptions { get; protected set; }
        public EdgeDriver? _edgeSelenium { get; protected set; }
        public ChromeDriverService? _chromeDriver { get; protected set; }
        public ChromeOptions? _chromeOptions { get; protected set; }
        public ChromeDriver? _chromeSelenium { get; protected set; }
        public FirefoxDriverService? _firefoxDriver { get; protected set; }
        public FirefoxOptions? _firefoxOptions { get; protected set; }
        public FirefoxDriver? _firefoxSelenium { get; protected set; }
        public BrowserEnum? _browserEnum { get; protected set; }
        /// <summary>
        /// 获取当前窗口数量
        /// </summary>
        public int GetWindowCount
        {
            get
            {
                return _browserEnum switch
                {
                    BrowserEnum.Firefox => _firefoxSelenium!.WindowHandles.Count,
                    BrowserEnum.Chrome => _chromeSelenium!.WindowHandles.Count,
                    BrowserEnum.Edge => _edgeSelenium!.WindowHandles.Count,
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
        public List<string> WindowHandles { get
            {

                return _browserEnum switch
                {
                    BrowserEnum.Firefox => _firefoxSelenium!.WindowHandles.ToList(),
                    BrowserEnum.Chrome => _chromeSelenium!.WindowHandles.ToList(),
                    BrowserEnum.Edge => _edgeSelenium!.WindowHandles.ToList(),
                    _ => throw new NullReferenceException("不存在浏览器适配"),
                };
            } }

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
            Size? size =null,
            bool hideCommandPromptWindow = false, 
            PageLoadStrategy pageLoadStrategy= PageLoadStrategy.Normal,
            bool isEnableVerboseLogging=false,
            bool isGpu=false,
            bool isShowBrowser=true,
            string[]? argument =null
            )
        {
            _browserEnum = browser;
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    DriverSetup.ConfigureChrome()
                        .WithAutoVersion()
                        .SetUp();
                    _chromeDriver = ChromeDriverService.CreateDefaultService();
                    _chromeDriver.HideCommandPromptWindow = hideCommandPromptWindow;
                    if (isEnableVerboseLogging)
                    {
                        _chromeDriver.LogPath = AppDomain.CurrentDomain.BaseDirectory + "chromedriver.log";
                        _chromeDriver.EnableVerboseLogging= true;
                    }
                    _chromeOptions = new ChromeOptions
                    {
                        PageLoadStrategy = pageLoadStrategy
                    };
                    if (!isShowBrowser)
                    {
                        _chromeOptions.AddArgument("--headless");//隐藏浏览器
                    }
                    else
                    {
                        if (size == null) size = new Entitys.Size(500, 1200);
                        _chromeOptions.AddArgument($"--window-size={size.Width},{size.Height}");
                    }
                    if (!isGpu)
                    {
                        _chromeOptions.AddArgument("--disable-gpu");
                    }
                    if (argument != null)
                    {
                        foreach (var arg in argument)
                        {
                            _chromeOptions.AddArgument(arg);
                        }
                    }
                    _chromeSelenium = new ChromeDriver(_chromeDriver, _chromeOptions);
                    break;
                case BrowserEnum.Edge:
                    DriverSetup.ConfigureEdge()
                        .WithAutoVersion()
                        .SetUp();
                    _edgeDriver = EdgeDriverService.CreateDefaultService();
                    _edgeDriver.HideCommandPromptWindow = hideCommandPromptWindow;
                    if (isEnableVerboseLogging)
                    {
                        _edgeDriver.LogPath = AppDomain.CurrentDomain.BaseDirectory + "edgedriver.log";
                        _edgeDriver.EnableVerboseLogging = true;
                    }
                    _edgeOptions = new EdgeOptions
                    {
                        PageLoadStrategy = pageLoadStrategy
                    };
                    if (!isShowBrowser)
                    {
                        _edgeOptions.AddArgument("--headless");//隐藏浏览器
                    }
                    else
                    {
                        if (size == null) size = new Entitys.Size(500, 1200);
                        _edgeOptions.AddArgument($"--window-size={size.Width},{size.Height}");
                    }
                    if (!isGpu)
                    {
                        _edgeOptions.AddArgument("--disable-gpu");
                    }
                    if (argument != null)
                    {
                        foreach (var arg in argument)
                        {
                            _edgeOptions.AddArgument(arg);
                        }
                    }
                    _edgeSelenium = new EdgeDriver(_edgeDriver, _edgeOptions);
                    break;
                case BrowserEnum.Firefox:
                    DriverSetup.ConfigureFirefox()
                        .WithAutoVersion()
                        .SetUp();
                    _firefoxDriver = FirefoxDriverService.CreateDefaultService();
                    _firefoxDriver.HideCommandPromptWindow = hideCommandPromptWindow;
                    _firefoxOptions = new FirefoxOptions
                    {
                        PageLoadStrategy = pageLoadStrategy
                    };
                    if (!isShowBrowser)
                    {
                        _firefoxOptions.AddArgument("--headless");//隐藏浏览器
                    }
                    else
                    {
                        if (size == null) size = new Entitys.Size(500, 1200);
                        _firefoxOptions.AddArgument($"--window-size={size.Width},{size.Height}");
                    }
                    if (!isGpu)
                    {
                        _firefoxOptions.AddArgument("--disable-gpu");
                    }
                    if (argument != null)
                    {
                        foreach (var arg in argument)
                        {
                            _firefoxOptions.AddArgument(arg);
                        }
                    }
                    _firefoxSelenium = new FirefoxDriver(_firefoxDriver, _firefoxOptions);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }
        /// <summary>
        /// 默认创建Edge
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public static CrawlerMain NewCrawlerMain(BrowserEnum browser=BrowserEnum.Edge
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
            switch (_browserEnum)
            {
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Navigate().GoToUrl(url);
                    break;
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Navigate().GoToUrl(url);
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Navigate().GoToUrl(url);
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
            switch (_browserEnum)
            {
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Navigate().GoToUrl(url);
                    break;
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Navigate().GoToUrl(url);
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Navigate().GoToUrl(url);
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
            switch (_browserEnum)
            {
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Navigate().Back();
                    break;
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Navigate().Back();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Navigate().Back();
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
            switch (_browserEnum)
            {
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Navigate().Forward();
                    break;
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Navigate().Forward();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Navigate().Forward();
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
            switch (_browserEnum)
            {
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Navigate().Refresh();
                    break;
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Navigate().Refresh();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Navigate().Refresh();
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.ClassName(className)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.ClassName(className)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.ClassName(className)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.CssSelector(cssSelector)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.CssSelector(cssSelector)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.CssSelector(cssSelector)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.Id(id)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.Id(id)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.Id(id)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.LinkText(linkText)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.LinkText(linkText)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.LinkText(linkText)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.PartialLinkText(partialLinkText)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.PartialLinkText(partialLinkText)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.PartialLinkText(partialLinkText)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.TagName(tagName)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.TagName(tagName)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.TagName(tagName)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElements(By.XPath(xpath)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElements(By.XPath(xpath)),
                BrowserEnum.Edge => _edgeSelenium!.FindElements(By.XPath(xpath)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.ClassName(className)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.ClassName(className)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.ClassName(className)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.CssSelector(cssSelector)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.CssSelector(cssSelector)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.CssSelector(cssSelector)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.Id(id)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.Id(id)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.Id(id)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.LinkText(linkText)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.LinkText(linkText)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.LinkText(linkText)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.PartialLinkText(partialLinkText)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.PartialLinkText(partialLinkText)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.PartialLinkText(partialLinkText)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.TagName(tagName)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.TagName(tagName)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.TagName(tagName)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.FindElement(By.XPath(xpath)),
                BrowserEnum.Chrome => _chromeSelenium!.FindElement(By.XPath(xpath)),
                BrowserEnum.Edge => _edgeSelenium!.FindElement(By.XPath(xpath)),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.Manage(),
                BrowserEnum.Chrome => _chromeSelenium!.Manage(),
                BrowserEnum.Edge => _edgeSelenium!.Manage(),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }
        /// <summary>
        /// 关闭浏览器
        /// </summary>
        public void CloseBrowser()
        {
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Close();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Close();
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Close();
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.SwitchTo(),
                BrowserEnum.Chrome => _chromeSelenium!.SwitchTo(),
                BrowserEnum.Edge => _edgeSelenium!.SwitchTo(),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.ExecuteScript(script, args),
                BrowserEnum.Chrome => _chromeSelenium!.ExecuteScript(script, args),
                BrowserEnum.Edge => _edgeSelenium!.ExecuteScript(script, args),
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.ExecuteAsyncScript(script, args),
                BrowserEnum.Chrome => _chromeSelenium!.ExecuteAsyncScript(script, args),
                BrowserEnum.Edge => _edgeSelenium!.ExecuteAsyncScript(script, args),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }
        /// <summary>
        /// 重置操作执行程序的输入状态。  
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void ResetInputState()
        {
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.ResetInputState();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.ResetInputState();
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.ResetInputState();
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
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.PerformActions(actionSequenceList);
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.PerformActions(actionSequenceList);
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.PerformActions(actionSequenceList);
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
            return _browserEnum switch
            {
                BrowserEnum.Firefox => _firefoxSelenium!.GetScreenshot(),
                BrowserEnum.Chrome => _chromeSelenium!.GetScreenshot(),
                BrowserEnum.Edge => _edgeSelenium!.GetScreenshot(),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }
        /// <summary>
        /// 关闭浏览器关闭驱动
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Dispose()
        {

            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Dispose();
                    _chromeDriver!.Dispose();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Dispose();
                    _edgeDriver!.Dispose();
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Dispose();
                    _firefoxDriver!.Dispose();
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
        public async Task JavaScriptConsoleApiCalled(EventHandler<JavaScriptConsoleApiCalledEventArgs> javaScriptExceptionListening)
        {
            if (isJavaScriptMonitor) return;
            IJavaScriptEngine? monitor =null;
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    monitor = new JavaScriptEngine(_chromeSelenium);
                    break;
                case BrowserEnum.Edge:
                    monitor = new JavaScriptEngine(_edgeSelenium);
                    break;
                case BrowserEnum.Firefox:
                    monitor = new JavaScriptEngine(_firefoxSelenium);
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
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    monitor = new JavaScriptEngine(_chromeSelenium);
                    break;
                case BrowserEnum.Edge:
                    monitor = new JavaScriptEngine(_edgeSelenium);
                    break;
                case BrowserEnum.Firefox:
                    monitor = new JavaScriptEngine(_firefoxSelenium);
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.SwitchTo().Frame(data),
                BrowserEnum.Edge => _edgeSelenium!.SwitchTo().Frame(data),
                BrowserEnum.Firefox => _firefoxSelenium!.SwitchTo().Frame(data),
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.SwitchTo().Frame(data),
                BrowserEnum.Edge => _edgeSelenium!.SwitchTo().Frame(data),
                BrowserEnum.Firefox => _firefoxSelenium!.SwitchTo().Frame(data),
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.SwitchTo().Frame(data),
                BrowserEnum.Edge => _edgeSelenium!.SwitchTo().Frame(data),
                BrowserEnum.Firefox => _firefoxSelenium!.SwitchTo().Frame(data),
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.SwitchTo().DefaultContent(),
                BrowserEnum.Edge => _edgeSelenium!.SwitchTo().DefaultContent(),
                BrowserEnum.Firefox => _firefoxSelenium!.SwitchTo().DefaultContent(),
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
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Manage().Cookies.AddCookie(new Cookie(key, value));
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Manage().Cookies.AddCookie(new Cookie(key, value));
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Manage().Cookies.AddCookie(new Cookie(key, value));
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
        public void AddCookie(Dictionary<string,string> cookies)
        {
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    foreach (var d in cookies)
                    {
                        _chromeSelenium!.Manage().Cookies.AddCookie(new Cookie(d.Key, d.Value));
                    }
                    break;
                case BrowserEnum.Edge:
                    foreach (var d in cookies)
                    {
                        _edgeSelenium!.Manage().Cookies.AddCookie(new Cookie(d.Key, d.Value));
                    }
                    break;
                case BrowserEnum.Firefox:
                    foreach (var d in cookies)
                    {
                        _firefoxSelenium!.Manage().Cookies.AddCookie(new Cookie(d.Key, d.Value));
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.Manage().Cookies.GetCookieNamed(name),
                BrowserEnum.Edge => _edgeSelenium!.Manage().Cookies.GetCookieNamed(name),
                BrowserEnum.Firefox => _firefoxSelenium!.Manage().Cookies.GetCookieNamed(name),
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
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Manage().Cookies.DeleteCookieNamed(key);
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Manage().Cookies.DeleteCookieNamed(key);
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Manage().Cookies.DeleteCookieNamed(key);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
        }
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void DeleteCookie(Cookie cookie)
        {
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Manage().Cookies.DeleteCookie(cookie);
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Manage().Cookies.DeleteCookie(cookie);
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Manage().Cookies.DeleteCookie(cookie);
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.Manage().Cookies.AllCookies,
                BrowserEnum.Edge => _edgeSelenium!.Manage().Cookies.AllCookies,
                BrowserEnum.Firefox => _firefoxSelenium!.Manage().Cookies.AllCookies,
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }
        /// <summary>
        /// 删除所有Cookie
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void DeleteCookieAll()
        {
            switch (_browserEnum)
            {
                case BrowserEnum.Chrome:
                    _chromeSelenium!.Manage().Cookies.DeleteAllCookies();
                    break;
                case BrowserEnum.Edge:
                    _edgeSelenium!.Manage().Cookies.DeleteAllCookies();
                    break;
                case BrowserEnum.Firefox:
                    _firefoxSelenium!.Manage().Cookies.DeleteAllCookies();
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
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.SwitchTo().Window(windowName),
                BrowserEnum.Edge => _edgeSelenium!.SwitchTo().Window(windowName),
                BrowserEnum.Firefox => _firefoxSelenium!.SwitchTo().Window(windowName),
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }
        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            return _browserEnum switch
            {
                BrowserEnum.Chrome => _chromeSelenium!.Title,
                BrowserEnum.Edge => _edgeSelenium!.Title,
                BrowserEnum.Firefox => _firefoxSelenium!.Title,
                _ => throw new NullReferenceException("不存在浏览器适配"),
            };
        }

    }
}
