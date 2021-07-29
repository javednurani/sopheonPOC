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
	public sealed class VerifyCellPhoneFieldOnlyAcceptsNumbers
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyCellPhoneFieldOnlyAcceptsNumbers()
		{
		}

		[When(@"the user adds non numeric characters to the cell phone field")]
		public void WhenTheUserAddsNonNumericCharactersToTheCellPhoneField()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
			accountSetUpPage.nameBoxType();
			accountSetUpPage.emailBoxType();
			accountSetUpPage.newPassWordBoxType();
			accountSetUpPage.reenterPassWordBoxType();
			accountSetUpPage.mobileBoxType();
		}

		[Then(@"the account is not created")]
		public void ThenTheAccountIsNotCreated()
		{
			accountSetUpPage.clickCreateButton();
			Assert.That(accountSetUpPage.isErrorMessageForPhoneMessageExist, Is.False, "Please Enter a Valid Phone Number");
		}
	}
}
