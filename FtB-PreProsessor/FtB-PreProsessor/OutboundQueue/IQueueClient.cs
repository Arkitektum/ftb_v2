namespace FtB_PreProsessor.OutboundQueue
{
    public interface IQueueClient
    {
        void QueueFormForProcessing(QueueMessage queueMessage);
    }
}