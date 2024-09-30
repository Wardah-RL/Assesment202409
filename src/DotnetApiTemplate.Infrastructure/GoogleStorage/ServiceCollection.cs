using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Shared.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.GoogleStorage
{
    public static class ServiceCollection
    {
        public static void AddGoogleStorage(this IServiceCollection services, IConfiguration configuration, string configName = "GoogleStorage")
        {
            var options = services.GetOptions<GoogleStorageOptions>(configName);
            ArgumentException.ThrowIfNullOrEmpty(nameof(options));

            services.TryAddSingleton(options);
            services.AddHttpClient<IGoogleStorageService, GoogleStorageService>();
            services.AddScoped<IGoogleStorageService, GoogleStorageService>();
        }
    }
}
