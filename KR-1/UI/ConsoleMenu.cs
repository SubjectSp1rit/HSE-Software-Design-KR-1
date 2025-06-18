namespace KR_1.UI;

public class ConsoleMenu
{
    private readonly List<string> _options;
    private int _selectedIndex;

    public ConsoleMenu(IEnumerable<string> options)
    {
        if (options == null || !options.Any())
            throw new ArgumentException("Список опций не должен быть пустым", nameof(options));
        _options = new List<string>(options);
        _selectedIndex = 0;
    }

    public int Show()
    {
        ConsoleKey keyPressed;
        do
        {
            Console.Clear();
            DisplayOptions();

            var keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow)
                _selectedIndex = (_selectedIndex == 0) ? _options.Count - 1 : _selectedIndex - 1;
            else if (keyPressed == ConsoleKey.DownArrow)
                _selectedIndex = (_selectedIndex + 1) % _options.Count;

        } while (keyPressed != ConsoleKey.Enter);

        return _selectedIndex;
    }

    private void DisplayOptions()
    {
        for (int i = 0; i < _options.Count; i++)
        {
            string option = _options[i];
            if (i == _selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(option);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(option);
            }
        }
    }
}