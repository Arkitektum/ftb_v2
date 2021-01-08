using System.ComponentModel;

namespace FtB_Common.BusinessModels
{
    public enum ActorType
    {
        Privatperson,
        Foretak,
        [Description("Offentlig myndighet")]
        OffentligMyndighet,
        Organisasjon
    }
}
