using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.UseChromium = true;
            edgeOptions.AddAdditionalCapability("useAutomationExtension", false);
            edgeOptions.AddExcludedArgument("enable-automation");

            int loopTimes = Properties.Settings.Default.LoopTimes;
            string keyWord = Properties.Settings.Default.KeyWord;
            string browserType = Properties.Settings.Default.BrowserType;
            IWebDriver driver = null;
            Random random = new Random();
            try
            {


                for (int i = 0; i < loopTimes; i++)
                {
                    if (browserType == "Chrome")
                    {
                        driver = new ChromeDriver(driverPath, chromeOptions);
                    }
                    else if (browserType == "Edge")
                    {
                        driver = new EdgeDriver(driverPath, edgeOptions);
                    }
                    driver.Manage().Window.Maximize();
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    for (int j = 0; j < 10; j++)
                    {
                        driver.Url = "https://www.google.com/";

                        IWebElement searchElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//input[@name='q']")));
                        searchElement.SendKeys(keyWord);
                        searchElement.SendKeys(Keys.Enter);

                        IReadOnlyCollection<IWebElement> listElements = driver.FindElements(By.XPath("//a[@href='http://oxbridge.com.vn/']"));
                        IReadOnlyCollection<IWebElement> nextElements = null;
                        int page = 1;
                        while (listElements.Count != 1)
                        {
                            page++;
                            nextElements = driver.FindElements(By.XPath("//a[@aria-label = 'Page " + page + "']"));
                            if (nextElements.Count > 0)
                            {
                                nextElements.ElementAt(0).Click();
                            }
                            listElements = driver.FindElements(By.XPath("//a[@href='http://oxbridge.com.vn/']"));
                        }
                        listElements.ElementAt(0).Click();
                        Task.Delay(random.Next(60000, 180000)).Wait();
                    }
                    driver.Quit();
                }
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                driver.Quit();
                throw;
            }
        }
    }
}
