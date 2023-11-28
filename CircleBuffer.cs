public class CircleBuffer<T>
{
    private T[] buffer;
    private int bufferSize;
    private int head;
    private int tail;
    private int length;

    private bool isFull => length == bufferSize;
    public int UndoCount => length;
    public int RedoCount { get; private set; }


    public CircleBuffer(int capacity)
    {
        buffer = new T[capacity];
        bufferSize = capacity;
        head = 0;
    }

    public void Add(T item)
    {
        head = NextPosition(head);
        buffer[head] = item;


        if (isFull)
        {
            tail = NextPosition(tail);
        }
        else
        {
            length++;
        }

        RedoCount = 0;
    }

    public bool TryGetUndo(out T item)
    {
        if (UndoCount == 0)
        {
            item = default;
            return false;
        }

        length--;
        RedoCount++;
        item = buffer[head];

        var previousPosition = PreviousPosition(head);

        if (tail == head)
        {
            tail = previousPosition;
        }

        head = previousPosition;

        return true;
    }

    public bool TryGetRedo(out T item)
    {
        if (RedoCount == 0)
        {
            item = default;
            return false;
        }

        length++;
        RedoCount--;

        head = NextPosition(head);
        item = buffer[head];
        return true;
    }

    private int NextPosition(int position)
    {
        return (position + 1) % bufferSize;
    }

    private int PreviousPosition(int position)
    {
        position = (position - 1) % bufferSize;

        return position % bufferSize;
    }
}