using NUnit.Framework;
using SopheonSpecFlowProject2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.Terms_of_Service_Review
{
    [Binding]
    public sealed class Verify_the_user_does_not_see_a_Back_button_on_TOS_Step
    {


        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        TermsOfServicePage termsOfServicePage = new TermsOfServicePage();

        public Verify_the_user_does_not_see_a_Back_button_on_TOS_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
            termsOfServicePage = new TermsOfServicePage();
        }

        [When(@"the user is on the TOS page the user will not have the option to click on a back button from the page")]
        public void WhenTheUserIsOnTheTOSPageTheUserWillNotHaveTheOptionToClickOnABackButtonFromThePage()
        {
            
        }

        [Then(@"user can still click on the back button on the browser and be taken to the Sign Up page from which they came")]
        public void ThenUserCanStillClickOnTheBackButtonOnTheBrowserAndBeTakenToTheSignUpPageFromWhichTheyCame()
        {
            
        }

        [When(@"the user click on the back button on the browser")]
        public void WhenTheUserClickOnTheBackButtonOnTheBrowser()
        {
            signUpPage.NavigateSignUpPagePage();
        }

        [Then(@"the user is taken to a blank Login Screen")]
        public void ThenTheUserIsTakenToABlankLoginScreen()
        {
            Assert.That(signUpPage.IsExistingMessageExist, Is.True, "Sign in with your existing account");
        }

    }
}
