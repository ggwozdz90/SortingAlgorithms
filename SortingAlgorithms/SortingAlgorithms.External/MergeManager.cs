using System.Text;

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
        var fileBufferSize = bufferSize / chunkFiles.Count;
        var outputFileBuffer = 64 * 1024 * 1024;

        try
        {
            foreach (var chunkFile in chunkFiles)
            {
                var reader = new StreamReader(chunkFile);
                readers.Add(reader);
                buffers.Add(new Queue<T>());
            }

            using var outputStream = new StreamWriter(output, false, Encoding.UTF8, outputFileBuffer);

            for (var i = 0; i < readers.Count; i++)
                FillBuffer(readers[i], buffers[i], fileBufferSize);

            var minHeap = new SortedSet<(T value, int index)>();

            for (var i = 0; i < buffers.Count; i++)
                if (buffers[i].Count > 0)
                    minHeap.Add((buffers[i].Peek(), i));

            while (minHeap.Count > 0)
            {
                var (_, smallestIndex) = minHeap.Min;
                minHeap.Remove(minHeap.Min);

                outputStream.WriteLine(buffers[smallestIndex].Dequeue());

                if (buffers[smallestIndex].Count > 0)
                {
                    minHeap.Add((buffers[smallestIndex].Peek(), smallestIndex));
                }
                else
                {
                    FillBuffer(readers[smallestIndex], buffers[smallestIndex], fileBufferSize);

                    if (buffers[smallestIndex].Count > 0)
                    {
                        minHeap.Add((buffers[smallestIndex].Peek(), smallestIndex));
                    }
                    else
                    {
                        readers[smallestIndex].Dispose();
                    }
                }
            }
        }
        finally
        {
            foreach (var reader in readers)
                reader.Dispose();

            foreach (var file in chunkFiles)
                File.Delete(file);
        }
    }

    /// <summary>
    /// Fills the buffer with data read from the provided StreamReader.
    /// </summary>
    /// <param name="reader">The StreamReader to read data from.</param>
    /// <param name="buffer">The buffer to fill with parsed objects.</param>
    /// <param name="fileBufferSize">The maximum size of the buffer in bytes.</param>
    private void FillBuffer(
        StreamReader reader,
        Queue<T> buffer,
        long fileBufferSize)
    {
        for (var i = 0; i < fileBufferSize && !reader.EndOfStream;)
        {
            var line = reader.ReadLine();
            var lineSize = Encoding.UTF8.GetByteCount(line ?? string.Empty);
            var entry = lineToObjectParser(line);
            buffer.Enqueue(entry);
            i += lineSize;
        }
    }
}