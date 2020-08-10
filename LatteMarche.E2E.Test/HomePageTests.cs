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
    public class HomePageTests
    {
        private const string webAppBaseUrl = "http://robertoborsini.myqnapcloud.com:81/account/login";
        private static IWebDriver driver;

        [ClassInitialize]
        public static void ClassInitialise(TestContext testContext)
        {
            SetupDriver();
            driver.Url = webAppBaseUrl;
            driver.Navigate();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TeardownDriver();
        }

        [TestMethod]
        public void HomePageHeadingContainsWelcome()
        {
            // Arrange/Act/Assert
            driver.FindElement(By.Id("Username")).Text.Should().Be("");
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
