using KR_1.Domain;

namespace KR_1.Importers;

public class CSVImporter : DataImporter
{
    protected override IEnumerable<IExportable> ParseData(string content)
    {
        Console.WriteLine("Парсинг данных из CSV файла");
        
        // тут типа надо парсить данные из CSV файла
        Console.WriteLine("Будем считать, что данные спарсились");
        
        return new List<IExportable>();
    }

    protected override void SaveData(IEnumerable<IExportable> data)
    {
        Console.WriteLine("Сохранение данных из CSV файла");
    }
}