using KR_1.Domain;
using KR_1.Importers;

namespace KR_1_Tests.Mock_objects;

class TestImporter : DataImporter
{
    public List<string> ParsedData { get; } = new List<string>();
    protected override IEnumerable<IExportable> ParseData(string content)
    {
        ParsedData.Add(content);
        return new List<IExportable>();
    }
    protected override void SaveData(IEnumerable<IExportable> data)
    {
        // Заглушка для теста
    }
}