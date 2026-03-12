using System;

namespace API.Helpers;

public class AzureBlobSettings
{
    public required string ConnectionString { get; set; }
    public required string ContainerName { get; set; }

}

