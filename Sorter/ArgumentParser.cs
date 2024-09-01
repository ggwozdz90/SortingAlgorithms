namespace Sorter;

/// <summary>
///     Parses command-line arguments.
/// </summary>
public class ArgumentParser
{
    public ArgumentParser(string[] args)
    {
        ParseArguments(args);
    }

    public string Input { get; private set; }
    public string Output { get; private set; }
    public long BufferSizeMB { get; private set; }
    public int MaxParallelism { get; private set; }
    public bool ShowHelp { get; private set; }

    private void ParseArguments(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "-input":
                    if (i + 1 < args.Length)
                    {
                        Input = args[i + 1];
                        i++;
                    }

                    break;
                case "-output":
                    if (i + 1 < args.Length)
                    {
                        Output = args[i + 1];
                        i++;
                    }

                    break;
                case "-bufferSizeMB":
                    if (i + 1 < args.Length)
                    {
                        if (long.TryParse(args[i + 1], out var bufferSizeMB)) BufferSizeMB = bufferSizeMB;
                        i++;
                    }

                    break;
                case "-maxParallelism":
                    if (i + 1 < args.Length)
                    {
                        if (int.TryParse(args[i + 1], out var maxParallelism)) MaxParallelism = maxParallelism;
                        i++;
                    }

                    break;
                case "-help":
                    ShowHelp = true;
                    break;
            }
    }
}