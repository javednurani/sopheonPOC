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
    public sealed class CLOUD_T37_Step
    {
        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T37_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }


        [When(@"the user adds non numeric characters to the cell phone field")]
        public void WhenTheUserAddsNonNumericCharactersToTheCellPhoneField()
        {

            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
            accountSetUpPage.nameBoxType();
            accountSetUpPage.emailBoxType();
            accountSetUpPage.newPassWordBoxType();
            accountSetUpPage.reenterPassWordBoxType();
            accountSetUpPage.mobileBoxType();
        }

        [Then(@"the account is not created")]
        public void ThenTheAccountIsNotCreated()
        {

            accountSetUpPage.clickCreateButton();
            Assert.That(accountSetUpPage.isErrorMessageForPhoneMessageExist, Is.False, "Please Enter a Valid Phone Number");
        }

    }
}
