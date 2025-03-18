using KR_1.UI;
using System.Reflection;

namespace KR_1_Tests;

public class UITests
{
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenOptionsIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ConsoleMenu(null));
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenOptionsIsEmpty()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ConsoleMenu(new List<string>()));
    }

    [Fact]
    public void Constructor_InitializesSelectedIndex_ToZero()
    {
        // Arrange
        var options = new List<string> { "Option1", "Option2" };
        var menu = new ConsoleMenu(options);

        // Act
        FieldInfo selectedIndexField = typeof(ConsoleMenu)
            .GetField("_selectedIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        int selectedIndex = (int)selectedIndexField.GetValue(menu);

        // Assert
        Assert.Equal(0, selectedIndex);
    }

    [Fact]
    public void DisplayOptions_WritesAllOptions_ToConsole()
    {
        // Arrange
        var options = new List<string> { "Option1", "Option2", "Option3" };
        var menu = new ConsoleMenu(options);
        
        FieldInfo selectedIndexField = typeof(ConsoleMenu)
            .GetField("_selectedIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        selectedIndexField.SetValue(menu, 1);
        
        MethodInfo displayMethod = typeof(ConsoleMenu)
            .GetMethod("DisplayOptions", BindingFlags.NonPublic | BindingFlags.Instance);

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            displayMethod.Invoke(menu, null);
            string output = sw.ToString();

            // Assert
            foreach (var option in options)
            {
                Assert.Contains(option, output);
            }
        }
    }
}