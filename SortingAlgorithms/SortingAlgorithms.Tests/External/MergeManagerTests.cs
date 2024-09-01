using FluentAssertions;
using SortingAlgorithms.External;

namespace SortingAlgorithms.Tests.External
{
    [TestFixture]
    public class MergeManagerTests
    {
        private const string OutputDirectory = "MergeManagerTests";
        private const string OutputFilePath = "MergeManagerTests/merged_output.txt";
        private List<string> _chunkFiles;

        [SetUp]
        public void SetUp()
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            _chunkFiles =
            [
                Path.Combine(OutputDirectory, "chunk1.txt"),
                Path.Combine(OutputDirectory, "chunk2.txt"),
                Path.Combine(OutputDirectory, "chunk3.txt")
            ];

            var chunk1 = new List<string> { "apple", "banana", "cherry" };
            var chunk2 = new List<string> { "apricot", "blueberry", "citrus" };
            var chunk3 = new List<string> { "avocado", "blackberry", "coconut" };

            File.WriteAllLines(_chunkFiles[0], chunk1);
            File.WriteAllLines(_chunkFiles[1], chunk2);
            File.WriteAllLines(_chunkFiles[2], chunk3);
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
        public void Merge_ShouldMergeAndSortChunksCorrectly()
        {
            // Arrange
            var mergeManager = new MergeManager<string>(
                OutputFilePath,
                bufferSize: 10,
                lineToObjectParser: line => line
            );

            // Act
            mergeManager.Merge(_chunkFiles);

            // Assert
            var expectedOutput = new List<string>
            {
                "apple",
                "apricot",
                "avocado",
                "banana",
                "blackberry",
                "blueberry",
                "cherry",
                "citrus",
                "coconut"
            };

            var actualOutput = File.ReadAllLines(OutputFilePath).ToList();
            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}