namespace SortingAlgorithms.External;

using System.Text;
using System.Threading.Channels;

/// <summary>
/// Manages the splitting of input data into chunks and processes them in parallel.
/// </summary>
/// <typeparam name="T">The type of the objects that the input lines will be parsed into.</typeparam>
/// <param name="input">The input file path.</param>
/// <param name="bufferSize">The size of the buffer to use for reading the input file.</param>
/// <param name="maxParallelism">The maximum number of parallel tasks to use for processing.</param>
/// <param name="lineToObjectParser">A function that parses a line of input into an object of type T.</param>
public class ChunkManager<T>(
    string input,
    long bufferSize,
    int maxParallelism,
    Func<string, T> lineToObjectParser)
{
    /// <summary>
    /// Splits the input data into chunks and writes the parsed objects to the provided channel writer.
    /// </summary>
    /// <param name="writer">The channel writer to which the parsed objects will be written.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Split(
        ChannelWriter<List<T>> writer)
    {
        var buffer = new char[bufferSize / maxParallelism];
        var lines = new List<string>();
        var parsingTasks = new List<Task>();

        using var reader = new StreamReader(input);
        var stringBuilder = new StringBuilder();
        var chunkIndex = 0;

        while (true)
        {
            chunkIndex++;

            var bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0)
                break;

            stringBuilder.Append(buffer, 0, bytesRead);
            var chunkData = stringBuilder.ToString();
            var lastNewLineIndex = chunkData.LastIndexOf(Environment.NewLine, StringComparison.Ordinal);

            if (lastNewLineIndex == -1)
                continue;

            var chunkText = chunkData[..(lastNewLineIndex + Environment.NewLine.Length)];
            stringBuilder.Remove(0, lastNewLineIndex + Environment.NewLine.Length);

            lines.AddRange(chunkText.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries));

            var linesToParse = new List<string>(lines);
            var parsingTask = ProcessChunk(linesToParse, writer, chunkIndex);
            parsingTasks.Add(parsingTask);
            lines.Clear();
        }

        if (stringBuilder.Length > 0)
        {
            lines.Add(stringBuilder.ToString());
            await ProcessChunk(lines, writer, chunkIndex);
        }

        await Task.WhenAll(parsingTasks);
        writer.Complete();
    }

    private async Task ProcessChunk(
        IEnumerable<string> lines,
        ChannelWriter<List<T>> writer,
        int chunkIndex)
    {
        var chunk = new List<T>();

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxParallelism };

        Parallel.ForEach(lines, parallelOptions, line =>
        {
            var parsedLine = lineToObjectParser(line);
            lock (chunk)
            {
                chunk.Add(parsedLine);
            }
        });

        await writer.WriteAsync(chunk);

        Console.WriteLine($"Chunk {chunkIndex} extracted.");
    }
}