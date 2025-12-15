using System.Collections;

namespace AdventOfCode.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var foo = new BitArray([false, true, true, false]);
        var bar = new BitArray([true, false, true, false]);
        var result = foo.Xor(bar);

        var expectation = new BitArray([true, true, false, false]);

        for (var i = 0; i < expectation.Length; i++) Assert.Equal(result[i], expectation[i]);
    }

    [Fact]
    public void Test2()
    {
        var foo = new BitArray(4);

        for (var i = 0; i < foo.Length; i++) Assert.False(foo[i]);
    }

    [Fact]
    public void Test3()
    {
        var foo = new BitArray([false, true, true, false]);
        var bar = new BitArray([true, true, true, true]);
        var result = ((BitArray)foo.Clone()).Xor(bar);

        var expectation = new BitArray([false, true, true, false]);

        for (var i = 0; i < expectation.Length; i++)
        {
            Assert.Equal(foo[i], expectation[i]);
        }
    }
}