using DotnetApiTemplate.Domain.Entities;

namespace DotnetApiTemplate.UnitTests.Entities;

public class FileRepositoryTests
{
    [Fact]
    public void FileRepository_Ctor_Should_Do_As_Expected()
    {
        var fileRepository = new FileRepository();
        fileRepository.Id.ShouldNotBe(Guid.Empty);
        fileRepository.IsFileDeleted.ShouldBeFalse();
    }

    [Fact]
    public void FileRepository_DeleteTheFile_Should_Do_As_Expected()
    {
        var fileRepository = new FileRepository();
        fileRepository.IsFileDeleted.ShouldBeFalse();

        fileRepository.DeleteTheFile();

        fileRepository.IsFileDeleted.ShouldBeTrue();
    }
}