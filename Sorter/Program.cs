using System.Diagnostics;
using Sorter;
using SortingAlgorithms.External;


// Sorter entry point.
var parser = new ArgumentParser(args);

if (parser.ShowHelp || string.IsNullOrEmpty(parser.Input) || string.IsNullOrEmpty(parser.Output))
{
    HelpPresenter.ShowHelp();
    return;
}

try
{
    var stopwatch = Stopwatch.StartNew();

    var config = new ExternalSortingConfig<FileEntry>(
        parser.Input,
        parser.Output,
        FileEntry.Parse,
        parser.BufferSizeMB * 1024 * 1024,
        parser.MaxParallelism);

    var sorter = new ExternalSortingPipeline<FileEntry>(config);

    await sorter.Run();

    stopwatch.Stop();

    Console.WriteLine($"File sorted in {stopwatch.ElapsedMilliseconds} ms");
}
catch (Exception ex)
{
    Console.WriteLine($"Error sorting file: {ex.Message}");
}