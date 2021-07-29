using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.NewUserAccountSetup
{
	[Binding]
	public sealed class VerifyLinkToStartSignupProcess
	{
		HomePage homePage = new HomePage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyLinkToStartSignupProcess()
		{
		}

		[Given(@"The user is on the landing page \(Test endpoint URL tbd\)")]
		public void GivenTheUserIsOnTheLandingPageTestEndpointURLTbd()
		{
			homePage.NavigateToHomePage();
		}

		[When(@"the user clicking on the Sign Up link")]
		public void WhenTheUserClickingOnTheSignUpLink()
		{
			homePage.signUpButtonClick();
		}

		[Then(@"the user is taken to the PL Account Setup Page \(redirect to new page\)")]
		public void ThenTheUserIsTakenToThePLAccountSetupPageRedirectToNewPage()
		{
			signUpPage.NavigateSignUpPagePage();
		}
	}
}
