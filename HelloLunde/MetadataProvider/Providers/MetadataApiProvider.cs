using MetadataProvider.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataProvider.Providers
{
    [ProviderType(Id = "ApiProvider")]
    public class MetadataApiProvider : IMetadataProvider
    {
        private readonly XkcdService _xkcdService;

        public MetadataApiProvider(XkcdService xkcdService)
        {
            _xkcdService = xkcdService;
        }
        public async Task<IEnumerable<MetadataItem>> GetMetadata()
        {
            var listOfTasks = new List<Task<MetadataItem>>();

            foreach (var comicNumber in GenerateRandomComicNumbers())
                listOfTasks.Add(_xkcdService.GetComic(comicNumber));

            return await Task.WhenAll(listOfTasks);
        }

        private int[] GenerateRandomComicNumbers()
        {
            var numberOfComics = new Random().Next(10);

            int[] comicNumbers = new int[numberOfComics];

            for (int i = 0; i < numberOfComics; i++)
            {
                comicNumbers[i] = new Random().Next(2345);
            }
            return comicNumbers;
        }
    }
}
