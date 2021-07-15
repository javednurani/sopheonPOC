using SopheonSpecFlowProject2.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SopheonSpecFlowProject2.Steps.New_User_Account_Setup
{
    [Binding]
    public sealed class CLOUD_T44_Step
    {

        AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
        SignUpPage signUpPage = new SignUpPage();
        public CLOUD_T44_Step()
        {

            accountSetUpPage = new AccountSetUpPage();
            signUpPage = new SignUpPage();
        }
        [When(@"the user clicks submit with a blank Password Field")]
        public void WhenTheUserClicksSubmitWithABlankPasswordField()
        {
            accountSetUpPage.clickCreateButton();
        }

    }
}
