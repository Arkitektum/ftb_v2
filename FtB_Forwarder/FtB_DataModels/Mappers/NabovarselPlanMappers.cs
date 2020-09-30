using AutoMapper;

namespace FtB_DataModels.Mappers
{
    public static class NabovarselPlanMappers
    {
        public static IMapper GetNabovarselForslagsstillerTypeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType, no.kxml.skjema.dibk.nabovarselsvarPlan.ForslagsstillerType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.EnkelAdresseType, no.kxml.skjema.dibk.nabovarselsvarPlan.EnkelAdresseType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KodeType, no.kxml.skjema.dibk.nabovarselsvarPlan.KodeType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KontaktpersonType, no.kxml.skjema.dibk.nabovarselsvarPlan.KontaktpersonType>();

            });
            return config.CreateMapper();
        }

        public static IMapper GetNabovarselBerortPartMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.GjelderEiendomType, no.kxml.skjema.dibk.nabovarselsvarPlan.GjelderEiendomType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.MatrikkelnummerType, no.kxml.skjema.dibk.nabovarselsvarPlan.MatrikkelnummerType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.EiendommensAdresseType, no.kxml.skjema.dibk.nabovarselsvarPlan.EiendommensAdresseType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType, no.kxml.skjema.dibk.nabovarselsvarPlan.BeroertPartType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KodeType, no.kxml.skjema.dibk.nabovarselsvarPlan.KodeType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.EnkelAdresseType, no.kxml.skjema.dibk.nabovarselsvarPlan.EnkelAdresseType>();
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselPlan.KontaktpersonType, no.kxml.skjema.dibk.nabovarselsvarPlan.KontaktpersonType>();

            });
            return config.CreateMapper();
        }

        public static IMapper GetNabovarselReceiverMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<no.kxml.skjema.dibk.nabovarselsvarPlan.BeroertPartType, BerortPart>()
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
