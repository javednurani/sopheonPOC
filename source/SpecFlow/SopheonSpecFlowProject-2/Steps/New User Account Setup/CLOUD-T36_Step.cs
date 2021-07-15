using SopheonSpecFlowProject2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.New_User_Account_Setup
{
    [Binding]
    public sealed class CLOUD_T36_Step
    {

        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T36_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }

        [Given(@"the user is on the PL Account setup page")]
        public void GivenTheUserIsOnThePLAccountSetupPage()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
        }

        [When(@"the user creates an account without a cell phone")]
        public void WhenTheUserCreatesAnAccountWithoutACellPhone()
        {
            accountSetUpPage.nameBoxType();
            accountSetUpPage.emailBoxType();
            accountSetUpPage.newPassWordBoxType();
            accountSetUpPage.reenterPassWordBoxType();
        }

        [Then(@"the account is created")]
        public void ThenTheAccountIsCreated()
        {
            accountSetUpPage.clickCreateButton();
        }

    }
}
