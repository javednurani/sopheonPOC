using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Sopheon.Cloud.SpecFlow.Base.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Hooks
{
	[Binding]
	public class Hook : BaseHook
	{
        public Hook(IObjectContainer objectContainer) : base(objectContainer)
		{
		}

		public Hook(FeatureContext featureContext, ScenarioContext scenarioContext) : base(featureContext, scenarioContext)
		{
		}
    }
}
