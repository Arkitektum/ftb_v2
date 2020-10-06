using AutoMapper;
using System.Linq;

namespace FtB_DataModels.Mappers
{
    public static class NabovarselPlanAltinn3Mappers
    {
        public static IMapper GetNabovarselForslagsstillerTypeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType, FtB_DataModels.Datamodels.NabovarelPlan.ForslagsstillerType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.EnkelAdresseType, FtB_DataModels.Datamodels.NabovarelPlan.EnkelAdresseType >();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KodeType, FtB_DataModels.Datamodels.NabovarelPlan.KodeType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KontaktpersonType, FtB_DataModels.Datamodels.NabovarelPlan.KontaktpersonType>();

            });
            return config.CreateMapper();
        }

        public static IMapper GetNabovarselBerortPartMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType, FtB_DataModels.Datamodels.NabovarelPlan.BeroertPartType>()
                //.ForMember(dest => dest.gjelderEiendom.gjeldereiendom, opt => opt.MapFrom(src => src.gjelderEiendom.ToList()))
                
                ;
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.GjelderEiendomType[], FtB_DataModels.Datamodels.NabovarelPlan.GjelderEiendomListe>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.GjelderEiendomType, FtB_DataModels.Datamodels.NabovarelPlan.GjelderEiendomType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.MatrikkelnummerType, FtB_DataModels.Datamodels.NabovarelPlan.MatrikkelnummerType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.EiendommensAdresseType, FtB_DataModels.Datamodels.NabovarelPlan.EiendommensAdresseType>();
                
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KodeType, FtB_DataModels.Datamodels.NabovarelPlan.KodeType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.EnkelAdresseType, FtB_DataModels.Datamodels.NabovarelPlan.EnkelAdresseType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KontaktpersonType, FtB_DataModels.Datamodels.NabovarelPlan.KontaktpersonType>()
                
                
                ;

            });
            return config.CreateMapper();
        }

        public static IMapper GetNabovarselReceiverMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap< FtB_DataModels.Datamodels.NabovarelPlan.BeroertPartType, BerortPart>()
                .ForMember(dest => dest.Ssn, opt => opt.MapFrom(src => src.foedselsnummer))
                .ForMember(dest => dest.Orgnr, opt => opt.MapFrom(src => src.organisasjonsnummer))
                .ForMember(dest => dest.Epost, opt => opt.MapFrom(src => src.epost))
                .ForMember(dest => dest.Kommentar, opt => opt.MapFrom(src => src.kommentar))
                .ForMember(dest => dest.Navn, opt => opt.MapFrom(src => src.navn))
                .ForMember(dest => dest.SystemReferanse, opt => opt.MapFrom(src => src.systemReferanse))
                .ForMember(dest => dest.Telefon, opt => opt.MapFrom(src => src.telefon))
                .ForSourceMember(s => s.adresse, opt => opt.DoNotValidate())
                .ForSourceMember(s => s.kontaktperson, opt => opt.DoNotValidate())
                .ForSourceMember(s => s.gjelderEiendom, opt => opt.DoNotValidate())
                .ForSourceMember(s => s.partstype, opt => opt.DoNotValidate())
                ;
            });
            return config.CreateMapper();
        }
    }
}
