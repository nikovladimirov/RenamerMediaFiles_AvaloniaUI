using Moq.AutoMock;
using RenamerMediaFiles.Models;

namespace RenamerMediaFiles.Tests;

public class FileItemModelTests
{
    [Theory]
    [InlineData("./TestFiles/apple/20230630_152915.JPG", "./TestFiles/apple",  @"\")]
    [InlineData("./TestFiles/apple/20230630_152915.JPG", "./TestFiles/",@"apple\")]
    public void CheckRelativePath_WithDifferentPathsArguments_ReturnsRelativePath(string filePath, string rootPath, string expectedValue)
    {
        var fileInfo = new FileInfo(filePath);
        var rootInfo = new FileInfo(rootPath);
        var mocker = new AutoMocker();
        var mockFileItemModel = mocker.CreateInstance<FileItemModel>();
        
        mockFileItemModel.RefreshFileInfo(fileInfo, rootInfo.FullName);

        Assert.Equal(expectedValue, mockFileItemModel.FilePathDisplayValue);
    }
    
    [Theory]
    [InlineData("20230630_152915 Test", "^\\d[\\.\\(\\)\\d _-]*(IMG_\\d+)?",   true, @"Test")]
    [InlineData("20230630_152915 Test", "^IMG[\\(\\)\\d _-]+(edit|COVER|BURST)?[\\(\\)\\d _-]+(COVER)?", false,"20230630_152915 Test")]
    public void GetAdditionalName_WithDifferentMasks_ReturnsAdditionalPath(string testFileName, string removingMask, bool expectedPassMask, string expectedValue)
    {
        var mocker = new AutoMocker();
        var mockFileItemModel = mocker.CreateInstance<FileItemModel>();

        var actualValue = mockFileItemModel.GetAdditionalName(testFileName, new[] { new StringModel(removingMask) });

        Assert.Equal(expectedPassMask, mockFileItemModel.UsedMask != null);
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData("./TestFiles/apple/20230630_152915.JPG",1,1)]
    [InlineData("./TestFiles/huawei/20230630_152820.mp4",1,2)]
    public void ReadAllMetadata_WithDefaultMetadataInfos_ReturnsMetadataCount(string relativeFilePath, int expectedMetaDataItems, int expectedMetaInfoCount)
    {
        var mocker = new AutoMocker();
        var stubSettings = mocker.CreateInstance<SettingsModel>();
        stubSettings.SetDefaultMetadataInfosMethod();
        mocker.Use(stubSettings);
        var mockFileItemModel = mocker.CreateInstance<FileItemModel>();
        
        mockFileItemModel.ReadAllMetadata(relativeFilePath, string.Empty, null);
        
        var actualMetaDataItems = mockFileItemModel.MetaDataItems.Count;
        var actualMetaInfoCount = actualMetaDataItems > 0 ? mockFileItemModel.MetaDataItems.First().MetaInfoCaptions.Count : 0;
        Assert.Equal(expectedMetaDataItems, actualMetaDataItems);
        Assert.Equal(expectedMetaInfoCount, actualMetaInfoCount);
    }
}