using FtB_Common.Adapters;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace AltinnServiceAdapters
{
    public class Altinn3PrefillAdapter : IPrefillAdapter
    {
        private readonly ILogger _logger;

        public Altinn3PrefillAdapter(ILogger<Altinn3PrefillAdapter> logger)
        {
            _logger = logger;
        }
        public PrefillResult SendPrefill(PrefillData prefillData)
        {
            _logger.LogDebug(@"*               _ _   _               ____   ___  ");
            _logger.LogDebug(@"*         /\   | | | (_)             |___ \ / _ \ ");
            _logger.LogDebug(@"*        /  \  | | |_ _ _ __  _ __     __) | | | |");
            _logger.LogDebug(@"*       / /\ \ | | __| | '_ \| '_ \   |__ <| | | |");
            _logger.LogDebug(@"*      / ____ \| | |_| | | | | | | |  ___) | |_| |");
            _logger.LogDebug(@"*     /_/    \_\_|\__|_|_| |_|_| |_| |____(_)___/ ");

            return new PrefillResult() { ResultMessage = "Eltinn tri esså", ResultType = PrefillResultType.Ok };
        }
    }
}
