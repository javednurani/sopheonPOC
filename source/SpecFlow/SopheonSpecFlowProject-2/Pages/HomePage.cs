using OpenQA.Selenium;
using Sopheon.Cloud.SpecFlow.Base.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sopheon.Cloud.SpecFlow.Base.Pages
{
	public class HomePage : PageBase
	{
		private string url => MarketingUrl;

		IWebDriver driver;
		IWebElement signUpButton => driver.FindElement(By.XPath("//button[contains(text(),'Sign Up / Sign In')]"));


		public void signUpButtonClick() => signUpButton.Click();

		public HomePage()
		{
			driver = BaseHook.driver;
		}

		public void NavigateToHomePage() => driver.Navigate().GoToUrl(url);
	}
}
