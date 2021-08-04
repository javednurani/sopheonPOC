using OpenQA.Selenium;
using Sopheon.Cloud.SpecFlow.Base.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sopheon.Cloud.SpecFlow.Base.Pages
{
	public class AccountSetUpPage : PageBase
	{
		public static string url = "https://stratusqa.b2clogin.com/StratusQA.onmicrosoft.com/B2C_1A_signup_signin/api/CombinedSigninAndSignup/unified?local=signup&csrf_token=cjhxL3ZzbzJPdnZCdUtiNlVONjc1WWova0pacEx3R2ZtazVOekpYbGg0ay9saU9SVWNDUFhadnl6L0tSZFdJbldNWjZkVTBMY3ROaHNHNUJoZVlvRVE9PTsyMDIxLTA3LTA5VDE5OjIyOjMyLjM3MTIzNzJaOzRSRjdzYnFXbEhyVndndmZha24yZEE9PTt7Ik9yY2hlc3RyYXRpb25TdGVwIjoxfQ==&tx=StateProperties=eyJUSUQiOiJlMDEyZDcxMS04MjFjLTQ1OTgtYjZlOC03Njc0ZmUzNTkzYzkifQ&p=B2C_1A_signup_signin";
		IWebDriver driver;
		IWebElement nameBox => driver.FindElement(By.XPath("//input[@id='displayName']"));
		IWebElement createButton => driver.FindElement(By.Id("continue"));
		IWebElement errorMessage => driver.FindElement(By.Id("requiredFieldMissing"));
		IWebElement newPassWordBox => driver.FindElement(By.XPath("//input[@id='newPassword']"));
		IWebElement passWordMessage => driver.FindElement(By.XPath("//body[1]/div[3]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[5]/ul[1]/li[5]/div[1]/div[1]/p[1]"));
		IWebElement emailBox => driver.FindElement(By.Id("email"));
		IWebElement reenterPassWordBox => driver.FindElement(By.Id("reenterPassword"));
		IWebElement mobilePhoneBox => driver.FindElement(By.Id("mobile"));
		IWebElement errorMessageForPhone => driver.FindElement(By.XPath("//body/div[3]/div[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[5]/ul[1]/li[5]/div[1]/div[1]"));
		IWebElement checkBox => driver.FindElement(By.Id("extension_newsletter_subscribed"));



		public bool isPassWordMassegeExist() => passWordMessage.Displayed;
		public bool isErrorMessageForPhoneMessageExist() => errorMessageForPhone.Displayed;
		public bool IsNameExist() => nameBox.Displayed;
		public void clickCreateButton() => createButton.Click();
		public void newPassWordBoxType() => newPassWordBox.SendKeys("Boy1@12345");
		public void reenterPassWordBoxType() => reenterPassWordBox.SendKeys("Boy1@12345");
		public void newPassWord2BoxType() => newPassWordBox.SendKeys("Boy3@12345");
		public void reenterPassWordBox2Type() => reenterPassWordBox.SendKeys("Boy3@12345");
		public void nameBoxType() => nameBox.SendKeys("boy1");
		public void nameBox2Type() => nameBox.SendKeys("boy3");
		public void emailBoxType() => emailBox.SendKeys("boy1@sam.com");
		public void emailBox2Type() => emailBox.SendKeys("boy3@sam.com");
		public void mobileBoxType() => mobilePhoneBox.SendKeys("8794561230");
		public void mobileBox2Type() => mobilePhoneBox.SendKeys("8994561230");


		public bool isCheckBoxExist() => checkBox.Displayed;


		public string errorMessageText
		{
			get { return errorMessage.Text; }
		}





		public AccountSetUpPage()
		{
			driver = BaseHook.driver;
		}

		public void NavigateAccountSetUpPage() => driver.Navigate().GoToUrl(url);

	}
}
