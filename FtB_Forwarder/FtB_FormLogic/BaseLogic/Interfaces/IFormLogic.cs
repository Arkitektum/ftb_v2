using System.Threading.Tasks;

namespace FtB_FormLogic
{
    public interface IFormLogic<T,U>
    {
        Task<T> Execute(U input);
    }
}
