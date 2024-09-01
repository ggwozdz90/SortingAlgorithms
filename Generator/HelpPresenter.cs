namespace Generator;

/// <summary>
///     Displays help information.
/// </summary>
public static class HelpPresenter
{
    public static void ShowHelp()
    {
        Console.WriteLine(
            "Usage: Generator -path <relative_file_path> -mbsize <file_size_in_mb> | -gbsize <file_size_in_gb>");
        Console.WriteLine("Options:");
        Console.WriteLine("  -path    Specifies the relative path to the file.");
        Console.WriteLine("  -mbsize  Specifies the size of the file in megabytes.");
        Console.WriteLine("  -gbsize  Specifies the size of the file in gigabytes.");
        Console.WriteLine("  -help    Shows this help message.");
    }
}