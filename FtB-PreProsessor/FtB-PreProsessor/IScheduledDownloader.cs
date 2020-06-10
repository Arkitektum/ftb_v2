using System.Threading;
using System.Threading.Tasks;

namespace FtB_PreProsessor
{
    public interface IScheduledDownloader
    {
        Task DoWork(CancellationToken cancelationToken);
        void DownloadFormsForProcessing();
    }
}