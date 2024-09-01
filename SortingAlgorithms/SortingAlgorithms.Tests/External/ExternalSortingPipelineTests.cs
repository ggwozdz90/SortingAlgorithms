using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SortingAlgorithms.External;

namespace SortingAlgorithms.Tests.External;

[TestFixture]
public class ExternalSortingPipelineTests
{
    private const string TestDirectory = "ExternalSortingPipelineTests";
    private const string OutputFilePath = $"{TestDirectory}/output.txt";

    [SetUp]
    public void SetUp()
    {
        if (Directory.Exists(TestDirectory))
            Directory.Delete(TestDirectory, true);

        Directory.CreateDirectory(TestDirectory);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(TestDirectory))
            Directory.Delete(TestDirectory, true);
    }

    [Test]
    public async Task ExternalSortingPipeline_ShouldSortDataCorrectly()
    {
        // Arrange
        var inputLines = new[]
        {
            "45. Orange",
            "3. Grape",
            "1. Orange",
            "32. Grape",
            "2. Mango"
        };

        await File.WriteAllLinesAsync($"{TestDirectory}/input.txt", inputLines);

        var config = new ExternalSortingConfig<string>(
            $"{TestDirectory}/input.txt",
            OutputFilePath,
            bufferSize: 50,
            maxParallelism: 2,
            lineToObjectParser: line => line);

        var pipeline = new ExternalSortingPipeline<string>(config);

        // Act
        await pipeline.Run();

        // Assert
        var expectedOutputLines = new[]
        {
            "1. Orange",
            "2. Mango",
            "3. Grape",
            "32. Grape",
            "45. Orange"
        };

        var outputLines = await File.ReadAllLinesAsync(OutputFilePath);
        outputLines.Should().BeEquivalentTo(expectedOutputLines);
    }
}