using OpenQA.Selenium;
using Sopheon.Cloud.SpecFlow.Base.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sopheon.Cloud.SpecFlow.Base.Pages
{
	public class TermsOfServicePage : PageBase
	{
		public static string url = "https://stratusqa.b2clogin.com/StratusQA.onmicrosoft.com/B2C_1A_signup_signin/api/SelfAsserted/confirmed?csrf_token=UjRoSENzeDN2ZEkyTXlqN2ZpZXNEcWkvaFh0dkhud3IybEMzTmovSklZaURod29mRDB4a0dFQ3dEWGZuRWxmcW5xdmJCNEJKRExPeUQreWZLSjVqSWc9PTsyMDIxLTA3LTI3VDE1OjEwOjA3LjM3NzM2MTZaO3ZpdGtpMjNhZnFKd1AzRHo3ZUxjb0E9PTt7IlRhcmdldEVudGl0eSI6IlNpZ25VcFdpdGhMb2dvbkVtYWlsRXhjaGFuZ2UiLCJPcmNoZXN0cmF0aW9uU3RlcCI6Mn0=&tx=StateProperties=eyJUSUQiOiJkMWRjZWQzNC02OTE4LTQ5NjgtOGRmZi1kYzM3YzM4NTg5ZjgifQ&p=B2C_1A_signup_signin&diags=%7B%22pageViewId%22%3A%2261c8dc75-7e28-46df-9312-2363c387ba42%22%2C%22pageId%22%3A%22SelfAsserted%22%2C%22trace%22%3A%5B%7B%22ac%22%3A%22T005%22%2C%22acST%22%3A1627398607%2C%22acD%22%3A1%7D%2C%7B%22ac%22%3A%22T021%20-%20URL%3Ahttps%3A%2F%2Fstratusqa.b2clogin.com%2Fstatic%2Ftenant%2Ftemplates%2FAzureBlue%2FselfAsserted.cshtml%3Fslice%3D001-000%26dc%3DBY1%22%2C%22acST%22%3A1627398607%2C%22acD%22%3A149%7D%2C%7B%22ac%22%3A%22T029%22%2C%22acST%22%3A1627398607%2C%22acD%22%3A6%7D%2C%7B%22ac%22%3A%22T004%22%2C%22acST%22%3A1627398607%2C%22acD%22%3A1%7D%2C%7B%22ac%22%3A%22T019%22%2C%22acST%22%3A1627398607%2C%22acD%22%3A10%7D%2C%7B%22ac%22%3A%22T003%22%2C%22acST%22%3A1627398607%2C%22acD%22%3A1%7D%2C%7B%22ac%22%3A%22T017%22%2C%22acST%22%3A1627398608%2C%22acD%22%3A1443%7D%2C%7B%22ac%22%3A%22T017T010%22%2C%22acST%22%3A1627405477%2C%22acD%22%3A1210%7D%2C%7B%22ac%22%3A%22T002%22%2C%22acST%22%3A0%2C%22acD%22%3A0%7D%5D%7D";
		IWebDriver driver;

		IWebElement TOSMessage => driver.FindElement(By.XPath("//label[contains(text(),'Accept the Terms of Service to keep going')]"));
		IWebElement continueButton => driver.FindElement(By.CssSelector("#continue"));
		IWebElement pageTittle => driver.FindElement(By.XPath("//h1[@id='page-title']"));
		IWebElement TOSText => driver.FindElement(By.XPath("//p[contains(text(),'Lorem ipsum dolor sit amet, consectetur adipiscing')]"));



		public void continueButtonClick() => continueButton.Click();
		public bool isTOSMassegeExist() => TOSMessage.Displayed;
		public bool isPageTittleExist() => pageTittle.Displayed;
		public bool IsTOSTextExist() => TOSText.Displayed;


		public TermsOfServicePage()
		{
			driver = BaseHook.driver;
		}

		public void NavigateAccountSetUpPage() => driver.Navigate().GoToUrl(url);
	}
}
