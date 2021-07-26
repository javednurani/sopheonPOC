using SopheonSpecFlowProject2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.Terms_of_Service_Review
{
    [Binding]
    public sealed class Verify_wording_for_acceptance_on_TOS_screen_Step
    {
        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        TermsOfServicePage termsOfServicePage = new TermsOfServicePage();

        public Verify_wording_for_acceptance_on_TOS_screen_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
            termsOfServicePage = new TermsOfServicePage();
        }



        [Given(@"the user is on the TOS screen")]
        public void GivenTheUserIsOnTheTOSScreen()
        {

        }

        [When(@"the user goes to accept the TOS")]
        public void WhenTheUserGoesToAcceptTheTOS()
        {
           
        }

        [Then(@"the user sees ""(.*)""")]
        public void ThenTheUserSees(string p0)
        {
            
        }

    }
}
