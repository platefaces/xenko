// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System.Globalization;
using System.Runtime.InteropServices;
using SiliconStudio.Core;

namespace SiliconStudio.Xenko.Animations
{
    /// <summary>
    /// A single key frame value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyFrameData<T>
    {
        public KeyFrameData(CompressedTimeSpan time, T value)
        {
            Time = time;
            Value = value;
        }

        public CompressedTimeSpan Time;
        public T Value;

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Time: {0} Value:{1}", Time.Ticks, Value);
        }
    }
}