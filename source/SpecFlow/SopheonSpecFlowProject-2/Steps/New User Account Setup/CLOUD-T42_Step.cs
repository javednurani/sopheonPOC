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
    public sealed class CLOUD_T42_Step
    {
        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T42_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }

        [Then(@"the Account Password field is marked as required")]
        public void ThenTheAccountPasswordFieldIsMarkedAsRequired()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.nameBoxType();
            signUpPage.signInButtonClick();
            Assert.That(signUpPage.isPassWordRequireMessageExist,Is.True, "Please enter your password");

        }

    }
}
