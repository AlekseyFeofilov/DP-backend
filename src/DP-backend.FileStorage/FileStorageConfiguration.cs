using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DP_backend.FileStorage;

public class FileStorageConfiguration
{
    [StringLength(32, MinimumLength = 1)]
    public string BucketPrefix { get; set; }
}