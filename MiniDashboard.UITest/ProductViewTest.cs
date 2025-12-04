using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using MiniDashboard.App.ViewModels;
using MiniDashboard.App.Views;

namespace MiniDashboard.UITest
{
    public class ProductViewTest
    {
        [Fact]     
        public void NavigateToProductView_ThenAddButton_ShouldOpenDialog()
        {
            using var app = Application.Launch("MiniDashboard.App.exe");
            using var automation = new UIA3Automation();

            var mainWindow = app.GetMainWindow(automation);

            // Step 1: Navigate to ProductView
            var navButtonElement = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ProductView"));
            Assert.NotNull(navButtonElement);

            var navButton = navButtonElement.AsRadioButton();
            Assert.NotNull(navButton);
            navButton.Click(); // simulate user click

            // Step 2: Wait for ProductView root
            var productRoot = Retry.WhileNull(
                () => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ProductViewRoot")),
                timeout: TimeSpan.FromSeconds(5),
                throwOnTimeout: true).Result;

            // Step 3: Wait for Add button
            var addButton = Retry.WhileNull(
                () => productRoot.FindFirstDescendant(cf => cf.ByAutomationId("AddProductButton")),
                timeout: TimeSpan.FromSeconds(5),
                throwOnTimeout: true).Result;

            addButton.AsButton().Invoke();

            // Step 4: Verify dialog
            var dialog = Retry.WhileNull(
                () => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ProductAddEditDialog")),
                timeout: TimeSpan.FromSeconds(5),
                throwOnTimeout: true).Result;

            Assert.NotNull(dialog);

            // Step 5: Read IsEdit via HelpText
            var isEditValue = dialog.Properties.HelpText.Value;
            Assert.Equal("False", isEditValue); // for Add scenario

        }
    }
}