using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.NewUserAccountSetup
{
	[Binding]
	public sealed class VerifyCreatingAnAccountWithMarketingNewsletterUnchecked
	{
		SignUpPage signUpPage = new SignUpPage();

		public VerifyCreatingAnAccountWithMarketingNewsletterUnchecked()
		{
		}

		[When(@"the user submits an account with the Marketing Newsletter unchecked")]
		public void WhenTheUserSubmitsAnAccountWithTheMarketingNewsletterUnchecked()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
		}
	}
}
