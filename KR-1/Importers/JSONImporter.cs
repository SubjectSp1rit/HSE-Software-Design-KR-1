using KR_1.Domain;

namespace KR_1.Importers;

public class JSONImporter : DataImporter
{
    protected override IEnumerable<IExportable> ParseData(string content)
    {
        Console.WriteLine("Парсинг данных из JSON файла");
        
        // тут типа надо парсить данные из JSON файла
        Console.WriteLine("Будем считать, что данные спарсились");
        
        return new List<IExportable>();
    }

    protected override void SaveData(IEnumerable<IExportable> data)
    {
        Console.WriteLine("Сохранение данных из JSON файла");
    }
}