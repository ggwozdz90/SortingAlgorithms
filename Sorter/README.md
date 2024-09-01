# Sorting Application

This application sorts large datasets using external sorting algorithms. It splits, sorts, saves, and merges data chunks in parallel, making it suitable for processing data that doesn't fit into memory.

## Files

### `Program.cs`

The entry point of the application. It parses command-line arguments, configures the external sorting pipeline, and executes the sorting process.

### `ArgumentParser.cs`

Parses command-line arguments to configure the sorting process.

### `FileEntry.cs`

Represents a single entry in the file to be sorted. Implements `IComparable<FileEntry>` to enable sorting.

## Usage

To run the application, use the following command:

```sh
dotnet run -- -input "data/generated_data.txt" -output "data/sorted_data.txt" -bufferSizeMB 64 -maxParallelism 4
```

### Command-Line Arguments

- `-input`: The input file path.
- `-output`: The output file path.
- `-bufferSizeMB`: The size of the buffer to use for reading and writing files (in megabytes).
- `-maxParallelism`: The maximum number of parallel tasks to use for processing.
- `-help`: Displays help information.

## Classes

### `Program`

The main class that sets up and runs the sorting pipeline.

#### Methods

- `Main(string[] args)`: The entry point of the application. Parses arguments, configures the sorting pipeline, and runs it.

### `ArgumentParser`

Parses command-line arguments.

#### Properties

- `Input`: The input file path.
- `Output`: The output file path.
- `BufferSizeMB`: The size of the buffer to use for reading and writing files (in megabytes).
- `MaxParallelism`: The maximum number of parallel tasks to use for processing.
- `ShowHelp`: Indicates whether to show help information.

#### Methods

- `ArgumentParser(string[] args)`: Constructor that initializes the parser with command-line arguments.
- `ParseArguments(string[] args)`: Parses the command-line arguments.

### `FileEntry`

Represents a single entry in the file to be sorted.

#### Properties

- `NumberString`: The numeric part of the entry.
- `Text`: The text part of the entry.

#### Methods

- `CompareTo(FileEntry other)`: Compares this instance with another `FileEntry` for sorting.
- `ToString()`: Returns a string representation of the entry.
- `Parse(string line)`: Parses a line of text into a `FileEntry` object.

## Example

Here is an example of how to use the application:

```sh
dotnet run -- -input "data/generated_data.txt" -output "data/sorted_data.txt" -bufferSizeMB 64 -maxParallelism 4
```
This command will sort the data in `generated_data.txt` and save the sorted data to `sorted_data.txt` using a buffer size of 64 MB and a maximum parallelism of 4.