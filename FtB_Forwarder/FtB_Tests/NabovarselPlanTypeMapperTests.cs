using FtB_DataModels.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FtB_Tests
{
    [TestClass]
    public class NabovarselPlanTypeMapperTests
    {
        [TestMethod]
        public void Forslagstiller_success()
        {
            var nabovarsel = new no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType();
            nabovarsel.forslagsstiller = new no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType()
            {
                adresse = new no.kxml.skjema.dibk.nabovarselPlan.EnkelAdresseType()
                {
                    adresselinje1 = "adresse1",
                    adresselinje2 = "adresse2",
                    adresselinje3 = "adresse3",
                    landkode = "no",
                    postnr = "0101",
                    poststed = "haugenstua"
                },
                epost = "epost@test.no",
                foedselsnummer = "12345678912",
                navn = "Ola Halvorsen",
                kontaktperson = new no.kxml.skjema.dibk.nabovarselPlan.KontaktpersonType()
                { epost = "kontaktperson@epost.no", mobilnummer = "98765432", navn = "Mæhmed", telefonnummer = "12345678" }
                ,
                partstype = new no.kxml.skjema.dibk.nabovarselPlan.KodeType() { kodebeskrivelse = "kodebeskrivelse", kodeverdi = "kodeverdi" }
                ,
                telefon = "98765432",
                organisasjonsnummer = "98765432"
            };

            var result = NabovarselPlanMappers.GetNabovarselForslagsstillerTypeMapper().Map<no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType, no.kxml.skjema.dibk.nabovarselsvarPlan.ForslagsstillerType>(nabovarsel.forslagsstiller);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BerorteParter_success()
        {
            var nabovarsel = new no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType();
            var berortList = new List<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType>();
            berortList.Add(new no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType()
            {
                adresse = new no.kxml.skjema.dibk.nabovarselPlan.EnkelAdresseType()
                {
                    adresselinje1 = "adresse1",
                    adresselinje2 = "adresse2",
                    adresselinje3 = "adresse3",
                    landkode = "no",
                    postnr = "0101",
                    poststed = "haugenstua"
                },
                beskrivelseForVarsel = "beskrivelse for varsel 1",
                epost = "epost@test.no",
                foedselsnummer = "12345678912",
                navn = "Ola Halvorsen",
                kontaktperson = new no.kxml.skjema.dibk.nabovarselPlan.KontaktpersonType()
                { epost = "kontaktperson@epost.no", mobilnummer = "98765432", navn = "Mæhmed", telefonnummer = "12345678" }
                ,
                partstype = new no.kxml.skjema.dibk.nabovarselPlan.KodeType() { kodebeskrivelse = "kodebeskrivelse", kodeverdi = "kodeverdi" }
                ,
                telefon = "98765432",
                organisasjonsnummer = "98765432",
                gjelderEiendom = (new List<no.kxml.skjema.dibk.nabovarselPlan.GjelderEiendomType>()
                {
                    new no.kxml.skjema.dibk.nabovarselPlan.GjelderEiendomType()
                    {
                        adresse = new no.kxml.skjema.dibk.nabovarselPlan.EiendommensAdresseType()
                        {
                            adresselinje1 = "adresse1",
                            adresselinje2 = "adresse2",
                            adresselinje3 = "adresse3",
                            landkode = "no",
                            postnr = "0101",
                            poststed = "haugenstua",
                            bokstav ="A",
                            gatenavn ="Down town",
                            husnr = "123"
                        },
                        bolignummer = "1111",
                        bygningsnummer = "5555",
                        eiendomsidentifikasjon = new no.kxml.skjema.dibk.nabovarselPlan.MatrikkelnummerType()
                        { 
                            bruksnummer = "brnr",
                            festenummer = "festenr",
                            gaardsnummer = "gardsnummer",
                            kommunenummer = "kommunenummer",
                            seksjonsnummer = "seksjonsnummer"
                        }, kommunenavn = "Jadda"

                    },
                      new no.kxml.skjema.dibk.nabovarselPlan.GjelderEiendomType()
                    {
                        adresse = new no.kxml.skjema.dibk.nabovarselPlan.EiendommensAdresseType()
                        {
                            adresselinje1 = "adresse1-2",
                            adresselinje2 = "adresse2-2",
                            adresselinje3 = "adresse3-2",
                            landkode = "no",
                            postnr = "0101",
                            poststed = "haugenstua",
                            bokstav ="B",
                            gatenavn ="Down town",
                            husnr = "123"
                        },
                        bolignummer = "1111",
                        bygningsnummer = "5555",
                        eiendomsidentifikasjon = new no.kxml.skjema.dibk.nabovarselPlan.MatrikkelnummerType()
                        {
                            bruksnummer = "brnr",
                            festenummer = "festenr",
                            gaardsnummer = "gardsnummer",
                            kommunenummer = "kommunenummer",
                            seksjonsnummer = "seksjonsnummer"
                        }, kommunenavn = "Jadda"

                    }
                }).ToArray()


            });

            nabovarsel.beroerteParter = berortList.ToArray();

            var result = NabovarselPlanMappers.GetNabovarselBerortPartMapper()
                .Map<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType, no.kxml.skjema.dibk.nabovarselsvarPlan.BeroertPartType>(nabovarsel.beroerteParter.First());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.gjelderEiendom.Count(), 2);
        }
    }
}
