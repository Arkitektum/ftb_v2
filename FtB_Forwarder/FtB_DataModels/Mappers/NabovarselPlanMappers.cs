using AutoMapper;

namespace FtB_DataModels
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
    }
}
