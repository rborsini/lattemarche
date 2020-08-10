using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.IO;

namespace LatteMarche.E2E.Test
{
    /// <summary>
    /// https://www.stuartwhiteford.com/running-selenium-ui-tests-in-an-azure-devops-pipeline/
    /// </summary>
    [TestClass]
    public class MvcRefresh
    {
        private string baseUrl = "http://robertoborsini.myqnapcloud.com:81";
        private static IWebDriver driver;

        [ClassInitialize]
        public static void ClassInitialise(TestContext testContext)
        {
            SetupDriver();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TeardownDriver();
        }

        [TestMethod]
        public void Refresh()
        {
            driver.Url = $"{this.baseUrl}/account/login";
            driver.Navigate();

            // login -> dashboard
            driver.FindElement(By.Id("Username")).SendKeys("02102002");
            driver.FindElement(By.Id("Password")).SendKeys("giorgia2");
            driver.FindElement(By.Id("btnLogin")).Click();

            // utenti
            driver.Url = $"{this.baseUrl}/utenti";
            driver.Navigate();

            // dispositivi
            driver.Url = $"{this.baseUrl}/dispositivi";
            driver.Navigate();

            // giri
            driver.Url = $"{this.baseUrl}/utenti";
            driver.Navigate();

            // prelievi
            driver.Url = $"{this.baseUrl}/prelievi";
            driver.Navigate();

            // analisi
            driver.Url = $"{this.baseUrl}/analisi";
            driver.Navigate();

            true.Should().BeTrue();
        }

        private static void SetupDriver()
        {
            try
            {
                //InternetExplorerOptions ieOptions = new InternetExplorerOptions
                //{
                //    EnableNativeEvents = false,
                //    UnhandledPromptBehavior = UnhandledPromptBehavior.Accept,
                //    EnablePersistentHover = true,
                //    IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                //    IgnoreZoomLevel = true,
                //    EnsureCleanSession = true,
                //};

                // Attempt to read the IEWebDriver environment variable that exists on the Azure
                // platform and then fall back to the local directory.
                //string ieWebDriverPath = Environment.GetEnvironmentVariable("IEWebDriver");
                //if (string.IsNullOrEmpty(ieWebDriverPath))
                //{
                //    ieWebDriverPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                //}

                //driver = new InternetExplorerDriver(ieWebDriverPath, ieOptions)
                //{
                //    Url = webAppBaseUrl
                //};

                driver = new ChromeDriver();
            }
            catch (Exception ex)
            {
                TeardownDriver();
                throw new ApplicationException("Could not setup IWebDriver.", ex);
            }
        }

        private static void TeardownDriver()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }

    }
}
