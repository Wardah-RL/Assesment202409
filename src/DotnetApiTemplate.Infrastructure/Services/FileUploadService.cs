using Azure.Core;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models;
using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Files;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IGoogleStorageService _GoogleStorageService;
        private readonly IAmazonS3Service _AmazonS3Service;
        private readonly IAzureBlobService _azureBlobService;

        public FileUploadService(IDbContext dbContext,
               IConfiguration configuration,
               IFileService fileService,
               IGoogleStorageService googleStorageService,
               IAmazonS3Service amazonS3Service,
               IAzureBlobService azureBlobService)
        {
            _configuration = configuration;
            _fileService = fileService;
            _GoogleStorageService = googleStorageService;
            _AmazonS3Service = amazonS3Service;
            _azureBlobService = azureBlobService;
        }
        public async Task<FileResponse> UploadFileAsync(Stream stream, string containerName, string fileName,
        CancellationToken cancellationToken = default)
        {
            var fileStoreAt = _configuration.GetValue<FileStoreAt>(
                    "fileOptions:FileStoreAt");

            var fileResponse = new FileResponse(string.Empty);

            if (fileStoreAt == FileStoreAt.FileSystem)
            {
                fileResponse = await _fileService.UploadAsync(
                new FileRequest(fileName, stream),
                    cancellationToken);
            }
            else if (fileStoreAt == FileStoreAt.GcpBlob)
            {
                var fileGoogleResponse = await _GoogleStorageService.UploadAsync(stream,
                    containerName,
                    fileName,
                    cancellationToken);

                fileResponse.NewFileName = fileGoogleResponse.NewFileName;
            }else if (fileStoreAt == FileStoreAt.AwsBlob)
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                var fileAwsResponse = await _AmazonS3Service.UploadFile(fileName, memoryStream);

                if (fileAwsResponse)
                    fileResponse.NewFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}"; ;
            }else if(fileStoreAt == FileStoreAt.AzureBlob)
            {
                var fileAzureResponse = await _azureBlobService.UploadAsync(stream, containerName, fileName, cancellationToken);

                fileResponse.NewFileName = fileAzureResponse.NewFileName;
            }

            return fileResponse;
        }
    }
}
