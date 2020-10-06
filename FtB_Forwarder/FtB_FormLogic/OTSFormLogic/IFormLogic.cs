namespace FtB_FormLogic.OTSFormLogic
{
    public interface IFormLogic<T, U>
    {
        T Execute(U input);
    }
}
