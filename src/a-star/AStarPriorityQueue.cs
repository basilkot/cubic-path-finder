namespace AStarConsoleApp;

public class AStarPriorityQueue
{
    private PriorityQueue<long, int> _queue
        = new PriorityQueue<long, int>();

    public void Enqueue(long item, int priority)
    {
        _queue.Enqueue(item, priority);
    }

    public long Dequeue()
    {
        return _queue.Dequeue();
    }

    public int Count => _queue.Count;
}