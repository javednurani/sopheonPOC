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
	public sealed class VerifyPasswordHintIsDisplayedToUser
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		
		public VerifyPasswordHintIsDisplayedToUser()
		{
		}

		[When(@"the user clicks Submit with a password not meeting default settings \(all lower case\)")]
		public void WhenTheUserClicksSubmitWithAPasswordNotMeetingDefaultSettingsAllLowerCase()
		{
			accountSetUpPage.newPassWordBoxType();
		}

		[Then(@"the user is given a Password hint message")]
		public void ThenTheUserIsGivenAPasswordHintMessage()
		{
			Assert.That(accountSetUpPage.isPassWordMassegeExist, Is.False, "8-16 characters, containing 3 out of 4 of the following: Lowercase characters, uppercase characters, digits (0-9), and one or more of the following symbols: ");
		}

	}
}
