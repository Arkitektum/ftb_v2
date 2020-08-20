using System.Threading.Tasks;

namespace Distributor
{
    public interface IDistributor
    {
        Task Distribute(dynamic distributionElement);
    }
}
