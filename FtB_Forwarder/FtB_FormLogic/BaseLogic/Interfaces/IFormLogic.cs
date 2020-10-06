namespace FtB_FormLogic
{
    public interface IFormLogic<T,U>
    {
        T Execute(U input);
    }
}
