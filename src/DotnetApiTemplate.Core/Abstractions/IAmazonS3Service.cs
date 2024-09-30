using Amazon.S3.Model;

namespace DotnetApiTemplate.Core.Abstractions
{
    public interface IAmazonS3Service
    {
        Task<bool> UploadFile(string filename, MemoryStream fileStream);
        Task<bool> UploadFileWithPrefix(string filename, string? prefix, MemoryStream fileStream);
        Task<GetObjectResponse> DownloadFile(string filename);
        Task<GetObjectResponse> DownloadFile(string bucketName, string filename);
    }
}
