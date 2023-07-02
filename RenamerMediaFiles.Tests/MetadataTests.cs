using MetadataExtractor;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.Tests;

public class MetadataTests
{
    [Theory]
    [InlineData("./TestFiles/apple/20230630_152915.JPG", "2023.06.30 15:29:15")]
    [InlineData("./TestFiles/samsung/20230630_153027.jpg", "2023.06.30 15:30:27")]
    [InlineData("./TestFiles/huawei/20230630_152818.jpg", "2023.06.30 15:28:18")]
    [InlineData("./TestFiles/gopro/20230630_154155.JPG", "2023.06.30 15:41:55")]
    public void CheckParsing_ExifIFD0_DateTime(string filePath, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, 0, expectedDate, DateSource.Exif_IFD0);
    }

    [Theory]
    [InlineData("./TestFiles/apple/20230630_152919.mov", 4, "2023.06.30 15:29:19")]
    public void CheckParsing_QuickTimeMetadata_DateTime(string filePath, float offserHour,
        string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, offserHour, expectedDate, DateSource.QuickTime_Metadata);
    }

    [Theory]
    [InlineData("./TestFiles/apple/20230630_152919.mov", 4, "2023.06.30 15:29:19")]
    [InlineData("./TestFiles/samsung/20230630_153036.mp4", 4, "2023.06.30 15:30:36")]
    [InlineData("./TestFiles/huawei/20230630_152820.mp4", 4, "2023.06.30 15:28:20")]
    [InlineData("./TestFiles/gopro/20230701_223321.MP4", 4, "2023.07.01 22:33:21")]
    public void CheckParsing_QuickTimeMovie_DateTime(string filePath, float offserHour, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, offserHour, expectedDate, DateSource.QuickTime_Movie);
    }

    [Theory]
    [InlineData("./TestFiles/apple/20230630_152919.mov", 4, "2023.06.30 15:29:19")]
    [InlineData("./TestFiles/samsung/20230630_153036.mp4", 4, "2023.06.30 15:30:36")]
    [InlineData("./TestFiles/huawei/20230630_152820.mp4", 4, "2023.06.30 15:28:20")]
    [InlineData("./TestFiles/gopro/20230701_223321.MP4", 4, "2023.07.01 22:33:21")]
    public void CheckParsing_QuickTimeTrack_DateTime(string filePath, float offserHour, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, offserHour, expectedDate, DateSource.QuickTime_Track);
    }


    private void MetadataInfoTest(FileInfo fileInfo, float offsetHour, DateTime expectedDate, string dateSource)
    {
        var metadataInfo = DefaultSettings.DefaultMetadataInfos[dateSource];
        metadataInfo.OffsetHour = offsetHour;

        var metadata = ImageMetadataReader.ReadMetadata(fileInfo.FullName);
        var result = FileItemModel.GetDateTime(metadata, metadataInfo);

        Assert.NotNull(result);

        Assert.Equal(expectedDate, result.Value);
    }
}