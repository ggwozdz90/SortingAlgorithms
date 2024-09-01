namespace Sorter;

/// <summary>
///     Displays help information.
/// </summary>
public static class HelpPresenter
{
    public static void ShowHelp()
    {
        Console.WriteLine("Usage: Generator -input <input_file_path> -output <output_file_path> [-help]");
        Console.WriteLine("Options:");
        Console.WriteLine("  -input   Specifies the input file path.");
        Console.WriteLine("  -output  Specifies the output file path.");
        Console.WriteLine("  -help    Shows this help message.");
    }
}