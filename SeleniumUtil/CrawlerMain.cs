using Atata.WebDriverSetup;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;

namespace SeleniumUtil
{
    /// <summary>
    /// 简易封装seslenium工具包
    /// </summary>
    public class CrawlerMain
    {
        /// <summary>
        /// 请小心使用以下
        /// </summary>
        public EdgeDriverService? _edgeDriver = null;
        public EdgeOptions? _edgeOptions = null;
        public EdgeDriver? _edgeSelenium = null;
        public ChromeDriverService? _chromeDriver = null;
        public ChromeOptions? _chromeOptions = null;
        public ChromeDriver? _chromeSelenium = null;
        public FirefoxDriverService? _firefoxDriver = null;
        public FirefoxOptions? _firefoxOptions = null;
        public FirefoxDriver? _firefoxSelenium = null;
        private BrowserEnum? _browserEnum = null;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="browser">选择浏览器</param>
        /// <param name="hideCommandPromptWindow">是否隐藏命令窗口</param>
        /// <exception cref="NullReferenceException"></exception>
        public CrawlerMain(BrowserEnum browser, bool hideCommandPromptWindow = false)
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
                    _chromeOptions = new ChromeOptions();
                    _chromeSelenium = new ChromeDriver(_chromeDriver, _chromeOptions);
                    break;
                case BrowserEnum.Edge:
                    DriverSetup.ConfigureEdge()
                        .WithAutoVersion()
                        .SetUp();
                    _edgeDriver = EdgeDriverService.CreateDefaultService();
                    _edgeDriver.HideCommandPromptWindow = hideCommandPromptWindow;
                    _edgeOptions = new EdgeOptions();
                    _edgeSelenium = new EdgeDriver(_edgeDriver, _edgeOptions);
                    break;
                case BrowserEnum.Firefox:
                    DriverSetup.ConfigureFirefox()
                        .WithAutoVersion()
                        .SetUp();
                    _firefoxDriver = FirefoxDriverService.CreateDefaultService();
                    _firefoxDriver.HideCommandPromptWindow = hideCommandPromptWindow;
                    _firefoxOptions = new FirefoxOptions();
                    _firefoxSelenium = new FirefoxDriver(_firefoxDriver, _firefoxOptions);
                    break;
                default:
                    throw new NullReferenceException("不存在浏览器适配");
            }
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
    }
    public enum BrowserEnum
    {
        Chrome,
        Edge,
        Firefox
    }
}
