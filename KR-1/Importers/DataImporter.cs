using KR_1.Domain;

namespace KR_1.Importers;

public abstract class DataImporter
{
    public void Import(string filePath)
    {
        Console.WriteLine($"Импорт данных из файла по пути {filePath}");
        string content = System.IO.File.Exists(filePath) ? System.IO.File.ReadAllText(filePath) : "";
        var data = ParseData(content);
        SaveData(data);
    }

    protected abstract IEnumerable<IExportable> ParseData(string content);
    protected abstract void SaveData(IEnumerable<IExportable> data);
}