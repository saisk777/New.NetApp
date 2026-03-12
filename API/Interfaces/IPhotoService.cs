using System;
using API.DTOs;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
public interface IPhotoService
{
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<ImageDeleteResult> DeletePhotoAsync(string blobName);
}
