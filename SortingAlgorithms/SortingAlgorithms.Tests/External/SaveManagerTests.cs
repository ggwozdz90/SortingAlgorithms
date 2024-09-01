using System.Threading.Channels;
using FluentAssertions;
using SortingAlgorithms.External;

namespace SortingAlgorithms.Tests.External
{
    [TestFixture]
    public class SaveManagerTests
    {
        private const string OutputDirectory = "SaveManagerTests";
        private const string OutputFileName = "output.txt";
        private string _outputFilePath;

        [SetUp]
        public void SetUp()
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            _outputFilePath = Path.Combine(OutputDirectory, OutputFileName);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(OutputDirectory))
            {
                Directory.Delete(OutputDirectory, true);
            }
        }

        [Test]
        public async Task Save_ShouldSaveChunksToFiles()
        {
            // Arrange
            var bufferSize = 50L;
            var maxParallelism = 2;
            var saveManager = new SaveManager<string>(_outputFilePath, bufferSize, maxParallelism);
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

            // Act
            var savedFiles = await saveManager.Save(reader);

            // Assert
            savedFiles.Count.Should().Be(3);
            var expectedContents = new List<string>
            {
                "2. Mango\r\n",
                "1. Orange\r\n32. Grape\r\n",
                "45. Orange\r\n3. Grape\r\n"
            };

            foreach (var expectedContent in expectedContents)
            {
                savedFiles.Should().Contain(file => File.ReadAllText(file) == expectedContent);
            }
        }
    }
}