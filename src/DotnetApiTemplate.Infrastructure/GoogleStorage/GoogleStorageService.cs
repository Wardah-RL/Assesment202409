using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.GoogleStorage
{
    public class GoogleStorageService : IGoogleStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly UrlSigner _urlSigner;
        private readonly GoogleStorageOptions _options;
        private readonly ILogger<GoogleStorageService> _logger;

        public GoogleStorageService(GoogleStorageOptions options, ILogger<GoogleStorageService> logger)
        {
            _options = options;

            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(_options.ServiceAccountKeyPath));

            var path = Assembly.GetExecutingAssembly().Location.Replace("BFashion.Hotel.Membership.Infrastructure.dll", "");
            var filePath = Path.Combine(path, "GoogleStorage", _options.ServiceAccountKeyPath!);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The specified ServiceAccountKeyPath file does not exist, {_options.ServiceAccountKeyPath}");

            GoogleCredential googleCredential = GoogleCredential.FromFile(filePath).CreateScoped(_options.Scopes);

            _storageClient = StorageClient.Create(googleCredential);
            _urlSigner = UrlSigner.FromCredential(googleCredential);
            _logger = logger;
        }

        public async Task<GoogleCloudStorageResponse> UploadAsync(Stream stream, string objectName, string fileName, CancellationToken cancellationToken = default)
        {
            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var newObjectName = $"{objectName}/{newFileName}";

            _ = await _storageClient.UploadObjectAsync(_options.BucketName, newObjectName, "application/octet-stream", stream, null, cancellationToken);

            return new GoogleCloudStorageResponse
            {
                NewFileName = newFileName
            };
        }

        public async Task<bool> DeleteAsync(string objectName, string fileName, CancellationToken cancellationToken = default)
        {
            var newObjectName = $"{objectName}/{fileName}";
            try
            {
                await _storageClient.DeleteObjectAsync(_options.BucketName, newObjectName, null, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Sending email message is complated : {ex}");
                return false;
            }
        }

        public async Task<byte[]> DownloadAsync(string objectName, string fileName, CancellationToken cancellationToken = default)
        {
            var newObjectName = $"{objectName}/{fileName}";

            MemoryStream stream = new();
            await _storageClient.DownloadObjectAsync(_options.BucketName, newObjectName, stream, null, cancellationToken);

            stream.Seek(0, SeekOrigin.Begin);
            return stream.ToArray();
        }

        public async Task<Uri> GenerateSignedUrlAsync(string objectName, string fileName, CancellationToken cancellationToken = default)
        {
            var newObjectName = $"{objectName}/{fileName}";

            string uri = await _urlSigner.SignAsync(_options.BucketName, newObjectName, TimeSpan.FromHours(1), cancellationToken: cancellationToken);
            return new Uri(uri);
        }

        public async Task CreateBucketAsync(string bucketName)
        {
            var bucket = await _storageClient.GetBucketAsync(bucketName);
            if (bucket is not null) return;

            await _storageClient.CreateBucketAsync(_options.ProjectId, bucketName);
        }
    }
}
