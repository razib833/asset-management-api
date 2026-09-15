namespace JamunaBank.Procurement.API.Tests;

internal static class TestAssert
{
    public static void True(bool condition)
    {
        if (!condition) throw new InvalidOperationException("Expected condition to be true.");
    }

    public static void False(bool condition) => True(!condition);

    public static void Equal<T>(T expected, T actual) where T : notnull
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException($"Expected '{expected}', actual '{actual}'.");
    }

    public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        if (!expected.SequenceEqual(actual))
            throw new InvalidOperationException("Sequences are not equal.");
    }

    public static TException Throws<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException exception) { return exception; }
        throw new InvalidOperationException($"Expected exception '{typeof(TException).Name}'.");
    }
}
