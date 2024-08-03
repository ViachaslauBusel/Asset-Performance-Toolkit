using Cysharp.Threading.Tasks;

namespace AssetPerformanceToolkit.FrameBalancer
{
    public interface ITask
    {
        bool IsCompleted { get; }
        void Cancel();
        UniTask Wait();
    }
}
