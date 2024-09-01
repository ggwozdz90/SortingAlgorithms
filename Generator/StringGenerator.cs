namespace Generator;

/// <summary>
///     Generates string with specified pattern.
/// </summary>
public class StringGenerator
{
    public string Generate(Random random, int minNumber, int maxNumber, int minLength, int maxLength)
    {
        var randomNumber = GenerateRandomNumber(random, minNumber, maxNumber);
        var randomString = GenerateRandomString(random, minLength, maxLength);
        var line = $"{randomNumber}. {randomString}";

        return line;
    }

    private int GenerateRandomNumber(Random random, int minNumber, int maxNumber)
    {
        return random.Next(minNumber, maxNumber);
    }

    private string GenerateRandomString(Random random, int minLength, int maxLength)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
        var length = random.Next(minLength, maxLength);
        Span<char> span = stackalloc char[length];

        for (var i = 0; i < length; i++)
            span[i] = chars[random.Next(chars.Length)];

        return new string(span);
    }
}