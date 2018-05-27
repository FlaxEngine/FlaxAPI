using System;

namespace FlaxEngine.Assertions
{
    internal class AssertionMessageUtil
    {
        public static string BooleanFailureMessage(bool expected)
        {
            return GetMessage(string.Concat("Value was ", !expected), expected.ToString());
        }

        public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
        {
            string str = string.Format("Values are {0}equal.", new object[] { !expectEqual ? string.Empty : "not " });
            object[] objArray =
            {
                actual,
                expected,
                null
            };
            objArray[2] = !expectEqual ? "!=" : "==";
            return GetMessage(str, string.Format("{0} {2} {1}", objArray));
        }

        public static string GetMessage(string failureMessage)
        {
            return string.Format("{0} {1}", new object[]
            {
                "Assertion failed.",
                failureMessage
            });
        }

        public static string GetMessage(string failureMessage, string expected)
        {
            return GetMessage($"{failureMessage}{Environment.NewLine}Expected: {expected}");
        }

        public static string NullFailureMessage(object value, bool expectNull)
        {
            return GetMessage(string.Format("Value was {0}Null", new object[] { !expectNull ? string.Empty : "not " }), string.Format("Value was {0}Null", new object[] { !expectNull ? "not " : string.Empty }));
        }
    }
}
