using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CloudQAFormAutomation
{
    public class RobustFormTester
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const int TIMEOUT_SECONDS = 10;

        public RobustFormTester()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TIMEOUT_SECONDS));
        }

        public void RunFormTests()
        {
            try
            {
                driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");
                
                Console.WriteLine("Starting robust form field tests...");
                
                TestTextField();
                TestEmailField();
                TestDropdownField();
                
                Console.WriteLine("All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test execution failed: {ex.Message}");
                throw;
            }
        }

        private void TestTextField()
        {
            Console.WriteLine("Testing Text Field...");
            
            string testValue = "Avadhoot Smart";
            IWebElement textField = FindElementRobustly(GetTextFieldLocators());
            
            if (textField != null)
            {
                textField.Clear();
                textField.SendKeys(testValue);
                
                string actualValue = textField.GetAttribute("value");
                if (actualValue.Equals(testValue))
                {
                    Console.WriteLine("✓ Text field test passed");
                }
                else
                {
                    throw new Exception($"Text field test failed. Expected: {testValue}, Actual: {actualValue}");
                }
            }
            else
            {
                throw new Exception("Text field not found with any locator strategy");
            }
        }

        private void TestEmailField()
        {
            Console.WriteLine("Testing Email Field...");
            
            string testEmail = "test@gmail.com";
            IWebElement emailField = FindElementRobustly(GetEmailFieldLocators());
            
            if (emailField != null)
            {
                emailField.Clear();
                emailField.SendKeys(testEmail);
                
                string actualEmail = emailField.GetAttribute("value");
                if (actualEmail.Equals(testEmail))
                {
                    Console.WriteLine("✓ Email field test passed");
                }
                else
                {
                    throw new Exception($"Email field test failed. Expected: {testEmail}, Actual: {actualEmail}");
                }
            }
            else
            {
                throw new Exception("Email field not found with any locator strategy");
            }
        }

        private void TestDropdownField()
        {
            Console.WriteLine("Testing Dropdown Field...");
            
            IWebElement dropdownField = FindElementRobustly(GetDropdownFieldLocators());
            
            if (dropdownField != null)
            {
                SelectElement dropdown = new SelectElement(dropdownField);
                
                var options = dropdown.Options;
                if (options.Count > 1)
                {
                    dropdown.SelectByIndex(1);
                    
                    string selectedValue = dropdown.SelectedOption.Text;
                    Console.WriteLine($"✓ Dropdown field test passed. Selected: {selectedValue}");
                }
                else
                {
                    throw new Exception("Dropdown has insufficient options for testing");
                }
            }
            else
            {
                throw new Exception("Dropdown field not found with any locator strategy");
            }
        }

        private IWebElement FindElementRobustly(List<By> locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                    if (element != null && element.Displayed && element.Enabled)
                    {
                        Console.WriteLine($"  Found element using: {locator}");
                        return element;
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine($"  Locator failed: {locator}");
                    continue;
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"  Locator failed: {locator}");
                    continue;
                }
            }
            return null;
        }

        private List<By> GetTextFieldLocators()
        {
            return new List<By>
            {
                By.Id("firstName"),
                By.Name("firstName"),
                By.CssSelector("input[placeholder*='First Name']"),
                By.CssSelector("input[type='text']:first-of-type"),
                By.XPath("//input[@placeholder='First Name']"),
                By.XPath("//input[contains(@class, 'form-control') and @type='text'][1]"),
                By.XPath("//label[contains(text(), 'Name')]/following-sibling::input"),
                By.XPath("//label[contains(text(), 'First')]/following-sibling::input"),

                By.CssSelector("input[type='text']"),
                By.TagName("input")
            };
        }

        private List<By> GetEmailFieldLocators()
        {
            return new List<By>
            {
                By.Id("userEmail"),
                By.Name("userEmail"),
                By.CssSelector("input[type='email']"),
                By.CssSelector("input[placeholder*='Email']"),
                By.XPath("//input[@type='email']"),
                By.XPath("//input[@placeholder='name@example.com']"),
                By.XPath("//input[contains(@placeholder, 'email')]"),
                By.XPath("//label[contains(text(), 'Email')]/following-sibling::input"),
                By.XPath("//input[contains(@class, 'form-control') and contains(@placeholder, 'email')]"),

                By.CssSelector("input[placeholder*='@']"),
                By.XPath("//input[contains(@placeholder, '@')]")
            };
        }

        private List<By> GetDropdownFieldLocators()
        {
            return new List<By>
            {
                
                By.Id("state"),
                By.Name("state"),
                By.CssSelector("select[id='state']"),
                By.CssSelector("select.form-control"),
                By.XPath("//select[@id='state']"),
                By.XPath("//label[contains(text(), 'State')]/following-sibling::select"),
                By.XPath("//select[contains(@class, 'form-control')]"),

                By.TagName("select"),
                By.CssSelector("select"),
                By.XPath("//select[1]")
            };
        }

        public void Cleanup()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            RobustFormTester tester = null;
            
            try
            {
                Console.WriteLine("Initializing Form Automation Test");
                Console.WriteLine("========================================");
                
                tester = new RobustFormTester();
                tester.RunFormTests();
                
                Console.WriteLine("========================================");
                Console.WriteLine("Test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                tester?.Cleanup();
                
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}

