using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.Base.Hooks
{

    public abstract class BaseHook
    {
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private ExtentTest _currentScenarioName;
        public static IWebDriver driver;
        private readonly IObjectContainer _objectContainer;
        private static ExtentTest featureName;
        private static AventStack.ExtentReports.ExtentReports extent;

        public BaseHook(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        public BaseHook(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        //[AfterStep]
        public void AfterEachStep()
        {

            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();


            if (_scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    _currentScenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
            }

            //Dont Take screenshots for non UI based scenarios 
            else if (_scenarioContext.TestError != null && !((IList<string>)_scenarioContext.ScenarioInfo.Tags).Contains("ui"))
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.StackTrace);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.StackTrace);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.StackTrace);

                //Take Screenshot
                //DriverFactory.Instance.Driver.CaptureScreenShot(TestContext.CurrentContext.Test.MethodName);
            }
            else if (_scenarioContext.TestError != null && ((IList<string>)_scenarioContext.ScenarioInfo.Tags).Contains("ui"))
            {
                //var image = DriverFactory.Instance.Driver.CaptureScreenshotAndEncode(TestContext.CurrentContext.Test.MethodName);

                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);


                //Take Screenshot
                //DriverFactory.Instance.Driver.CaptureScreenShot(TestContext.CurrentContext.Test.MethodName);
            }
            else if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

            }
        }

        [BeforeTestRun]
        public static void TestInitalize()
        {

            //Initialize Extent report before test starts
            //var htmlReporter = new ExtentHtmlReporter($@"{AppDomain.CurrentDomain.BaseDirectory}\TestResults\");
            //htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;

            //Attach report to reporter
            //extent = new ExtentReports();

            //extent.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            //extent.Flush();
        }

        [BeforeFeature]
        public static void InitializeReport(FeatureContext featureContext)
        {
            //Get feature Name
            //featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void Initialize()
        {
            driver = new ChromeDriver($"{AppDomain.CurrentDomain.BaseDirectory}\\Drivers");
            //_currentScenarioName = extent.CreateTest<Feature>(_scenarioContext.ScenarioInfo.Title).CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(600);
        }

        [AfterScenario]
        public void TestStop()
        {
            driver.Quit();
        }
    }
}

