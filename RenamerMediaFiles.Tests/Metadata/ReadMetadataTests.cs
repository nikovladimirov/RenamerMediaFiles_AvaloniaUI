using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Tests;

public class ReadMetadataTests
{
    [Theory]
    [InlineData("./TestFiles/apple/20230630_152915.JPG", "Exif IFD0", "Date/Time")]
    [InlineData("./TestFiles/apple/20230630_152919.mov", "QuickTime Metadata Header", "Creation Date")]
    [InlineData("./TestFiles/huawei/20230630_152820.mp4", "QuickTime Movie Header", "Created")]
    [InlineData("./TestFiles/samsung/20230630_153036.mp4", "QuickTime Track Header", "Created")]
    public void CheckExistTag_WithFilesFromDifferentDevices_ReturnsNotNull(string filePath, string metadataName, string metadataTag)
    {
        var fileInfo = new FileInfo(filePath);
        var metadata = MediaMetadataWrapper.ReadMetadata(fileInfo.FullName);

        var directory = metadata.FirstOrDefault(x => x.Name == metadataName);
        var tag = directory?.Tags.FirstOrDefault(x => string.Equals(x.Name, metadataTag));
        
        Assert.NotNull(directory);
        Assert.NotNull(tag);
    }
}