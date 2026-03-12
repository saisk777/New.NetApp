using System;

namespace API.DTOs;

public class ImageUploadResult
{
    public required string Url { get; set; }         // Public Blob URL
    public  string? BlobName { get; set; }    // Name of the blob in storage
    public object Error { get; internal set; }
    // public required UploadError Error { get; set; }
}

// public class UploadError
// {
//     public string? Message { get; set; }
//     public string? Type { get; set; } // e.g. RequestFailedException, etc.
// }