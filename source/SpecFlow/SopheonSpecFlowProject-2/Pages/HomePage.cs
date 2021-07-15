using OpenQA.Selenium;
using SpecFlowProject2.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SopheonSpecFlowProject2.Pages
{
    class HomePage
    {
        public static string url = "https://stratuswebsiteqa.z22.web.core.windows.net/";
        IWebDriver driver;

        IWebElement signUpButton => driver.FindElement(By.XPath("//button[contains(text(),'Sign Up / Sign In')]"));















        public void signUpButtonClick() => signUpButton.Click();


        public HomePage()
        {
            driver = Hook.driver;
        }

        public void NavigateToHomePage() => driver.Navigate().GoToUrl(url);

    }
}
