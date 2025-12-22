using System.Collections;
using AdventOfCode._2025.Day1;

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

    // [Fact]
    // public void Test3()
    // {
    //     var foo = new BitArray([false, true, true, false]);
    //     var bar = new BitArray([true, true, true, true]);
    //     var result = ((BitArray)foo.Clone()).Xor(bar);
    //
    //     var expectation = new BitArray([false, true, true, false]);
    //
    //     for (var i = 0; i < expectation.Length; i++)
    //     {
    //         Assert.Equal(foo[i], expectation[i]);
    //     }
    // }

    [Fact]
    public void Test4()
    {
        var arr = new bool[3, 3]
        {
            { true, true, true },
            { true, false, false },
            { true, false, false }
        };

        var newArray = Day12.Rotate(arr, 1);

        var target = new bool[3, 3]
        {
            { true, false, false },
            { true, false, false },
            { true, true, true }
        };

        for (var y = 0; y < target.GetLength(0); y++)
        for (var x = 0; x < target.GetLength(1); x++)
            Assert.Equal(target[y, x], newArray[y, x]);
    }

    [Fact]
    public void Test5()
    {
        var arr = new bool[3, 3]
        {
            { true, true, true },
            { true, false, false },
            { true, false, false }
        };

        var newArray = Day12.Rotate(arr, 2);

        var target = new bool[3, 3]
        {
            { false, false, true },
            { false, false, true },
            { true, true, true }
        };

        for (var y = 0; y < target.GetLength(0); y++)
        for (var x = 0; x < target.GetLength(1); x++)
            Assert.Equal(target[y, x], newArray[y, x]);
    }

    [Fact]
    public void Test6()
    {
        var arr = new bool[3, 3]
        {
            { true, true, true },
            { true, false, false },
            { true, false, false }
        };

        var newArray = Day12.Flip(arr, (-1, 1));

        var target = new bool[3, 3]
        {
            { true, false, false },
            { true, false, false },
            { true, true, true }
        };

        for (var y = 0; y < target.GetLength(0); y++)
        for (var x = 0; x < target.GetLength(1); x++)
            Assert.Equal(target[y, x], newArray[y, x]);
    }

    [Fact]
    public void Test7()
    {
        var arr = new bool[3, 3]
        {
            { true, true, true },
            { true, false, false },
            { true, false, false }
        };

        var newArray = Day12.Flip(arr, (-1, -1));

        var target = new bool[3, 3]
        {
            { false, false, true },
            { false, false, true },
            { true, true, true }
        };

        for (var y = 0; y < target.GetLength(0); y++)
        for (var x = 0; x < target.GetLength(1); x++)
            Assert.Equal(target[y, x], newArray[y, x]);
    }
}