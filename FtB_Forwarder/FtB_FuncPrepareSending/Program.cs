using FtB_Common.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FtB_FuncPrepareSending
{
    class Program
    {
        static void Main(string[] args)
        {

            //var serviceProvider = new ServiceCollection()
            //.AddScoped<ArchivedItemQueueProcessor>()
            //.AddScoped<IBlobOperations, BlobOperations>()
            //.AddScoped<BlobStorage>()
            //.AddDistributorPrepareService()
            //.BuildServiceProvider();
            
            //.AddScoped<BlobStorage>()
            //.AddScoped<BlobOperations>()

            Console.WriteLine("Oppstart");

            //if (args != null || args.Length == 1)
            //{
            //    string archiveReference = args[0];
            //    var processor = serviceProvider.GetService<ArchivedItemQueueProcessor>();
            //    processor.ExecuteProcessingStrategy(archiveReference);
            //}
            //else
            //{
            //    Console.WriteLine("Bruk: arg1 (formatId), arg2 (prosess-steg (P/E/R)");
            //}
        }
    }
}
