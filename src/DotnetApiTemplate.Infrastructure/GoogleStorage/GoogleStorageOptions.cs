using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.GoogleStorage
{
    public class GoogleStorageOptions
    {
        public string? ServiceAccountKeyPath { get; set; }
        public string? ProjectId { get; set; }
        public string? BucketName { get; set; }
        public string[] Scopes { get; set; } = [];
    }
}
