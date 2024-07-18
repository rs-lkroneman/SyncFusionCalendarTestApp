using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace AppiumTest;

public class AndroidSelectDateTest
{
    private AndroidDriver driver;

    [OneTimeSetUp]
    public void Setup()
    {
        var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
        var driverOptions = new AppiumOptions() {
            AutomationName = AutomationName.AndroidUIAutomator2,
            PlatformName = "Android",
            DeviceName = "Android Emulator",
        };

        driverOptions.AddAdditionalAppiumOption("appPackage", "com.companyname.syncfusioncalendartestapp");
        driverOptions.AddAdditionalAppiumOption("appActivity", "com.companyname.SyncFusionCalendarTestApp.MainActivity");
        // NoReset assumes the app com.google.android is preinstalled on the emulator
        driverOptions.AddAdditionalAppiumOption("noReset", true);

        driver = new AndroidDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        driver.Dispose();
    }

    [Test]
    public void DateSelector_WhenDateIsSelected_ItIsRenderedInLabelBelow()
    {
        // Arrange
        driver.StartActivity("com.companyname.syncfusioncalendartestapp", "com.companyname.SyncFusionCalendarTestApp.MainActivity");
        driver.FindElement(By.XPath("//android.widget.Button[@resource-id=\"android:id/button3\"]")).Click();
        
        // Act
        var calendarElement = driver.FindElement(By.Id("CalendarControl"));
        var dateToSelect = calendarElement.FindElements(By.XPath("//*[@*=\"Tuesday, 16/July/2024\"]")).First();
        dateToSelect.Click();
        

        // Assert
        Assert.True(driver.FindElement(By.Id("SelectedCalendarDate")).Text.Contains("7/16/2024"));
    }
}