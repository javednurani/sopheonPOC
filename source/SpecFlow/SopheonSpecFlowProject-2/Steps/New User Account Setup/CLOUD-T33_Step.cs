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
    public sealed class CLOUD_T33_Step
    {

        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T33_Step()
    {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }


    

        [Given(@"the user is on the PL Account Setup Page")]
        public void GivenTheUserIsOnThePLAccountSetupPage()
        {
            signUpPage.NavigateSignUpPagePage();
        }

        [When(@"loads the page")]
        public void WhenLoadsThePage()
        {
            signUpPage.signUpNowButtonClick();
        }

        [Then(@"the First Name field is marked as required")]
        public void ThenTheFirstNameFieldIsMarkedAsRequired()
        {
            Assert.That(accountSetUpPage.IsNameExist, Is.True, "Name");
        }

    }
}
