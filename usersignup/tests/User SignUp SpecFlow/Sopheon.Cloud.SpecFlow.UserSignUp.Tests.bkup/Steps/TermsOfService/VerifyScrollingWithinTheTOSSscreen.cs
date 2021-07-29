using NUnit.Framework;
using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.Terms_of_Service_Review
{
	[Binding]
    public sealed class VerifyScrollingWithinTheTOSSscreen
    {
        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        TermsOfServicePage termsOfServicePage = new TermsOfServicePage();

        public VerifyScrollingWithinTheTOSSscreen()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
            termsOfServicePage = new TermsOfServicePage();
        }

        [When(@"the user uses the scroll bar")]
        public void WhenTheUserUsesTheScrollBar()
        {
            signUpPage.NavigateSignUpPagePage();
            signUpPage.signUpNowButtonClick();
            accountSetUpPage.nameBoxType();
            accountSetUpPage.emailBoxType();
            accountSetUpPage.newPassWordBoxType();
            accountSetUpPage.reenterPassWordBoxType();
            accountSetUpPage.mobileBoxType();
            accountSetUpPage.clickCreateButton();
        }

        [Then(@"the Terms of Service scrolls down the page")]
        public void ThenTheTermsOfServiceScrollsDownThePage()
        {
            Assert.That(termsOfServicePage.IsTOSTextExist, Is.True, "Lorem ipsum dolor sit amet, consectetur adipiscing");
        }
    }
}
