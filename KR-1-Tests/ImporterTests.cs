using KR_1_Tests.Mock_objects;
using KR_1.Importers;

namespace KR_1_Tests;

public class ImporterTests
{
    [Fact]
    public void DataImporter_ParsesFileContentCorrectly()
    {
        // Arrange
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, "Test content");
        var importer = new TestImporter();

        // Act
        importer.Import(tempFile);

        // Assert
        Assert.Contains("Test content", importer.ParsedData);
        File.Delete(tempFile);
    }
    
    [Fact]
    public void CSVImporter_WithNonExistingFile_DoesNotThrowException()
    {
        // Arrange
        var importer = new CSVImporter();
        string nonExistingFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".csv");

        // Act & Assert
        var ex = Record.Exception(() => importer.Import(nonExistingFile));
        Assert.Null(ex);
    }
}