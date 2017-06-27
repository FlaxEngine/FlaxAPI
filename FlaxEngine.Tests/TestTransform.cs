using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlaxEngine.Tests
{
    /// <summary>
    /// Tests for <see cref="Transform"/>.
    /// </summary>
    [TestClass]
    public class TestTransform
    {
        /// <summary>
        /// Test conversions between entity local/world space
        /// </summary>
        [TestMethod]
        public void TestWorldAndLocalSpace()
        {
            Transform trans = new Transform(new Vector3(1, 2, 3));

            // Test point to world/local space conversion
            Assert.AreEqual(new Vector3(1, 2, 3), trans.LocalToWorld(new Vector3(0, 0, 0)));
            Assert.AreEqual(new Vector3(4, 4, 4), trans.LocalToWorld(new Vector3(3, 2, 1)));
            Assert.AreEqual(new Vector3(-1, -2, -3), trans.WorldToLocal(new Vector3(0, 0, 0)));
            Assert.AreEqual(new Vector3(0, 0, 0), trans.WorldToLocal(new Vector3(1, 2, 3)));
            trans.Translation = new Vector3(1, 0, 0);
            trans.Orientation = Quaternion.RotationX((float)Math.PI * 0.5f);
            trans.Scale = new Vector3(2, 2, 2);
            Assert.AreEqual(new Vector3(1, 0, 2), trans.LocalToWorld(new Vector3(0, 1, 0)));
            Transform t1 = trans.WorldToLocal(new Transform(Vector3.Zero));
            Assert.AreEqual(new Vector3(-0.5f, 0, 0), t1.Translation);
            Assert.AreEqual(Quaternion.RotationX((float)Math.PI * -0.5f), t1.Orientation);
            Assert.AreEqual(new Vector3(0.5f, 0.5f, 0.5f), t1.Scale);
        }
    }
}
