using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ArgusWebContentTracker.Services
{
    public interface IProcessMonitorService
    {

        Task StartMonitoringAsync(BlockingCollection<string> _channel, CancellationToken _cancellationToken);
        Task ListenAsync(BlockingCollection<string> _channel, CancellationToken _cancellationToken);
    }
}
