using SopheonSpecFlowProject2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.New_User_Account_Setup
{
    [Binding]
    public sealed class CLOUD_T39_Step
    {
        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T39_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }

        [When(@"the user submits an account with the Marketing Newsletter Checked")]
        public void WhenTheUserSubmitsAnAccountWithTheMarketingNewsletterChecked()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
            accountSetUpPage.nameBoxType();
            accountSetUpPage.emailBoxType();
            accountSetUpPage.newPassWordBoxType();
            accountSetUpPage.reenterPassWordBoxType();
           
        }

        [Then(@"the Account is created")]
        public void ThenTheAccountIsCreated()
        {
             accountSetUpPage.mobileBoxType();
            accountSetUpPage.clickCreateButton();
        }

    }
}
