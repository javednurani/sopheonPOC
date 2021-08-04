using NUnit.Framework;
using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.NewUserAccountSetup
{
	[Binding]
	public sealed class VerifyMarketingNewsletterCheckboxIsChecked
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyMarketingNewsletterCheckboxIsChecked()
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
