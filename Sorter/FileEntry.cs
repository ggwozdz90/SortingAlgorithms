namespace Sorter;

public record FileEntry : IComparable<FileEntry>
{
    public string NumberString { get; private init; }
    public string Text { get; private init; }

    public int CompareTo(FileEntry other)
    {
        var compare = string.Compare(Text, other.Text, StringComparison.Ordinal);

        if (compare != 0)
            return compare;

        var thisNumber = int.Parse(NumberString);
        var otherNumber = int.Parse(other.NumberString);

        return thisNumber.CompareTo(otherNumber);
    }

    public override string ToString()
    {
        return $"{NumberString}. {Text}";
    }

    public static FileEntry Parse(string line)
    {
        var dotIndex = line.IndexOf(". ", StringComparison.Ordinal);

        var number = line[..dotIndex];
        var text = line[(dotIndex + 2)..];

        return new FileEntry
        {
            NumberString = number,
            Text = text
        };
    }
}