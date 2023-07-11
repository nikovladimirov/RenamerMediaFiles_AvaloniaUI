using Moq;
using Moq.AutoMock;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Tests;

public class FilesReaderTest
{
    [Theory]
    [InlineData("./TestFiles/apple", 2, 2, 4)]
    [InlineData("./TestFiles/samsung", 2,2,3)]
    [InlineData("./TestFiles/huawei", 2,2,3)]
    [InlineData("./TestFiles/gopro", 2,2,3)]
    public void ReadRealCountFilesAndMetadata_WithDifferentFiles_ReturnsMetadataCount(string rootPath, int expectedFilesCount,
        int expectedMetadataCount, int expectedCaptionsCount)
    {
        var mocker = new AutoMocker();
        var stubSettings = mocker.CreateInstance<SettingsModel>();
        stubSettings.SetDefaultMetadataInfosMethod();
        mocker.Use(stubSettings);
        var mockFilesReader = new Mock<FilesReader>(
            rootPath,
            stubSettings.ExtensionText,
            stubSettings.NewNameFormat,
            stubSettings.ChangeNameByMasks,
            stubSettings.RemovingByMasks
        );
        mockFilesReader.Setup(fr => fr.CreateFileItemModelInstance()).Returns(() => mocker.CreateInstance<FileItemModel>());

        var result = mockFilesReader.Object.ReadFiles();

        Assert.Equal(expectedFilesCount, result.Count);
        Assert.Equal(expectedMetadataCount, result.Sum(x => x.MetaDataItems.Count));
        Assert.Equal(expectedCaptionsCount, result.Sum(x => x.MetaDataItems.Sum(y => y.MetaInfoCaptions.Count)));
    }
}