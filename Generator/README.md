# File Generator Application

This application generates large files with random data. It allows you to specify the file path and size through command-line arguments.

## Command-Line Arguments

- `-path`: The output file path.
- `-mbsize`: The size of the file to generate in megabytes.
- `-gbsize`: The size of the file to generate in gigabytes.
- `-help`: Displays help information.

## Classes

### `Program`

The main class that sets up and runs the file generation process.

#### Methods

- `Main(string[] args)`: The entry point of the application. Parses arguments, configures the file generator, and runs it.

### `ArgumentParser`

Parses command-line arguments.

#### Properties

- `Path`: The output file path.
- `FileSize`: The size of the file to generate.
- `ShowHelp`: Indicates whether to show help information.

#### Methods

- `ArgumentParser(string[] args)`: Constructor that initializes the parser with command-line arguments.
- `ParseArguments(string[] args)`: Parses the command-line arguments.

### `FileGenerator`

Generates files with specified size.

#### Methods

- `GenerateFile(string filePath, long fileSize)`: Generates a file at the specified path with the specified size.

### `StringGenerator`

Generates strings with a specified pattern.

#### Methods

- `Generate(Random random, int minNumber, int maxNumber, int minLength, int maxLength)`: Generates a string with a random number and random text.
- `GenerateRandomNumber(Random random, int minNumber, int maxNumber)`: Generates a random number within the specified range.
- `GenerateRandomString(Random random, int minLength, int maxLength)`: Generates a random string with the specified length.

## Usage Example

To run the application, use the following command:

```sh
dotnet run -- -path "output.txt" -mbsize 100
```

This command will generate a file named `output.txt` with a size of 100 MB.

