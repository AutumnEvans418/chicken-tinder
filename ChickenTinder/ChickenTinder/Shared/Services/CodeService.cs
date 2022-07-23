namespace ChickenTinder.Shared.Services;

public class CodeManager
{
    private static Random _random = new();

    public static string RandomString(int length = 5)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
