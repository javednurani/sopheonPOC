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
    public sealed class CLOUD_T34_Step
    {

        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T34_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }

        [Given(@"the user is on the PL Account Sign Up page")]
        public void GivenTheUserIsOnThePLAccountSignUpPage()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
        }

        [When(@"the user clicks submit with a blank First Name Field")]
        public void WhenTheUserClicksSubmitWithABlankFirstNameField()
        {
            accountSetUpPage.clickCreateButton();
        }

        [Then(@"the user is not taken to new account page")]
        public void ThenTheUserIsNotTakenToNewAccountPage()
        {
            Assert.AreEqual("A required field is missing. Please fill out all required fields and try again.", accountSetUpPage.errorMessageText);
        }

    }
}
