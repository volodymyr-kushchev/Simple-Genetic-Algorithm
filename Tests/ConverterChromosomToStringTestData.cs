using System.Collections;

namespace Tests;

public class ConverterChromosomToStringTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [new bool[] { true, false, true }, "101"];
        yield return [new bool[] { false, false, false }, "000"];
        yield return [new bool[] { false }, "0"];
        yield return [new bool[] { true }, "1"];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}