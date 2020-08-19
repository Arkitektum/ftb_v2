using MetadataProvider.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetadataProvider.Providers
{
    [ProviderType(Id = "StaticProvider")]
    public class ComicStaticProvider : IComicProvider
    {
        public Task<IEnumerable<ComicItem>> GetMetadata()
        {
            return GetStaticContent();
        }

        private async Task<IEnumerable<ComicItem>> GetStaticContent()
        {
            var result = new List<ComicItem>
            {
                new ComicItem() { 
                    Month = "11", 
                    Num=666, 
                    Year="2009", 
                    Safe_Title="Silent Hammer", 
                    Transcript = @"[[Hat guy is hammering something on a table.]]\nGuy: What--\nHat Guy: Silent hammer. I've made a set of silent tools.\nGuy: Why?\nHammer: <<whoosh whoosh whoosh>>\n\nHat Guy: Stealth carpentry. Breaking into a house at night and moving windows, adjusting walls, etc.\n[[He takes his silent hammer over to a tool bench with other things on it. Two boxes underneath are labeled \""Drills\"" and \""Non-Drills.\""]]\n\nHat Guy, narrating: After a week or so of questioning his own sanity, the owner will stay up to watch the house at night. I'll make scratching noises in the walls, pipe in knockout gas, move him up to his bed, and never bother him again.\n[[The events he's describing are shown in two mini-panels below.]]\n\nGuy, off-panel: Nice prank, I guess, but what's the point?\nHat Guy: Check out the owner's card, on the table.\nGuy, off-panel: Chair of the American Skeptics Society? Oh, god.\nHat guy: Yeah, this doesn't end well for him.\n\n{{Title text: I bet he'll keep quiet for a couple weeks and then-- wait, did you nail a piece of scrap wood to my antique table a moment ago?}}",
                    Alt="I bet he'll keep quiet for a couple weeks and then-- wait, did you nail a piece of scrap wood to my antique table a moment ago?", Img="https://imgs.xkcd.com/comics/silent_hammer.png", 
                    Title="Safe Hammer", 
                    Day="23"
                }
            };
            return result;
        }
    }
}
