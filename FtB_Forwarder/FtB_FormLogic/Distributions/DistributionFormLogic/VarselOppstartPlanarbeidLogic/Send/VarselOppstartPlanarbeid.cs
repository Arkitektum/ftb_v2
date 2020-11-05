using no.kxml.skjema.dibk.nabovarselsvarPlan;

namespace FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Send
{
    public class VarselOppstartPlanarbeidData : PrefillSendData<SvarPaaNabovarselPlanType>
    {
        public VarselOppstartPlanarbeidData(SvarPaaNabovarselPlanType formInstance) : base(formInstance)
        {}

        public override string PrefillFormName => "Uttalelse til oppstart av reguleringsplanarbeid";

        public override string InitialExternalSystemReference { get => FormInstance.hovedinnsendingsnummer; set { FormInstance.hovedinnsendingsnummer = value; } }

        public override string ExternalSystemReference => FormInstance.beroertPart.systemReferanse;

        public override string PrefillServiceCode => "5419";

        public override string PrefillServiceEditionCode => "1";
    }
}
