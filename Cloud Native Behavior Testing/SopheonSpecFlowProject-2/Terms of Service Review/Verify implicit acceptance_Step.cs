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
    public sealed class CLOUD_T46_Step
    {


        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        TermsOfServicePage termsOfServicePage = new TermsOfServicePage();

        public CLOUD_T46_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
            termsOfServicePage = new TermsOfServicePage();
        }

        [Given(@"the user is on the TOS page")]
        public void GivenTheUserIsOnTheTOSPage()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
            accountSetUpPage.nameBoxType();
            accountSetUpPage.emailBoxType();
            accountSetUpPage.newPassWordBoxType();
            accountSetUpPage.reenterPassWordBoxType();
            accountSetUpPage.mobileBoxType();
            accountSetUpPage.clickCreateButton();
        }

        [When(@"the user clicks on Continue")]
        public void WhenTheUserClicksOnContinue()
        {
            
           
        }

        [Then(@"the user account is created")]
        public void ThenTheUserAccountIsCreated()
        {
            
        }

    }
}
