using OpenQA.Selenium;
using Sopheon.Cloud.SpecFlow.Base.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sopheon.Cloud.SpecFlow.Base.Pages
{
	public class SignUpPage : PageBase
	{
		public string url => $"https://{B2CLoginUrl}/oauth2/v2.0/authorize?p=B2C_1A_SIGNUP_SIGNIN&client_id={B2CClientId}&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=openid&response_type=id_token&prompt=login";

		IWebDriver driver;
		IWebElement nameBox => driver.FindElement(By.XPath("//input[@id='signInName']"));
		IWebElement signUpNowButton => driver.FindElement(By.Id("createAccount"));
		IWebElement signInButton => driver.FindElement(By.XPath("//button[@id='next']"));
		IWebElement passWordBox => driver.FindElement(By.Id("password"));
		IWebElement passWordRequireMessage => driver.FindElement(By.XPath("//p[contains(text(),'Please enter your password')]"));
		IWebElement emailAdressRequireMessage => driver.FindElement(By.XPath("//p[contains(text(),'Please enter your email')]"));

		public bool isEmailAdressRequireMessageExist() => emailAdressRequireMessage.Displayed;
		public bool isPassWordRequireMessageExist() => passWordRequireMessage.Displayed;
		public void signUpNowButtonClick() => signUpNowButton.Click();
		public void signInButtonClick() => signInButton.Click();
		public void passWordBoxClick() => passWordBox.Click();
		public void nameBoxType() => nameBox.SendKeys("abc@abc.com");
		public void nameBoxTypeBlank() => nameBox.SendKeys("");

		public string getNameText
		{
			get { return nameBox.Text; }
		}


		public SignUpPage()
		{
			driver = BaseHook.driver;
		}

		public void NavigateSignUpPagePage() => driver.Navigate().GoToUrl(url);

	}
}
