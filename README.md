# Sorting Application Repository

This repository contains applications and libraries for generating large files with random data and sorting them using external sorting algorithms.

## Overview

The repository includes the following components:

1. **File Generator**: Generates large files with random data.
2. **Sorter**: Sorts large datasets using external sorting algorithms.
3. **SortingAlgorithms.External**: A .NET library for handling external sorting of large datasets.

## File Generator

The File Generator application generates large files with random data. It allows you to specify the file path and size through command-line arguments.

### Command-Line Arguments

- `-path`: The output file path.
- `-mbsize`: The size of the file to generate in megabytes.
- `-gbsize`: The size of the file to generate in gigabytes.
- `-help`: Displays help information.

### Usage Example

To generate a file, use the following command:

```sh
dotnet run --project Generator -- -path "data/generated_data.txt" -mbsize 100
```

This command will generate a file named `generated_data.txt` with a size of 100 MB.

## Sorter

The Sorter application sorts large datasets using external sorting algorithms. It splits, sorts, saves, and merges data chunks in parallel, making it suitable for processing data that doesn't fit into memory.

### Command-Line Arguments

- `-input`: The input file path.
- `-output`: The output file path.
- `-bufferSizeMB`: The size of the buffer to use for reading and writing files (in megabytes).
- `-maxParallelism`: The maximum number of parallel tasks to use for processing.
- `-help`: Displays help information.

### Usage Example

To sort a generated file, use the following command:

```sh
dotnet run --project Sorter -- -input "data/generated_data.txt" -output "data/sorted_data.txt" -bufferSizeMB 64 -maxParallelism 4
```

This command will sort the data in `generated_data.txt` and save the sorted data to `sorted_data.txt` using a buffer size of 64 MB and a maximum parallelism of 4.

## SortingAlgorithms.External

`SortingAlgorithms.External` is a .NET library designed to handle external sorting of large datasets. It splits, sorts, saves, and merges data chunks in parallel, making it suitable for processing data that doesn't fit into memory.

### Example

Here is an example of how to use the `SortingAlgorithms.External` library:

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

This example demonstrates how to configure and run the external sorting pipeline using the `SortingAlgorithms.External` package.

## Summary

1. **Generate a file**:
    ```sh
    dotnet run --project Generator -- -path "data/generated_data.txt" -mbsize 100
    ```

2. **Sort the generated file**:
    ```sh
    dotnet run --project Sorter -- -input "data/generated_data.txt" -output "data/sorted_data.txt" -bufferSizeMB 64 -maxParallelism 4
    ```

This repository provides tools to generate large datasets and sort them efficiently using external sorting algorithms.