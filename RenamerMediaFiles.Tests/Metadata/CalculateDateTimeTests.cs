using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Tests;

public class CalculateDateTimeTests
{
    [Theory]
    [InlineData(MetadataCaptions.Exif_IFD0,"./TestFiles/apple/20230630_152915.JPG", 0, "2023.06.30 15:29:15")]
    [InlineData(MetadataCaptions.Exif_IFD0,"./TestFiles/samsung/20230630_153027.jpg", 0, "2023.06.30 15:30:27")]
    [InlineData(MetadataCaptions.Exif_IFD0,"./TestFiles/huawei/20230630_152818.jpg", 0, "2023.06.30 15:28:18")]
    [InlineData(MetadataCaptions.QuickTime_Metadata,"./TestFiles/apple/20230630_152919.mov", 4, "2023.06.30 15:29:19")]
    [InlineData(MetadataCaptions.QuickTime_Movie,"./TestFiles/apple/20230630_152919.mov", 4, "2023.06.30 15:29:19")]
    [InlineData(MetadataCaptions.QuickTime_Movie,"./TestFiles/samsung/20230630_153036.mp4", 4, "2023.06.30 15:30:36")]
    [InlineData(MetadataCaptions.QuickTime_Movie,"./TestFiles/huawei/20230630_152820.mp4", 4, "2023.06.30 15:28:20")]
    [InlineData(MetadataCaptions.QuickTime_Track,"./TestFiles/apple/20230630_152919.mov", 4, "2023.06.30 15:29:19")]
    [InlineData(MetadataCaptions.QuickTime_Track,"./TestFiles/samsung/20230630_153036.mp4", 4, "2023.06.30 15:30:36")]
    [InlineData(MetadataCaptions.QuickTime_Track,"./TestFiles/huawei/20230630_152820.mp4", 4, "2023.06.30 15:28:20")]
    public void CalculateDateTime_WithFilesFromDifferentDevices_ReturnsExpectedDateTime(string metaInfo, string filePath, float offsetHour, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);
        var metadataInfo = DefaultSettings.DefaultMetadataInfos[metaInfo];
        var metaDateTimeExtenstions =DefaultSettings.MetaDateTimeExtensions; 
        metaDateTimeExtenstions.ForEach(x=>x.OffsetHour = offsetHour);
        var metadata = MediaMetadataWrapper.ReadMetadataDirectories(fileInfo.FullName);
        
        var metaDateTime = MediaMetadataWrapper.ReadDateTime(metadata, metadataInfo);
        if (metaDateTime == null)
        {
            Assert.NotNull(metaDateTime);
            return;
        }
        
        var calculatedDateTime = MediaMetadataWrapper.ApplyMetadataExtensions(metadata, metaDateTimeExtenstions, metaDateTime.Value);
        Assert.Equal(expectedDate, calculatedDateTime);
    }
}