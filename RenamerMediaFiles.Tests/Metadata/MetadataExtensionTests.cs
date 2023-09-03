using MetadataExtractor.Formats.FileSystem;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Tests;

public class MetadataExtensionTests
{
    [Theory]
    [InlineData(MetadataTypes.DirectoryName)]
    [InlineData(MetadataTypes.TagName)]
    [InlineData(MetadataTypes.TagDescription)]
    public void ApplyExtension_ContainAttribute_ReturnsExpectedDateTime(string attributeType)
    {
        var sourceDateTime = DateTime.MinValue;
        var offsetHour = 1.5f;
        var expectedDate = sourceDateTime.AddHours(offsetHour);
        var metadataDirectory = new FileMetadataDirectory();
        metadataDirectory.Set(FileMetadataDirectory.TagFileName, "testfile");
        var extension = new MetaDateTimeExtension(metadataDirectory.Name, attributeType, metadataDirectory.Name, "File Name", "testfile", offsetHour);

        var result = MediaMetadataWrapper.ApplyMetadataExtensions(new[] { metadataDirectory }, new[] { extension }, sourceDateTime);
        
        Assert.Equal(expectedDate, result);
    }
}