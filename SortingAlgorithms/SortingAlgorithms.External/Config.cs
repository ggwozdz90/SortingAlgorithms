namespace SortingAlgorithms.External;

/// <summary>
/// Configuration settings for the external sorting pipeline.
/// </summary>
/// <typeparam name="T">The type of the objects that will be processed.</typeparam>
public class ExternalSortingConfig<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSortingConfig{T}"/> class.
    /// </summary>
    /// <param name="input">The input file path.</param>
    /// <param name="output">The output file path.</param>
    /// <param name="lineToObjectParser">A function that parses a line of input into an object of type T. If null, the line itself is used.</param>
    /// <param name="bufferSize">The size of the buffer to use for reading and writing files. Default is 128.</param>
    /// <param name="maxParallelism">The maximum number of parallel tasks to use for processing. Default is 4.</param>
    /// <exception cref="ArgumentException">Thrown when input or output file path is null or empty, buffer size is less than or equal to zero, or max parallelism is less than or equal to zero.</exception>
    /// <remarks>
    /// The constructor validates the input parameters and initializes the configuration properties.
    /// If the lineToObjectParser is not provided, a default parser is used that treats each line as an object of type T.
    /// </remarks>
    public ExternalSortingConfig(
        string input,
        string output,
        Func<string, T> lineToObjectParser = null,
        long bufferSize = 128,
        int maxParallelism = 4)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input file path cannot be null or empty.", nameof(input));
        if (string.IsNullOrEmpty(output))
            throw new ArgumentException("Output file path cannot be null or empty.", nameof(output));
        if (bufferSize <= 0)
            throw new ArgumentException("Buffer size must be greater than zero.", nameof(bufferSize));
        if (maxParallelism <= 0)
            throw new ArgumentException("Max parallelism must be greater than zero.", nameof(maxParallelism));

        Input = input;
        Output = output;
        BufferSize = bufferSize;
        MaxParallelism = maxParallelism;
        LineToObjectParser = lineToObjectParser ?? (line => (T)(object)line);
    }


    /// <summary>
    /// Gets the input file path.
    /// </summary>
    public string Input { get; }

    /// <summary>
    /// Gets the output file path.
    /// </summary>
    public string Output { get; }

    /// <summary>
    /// Gets the size of the buffer to use for reading and writing files.
    /// </summary>
    public long BufferSize { get; }

    /// <summary>
    /// Gets the maximum number of parallel tasks to use for processing.
    /// </summary>
    public int MaxParallelism { get; }

    /// <summary>
    /// Gets the function that parses a line of input into an object of type T.
    /// </summary>
    public Func<string, T> LineToObjectParser { get; }
}