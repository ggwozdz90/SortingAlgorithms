namespace SortingAlgorithms.External;

using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

/// <summary>
/// Manages the saving of sorted data chunks to output files.
/// </summary>
/// <typeparam name="T">The type of the objects that will be saved.</typeparam>
/// <param name="output">The output file path.</param>
/// <param name="bufferSize">The size of the buffer to use for writing the output files.</param>
/// <param name="maxParallelism">The maximum number of parallel tasks to use for saving.</param>
public class SaveManager<T>(
    string output,
    long bufferSize,
    int maxParallelism)
{
    /// <summary>
    /// Saves the sorted data chunks read from the provided channel reader to output files.
    /// </summary>
    /// <param name="reader">The channel reader from which the sorted data chunks will be read.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of file paths to the saved chunks.</returns>
    /// <remarks>
    /// The method reads sorted data chunks from the reader channel, writes each chunk to a separate file, and returns a list of file paths to the saved chunks.
    /// The degree of parallelism is controlled by the maxParallelism parameter.
    /// </remarks>
    public async Task<List<string>> Save(
        ChannelReader<List<T>> reader)
    {
        var fileChunks = new ConcurrentBag<string>();
        var chunkIndex = 0;
        var chunkSize = bufferSize / maxParallelism;

        await foreach (var chunk in reader.ReadAllAsync())
        {
            var chunkFileName = Path.Combine(Path.GetDirectoryName(output) ?? string.Empty,
                $"chunk_{Interlocked.Increment(ref chunkIndex)}_{Path.GetFileName(output)}");

            await using var writer = new StreamWriter(chunkFileName, false, Encoding.UTF8, (int)chunkSize);

            foreach (var entry in chunk) await writer.WriteLineAsync(entry.ToString());

            fileChunks.Add(chunkFileName);

            Console.WriteLine($"Chunk saved: {chunkFileName}");
        }

        return fileChunks.ToList();
    }
}