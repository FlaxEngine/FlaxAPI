using System;
using FlaxEngine.Utilities;
using NUnit.Framework;

namespace FlaxEngine.Tests
{
	/// <summary>
	/// Tests for <see cref="Transform"/>.
	/// </summary>
	[TestFixture]
	public class TestTransform
	{
		/// <summary>
		/// Test conversions between transform local/world space
		/// </summary>
		[Test]
		public void TestWorldAndLocalSpace()
		{
			Transform trans = new Transform(new Vector3(1, 2, 3));

			Assert.AreEqual(new Vector3(1, 2, 3), trans.LocalToWorld(new Vector3(0, 0, 0)));
			Assert.AreEqual(new Vector3(4, 4, 4), trans.LocalToWorld(new Vector3(3, 2, 1)));
			Assert.AreEqual(new Vector3(-1, -2, -3), trans.WorldToLocal(new Vector3(0, 0, 0)));
			Assert.AreEqual(new Vector3(0, 0, 0), trans.WorldToLocal(new Vector3(1, 2, 3)));

			trans = new Transform(Vector3.Zero, Quaternion.Euler(0, 90, 0));
			Assert.AreEqual(new Vector3(0, 2, -1), trans.LocalToWorld(new Vector3(1, 2, 0)));

			trans.Translation = new Vector3(1, 0, 0);
			trans.Orientation = Quaternion.RotationX((float)Math.PI * 0.5f);
			trans.Scale = new Vector3(2, 2, 2);
			Assert.AreEqual(new Vector3(1, 0, 2), trans.LocalToWorld(new Vector3(0, 1, 0)));

			Transform t1 = trans.LocalToWorld(Transform.Identity);
			Assert.AreEqual(new Vector3(1.0f, 0, 0), t1.Translation);
			Assert.AreEqual(Quaternion.RotationX((float)Math.PI * 0.5f), t1.Orientation);
			Assert.AreEqual(new Vector3(2.0f, 2.0f, 2.0f), t1.Scale);

			Transform t2 = trans.WorldToLocal(Transform.Identity);
			Assert.AreEqual(new Vector3(-0.5f, 0, 0), t2.Translation);
			Assert.AreEqual(Quaternion.RotationX((float)Math.PI * -0.5f), t2.Orientation);
			Assert.AreEqual(new Vector3(0.5f, 0.5f, 0.5f), t2.Scale);

			var rand = new Random(10);
			for (int i = 0; i < 10; i++)
			{
				Transform a = new Transform(rand.NextVector3(), Quaternion.Euler(i * 10, 0, i), rand.NextVector3() * 10.0f);
				Transform b = new Transform(rand.NextVector3(), Quaternion.Identity, rand.NextVector3() * 0.3f);

				Transform ab = a.LocalToWorld(b);
				Transform ba = a.WorldToLocal(ab);

				Assert.AreEqual(b, ba);
			}
		}
	}
}
