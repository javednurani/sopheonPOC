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
    public sealed class CLOUD_T38_Step
    {


        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T38_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }
        [When(@"the page loads")]
        public void WhenThePageLoads()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
        }

        [Then(@"the Marketing Newsletter Checkbox is checked")]
        public void ThenTheMarketingNewsletterCheckboxIsChecked()
        {
            Assert.That(accountSetUpPage.isCheckBoxExist, Is.True, "Yes, keep me informed on product news!");
        }

    }
}
