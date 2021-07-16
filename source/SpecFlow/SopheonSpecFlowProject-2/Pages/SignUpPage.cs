using OpenQA.Selenium;
using SpecFlowProject2.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SopheonSpecFlowProject2.Pages
{
    public class SignUpPage : PageBase
    {

	    //public string url = $"{B2CLoginUrl}/oauth2/v2.0/authorize?p=B2C_1A_SIGNUP_SIGNIN&client_id=b279a355-b4cc-444d-be98-610cc1f0a7b0&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=openid&response_type=id_token&prompt=login";

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
            driver = Hook.driver;
        }

        public void NavigateSignUpPagePage() => driver.Navigate().GoToUrl($"{B2CLoginUrl}/oauth2/v2.0/authorize?p=B2C_1A_SIGNUP_SIGNIN&client_id={B2CClientId}&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fjwt.ms&scope=openid&response_type=id_token&prompt=login");

    }
}
