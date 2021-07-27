using OpenQA.Selenium;
using SpecFlowProject2.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SopheonSpecFlowProject2.Pages
{
    public class TermsOfServicePage : PageBase
    {
        public static string url = "https://stratusqa.b2clogin.com/stratusqa.onmicrosoft.com/B2C_1A_signup_signin/api/SelfAsserted/confirmed?csrf_token=aFpFdVVuVklYUHRDQnBtQnVKK3dqM1dObXVPN3lsUWV6YVBZMjZzMlBTaHN3b2h4ODlvSUU4d25KYzFXVHQ2elM5QVh0M2ljaEliNWxHdHdQMXMxOXc9PTsyMDIxLTA3LTE5VDE2OjQ4OjQ5LjM2MDUyODNaO1M1MExPVWdjVWFoOUp2aUZvN2NTWFE9PTt7IlRhcmdldEVudGl0eSI6IlNpZ25VcFdpdGhMb2dvbkVtYWlsRXhjaGFuZ2UiLCJPcmNoZXN0cmF0aW9uU3RlcCI6Mn0=&tx=StateProperties=eyJUSUQiOiI4OTRmNzI2NC0wMzk0LTRmYWYtOWIwNC00MzAzMDQ0MGEwODEifQ&p=B2C_1A_signup_signin&diags=%7B%22pageViewId%22%3A%227ba56a34-0936-47f4-adef-750d6ad81d60%22%2C%22pageId%22%3A%22SelfAsserted%22%2C%22trace%22%3A%5B%7B%22ac%22%3A%22T005%22%2C%22acST%22%3A1626713329%2C%22acD%22%3A2%7D%2C%7B%22ac%22%3A%22T021%20-%20URL%3Ahttps%3A%2F%2Fstratusqa.b2clogin.com%2Fstatic%2Ftenant%2Ftemplates%2FAzureBlue%2FselfAsserted.cshtml%3Fslice%3D001-000%26dc%3DCHI%22%2C%22acST%22%3A1626713329%2C%22acD%22%3A118%7D%2C%7B%22ac%22%3A%22T029%22%2C%22acST%22%3A1626713329%2C%22acD%22%3A8%7D%2C%7B%22ac%22%3A%22T004%22%2C%22acST%22%3A1626713329%2C%22acD%22%3A2%7D%2C%7B%22ac%22%3A%22T019%22%2C%22acST%22%3A1626713329%2C%22acD%22%3A20%7D%2C%7B%22ac%22%3A%22T003%22%2C%22acST%22%3A1626713329%2C%22acD%22%3A4%7D%2C%7B%22ac%22%3A%22T017T010%22%2C%22acST%22%3A1626713358%2C%22acD%22%3A959%7D%2C%7B%22ac%22%3A%22T002%22%2C%22acST%22%3A0%2C%22acD%22%3A0%7D%5D%7D";
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
            driver = Hook.driver;
        }

        public void NavigateAccountSetUpPage() => driver.Navigate().GoToUrl(url);
    }
}
