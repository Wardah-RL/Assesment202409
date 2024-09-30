﻿using DotnetApiTemplate.Shared.Infrastructure.Logging.Options;
using FileOptions = DotnetApiTemplate.Shared.Infrastructure.Logging.Options.FileOptions;

namespace DotnetApiTemplate.Shared.Infrastructure.Logging;

internal sealed class LoggerOptions
{
    public LoggerOptions()
    {
        Overrides = new Dictionary<string, string>();
        ExcludePaths = new List<string>();
        ExcludeProperties = new List<string>();
        Tags = new Dictionary<string, object>();
        Level = "Information";
    }

    public string Level { get; set; }
    public ConsoleOptions? Console { get; set; }
    public FileOptions? File { get; set; }
    public SeqOptions? Seq { get; set; }
    public IDictionary<string, string>? Overrides { get; set; }
    public List<string>? ExcludePaths { get; set; }
    public List<string>? ExcludeProperties { get; set; }
    public IDictionary<string, object>? Tags { get; set; }
}