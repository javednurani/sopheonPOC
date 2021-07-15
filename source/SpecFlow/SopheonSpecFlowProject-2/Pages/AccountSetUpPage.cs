using OpenQA.Selenium;
using SpecFlowProject2.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SopheonSpecFlowProject2.Pages
{
    class AccountSetUpPage
    {
        public static string url = "https://stratusqa.b2clogin.com/StratusQA.onmicrosoft.com/B2C_1A_signup_signin/api/CombinedSigninAndSignup/unified?local=signup&csrf_token=cjhxL3ZzbzJPdnZCdUtiNlVONjc1WWova0pacEx3R2ZtazVOekpYbGg0ay9saU9SVWNDUFhadnl6L0tSZFdJbldNWjZkVTBMY3ROaHNHNUJoZVlvRVE9PTsyMDIxLTA3LTA5VDE5OjIyOjMyLjM3MTIzNzJaOzRSRjdzYnFXbEhyVndndmZha24yZEE9PTt7Ik9yY2hlc3RyYXRpb25TdGVwIjoxfQ==&tx=StateProperties=eyJUSUQiOiJlMDEyZDcxMS04MjFjLTQ1OTgtYjZlOC03Njc0ZmUzNTkzYzkifQ&p=B2C_1A_signup_signin";
        IWebDriver driver;
        IWebElement nameBox => driver.FindElement(By.ClassName("textInput"));
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
        public void newPassWordBoxType() => newPassWordBox.SendKeys("Ben@10234567");
        public void reenterPassWordBoxType() => reenterPassWordBox.SendKeys("Ben@10234567");
        public void nameBoxType() => nameBox.SendKeys("Ben123");
        public void emailBoxType() => emailBox.SendKeys("asdfdg@acx.com");
        public void mobileBoxType() => mobilePhoneBox.SendKeys("1234567790");
        

        public bool isCheckBoxExist() => checkBox.Displayed;


        public string errorMessageText
        {
            get { return errorMessage.Text; }
        }

     



        public AccountSetUpPage()
        {
            driver = Hook.driver;
        }

        public void NavigateAccountSetUpPage() => driver.Navigate().GoToUrl(url);

    }
}
