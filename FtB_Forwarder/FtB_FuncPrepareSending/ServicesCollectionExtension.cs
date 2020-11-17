using FtB_Common.Encryption;
using FtB_Common.FormDataRepositories;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_FormLogic;
using Ftb_Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FtB_FuncPrepareSending
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddPrepareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<FormatIdToFormMapper>();
            services.AddScoped<VarselOppstartPlanarbeidPrepareLogic>();
            services.AddScoped<IFormDataRepo, FormDataRepository>();
            services.AddScoped<ITableStorage, TableStorage>();
            services.AddScoped<IBlobOperations, BlobOperations>();
            services.AddScoped<PrivateBlobStorage>();
            services.AddScoped<PublicBlobStorage>();
            services.AddFtbRepositories(configuration);

            services.AddScoped<IDecryption, Decryption>();
            services.AddScoped<IDecryptionFactory, DecryptionFactory>();
            services.Configure<EncryptionSettings>(configuration.GetSection("EncryptionSettings"));

            //Test reporter
            //services.AddScoped<TestPrepareStrategy>();
            //services.AddScoped<Func<StrategyTypeEnum, IStrategy<List<SendQueueItem>, SubmittalQueueItem>>>(serviceProvider => key =>
            //{
            //    if (key == StrategyTypeEnum.Distribution)
            //        return serviceProvider.GetService<TestPrepareStrategy>();
            //    else return null;
            //});

            return services;
        }
    }
}
