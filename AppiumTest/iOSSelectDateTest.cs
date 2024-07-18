using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Interactions;

namespace AppiumTest;

public class iOSSelectDateTest
{
    private IOSDriver driver;

    [OneTimeSetUp]
    public void Setup()
    {
        var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
        var driverOptions = new AppiumOptions
        {
            AutomationName = AutomationName.iOSXcuiTest,
            PlatformName = "iOS",
            DeviceName = "iPhone 15 Pro Max",
        };
        
        driverOptions.AddAdditionalAppiumOption("bundleId", "com.companyname.syncfusioncalendartestapp");
        // NoReset assumes the app com.google.android is preinstalled on the emulator
        driverOptions.AddAdditionalAppiumOption("noReset", true);
        
        driver = new IOSDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
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
        driver.FindElement(By.XPath("//*[@name=\"OK\"]")).Click();
        
        // Act
        var calendarElement = driver.FindElement(By.Id("CalendarControl"));
        
        // This is where we cannot select a date because the calendar control is a canvas or drawable
        // var dateToSelect = calendarElement.FindElements(By.XPath("//*[@*=\"Tuesday, 16/July/2024\"]")).First();
        
        // Alternative approach
        // Alternatively we could use x and y coordinates but that is very brittle and undesirable
        // preferably this would work like the test in the android implementation
        var mouse = new PointerInputDevice(PointerKind.Mouse, "default mouse");
        var sequence = new ActionSequence(mouse);
        var move = mouse.CreatePointerMove(calendarElement, -40, 0, TimeSpan.Zero);
        var actionPress = mouse.CreatePointerDown(MouseButton.Left, new PointerInputDevice.PointerEventProperties());
        var pause = mouse.CreatePause(TimeSpan.FromMilliseconds(100));
        var actionRelease = mouse.CreatePointerUp(MouseButton.Touch);
        
        sequence.AddAction(move);
        sequence.AddAction(actionPress);
        sequence.AddAction(pause);
        sequence.AddAction(actionRelease);
  
        var actions = new List<ActionSequence>
        {
            sequence
        };
 
        driver.PerformActions(actions);
        // End alternative approach

        // Assert
        Assert.True(driver.FindElement(By.Id("SelectedCalendarDate")).Text.Contains("7/16/2024"));
    }
}