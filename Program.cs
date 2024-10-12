//*****************************************************************************
//** 2406. Divide Intervals Into Minimum Number of Groups    leetcode        **
//*****************************************************************************



// Comparator function to sort intervals by their start time
int compare(const void* a, const void* b)
{
    int* intervalA = *(int**)a;
    int* intervalB = *(int**)b;
    return intervalA[0] - intervalB[0];
}

// Min heap push and pop helpers
void heapPush(int* heap, int* heapSize, int val)
{
    int idx = (*heapSize)++;
    heap[idx] = val;

    // Bubble up
    while (idx > 0)
    {
        int parent = (idx - 1) / 2;
        if (heap[parent] <= heap[idx])
            break;
        int temp = heap[parent];
        heap[parent] = heap[idx];
        heap[idx] = temp;
        idx = parent;
    }
}

int heapPop(int* heap, int* heapSize)
{
    int result = heap[0];
    heap[0] = heap[--(*heapSize)];

    // Bubble down
    int idx = 0;
    while (1)
    {
        int left = 2 * idx + 1;
        int right = 2 * idx + 2;
        int smallest = idx;

        if (left < *heapSize && heap[left] < heap[smallest])
            smallest = left;
        if (right < *heapSize && heap[right] < heap[smallest])
            smallest = right;

        if (smallest == idx)
            break;

        int temp = heap[idx];
        heap[idx] = heap[smallest];
        heap[smallest] = temp;
        idx = smallest;
    }

    return result;
}

int minGroups(int** intervals, int intervalsSize, int* intervalsColSize)
{
    // Sort intervals by start time
    qsort(intervals, intervalsSize, sizeof(int*), compare);

    // Min heap to track end times of active intervals in different groups
    int heap[intervalsSize];
    int heapSize = 0;

    for (int i = 0; i < intervalsSize; i++)
    {
        int start = intervals[i][0];
        int end = intervals[i][1];

        // If the new interval starts after the earliest ending interval in the heap,
        // pop the heap (reuse the group)
        if (heapSize > 0 && heap[0] < start)
        {
            heapPop(heap, &heapSize);
        }

        // Push the current interval's end time into the heap (assign to a group)
        heapPush(heap, &heapSize, end);
    }

    // The size of the heap represents the number of groups needed
    return heapSize;
}
