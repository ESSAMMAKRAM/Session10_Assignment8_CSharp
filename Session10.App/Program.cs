using System.Collections;
using System.Numerics;

// =====================================================================
//  Session10_Assignment8_CSharp  -  Assignment 1 (Advanced C#)
// =====================================================================

// ---------------------------------------------------------------------
// Q1: Optimized Bubble Sort
// ---------------------------------------------------------------------
Console.WriteLine("===== Q1: Optimized Bubble Sort =====");

int[] numbers = { 64, 34, 25, 12, 22, 11, 90 };
Console.WriteLine($"Before: {string.Join(", ", numbers)}");
int passes = Solutions.OptimizedBubbleSort(numbers);
Console.WriteLine($"After : {string.Join(", ", numbers)}  (passes used: {passes})");

int[] alreadySorted = { 1, 2, 3, 4, 5 };
int passesSorted = Solutions.OptimizedBubbleSort(alreadySorted);
Console.WriteLine($"Already sorted array finished in {passesSorted} pass only (best case O(n))");

// How can we optimize Bubble Sort?
// 1) "swapped" flag (early exit): if a full pass makes NO swap, the array is already sorted,
//    so we stop immediately instead of doing the remaining useless passes.
//    => best case (already sorted) becomes O(n) instead of O(n^2).
// 2) Shrink the inner loop: after each pass the largest remaining element is in its final
//    place at the end, so the next pass doesn't need to compare it again (n - 1 - pass).
//    => about half the comparisons compared with the plain version.
// Notes: the worst and average cases are still O(n^2) because the algorithm is still
// comparison-by-neighbours, but it does far less work on nearly-sorted data.
// Space = O(1) (in place), and it stays stable.

// ---------------------------------------------------------------------
// Q2: Generic Range<T>
// ---------------------------------------------------------------------
Console.WriteLine("\n===== Q2: Range<T> =====");

Range<int> intRange = new(1, 10);
Console.WriteLine($"Range<int> [1, 10] -> IsInRange(5)  = {intRange.IsInRange(5)}");
Console.WriteLine($"Range<int> [1, 10] -> IsInRange(15) = {intRange.IsInRange(15)}");
Console.WriteLine($"Range<int> [1, 10] -> Length()      = {intRange.Length()}");

Range<double> doubleRange = new(1.5, 7.5);
Console.WriteLine($"Range<double> [1.5, 7.5] -> IsInRange(7.5) = {doubleRange.IsInRange(7.5)}");
Console.WriteLine($"Range<double> [1.5, 7.5] -> Length()       = {doubleRange.Length()}");

// ---------------------------------------------------------------------
// Q3: Reverse an ArrayList in place (without the built-in Reverse)
// ---------------------------------------------------------------------
Console.WriteLine("\n===== Q3: Reverse ArrayList in place =====");

ArrayList list = new() { 1, "two", 3.5, 'D', 5 };
Console.WriteLine($"Before: {string.Join(", ", list.Cast<object>())}");
Solutions.ReverseArrayList(list);
Console.WriteLine($"After : {string.Join(", ", list.Cast<object>())}");

// ---------------------------------------------------------------------
// Q4: Even numbers from a list
// ---------------------------------------------------------------------
Console.WriteLine("\n===== Q4: Even numbers =====");

List<int> allNumbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -4, -3 };
List<int> evens = Solutions.GetEvenNumbers(allNumbers);
Console.WriteLine($"Input : {string.Join(", ", allNumbers)}");
Console.WriteLine($"Evens : {string.Join(", ", evens)}");

// ---------------------------------------------------------------------
// Q5: FixedSizeList<T>
// ---------------------------------------------------------------------
Console.WriteLine("\n===== Q5: FixedSizeList<T> =====");

FixedSizeList<string> fixedList = new(3);
fixedList.Add("A");
fixedList.Add("B");
fixedList.Add("C");
Console.WriteLine($"Get(1) = {fixedList.Get(1)}");

try
{
    fixedList.Add("D");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Add error  : {ex.Message}");
}

try
{
    fixedList.Get(5);
}
catch (IndexOutOfRangeException ex)
{
    Console.WriteLine($"Get error  : {ex.Message}");
}

// ---------------------------------------------------------------------
// Q6: First non-repeated character
// ---------------------------------------------------------------------
Console.WriteLine("\n===== Q6: First non-repeated character =====");

string[] words = { "swiss", "leetcode", "loveleetcode", "aabb" };
foreach (string word in words)
{
    Console.WriteLine($"\"{word}\" -> {Solutions.FirstNonRepeatedChar(word)}");
}

Console.WriteLine("\nDone.");


// =====================================================================
//  Types (must come AFTER the top-level statements)
// =====================================================================

public static class Solutions
{
    // Q1: returns how many passes were needed
    public static int OptimizedBubbleSort(int[] arr)
    {
        int n = arr.Length;
        int passes = 0;

        for (int pass = 0; pass < n - 1; pass++)
        {
            passes++;
            bool swapped = false;

            // the last "pass" elements are already in their final place
            for (int j = 0; j < n - 1 - pass; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                }
            }

            if (!swapped)
                break;   // no swaps => already sorted
        }

        return passes;
    }

    // Q3: reverse in place using two pointers (no built-in Reverse)
    public static void ReverseArrayList(ArrayList list)
    {
        for (int left = 0, right = list.Count - 1; left < right; left++, right--)
        {
            object? temp = list[left];
            list[left] = list[right];
            list[right] = temp;
        }
    }

    // Q4: return a NEW list containing only the even numbers
    public static List<int> GetEvenNumbers(List<int> numbers)
    {
        List<int> result = new();

        foreach (int number in numbers)
        {
            if (number % 2 == 0)
                result.Add(number);
        }

        return result;
    }

    // Q6: index of the first non-repeated character, or -1
    public static int FirstNonRepeatedChar(string text)
    {
        Dictionary<char, int> counts = new();

        // pass 1: count every character
        foreach (char c in text)
        {
            counts[c] = counts.GetValueOrDefault(c) + 1;
        }

        // pass 2: the first character whose count is 1
        for (int i = 0; i < text.Length; i++)
        {
            if (counts[text[i]] == 1)
                return i;
        }

        return -1;
    }
}

// Q2: T must be comparable (IComparable<T>, as the assignment says) and must support
// the "-" operator so Length() can compute Max - Min (int, long, double, decimal ...).
public class Range<T> where T : IComparable<T>, ISubtractionOperators<T, T, T>
{
    public T Min { get; }
    public T Max { get; }

    public Range(T min, T max)
    {
        if (min.CompareTo(max) > 0)
            throw new ArgumentException("Minimum value cannot be greater than the maximum value.");

        Min = min;
        Max = max;
    }

    public bool IsInRange(T value)
    {
        return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
    }

    public T Length()
    {
        return Max - Min;
    }
}

// Q5
public class FixedSizeList<T>
{
    private readonly T[] _items;
    private int _count;

    public int Capacity => _items.Length;
    public int Count => _count;

    public FixedSizeList(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

        _items = new T[capacity];
    }

    public void Add(T item)
    {
        if (_count == _items.Length)
            throw new InvalidOperationException(
                $"The list is full. Maximum capacity is {_items.Length} elements.");

        _items[_count] = item;
        _count++;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= _count)
            throw new IndexOutOfRangeException(
                $"Invalid index {index}. The list currently holds {_count} element(s).");

        return _items[index];
    }
}
