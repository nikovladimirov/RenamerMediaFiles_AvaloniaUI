using MetadataExtractor;
using RenamerMediaFiles.Helpers;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.Tests;

public class MetadataTests
{    
    [Theory]
    [InlineData("./TestFiles/apple/2023-06-10 160451.JPG","2023.06.10 16:04:51")]
    [InlineData("./TestFiles/gopro/2023-04-29 145618.JPG","2023.04.29 14:56:18")]
    [InlineData("./TestFiles/huawei/2023-05-28 110651.jpg","2023.05.28 11:06:51")]
    public void CheckParsing_ExifIFD0_DateTime(string filePath, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, 0, expectedDate, DateSource.Exif_IFD0);
    }

    [Theory]
    [InlineData("./TestFiles/apple/2023-06-03 174548.mov",4,"2023.06.03 17:45:48")]
    // [InlineData("./TestFiles/gopro/2022-08-23 111241.mp4",3,"2022.08.23 11:12:41")]
    // [InlineData("./TestFiles/huawei/2023-06-17 213943.mp4", 4,"2023.06.17 21:39:43")]
    public void CheckParsing_QuickTimeMetadataHeader_DateTime(string filePath, float offserHour,string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, offserHour, expectedDate, DateSource.QuickTime_Metadata_Header);
    }

    [Theory]
    [InlineData("./TestFiles/apple/2023-06-03 174548.mov",4,"2023.06.03 17:45:48")]
    [InlineData("./TestFiles/gopro/2022-08-23 111241.mp4",3,"2022.08.23 11:12:41")]
    [InlineData("./TestFiles/huawei/2023-06-17 213943.mp4",4,"2023.06.17 21:39:43")]
    public void CheckParsing_QuickTimeMovieHeader_DateTime(string filePath,float offserHour, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo, offserHour,expectedDate, DateSource.QuickTime_Movie_Header);
    }
    
    [Theory]
    [InlineData("./TestFiles/apple/2023-06-03 174548.mov",4,"2023.06.03 17:45:48")]
    [InlineData("./TestFiles/gopro/2022-08-23 111241.mp4",3,"2022.08.23 11:12:41")]
    [InlineData("./TestFiles/huawei/2023-06-17 213943.mp4",4,"2023.06.17 21:39:43")]
    public void CheckParsing_QuickTimeTrackHeader_DateTime(string filePath, float offserHour, string expectedDateString)
    {
        var fileInfo = new FileInfo(filePath);
        var expectedDate = DateTime.Parse(expectedDateString);

        MetadataInfoTest(fileInfo,offserHour, expectedDate, DateSource.QuickTime_Track_Header);
    }

    
    private void MetadataInfoTest(FileInfo fileInfo, float offsetHour, DateTime expectedDate, string dateSource)
    {
        var metadataInfo = DefaultSettings.DefaultMetadataInfos[dateSource];
        var metadata = ImageMetadataReader.ReadMetadata(fileInfo.FullName);
        var result =FileItemModel.GetDateTime(metadata, metadataInfo.DateSource, metadataInfo, offsetHour);
        
        Assert.NotNull(result);
        
        Assert.Equal(expectedDate, result.Value);
    }
    
}