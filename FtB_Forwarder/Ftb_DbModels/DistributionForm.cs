using System;
using System.ComponentModel.DataAnnotations;

namespace Ftb_DbModels
{
    public class DistributionForm 
    {        
        public Guid Id { get; set; }

        /// <summary>
        /// ID på innsendt distribusjonstjeneste (opprinnelsen)
        /// </summary>
        private string _archiveReference;
        public string InitialArchiveReference { get { return _archiveReference; } set { _archiveReference = value.ToUpper(); } }
        /// <summary>
        /// Sluttbrukersystem sin referanse til forsendelsene (Hovedinnsendingsnr i distribusjon skjema datamodell)
        /// </summary>
        public string InitialExternalSystemReference { get; set; }
        /// <summary>
        /// Type distribusjonstjeneste
        /// </summary>
        public string DistributionType { get; set; }
        /// <summary>
        /// Referanse til preutfyllingsfunsjonen
        /// </summary>
        public string SubmitAndInstantiatePrefilledFormTaskReceiptId { get; set; }

        /// <summary>
        /// Tidspunkt prefill er sendt
        /// </summary>
        public DateTime? SubmitAndInstantiatePrefilled { get; set; }
        /// <summary>
        /// Sluttbrukersystem sin referanse til forsendelse (VaarReferanse i skjema datamodell)
        /// </summary>
        public string ExternalSystemReference { get; set; }
        /// <summary>
        /// Referanse til signert preutfylt innsending
        /// </summary>        
        [StringLength(maximumLength: 20)]
        public string SignedArchiveReference { get; set; }
        /// <summary>
        /// Tidspunkt mottak av signert skjema
        /// </summary>
        public DateTime? Signed { get; set; }
        /// <summary>
        /// Referanse til videresendt melding med signert skjema (receiptSent)
        /// </summary>
        public string RecieptSentArchiveReference { get; set; }
        /// <summary>
        /// Tidspunkt videresending av signert skjema (receiptSent)
        /// </summary>
        public DateTime? RecieptSent { get; set; }
        /// <summary>
        /// Feilmelding ved status=error
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Status på distribusjonen
        /// </summary>
        public DistributionStatus DistributionStatus { get; set; }

        /// <summary>
        /// Om distribusjonen har gått til print pga reservert
        /// </summary>
        public bool Printed { get; set; }

        /// <summary>
        /// Indikerer om denne distribusjonen ble kombinert til en forsendelse i Altinn eller print, null for forsendelse , og inneholder en GUID om det er en combinert
        /// </summary>
        public Guid DistributionReference { get; set; }

        public DistributionForm()
        {
            DistributionStatus = DistributionStatus.submittedPrefilled;
        }
    }
    public enum DistributionStatus {
        error,
        submittedPrefilled,
        signed,
        receiptSent
    }
}