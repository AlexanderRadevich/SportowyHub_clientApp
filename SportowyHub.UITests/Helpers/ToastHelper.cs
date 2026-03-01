using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace SportowyHub.UITests.Helpers;

public static class ToastHelper
{
    public static void AssertNoErrorToast(AndroidDriver driver)
    {
        Thread.Sleep(1000);

        var snackbars = driver.FindElements(
            By.XPath("//android.view.View[@resource-id='com.google.android.material:id/snackbar_text']"));

        if (snackbars.Count == 0)
        {
            snackbars = driver.FindElements(
                By.ClassName("com.google.android.material.snackbar.SnackbarContentLayout"));
        }

        if (snackbars.Count > 0)
        {
            var text = snackbars[0].Text ?? "(no text)";
            Assert.Fail($"Unexpected error toast displayed: {text}");
        }
    }
}
