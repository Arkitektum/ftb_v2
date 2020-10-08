namespace Altinn2.Adapters.Models
{
    public interface IFormItem
    {
        string DataFormatId { get; set; }
        int DataFormatVersionId { get; set; }
        string FormData { get; set; }
        string ParentReference { get; set; }
        string Reference { get; set; }
    }

    public class FormItem : IFormItem
    {
        public string DataFormatId { get; set; }
        public int DataFormatVersionId { get; set; }
        public string FormData { get; set; }
        public string ParentReference { get; set; }
        public string Reference { get; set; }
        public AltinnFormType FormType { get; set; }


        public FormItem(string dataFormatId, int dataFormatVersionId, string formData, string parentReference, string reference, AltinnFormType formType)
        {
            DataFormatId = dataFormatId;
            DataFormatVersionId = dataFormatVersionId;
            FormData = formData;
            ParentReference = parentReference;
            Reference = reference;
            FormType = formType;
        }
    }

    public enum AltinnFormType
    {
        MainForm,
        SubForm
    }
}
