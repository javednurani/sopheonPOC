using OpenQA.Selenium;
using SpecFlowProject2.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SopheonSpecFlowProject2.Pages
{
    public class HomePage : PageBase
    {
        private string url => MarketingUrl;

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
