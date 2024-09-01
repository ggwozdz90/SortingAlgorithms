namespace SortingAlgorithms.External;

using System.Threading.Channels;

/// <summary>
/// Manages the sorting of data chunks in parallel.
/// </summary>
/// <typeparam name="T">The type of the objects that will be sorted.</typeparam>
/// <param name="maxParallelism">The maximum number of parallel tasks to use for sorting.</param>
public class SortManager<T>(
    int maxParallelism)
{
    /// <summary>
    /// Sorts the data chunks read from the provided channel reader and writes the sorted chunks to the provided channel writer.
    /// </summary>
    /// <param name="reader">The channel reader from which the unsorted data chunks will be read.</param>
    /// <param name="writer">The channel writer to which the sorted data chunks will be written.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// The method reads data chunks from the reader channel, sorts each chunk in parallel, and writes the sorted chunks to the writer channel.
    /// The degree of parallelism is controlled by the maxParallelism parameter.
    /// </remarks>
    public async Task Sort(
        ChannelReader<List<T>> reader,
        ChannelWriter<List<T>> writer)
    {
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxParallelism };

        await Parallel.ForEachAsync(
            reader.ReadAllAsync(),
            parallelOptions,
            async (chunk, cancellationToken) =>
        {
            chunk.Sort();
            await writer.WriteAsync(chunk, cancellationToken);
            Console.WriteLine("Chunk sorted");
        });

        writer.Complete();
    }
}