using System.Threading.Channels;
using FluentAssertions;
using SortingAlgorithms.External;

namespace SortingAlgorithms.Tests.External;

[TestFixture]
public class SortManagerTests
{
    [Test]
    public async Task Sort_ShouldSortChunksCorrectly()
    {
        // Arrange
        var maxParallelism = 2;
        var sortManager = new SortManager<string>(maxParallelism);
        var channel = Channel.CreateUnbounded<List<string>>();
        var writer = channel.Writer;
        var reader = channel.Reader;

        var inputChunks = new List<List<string>>
        {
            new() { "45. Orange", "3. Grape" },
            new() { "1. Orange", "32. Grape" },
            new() { "2. Mango" }
        };

        foreach (var chunk in inputChunks)
        {
            await writer.WriteAsync(chunk);
        }
        writer.Complete();

        var outputChannel = Channel.CreateUnbounded<List<string>>();
        var outputWriter = outputChannel.Writer;
        var outputReader = outputChannel.Reader;

        // Act
        await sortManager.Sort(reader, outputWriter);
        var resultChunks = new List<List<string>>();

        await foreach (var chunk in outputReader.ReadAllAsync())
        {
            resultChunks.Add(chunk);
        }

        // Assert
        var expectedChunks = new List<List<string>>
        {
            new() { "3. Grape", "45. Orange" },
            new() { "1. Orange", "32. Grape" },
            new() { "2. Mango" }
        };

        foreach (var resultChunk in resultChunks)
        {
            resultChunk.Should().BeEquivalentTo(expectedChunks.FirstOrDefault(chunk => chunk.SequenceEqual(resultChunk)));
        }
    }
}