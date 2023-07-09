using MetadataExtractor.Formats.FileSystem;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Tests;

public class MetadataExtensionTests
{
    
    [Theory]
    [InlineData(MetaTypes.AttributeName)]
    [InlineData(MetaTypes.AttributeTag)]
    [InlineData(MetaTypes.AttributeValue)]
    public void ApplyExtension_ContainAttribute_ReturnsExpectedDateTime(string attributeType)
    {
        var sourceDateTime = DateTime.MinValue;
        var offsetHour = 1.5f;
        var expectedDate = sourceDateTime.AddHours(offsetHour);
        var metadataStub = new FileMetadataDirectory();
        metadataStub.Set(FileMetadataDirectory.TagFileName, "testfile");
        var extensionStub = new MetaDateTimeExtension(metadataStub.Name, attributeType, metadataStub.Name, "File Name", "testfile", offsetHour);

        var result = MediaMetadataWrapper.ApplyMetadataExtensions(new[] { metadataStub }, new[] { extensionStub }, sourceDateTime);
        Assert.Equal(expectedDate, result);
    }
}