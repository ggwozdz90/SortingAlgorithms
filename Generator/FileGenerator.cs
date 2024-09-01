namespace Generator;

using System.Text;

/// <summary>
///     Generates files with specified size.
/// </summary>
public class FileGenerator
{
    private readonly StringGenerator _stringGenerator = new();

    public void GenerateFile(string filePath, long fileSize)
    {
        const int minNumber = 1;
        const int maxNumber = 1_000_000;
        const int minLength = 5;
        const int maxLength = 30;
        const int bufferSize = 8 * 1024 * 1024;

        var fullPath = Path.GetFullPath(filePath);

        using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Write);
        using var bs = new BufferedStream(fs, bufferSize);
        using var writer = new StreamWriter(bs, Encoding.UTF8, bufferSize);

        var random = new Random();
        long currentSize = 0;

        while (currentSize < fileSize)
        {
            var line = _stringGenerator.Generate(random, minNumber, maxNumber, minLength, maxLength);
            writer.WriteLine(line);
            currentSize += line.Length + Environment.NewLine.Length;
        }
    }
}