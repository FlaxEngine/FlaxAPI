using System.Reflection;

namespace FlaxEditor.Utilities
{
    /// <summary>
    ///     This structure represents the comparison of one member of an object to the corresponding member of another object.
    /// </summary>
    public struct MemberComparison
    {
        /// <summary>
        ///     Member this Comparison compares
        /// </summary>
        public readonly MemberInfo Member;

        /// <summary>
        ///     The value of first object respective member
        /// </summary>
        public readonly object Value1;

        /// <summary>
        ///     The value of second object respective member
        /// </summary>
        public readonly object Value2;

        public MemberComparison(MemberInfo member, object value1, object value2)
        {
            Member = member;
            Value1 = value1;
            Value2 = value2;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Member.Name + ": " + Value1.ToString() + (Value1.Equals(Value2) ? " == " : " != ") +
                   Value2.ToString();
        }
    }
}