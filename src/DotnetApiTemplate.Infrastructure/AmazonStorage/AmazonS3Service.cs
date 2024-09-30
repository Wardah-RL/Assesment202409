using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Infrastructure.AmazonStorage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.AmazonStorage
{
    public class AmazonS3Service : IAmazonS3Service
    {
        private readonly AwsConfigurationOptions _awsConfiguration;
        private readonly IAmazonS3 _client;

        public AmazonS3Service(IOptions<AwsConfigurationOptions> options)
        {
            _awsConfiguration = options.Value;
            var credentials = new BasicAWSCredentials(_awsConfiguration.AccessKey, _awsConfiguration.SecretKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_awsConfiguration.BucketLocation)
            };
            _client = new AmazonS3Client(credentials, config);
        }

        public async Task<bool> UploadFile(string filename, MemoryStream fileStream)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                //FilePath = "/",
                Key = filename, // filename
                BucketName = _awsConfiguration.BucketName// bucket name of S3
            };

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return true;
        }

        public async Task<bool> UploadFileWithPrefix(string filename, string? prefix, MemoryStream fileStream)
        {

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                //FilePath = FilePath,
                Key = string.IsNullOrEmpty(prefix) ? filename : $"{prefix?.TrimEnd('/')}/{filename}", // filename
                BucketName = $"{_awsConfiguration.BucketName}" // bucket name of S3
            };

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return true;
        }

        public async Task<GetObjectResponse> DownloadFile(string filename)
        {
            return await _client.GetObjectAsync(_awsConfiguration.BucketName, filename);
        }
        public async Task<GetObjectResponse> DownloadFile(string bucketName, string filename)
        {
            return await _client.GetObjectAsync(bucketName, filename);
        }
    }
}
