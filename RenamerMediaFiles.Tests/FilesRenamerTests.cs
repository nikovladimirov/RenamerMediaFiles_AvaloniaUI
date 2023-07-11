using Moq;
using Moq.AutoMock;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Workers;

namespace RenamerMediaFiles.Tests;

public class FilesRenamerTests
{
    [Theory]
    [InlineData("./FakeFiles/fakeFile.JPG", "fakeFile", true, false)]
    [InlineData("./FakeFiles/fakeFile.JPG", "fakeFile2",true,true)]
    [InlineData("./FakeFiles/fakeFile.JPG", "fakeFile2",false,false)]
    public void RenameFiles_WithDifferentParameters_ReturnsCountRenamedFiles(string fileRelativePath, string newName, bool selectedForRenaming, bool expectedRenamed)
    {
        var fileInfo = new FileInfo(fileRelativePath);
        var mocker = new AutoMocker();
        var file = mocker.CreateInstance<FileItemModel>();
        file.RefreshFileInfo(fileInfo, fileInfo.DirectoryName);
        file.MetaDataItems.Add(new MetadataItemModel("test", DateTime.MinValue, newName) { Selected = selectedForRenaming });
        var mockRenamer = new Mock<FilesRenamer>();
        mockRenamer.Setup(r => r.RenameActual(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = mockRenamer.Object.Rename(new List<FileItemModel> { file });

        Assert.Equal(expectedRenamed, result.isOk && mockRenamer.Object.CountRename == 1);
    }
}