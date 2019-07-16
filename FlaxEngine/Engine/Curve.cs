// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlaxEngine
{
    /// <summary>
    /// An animation spline represented by a set of keyframes, each representing an endpoint of a Bezier curve. 
    /// </summary>
    /// <typeparam name="T">The animated value type.</typeparam>
    public class Curve<T> where T : struct
    {
        interface IKeyframeAccess<TT> where TT : struct
        {
            void GetTangent(ref TT value, ref TT tangent, float lengthThird, out TT result);

            void Interpolate(ref TT a, ref TT b, float alpha, out TT result);
        }

        private class KeyframeAccess :
        IKeyframeAccess<int>,
        IKeyframeAccess<double>,
        IKeyframeAccess<float>,
        IKeyframeAccess<Vector2>,
        IKeyframeAccess<Vector3>,
        IKeyframeAccess<Vector4>,
        IKeyframeAccess<Quaternion>,
        IKeyframeAccess<Color>
        {
            /// <inheritdoc />
            public void GetTangent(ref int value, ref int tangent, float lengthThird, out int result)
            {
                result = value + (int)(tangent * lengthThird);
            }

            /// <inheritdoc />
            public void Interpolate(ref int a, ref int b, float alpha, out int result)
            {
                result = (int)(a + alpha * (b - a));
            }

            /// <inheritdoc />
            public void GetTangent(ref double value, ref double tangent, float lengthThird, out double result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref double a, ref double b, float alpha, out double result)
            {
                result = a + alpha * (b - a);
            }

            /// <inheritdoc />
            public void GetTangent(ref float value, ref float tangent, float lengthThird, out float result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref float a, ref float b, float alpha, out float result)
            {
                result = a + alpha * (b - a);
            }

            /// <inheritdoc />
            public void GetTangent(ref Vector2 value, ref Vector2 tangent, float lengthThird, out Vector2 result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref Vector2 a, ref Vector2 b, float alpha, out Vector2 result)
            {
                Vector2.Lerp(ref a, ref b, alpha, out result);
            }

            /// <inheritdoc />
            public void GetTangent(ref Vector3 value, ref Vector3 tangent, float lengthThird, out Vector3 result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref Vector3 a, ref Vector3 b, float alpha, out Vector3 result)
            {
                Vector3.Lerp(ref a, ref b, alpha, out result);
            }

            /// <inheritdoc />
            public void GetTangent(ref Vector4 value, ref Vector4 tangent, float lengthThird, out Vector4 result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref Vector4 a, ref Vector4 b, float alpha, out Vector4 result)
            {
                Vector4.Lerp(ref a, ref b, alpha, out result);
            }

            /// <inheritdoc />
            public void GetTangent(ref Quaternion value, ref Quaternion tangent, float lengthThird, out Quaternion result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref Quaternion a, ref Quaternion b, float alpha, out Quaternion result)
            {
                Quaternion.Slerp(ref a, ref b, alpha, out result);
            }

            /// <inheritdoc />
            public void GetTangent(ref Color value, ref Color tangent, float lengthThird, out Color result)
            {
                result = value + tangent * lengthThird;
            }

            /// <inheritdoc />
            public void Interpolate(ref Color a, ref Color b, float alpha, out Color result)
            {
                Color.Lerp(ref a, ref b, alpha, out result);
            }
        }

        /// <summary>
        /// A single keyframe that can be injected into Bezier curve.
        /// </summary>
        public struct Keyframe : IComparable, IComparable<Keyframe>
        {
            /// <summary>
            /// The time of the keyframe.
            /// </summary>
            [EditorOrder(0), Limit(float.MinValue, float.MaxValue, 0.01f), Tooltip("The time of the keyframe.")]
            public float Time;

            /// <summary>
            /// The value of the curve at keyframe.
            /// </summary>
            [EditorOrder(1), Limit(float.MinValue, float.MaxValue, 0.01f), Tooltip("The value of the curve at keyframe.")]
            public T Value;

            /// <summary>
            /// The input tangent (going from the previous key to this one) of the key.
            /// </summary>
            [EditorOrder(2), Limit(float.MinValue, float.MaxValue, 0.01f), Tooltip("The input tangent (going from the previous key to this one) of the key."), EditorDisplay(null, "Tangent In")]
            public T TangentIn;

            /// <summary>
            /// The output tangent (going from this key to next one) of the key.
            /// </summary>
            [EditorOrder(3), Limit(float.MinValue, float.MaxValue, 0.01f), Tooltip("The output tangent (going from this key to next one) of the key.")]
            public T TangentOut;

            /// <summary>
            /// Initializes a new instance of the <see cref="Keyframe"/> struct.
            /// </summary>
            /// <param name="time">The time.</param>
            /// <param name="value">The value.</param>
            public Keyframe(float time, T value)
            {
                Time = time;
                Value = value;
                TangentIn = TangentOut = default(T);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Keyframe"/> struct.
            /// </summary>
            /// <param name="time">The time.</param>
            /// <param name="value">The value.</param>
            /// <param name="tangentIn">The start tangent.</param>
            /// <param name="tangentOut">The end tangent.</param>
            public Keyframe(float time, T value, T tangentIn, T tangentOut)
            {
                Time = time;
                Value = value;
                TangentIn = tangentIn;
                TangentOut = tangentOut;
            }

            /// <inheritdoc />
            public int CompareTo(object obj)
            {
                if (obj is Keyframe other)
                    return Time > other.Time ? 1 : 0;
                return 1;
            }

            /// <inheritdoc />
            public int CompareTo(Keyframe other)
            {
                return Time > other.Time ? 1 : 0;
            }
        }

        /// <summary>
        /// The keyframes collection. Can be directly modified but ensure to sort it after editing so keyframes are organized by ascending time value.
        /// </summary>
        [Serialize]
        public Keyframe[] Keyframes;

        [NoSerialize]
        private readonly IKeyframeAccess<T> _accessor = new KeyframeAccess() as IKeyframeAccess<T>;

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve{T}"/> class.
        /// </summary>
        public Curve()
        {
            Keyframes = Utils.GetEmptyArray<Keyframe>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve{T}"/> class.
        /// </summary>
        /// <param name="keyframes">The keyframes.</param>
        public Curve(params Keyframe[] keyframes)
        {
            Keyframes = keyframes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve{T}"/> class.
        /// </summary>
        /// <param name="keyframes">The keyframes.</param>
        public Curve(IEnumerable<Keyframe> keyframes)
        {
            Keyframes = keyframes.ToArray();
        }

        /// <summary>
        /// Evaluates the animation curve value at the specified time.
        /// </summary>
        /// <param name="result">The interpolated value from the curve at provided time.</param>
        /// <param name="time">The time to evaluate the curve at.</param>
        /// <param name="loop">If true the curve will loop when it goes past the end or beginning. Otherwise the curve value will be clamped.</param>
        public void Evaluate(out T result, float time, bool loop = true)
        {
            if (Keyframes.Length == 0)
            {
                result = default(T);
                return;
            }

            float start = 0;
            float end = Keyframes.Last().Time;
            WrapTime(ref time, start, end, loop);

            FindKeys(time, out var leftKeyIdx, out var rightKeyIdx);

            Keyframe leftKey = Keyframes[leftKeyIdx];
            Keyframe rightKey = Keyframes[rightKeyIdx];

            if (leftKeyIdx == rightKeyIdx)
            {
                result = leftKey.Value;
                return;
            }

            float length = rightKey.Time - leftKey.Time;

            // Scale from arbitrary range to [0, 1]
            float t = Mathf.NearEqual(length, 0.0f) ? 0.0f : (time - leftKey.Time) / length;

            // Evaluate the value at the curve
            float lengthThird = length / 3.0f;
            _accessor.GetTangent(ref leftKey.Value, ref leftKey.TangentOut, lengthThird, out var leftTangent);
            _accessor.GetTangent(ref rightKey.Value, ref rightKey.TangentIn, lengthThird, out var rightTangent);
            Bezier(ref leftKey.Value, ref leftTangent, ref rightTangent, ref rightKey.Value, t, out result);
        }

        /// <summary>
        /// Evaluates the animation curve key at the specified time.
        /// </summary>
        /// <param name="result">The interpolated key from the curve at provided time.</param>
        /// <param name="time">The time to evaluate the curve at.</param>
        /// <param name="loop">If true the curve will loop when it goes past the end or beginning. Otherwise the curve value will be clamped.</param>
        public void EvaluateKey(out Keyframe result, float time, bool loop = true)
        {
            if (Keyframes.Length == 0)
            {
                result = new Keyframe(time, default(T));
                return;
            }

            float start = 0;
            float end = Keyframes.Last().Time;
            WrapTime(ref time, start, end, loop);

            FindKeys(time, out var leftKeyIdx, out var rightKeyIdx);

            Keyframe leftKey = Keyframes[leftKeyIdx];
            Keyframe rightKey = Keyframes[rightKeyIdx];

            if (leftKeyIdx == rightKeyIdx)
            {
                result = leftKey;
                return;
            }

            float length = rightKey.Time - leftKey.Time;

            // Scale from arbitrary range to [0, 1]
            float t = Mathf.NearEqual(length, 0.0f) ? 0.0f : (time - leftKey.Time) / length;

            // Evaluate the key at the curve
            result.Time = leftKey.Time + length * t;
            float lengthThird = length / 3.0f;
            _accessor.GetTangent(ref leftKey.Value, ref leftKey.TangentOut, lengthThird, out var leftTangent);
            _accessor.GetTangent(ref rightKey.Value, ref rightKey.TangentIn, lengthThird, out var rightTangent);
            Bezier(ref leftKey.Value, ref leftTangent, ref rightTangent, ref rightKey.Value, t, out result.Value);
            result.TangentIn = leftKey.TangentOut;
            result.TangentOut = rightKey.TangentIn;
        }

        /// <summary>
        /// Trims the curve keyframes to the specified time range.
        /// </summary>
        /// <param name="start">The time start.</param>
        /// <param name="end">The time end.</param>
        public void Trim(float start, float end)
        {
            // Early out
            if (Keyframes.Length == 0 || (Keyframes.First().Time >= start && Keyframes.Last().Time <= end))
                return;
            if (end - start <= Mathf.Epsilon)
            {
                // Erase the curve
                Keyframes = Utils.GetEmptyArray<Keyframe>();
                return;
            }

            var result = new List<Keyframe>(Keyframes);

            EvaluateKey(out var startValue, start, false);
            EvaluateKey(out var endValue, end, false);

            // Begin
            for (int i = 0; i < result.Count() && result.Count > 0; i++)
            {
                if (result[i].Time < start)
                {
                    result.RemoveAt(i);
                    i--;
                }
                else
                {
                    break;
                }
            }
            if (result.Count == 0 || !Mathf.NearEqual(result.First().Time, start))
            {
                Keyframe key = startValue;
                key.Time = start;
                result.Insert(0, key);
            }

            // End
            for (int i = result.Count() - 1; i >= 0 && result.Count > 0; i--)
            {
                if (result[i].Time > end)
                {
                    result.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
            if (result.Count == 0 || !Mathf.NearEqual(result.Last().Time, end))
            {
                Keyframe key = endValue;
                key.Time = end;
                result.Add(key);
            }

            Keyframes = result.ToArray();

            // Rebase the keyframes time
            if (!Mathf.IsZero(start))
            {
                for (int i = 0; i < Keyframes.Length; i++)
                    Keyframes[i].Time -= start;
            }
        }

        /// <summary>
        /// Applies the linear transformation (scale and offset) to the keyframes time values.
        /// </summary>
        /// <param name="timeScale">The time scale.</param>
        /// <param name="timeOffset">The time offset.</param>
        public void TransformTime(float timeScale, float timeOffset)
        {
            for (int i = 0; i < Keyframes.Length; i++)
                Keyframes[i].Time = Keyframes[i].Time * timeScale + timeOffset;
        }

        /// <summary>
        /// Returns a pair of keys that can be used for interpolating to field the value at the provided time.
        /// </summary>
        /// <param name="time">The time for which to find the relevant keys from. It is expected to be clamped to a valid range within the curve.</param>
        /// <param name="leftKey">The index of the key to interpolate from.</param>
        /// <param name="rightKey">The index of the key to interpolate to.</param>
        public void FindKeys(float time, out int leftKey, out int rightKey)
        {
            int start = 0;
            int searchLength = Keyframes.Length;

            while (searchLength > 0)
            {
                int half = searchLength >> 1;
                int mid = start + half;

                if (time < Keyframes[mid].Time)
                {
                    searchLength = half;
                }
                else
                {
                    start = mid + 1;
                    searchLength -= (half + 1);
                }
            }

            leftKey = Mathf.Max(0, start - 1);
            rightKey = Mathf.Min(start, Keyframes.Length - 1);
        }

        private void Bezier(ref T p0, ref T p1, ref T p2, ref T p3, float alpha, out T result)
        {
            T p01, p12, p23, p012, p123;
            _accessor.Interpolate(ref p0, ref p1, alpha, out p01);
            _accessor.Interpolate(ref p1, ref p2, alpha, out p12);
            _accessor.Interpolate(ref p2, ref p3, alpha, out p23);
            _accessor.Interpolate(ref p01, ref p12, alpha, out p012);
            _accessor.Interpolate(ref p12, ref p23, alpha, out p123);
            _accessor.Interpolate(ref p012, ref p123, alpha, out result);
        }

        private static void WrapTime(ref float time, float start, float end, bool loop)
        {
            float length = end - start;

            if (Mathf.NearEqual(length, 0.0f))
            {
                time = 0.0f;
                return;
            }

            // Clamp to start or loop
            if (time < start)
            {
                if (loop)
                    time = time + (Mathf.Floor(end - time) / length) * length;
                else
                    time = start;
            }

            // Clamp to end or loop
            if (time > end)
            {
                if (loop)
                    time = time - Mathf.Floor((time - start) / length) * length;
                else
                    time = end;
            }
        }
    }
}
