using System.Threading.Channels;
using FluentAssertions;
using SortingAlgorithms.External;

namespace SortingAlgorithms.Tests.External;

[TestFixture]
public class ChunkManagerTests
{
    private const string TestDataFilePath = "test_data.txt";

    [SetUp]
    public void SetUp()
    {
        File.WriteAllText(TestDataFilePath,
@"45. Orange
3. Grape
1. Orange
32. Grape
2. Mango");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(TestDataFilePath))
        {
            File.Delete(TestDataFilePath);
        }
    }

    [Test]
    public async Task Split_ShouldProcessChunksCorrectly()
    {
        // Arrange
        var bufferSize = 50L;
        var maxParallelism = 2;
        string LineToObjectParser(string line) => line;

        var chunkManager = new ChunkManager<string>(TestDataFilePath, bufferSize, maxParallelism, LineToObjectParser);
        var channel = Channel.CreateUnbounded<List<string>>();
        var writer = channel.Writer;
        var reader = channel.Reader;

        // Act
        await chunkManager.Split(writer);
        var chunks = new List<List<string>>();
        var chunksCount = 0;

        await foreach (var chunk in reader.ReadAllAsync())
        {
            chunksCount++;
            chunks.Add([.. chunk]);
        }

        // Assert
        chunksCount.Should().Be(3);
        chunks[0].Should().BeEquivalentTo(new List<string> { "45. Orange", "3. Grape" });
        chunks[1].Should().BeEquivalentTo(new List<string> { "1. Orange", "32. Grape" });
        chunks[2].Should().BeEquivalentTo(new List<string> { "2. Mango" });
    }
}