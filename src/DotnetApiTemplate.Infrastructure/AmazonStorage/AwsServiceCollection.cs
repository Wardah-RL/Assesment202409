using Amazon.Runtime;
using DotnetApiTemplate.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.AmazonStorage
{
    public static class AwsServiceCollection
    {
        public static IServiceCollection AddAws(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AwsConfigurationOptions>(configuration.GetSection(nameof(AwsConfigurationOptions)));

            services.AddScoped<IAmazonS3Service, AmazonS3Service>();

            return services;
        }
    }
}
