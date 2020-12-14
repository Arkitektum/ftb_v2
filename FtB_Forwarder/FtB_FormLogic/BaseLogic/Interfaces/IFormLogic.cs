using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public interface IFormLogic<T,U>
    {
        Task<T> ExecuteAsync(U input);
        Task PreExecuteAsync(U input);
        Task PostExecuteAsync(U input);
    }
}
