using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MathSortTest
{
    public int[] arrays = new[]
    {
        5, 3, 6, 9, 12, 0, 1, 52, 7, 16, 28, 99, 105, 116, 145
    };

    // A Test behaves as an ordinary method
    [Test]
    public void MathSortTestSimplePasses()
    {

    }
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator MathSortTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}