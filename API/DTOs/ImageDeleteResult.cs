using System;

namespace API.DTOs;

public class ImageDeleteResult
{
    public bool IsDeleted { get; set; }
    public string? BlobName { get; set; }

}
