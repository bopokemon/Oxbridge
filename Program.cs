using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEOWebsite
{
    class Program
    {
        static void Main(string[] args)
        {
            string driverPath = Directory.GetCurrentDirectory() + "\\Drivers";
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddExcludedArgument("enable-automation");

            int loopTimes = Properties.Settings.Default.LoopTimes;
            string keyWord = Properties.Settings.Default.KeyWord;
            for (int i = 0; i < loopTimes / 10; i++)
            {
                IWebDriver chromeDriver = new ChromeDriver(driverPath, chromeOptions);
                chromeDriver.Manage().Window.Maximize();
                WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(60));
                for (int j = 0; j < 10; j++)
                {
                    chromeDriver.Url = "https://www.google.com/";

                    IWebElement searchElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//input[@name='q']")));
                    searchElement.SendKeys(keyWord);
                    searchElement.SendKeys(Keys.Enter);

                    IReadOnlyCollection<IWebElement> listElements = chromeDriver.FindElements(By.XPath("//a[@href='http://oxbridge.com.vn/']"));
                    IReadOnlyCollection<IWebElement> nextElements = null;
                    int page = 1;
                    while (listElements.Count != 1)
                    {
                        page++;
                        nextElements = chromeDriver.FindElements(By.XPath("//a[@aria-label = 'Page " + page + "']"));
                        if (nextElements.Count > 0)
                        {
                            nextElements.ElementAt(0).Click();
                        }
                        listElements = chromeDriver.FindElements(By.XPath("//a[@href='http://oxbridge.com.vn/']"));
                    }
                    listElements.ElementAt(0).Click();
                    Task.Delay(60000).Wait();
                }
                chromeDriver.Quit();
            }
            Console.WriteLine("Done");
        }
    }
}
