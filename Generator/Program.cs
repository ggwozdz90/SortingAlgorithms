using System.Diagnostics;
using Generator;

// Generator entry point.
var parser = new ArgumentParser(args);

if (parser.ShowHelp || parser.Path == null || parser.FileSize == 0)
{
    HelpPresenter.ShowHelp();
    return;
}

try
{
    var stopwatch = Stopwatch.StartNew();

    var fileGenerator = new FileGenerator();
    fileGenerator.GenerateFile(parser.Path, parser.FileSize);

    stopwatch.Stop();
    Console.WriteLine($"File generated in {stopwatch.ElapsedMilliseconds} ms");
}
catch (Exception ex)
{
    Console.WriteLine($"Error generating file: {ex.Message}");
}