////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
using System.Reflection;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The inteager value element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class IntegerValueElement : LayoutElement, IIntegerValueEditor
	{
        /// <summary>
        /// The inteager value box.
        /// </summary>
        public readonly IntValueBox IntValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerValueElement"/> class.
        /// </summary>
        public IntegerValueElement()
        {
            IntValue = new IntValueBox(0);
        }
		
		/// <summary>
		/// Sets the editor limits from member <see cref="LimitAttribute"/>.
		/// </summary>
		/// <param name="member">The member.</param>
		public void SetLimits(MemberInfo member)
		{
			// Try get limit attribute for value min/max range setting and slider speed
			if (member != null)
			{
				var attributes = member.GetCustomAttributes(true);
				var limit = attributes.FirstOrDefault(x => x is LimitAttribute);
				if (limit != null)
				{
					IntValue.SetLimits((LimitAttribute)limit);
				}
			}
		}

		/// <summary>
		/// Sets the editor limits from member <see cref="LimitAttribute"/>.
		/// </summary>
		/// <param name="limit">The limit.</param>
		public void SetLimits(LimitAttribute limit)
		{
			if (limit != null)
			{
				IntValue.SetLimits(limit);
			}
		}

		/// <summary>
		/// Sets the editor limits from the other <see cref="IntegerValueElement"/>.
		/// </summary>
		/// <param name="other">The other.</param>
		public void SetLimits(IntegerValueElement other)
		{
			if (other != null)
			{
				IntValue.SetLimits(other.IntValue);
			}
		}

		/// <inheritdoc />
		public override Control Control => IntValue;

		/// <inheritdoc />
		public int Value
		{
			get => IntValue.Value;
			set => IntValue.Value = value;
		}

		/// <inheritdoc />
		public bool IsSliding => IntValue.IsSliding;
	}
}
