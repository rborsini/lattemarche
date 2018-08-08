using Autofac;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RB.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.PasswordSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoFacConfig.Configure();

            IWebDriver driver = Login();

            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {
                IUnitOfWork uow = scope.Resolve<IUnitOfWork>();
                IRepository<Utente, int> utentiRepository = uow.Get<Utente, int>();


                var utenti = utentiRepository.GetAll().Where(u => String.IsNullOrEmpty(u.Password)).ToList();

                int i = 1;

                foreach(var utente in utenti)
                {
                    utente.Password = GetPassword(driver, utente.Id);

                    utente.Password = new HashHelper().HashPassword(utente.Password);

                    utentiRepository.Update(utente);
                    uow.SaveChanges();

                    Console.WriteLine($"{i} di {utenti.Count}");

                    i++;
                }

            }

        }

        private static string GetPassword(IWebDriver driver, int idUtente)
        {
            driver.Navigate().GoToUrl($"http://www.lattemarche.it/Lattemarche/Admin/EditUtente.aspx?IDUTENTE={idUtente}");

            IWebElement passwordInputBox = driver.FindElement(By.Name("tbPassword"));

            return passwordInputBox.GetAttribute("value");
        }

        private static IWebDriver Login()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("disable-infobars");

            IWebDriver driver = new ChromeDriver($"{System.Environment.CurrentDirectory}\\chromedriver_win32", options);

            driver.Navigate().GoToUrl("http://www.lattemarche.it/LatteMarche/login.aspx");

            IWebElement usernameInputBox = driver.FindElement(By.Name("tbLogin"));
            usernameInputBox.SendKeys("02102002");

            IWebElement passwordInputBox = driver.FindElement(By.Name("tbPassword"));
            passwordInputBox.SendKeys("giorgia2");

            driver.FindElement(By.Id("LoginBtn")).Click();

            return driver;
        }
    }
}
