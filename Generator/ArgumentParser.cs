namespace Generator;

/// <summary>
///     Parses command-line arguments.
/// </summary>
public class ArgumentParser
{
    public ArgumentParser(string[] args)
    {
        ParseArguments(args);
    }

    public string Path { get; private set; }
    public long FileSize { get; private set; }
    public bool ShowHelp { get; private set; }

    private void ParseArguments(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
            switch (args[i])
            {
                case "-path":
                    if (i + 1 < args.Length)
                    {
                        Path = args[i + 1];
                        i++;
                    }

                    break;
                case "-mbsize":
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out var mbSize))
                    {
                        FileSize = mbSize * 1024L * 1024L;
                        i++;
                    }

                    break;
                case "-gbsize":
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out var gbSize))
                    {
                        FileSize = gbSize * 1024L * 1024L * 1024L;
                        i++;
                    }

                    break;
                case "-help":
                    ShowHelp = true;
                    break;
            }
    }
}