namespace KristofferStrube.Blazor.FileSystem.WasmExample.Search;

public static class StringExtensions
{
    public static (Dictionary<char, int> CharToIndex, char[] IndexToChar) CreateAlphabet(this string input)
    {
        Dictionary<char, int> charToIndex = new();

        ISet<char> set = input.ToHashSet();
        List<char> list = set.OrderBy(c => c).ToList();
        char[] indexToChar = new char[list.Count + 1];

        int count = 1;
        foreach (char c in list)
        {
            charToIndex[c] = count;
            indexToChar[count++] = c;
        }

        return (charToIndex, indexToChar);
    }

    public static int[] MapWithAlphabet(this string input, Dictionary<char, int> CharToIndex)
    {
        int[] output = new int[input.Length + 1];
        for (int i = 0; i < input.Length; i++)
        {
            if (!CharToIndex.ContainsKey(input[i]))
                throw new ArgumentException("character #" + i + " was not found in alphabet.");
            output[i] = CharToIndex[input[i]];
        }
        output[input.Length] = 0;
        return output;
    }

    public static int[] MapWithAlphabetWithoutSentinel(this string input, Dictionary<char, int> CharToIndex)
    {
        int[] output = new int[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            if (!CharToIndex.ContainsKey(input[i]))
                throw new ArgumentException("character #" + i + " was not found in alphabet.");
            output[i] = CharToIndex[input[i]];
        }
        return output;
    }

    public static int[] MapWithAlphabetWithoutSentinelAllowNewChars(this string input, Dictionary<char, int> CharToIndex)
    {
        int[] output = new int[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = CharToIndex.GetValueOrDefault(input[i], CharToIndex.Count + 1);
        }
        return output;
    }
}