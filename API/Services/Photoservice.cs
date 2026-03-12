using System;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace API.Services;

public class Photoservice : IPhotoService
{
    private readonly AzureBlobSettings _settings;
    public Photoservice(IOptions<AzureBlobSettings> settings)
    {
        _settings = settings.Value;

    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return new ImageUploadResult
            {
                Url = blobClient.Uri.ToString(),
                BlobName = blobName
            };
        }

        public async Task<ImageDeleteResult> DeletePhotoAsync(string blobName)
        {
            var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var deleted = await blobClient.DeleteIfExistsAsync();

            return new ImageDeleteResult
            {
                IsDeleted = deleted.Value,
                BlobName = blobName
            };
    }
}
