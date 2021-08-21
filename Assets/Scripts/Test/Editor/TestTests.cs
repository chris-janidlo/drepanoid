using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Drepanoid.Test.Editor
{
    public class TestTests
    {
        [Test]
        public void TestTestsSimplePasses()
        {
            Assert.That(true);
        }

        [UnityTest]
        public IEnumerator TestTestsWithEnumeratorPasses()
        {
            yield return null;
            Assert.That(true);
        }
    }
}
