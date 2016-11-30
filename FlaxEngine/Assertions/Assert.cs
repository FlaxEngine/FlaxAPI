using System.Collections.Generic;
using System.Diagnostics;

namespace FlaxEngine.Assertions
{
    /// <summary>
    /// The Assert class contains assertion methods for setting invariants in the code.
    /// </summary>
    [DebuggerStepThrough]
    public static class Assert
    {
        /// <summary>
        /// Should an exception be thrown on a failure.
        /// </summary>
        public static bool raiseExceptions;

        /// <summary>
        /// Asserts that the values are approximately equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// Note: Every time you call the method with tolerance specified, a new instance of Assertions.Comparers.FloatComparer
        /// is created. For performance reasons you might want to instance your own comparer and pass it to the AreEqual method.
        /// If the tolerance is not specifies, a default comparer is used and the issue does not occur.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual)
        {
            AreEqual(expected, actual, null, FloatComparer.ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Asserts that the values are approximately equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// Note: Every time you call the method with tolerance specified, a new instance of Assertions.Comparers.FloatComparer
        /// is created. For performance reasons you might want to instance your own comparer and pass it to the AreEqual method.
        /// If the tolerance is not specifies, a default comparer is used and the issue does not occur.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="message"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual, string message)
        {
            AreEqual(expected, actual, message, FloatComparer.ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Asserts that the values are approximately equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// Note: Every time you call the method with tolerance specified, a new instance of Assertions.Comparers.FloatComparer
        /// is created. For performance reasons you might want to instance your own comparer and pass it to the AreEqual method.
        /// If the tolerance is not specifies, a default comparer is used and the issue does not occur.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual, float tolerance)
        {
            AreApproximatelyEqual(expected, actual, tolerance, null);
        }

        /// <summary>
        /// Asserts that the values are approximately equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// Note: Every time you call the method with tolerance specified, a new instance of Assertions.Comparers.FloatComparer
        /// is created. For performance reasons you might want to instance your own comparer and pass it to the AreEqual method.
        /// If the tolerance is not specifies, a default comparer is used and the issue does not occur.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="message"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string message)
        {
            AreEqual(expected, actual, message, new FloatComparer(tolerance));
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreEqual <T>(T expected, T actual)
        {
            AreEqual(expected, actual, null);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreEqual <T>(T expected, T actual, string message)
        {
            AreEqual(expected, actual, message, EqualityComparer<T>.Default);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreEqual <T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
            {
                AreEqual((object)expected as Object, (object)actual as Object, message);
                return;
            }
            if (!comparer.Equals(actual, expected))
                Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, true), message);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreEqual(Object expected, Object actual, string message)
        {
            if (actual != expected)
                Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, true), message);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual)
        {
            AreNotEqual(expected, actual, null, FloatComparer.ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="message"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual, string message)
        {
            AreNotEqual(expected, actual, message, FloatComparer.ComparerWithDefaultTolerance);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance)
        {
            AreNotApproximatelyEqual(expected, actual, tolerance, null);
        }

        /// <summary>
        /// Asserts that the values are approximately not equal. An absolute error check is used for approximate equality check
        /// (|a-b| &lt; tolerance). Default tolerance is 0.00001f.
        /// </summary>
        /// <param name="tolerance">Tolerance of approximation.</param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="message"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance, string message)
        {
            AreNotEqual(expected, actual, message, new FloatComparer(tolerance));
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotEqual <T>(T expected, T actual)
        {
            AreNotEqual(expected, actual, null);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotEqual <T>(T expected, T actual, string message)
        {
            AreNotEqual(expected, actual, message, EqualityComparer<T>.Default);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotEqual <T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
            {
                AreNotEqual((object)expected as Object, (object)actual as Object, message);
                return;
            }
            if (comparer.Equals(actual, expected))
                Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, false), message);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void AreNotEqual(Object expected, Object actual, string message)
        {
            if (actual == expected)
                Fail(AssertionMessageUtil.GetEqualityMessage(actual, expected, false), message);
        }

        private static void Fail(string message, string userMessage)
        {
            if (Debugger.IsAttached)
                throw new AssertionException(message, userMessage);
            if (raiseExceptions)
                throw new AssertionException(message, userMessage);
            if (message == null)
                message = "Assertion has failed\n";
            if (userMessage != null)
                message = string.Concat(userMessage, '\n', message);
            Debug.LogAssertion(message);
        }

        /// <summary>
        /// Asserts that the condition is false.
        /// </summary>
        /// <param name="condition"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsFalse(bool condition)
        {
            IsFalse(condition, null);
        }

        /// <summary>
        /// Asserts that the condition is false.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
                Fail(AssertionMessageUtil.BooleanFailureMessage(false), message);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsNotNull <T>(T value)
            where T : class
        {
            IsNotNull(value, null);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsNotNull <T>(T value, string message)
            where T : class
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
                IsNotNull((object)value as Object, message);
            else if (value == null)
                Fail(AssertionMessageUtil.NullFailureMessage(null, false), message);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsNotNull(Object value, string message)
        {
            if (value == null)
                Fail(AssertionMessageUtil.NullFailureMessage(null, false), message);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsNull <T>(T value)
            where T : class
        {
            IsNull(value, null);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsNull <T>(T value, string message)
            where T : class
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
                IsNull((object)value as Object, message);
            else if (value != null)
                Fail(AssertionMessageUtil.NullFailureMessage(value, true), message);
        }

        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsNull(Object value, string message)
        {
            if (value != null)
                Fail(AssertionMessageUtil.NullFailureMessage(value, true), message);
        }

        /// <summary>
        /// Asserts that the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsTrue(bool condition)
        {
            IsTrue(condition, null);
        }

        /// <summary>
        /// Asserts that the condition is true.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        [Conditional("CELELEJ_ASSERTIONS")]
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
                Fail(AssertionMessageUtil.BooleanFailureMessage(true), message);
        }
    }
}
