using Ftb_DbModels;
using Ftb_Repositories.HttpClients;
using Ftb_Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ftb_Repositories
{
    public class FormMetadataRepository : IFormMetadataRepository
    {
        private string _archiveReference;
        private readonly ILogger<FormMetadataRepository> _logger;
        private readonly FormMetadataHttpClient _formMetadataClient;
        private FormMetadata _formMetadata;

        public FormMetadataRepository(ILogger<FormMetadataRepository> logger, FormMetadataHttpClient formMetadataClient)        
        {
            _logger = logger;
            _formMetadataClient = formMetadataClient;
        }

        public FormMetadata Get()
        {
            var result = _formMetadataClient.Get(_archiveReference);
            return result;
        }

        public void Update(FormMetadata formMetadata)
        {
            _formMetadata = formMetadata;
        }

        public void Save()
        {
            if (_formMetadata != null)
                _formMetadataClient.Update(_formMetadata);
        }

        public void SetArchiveReference(string archiveReference)
        {
            _archiveReference = archiveReference;
        }

        /*
                 public void UpdateValidationResultToFormMetadata(string archiveReference, string status, int errors, int warnings)
        {
            FormMetadata metadata = GetFormMetadata(archiveReference);
            metadata.Status = status;
            metadata.ValidationErrors = errors;
            metadata.ValidationWarnings = warnings;

            Update(metadata);
        }

        public void UpdateStatusToFormMetadata(string archiveReference, string status)
        {
            FormMetadata metadata = GetFormMetadata(archiveReference);
            metadata.Status = status;
            
            Update(metadata);
        }

        public void UpdateServiceCodeToFormMetadata(string archiveReference, string servicecode, string serviceeditioncode)
        {
            FormMetadata metadata = GetFormMetadata(archiveReference);
            metadata.ServiceCode = servicecode;
            metadata.ServiceEditionCode =  Convert.ToInt32(serviceeditioncode);

            Update(metadata);
        }


        public void SaveFormDataToFormMetadataLog(FormData formData)
        {
            FormMetadata metadata = GetFormMetadata(formData.ArchiveReference);

            if (formData.Mainform != null)
            {
                if (formData.Mainform.GetPropertyIdentifiers() != null)
                {
                    metadata.FormType = formData.Mainform.GetName();
                    metadata.Application = formData.Mainform.GetPropertyIdentifiers().FraSluttbrukersystem;

                    //More log data if present
                    metadata.ApplicantName = formData.Mainform.GetPropertyIdentifiers().AnsvarligSokerNavn;
                    metadata.ApplicantAddress = formData.Mainform.GetPropertyIdentifiers().AnsvarligSokerAdresselinje1;
                    metadata.PropertyFirstKnr = formData.Mainform.GetPropertyIdentifiers().Kommunenummer;
                    metadata.PropertyFirstGardsnr = formData.Mainform.GetPropertyIdentifiers().Gaardsnummer;
                    metadata.PropertyFirstBruksnr = formData.Mainform.GetPropertyIdentifiers().Bruksnummer;
                    metadata.PropertyFirstFestenr = formData.Mainform.GetPropertyIdentifiers().Festenummer;
                    metadata.PropertyFirstSeksjonsnr = formData.Mainform.GetPropertyIdentifiers().Seksjonsnummer;
                    metadata.PropertyFirstAddress = formData.Mainform.GetPropertyIdentifiers().Adresselinje1;
                    metadata.FirstActionType = formData.Mainform.GetPropertyIdentifiers().TiltakType;

                }
            }


            if (formData.ArchiveTimestamp != null)
                metadata.ArchiveTimestamp = formData.ArchiveTimestamp;

            if (!string.IsNullOrWhiteSpace(formData.SvarUtDocumentTitle))
                metadata.SvarUtDocumentTitle = formData.SvarUtDocumentTitle;
            
            Update(metadata);
        }

        public void SaveReceiver(Receiver receiver, string archiveReference)
        {
            FormMetadata metadata = GetFormMetadata(archiveReference);
            metadata.MunicipalityCode = receiver.GetMunicipalityCode();
            Update(metadata);
        }

        public void SaveMunicipalityArchiveCase(string archiveReference, int municipalityArchiveCaseYear, long municipalityArchiveCaseSequence)
        {
            FormMetadata metadata = GetFormMetadata(archiveReference);
            metadata.MunicipalityArchiveCaseYear = municipalityArchiveCaseYear;
            metadata.MunicipalityArchiveCaseSequence = municipalityArchiveCaseSequence;
            Update(metadata);
        }
         */
    }
}
