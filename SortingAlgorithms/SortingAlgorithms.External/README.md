# SortingAlgorithms.External

`SortingAlgorithms.External` is a .NET library designed to handle external sorting of large datasets. It splits, sorts, saves, and merges data chunks in parallel, making it suitable for processing data that doesn't fit into memory.

## Classes

### `ExternalSortingConfig<T>`

Configuration settings for the external sorting pipeline.

- **Properties:**
  - `Input`: The input file path.
  - `Output`: The output file path.
  - `BufferSize`: The size of the buffer to use for reading and writing files.
  - `MaxParallelism`: The maximum number of parallel tasks to use for processing.
  - `LineToObjectParser`: A function that parses a line of input into an object of type `T`.

### `ChunkManager<T>`

Manages the splitting of input data into chunks and processes them in parallel.

- **Methods:**
  - `Split(ChannelWriter<List<T>> writer)`: Splits the input data into chunks and writes the parsed objects to the provided channel writer.

### `SortManager<T>`

Manages the sorting of data chunks in parallel.

- **Methods:**
  - `Sort(ChannelReader<List<T>> reader, ChannelWriter<List<T>> writer)`: Sorts the data chunks read from the provided channel reader and writes the sorted chunks to the provided channel writer.

### `SaveManager<T>`

Manages the saving of sorted data chunks to output files.

- **Methods:**
  - `Save(ChannelReader<List<T>> reader)`: Saves the sorted data chunks read from the provided channel reader to output files.

### `MergeManager<T>`

Manages the merging of sorted data chunks into a single sorted output.

- **Methods:**
  - `Merge(List<string> chunkFiles)`: Merges the sorted data chunks from the provided list of chunk files into a single sorted output file.

### `ExternalSortingPipeline<T>`

Manages the entire external sorting pipeline, including splitting, sorting, saving, and merging data.

- **Methods:**
  - `Run()`: Executes the external sorting pipeline.

## Usage Example

```csharp
using SortingAlgorithms.External;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ExternalSortingConfig<string>(
            input: "input.txt",
            output: "output.txt",
            lineToObjectParser: line => line,
            bufferSize: 128,
            maxParallelism: 4
        );

        var pipeline = new ExternalSortingPipeline<string>(config);
        await pipeline.Run();
    }
}
```

This example demonstrates how to configure and run the external sorting pipeline using the SortingAlgorithms.External package.