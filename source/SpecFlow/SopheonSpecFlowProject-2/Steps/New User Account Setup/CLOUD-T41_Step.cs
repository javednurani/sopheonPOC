using NUnit.Framework;
using SopheonSpecFlowProject2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.New_User_Account_Setup
{
    [Binding]
    public sealed class CLOUD_T41_Step
    {
        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T41_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }
        [Then(@"the e-mail field is marked as required")]
        public void ThenTheE_MailFieldIsMarkedAsRequired()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signInButtonClick();
            Assert.That(signUpPage.isEmailAdressRequireMessageExist, Is.True, "Please enter your email");
        }

    }
}
