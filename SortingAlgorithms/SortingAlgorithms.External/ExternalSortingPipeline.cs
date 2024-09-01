namespace SortingAlgorithms.External;

using System.Threading.Channels;

/// <summary>
/// Manages the entire external sorting pipeline, including splitting, sorting, saving, and merging data.
/// </summary>
/// <typeparam name="T">The type of the objects that will be processed. Must implement IComparable&lt;T&gt;.</typeparam>
/// <param name="config">The configuration settings for the external sorting pipeline.</param>
public class ExternalSortingPipeline<T>(
    ExternalSortingConfig<T> config)
    where T : IComparable<T>
{
    /// <summary>
    /// Executes the external sorting pipeline.
    /// </summary>
    /// <remarks>
    /// The method orchestrates the entire external sorting process:
    /// 1. Splits the input data into chunks and writes them to a channel.
    /// 2. Sorts the chunks in parallel and writes the sorted chunks to another channel.
    /// 3. Saves the sorted chunks to files and collects the file paths.
    /// 4. Merges the sorted chunk files into a single sorted output file.
    /// </remarks>
    public async Task Run()
    {
        var chunkManager = new ChunkManager<T>(config.Input, config.BufferSize, config.MaxParallelism,
            config.LineToObjectParser);
        var sortManager = new SortManager<T>(config.MaxParallelism);
        var saveManager = new SaveManager<T>(config.Output, config.BufferSize, config.MaxParallelism);
        var mergeManager = new MergeManager<T>(config.Output, config.BufferSize, config.LineToObjectParser);

        var splitChannel = Channel.CreateBounded<List<T>>(config.MaxParallelism);
        var saveChannel = Channel.CreateBounded<List<T>>(config.MaxParallelism);

        var chunkTask = chunkManager.Split(splitChannel.Writer);
        var sortTask = sortManager.Sort(splitChannel.Reader, saveChannel.Writer);
        var saveTask = saveManager.Save(saveChannel.Reader);

        await Task.WhenAll(chunkTask, sortTask, saveTask);

        var chunkFiles = await saveTask;
        mergeManager.Merge(chunkFiles);
    }
}