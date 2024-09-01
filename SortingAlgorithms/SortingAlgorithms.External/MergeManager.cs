namespace SortingAlgorithms.External;

/// <summary>
/// Manages the merging of sorted data chunks into a single sorted output.
/// </summary>
/// <typeparam name="T">The type of the objects that will be merged. Must implement IComparable&lt;T&gt;.</typeparam>
/// <param name="output">The output file path.</param>
/// <param name="bufferSize">The size of the buffer to use for reading the chunk files.</param>
/// <param name="lineToObjectParser">A function that parses a line of input into an object of type T.</param>
public class MergeManager<T>(
    string output,
    long bufferSize,
    Func<string, T> lineToObjectParser)
    where T : IComparable<T>
{
    /// <summary>
    /// Merges the sorted data chunks from the provided list of chunk files into a single sorted output file.
    /// </summary>
    /// <param name="chunkFiles">The list of file paths to the sorted chunk files.</param>
    /// <remarks>
    /// The method opens each chunk file and reads data into buffers. It then repeatedly finds the smallest element
    /// across all buffers, writes it to the output file, and refills the buffer from which the element was taken.
    /// This process continues until all buffers are empty and all chunk files have been fully read.
    /// </remarks>
    public void Merge(
        List<string> chunkFiles)
    {
        var readers = new List<StreamReader>();
        var buffers = new List<Queue<T>>();

        try
        {
            foreach (var chunkFile in chunkFiles)
            {
                var reader = new StreamReader(chunkFile);
                readers.Add(reader);
                buffers.Add(new Queue<T>());
            }

            using var outputStream = new StreamWriter(output);

            for (var i = 0; i < readers.Count; i++)
                FillBuffer(readers[i], buffers[i]);

            while (readers.Count > 0)
            {
                T smallest = default;
                var smallestIndex = -1;

                for (var i = 0; i < buffers.Count; i++)
                    if (buffers[i].Count > 0)
                        if (smallest == null || buffers[i].Peek().CompareTo(smallest) < 0)
                        {
                            smallest = buffers[i].Peek();
                            smallestIndex = i;
                        }

                if (smallestIndex == -1)
                    break;

                outputStream.WriteLine(buffers[smallestIndex].Dequeue());

                if (buffers[smallestIndex].Count != 0)
                    continue;

                FillBuffer(readers[smallestIndex], buffers[smallestIndex]);

                if (buffers[smallestIndex].Count != 0)
                    continue;

                readers[smallestIndex].Dispose();
                readers.RemoveAt(smallestIndex);
                buffers.RemoveAt(smallestIndex);

                File.Delete(chunkFiles[smallestIndex]);
                chunkFiles.RemoveAt(smallestIndex);
            }
        }
        finally
        {
            foreach (var reader in readers)
                reader.Dispose();
        }
    }

    /// <summary>
    /// Fills the buffer with data read from the provided StreamReader.
    /// </summary>
    /// <param name="reader">The StreamReader to read data from.</param>
    /// <param name="buffer">The buffer to fill with parsed objects.</param>
    private void FillBuffer(
        StreamReader reader,
        Queue<T> buffer)
    {
        for (var i = 0; i < bufferSize && !reader.EndOfStream; i++)
            buffer.Enqueue(lineToObjectParser(reader.ReadLine()));
    }
}