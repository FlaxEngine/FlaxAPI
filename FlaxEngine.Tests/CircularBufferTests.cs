using System;
using System.Collections;
using FlaxEngine.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = FlaxEngine.Assertions.Assert;

namespace FlaxEngine.TestMethods
{
    [TestClass]
    public class CircularBufferTestMethods
    {
        [TestMethod]
        public void TestMethodFrontOverwrite()
        {
            var buffer = new CircularBuffer<long>(3);
            for (int i = 0; i < 5; i++)
            {
                buffer.PushFront(i);
            }
            Assert.AreEqual(2, buffer[0]);
            Assert.AreEqual(3, buffer[1]);
            Assert.AreEqual(4, buffer[2]);
            Assert.AreEqual(4, buffer.Front());
            Assert.AreEqual(2, buffer.Back());

            Assert.AreEqual(buffer.Count, 3);
            Assert.AreEqual(buffer.Capacity, 3);
        }

        [TestMethod]
        public void TestMethodBackOverwrite()
        {
            var buffer = new CircularBuffer<long>(3);
            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }
            Assert.AreEqual(4, buffer[0]);
            Assert.AreEqual(3, buffer[1]);
            Assert.AreEqual(2, buffer[2]);
            Assert.AreEqual(2, buffer.Front());
            Assert.AreEqual(4, buffer.Back());

            Assert.AreEqual(buffer.Count, 3);
            Assert.AreEqual(buffer.Capacity, 3);
        }

        [TestMethod]
        public void TestMethodMixedOverwrite()
        {
            var buffer = new CircularBuffer<long>(3);
            buffer.PushFront(0);

            buffer.PushFront(1);
            buffer.PushBack(-1);
            Assert.AreEqual(1, buffer.Front());
            Assert.AreEqual(-1, buffer.Back());
            buffer.PushFront(2);
            Assert.AreEqual(2, buffer.Front());
            Assert.AreEqual(0, buffer.Back());
            buffer.PushBack(-2);
            Assert.AreEqual(1, buffer.Front());
            buffer.PushFront(3);
            Assert.AreEqual(0, buffer.Back());
            buffer.PushBack(-3);

            Assert.AreEqual(-3, buffer[0]);
            Assert.AreEqual(0, buffer[1]);
            Assert.AreEqual(1, buffer[2]);
            Assert.AreEqual(1, buffer.Front());
            Assert.AreEqual(-3, buffer.Back());

            Assert.AreEqual(buffer.Count, 3);
            Assert.AreEqual(buffer.Capacity, 3);
        }

        [TestMethod]
        public void TestMethodFrontUnderwrite()
        {
            var buffer = new CircularBuffer<long>(5);
            for (int i = 0; i < 4; i++)
            {
                buffer.PushFront(i);
            }
            buffer.PopFront();
            buffer.PushFront(4);
            Assert.AreEqual(4, buffer.Front());
            Assert.AreEqual(0, buffer.Back());
            buffer.PopFront();
            buffer.PushFront(5);
            Assert.AreEqual(0, buffer[0]);
            Assert.AreEqual(1, buffer[1]);
            Assert.AreEqual(2, buffer[2]);
            Assert.AreEqual(5, buffer[3]);

            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { var test = buffer[4]; });

            Assert.AreEqual(5, buffer.Front());
            Assert.AreEqual(0, buffer.Back());

            Assert.AreEqual(buffer.Count, 4);
            Assert.AreEqual(buffer.Capacity, 5);
        }

        [TestMethod]
        public void TestMethodBackUnderwrite()
        {
            var buffer = new CircularBuffer<long>(5);
            for (int i = 0; i < 4; i++)
            {
                buffer.PushBack(i);
            }
            buffer.PopBack();
            buffer.PushBack(4);
            Assert.AreEqual(4, buffer.Back());
            Assert.AreEqual(0, buffer.Front());
            buffer.PopBack();
            buffer.PushBack(5);
            Assert.AreEqual(5, buffer[0]);
            Assert.AreEqual(2, buffer[1]);
            Assert.AreEqual(1, buffer[2]);
            Assert.AreEqual(0, buffer[3]);

            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { var test = buffer[4]; });

            Assert.AreEqual(0, buffer.Front());
            Assert.AreEqual(5, buffer.Back());

            Assert.AreEqual(buffer.Count, 4);
            Assert.AreEqual(buffer.Capacity, 5);
        }

        [TestMethod]
        public void TestMethodMixedUnderwrite()
        {
            var buffer = new CircularBuffer<long>(5);
            buffer.PushFront(0);

            buffer.PushFront(1);
            buffer.PushBack(-1);
            Assert.AreEqual(1, buffer.Front());
            Assert.AreEqual(-1, buffer.Back());

            buffer.PushFront(2);
            Assert.AreEqual(2, buffer.Front());
            Assert.AreEqual(-1, buffer.Back());

            buffer.PopBack();
            Assert.AreEqual(0, buffer.Back());
            Assert.AreEqual(2, buffer.Front());

            buffer.PushBack(-2);
            Assert.AreEqual(2, buffer.Front());
            Assert.AreEqual(-2, buffer.Back());

            Assert.AreEqual(-2, buffer[0]);
            Assert.AreEqual(0, buffer[1]);
            Assert.AreEqual(1, buffer[2]);
            Assert.AreEqual(2, buffer[3]);

            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { var test = buffer[4]; });

            Assert.AreEqual(buffer.Count, 4);
            Assert.AreEqual(buffer.Capacity, 5);
        }

        [TestMethod]
        public void TestMethodEnumerationFrontWhenOverflown()
        {
            var buffer = new CircularBuffer<long>(5);
            int i;
            for (i = 0; i < 11; i++)
            {
                buffer.PushFront(i);
            }
            for (i = 0; i < buffer.Capacity; i++)
            {
                Assert.AreEqual(buffer.Count + i + 1, buffer[i]);
            }
            i = 6;
            foreach (var item in buffer)
            {
                Assert.AreEqual(i++, item);
            }
            i = 6;
            var e = ((IEnumerable)buffer).GetEnumerator();
            while (e.MoveNext())
            {
                Assert.AreEqual(i++, (long)e.Current);
            }
        }

        [TestMethod]
        public void TestMethodEnumerationBackWhenOverflown()
        {
            var buffer = new CircularBuffer<long>(5);
            int i;
            for (i = 0; i < 11; i++)
            {
                buffer.PushBack(i);
            }
            for (i = 0; i < buffer.Capacity; i++)
            {
                Assert.AreEqual(10 - i, buffer[i]);
            }
            i = 10;
            foreach (var item in buffer)
            {
                Assert.AreEqual(i--, item);
            }
            i = 10;
            var e = ((IEnumerable)buffer).GetEnumerator();
            while (e.MoveNext())
            {
                Assert.AreEqual(i--, (long)e.Current);
            }
        }

        [TestMethod]
        public void TestMethodEnumerationWhenPartiallyFull()
        {
            var buffer = new CircularBuffer<long>(3);
            buffer.PushFront(1);
            buffer.PushBack(0);
            var i = 0;
            foreach (var item in buffer)
            {
                Assert.AreEqual(i++, item);
            }
            Assert.AreEqual(i, 2);
            Assert.AreEqual(buffer.Count, 2);
            Assert.AreEqual(buffer.Capacity, 3);
        }

        [TestMethod]
        public void TestMethodEnumerationWhenPartiallyFullLarge()
        {
            var buffer = new CircularBuffer<long>(20000);
            int i = 0;
            for (i = 0; i < 10000; i++)
            {
                buffer.PushFront(i);
            }
            i = 0;
            foreach (var value in buffer)
            {
                Assert.AreEqual(i++, value);
            }
            Assert.AreEqual(i, 10000);

            Assert.AreEqual(buffer.Count, 10000);
            Assert.AreEqual(buffer.Capacity, 20000);
        }

        [TestMethod]
        public void TestMethodEnumerationWhenEmpty()
        {
            var buffer = new CircularBuffer<long>(3);
            foreach (var value in buffer)
            {
                Assert.Fail("Unexpected Value: " + value);
            }
        }

        [TestMethod]
        public void TestMethodCopyToArray()
        {
            var buffer = new CircularBuffer<long>(3);
            for (int i = 0; i < 5; i++)
            {
                buffer.PushFront(i);
            }
            var testArray = buffer.ToArray();
            Assert.AreEqual(2, testArray[0]);
            Assert.AreEqual(3, testArray[1]);
            Assert.AreEqual(4, testArray[2]);

            testArray = new long[3];
            buffer.CopyTo(testArray, 0);
            Assert.AreEqual(2, testArray[0]);
            Assert.AreEqual(3, testArray[1]);
            Assert.AreEqual(4, testArray[2]);

            testArray = new long[5];
            buffer.CopyTo(testArray, 2);
            Assert.AreEqual(2, testArray[2]);
        }

        [TestMethod]
        public void TestMethodExceptions()
        {
            Assert.ExceptionExpected(typeof(ArgumentOutOfRangeException), () => { new CircularBuffer<long>(0); });
            Assert.ExceptionExpected(typeof(ArgumentOutOfRangeException), () => { new CircularBuffer<long>(-5); });
            var buffer = new CircularBuffer<long>(1);
            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { buffer.Front(); });
            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { buffer.Back(); });
            Assert.ExceptionExpected(typeof(ArgumentOutOfRangeException), () => { var test = buffer[-1]; });
            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { var test = buffer[1]; });
            Assert.ExceptionExpected(typeof(ArgumentOutOfRangeException), () => { buffer[-1] = 0; });
            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { buffer[1] = 0; });
            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { buffer.PopBack(); });
            Assert.ExceptionExpected(typeof(IndexOutOfRangeException), () => { buffer.PopFront(); });
        }
        
        [TestMethod]
        public void TestMethodForceSet()
        {
            var buffer = new CircularBuffer<long>(15);
            for (int i = 0; i < 5; i++)
            {
                buffer.PushFront(i);
            }
            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i] = 1;
            }
            foreach (var item in buffer)
            {
                Assert.AreEqual(1, item);
            }
            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i] = i;
            }
            int j = 0;
            foreach (var item in buffer)
            {
                Assert.AreEqual(j++, item);
            }
        }

        [TestMethod]
        public void TestMethodFrontAndBack()
        {
            var rand = new Random();
            var buffer = new CircularBuffer<long>(3);
            for (int i = 0; i < 25; i++)
            {
                var cur = rand.Next();
                buffer.PushFront(cur);
                Assert.AreEqual(cur, buffer.Front());
                buffer.PopFront();
            }
            for (int i = 0; i < 25; i++)
            {
                var cur = rand.Next();
                buffer.PushBack(cur);
                Assert.AreEqual(cur, buffer.Back());
                buffer.PopBack();
            }
        }

        [TestMethod]
        public void TestMethodConstructors()
        {
            var array = new long[5];
            for (int i = 0; i < 5; i++)
            {
                array[i] = i + 10;
            }
            var buffer = new CircularBuffer<long>(15, array, 5);
            Assert.AreEqual(10, buffer.Back());
            Assert.AreEqual(14, buffer.Front());
            Assert.AreEqual(5, buffer.Count);
            Assert.AreEqual(15, buffer.Capacity);
            Assert.AreEqual(false, buffer.IsEmpty);
        }
    }
}
