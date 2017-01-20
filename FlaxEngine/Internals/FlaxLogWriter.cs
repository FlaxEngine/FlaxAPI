// Flax Engine scripting API

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlaxEngine
{
    internal sealed class FlaxLogWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        public static void Init()
        {
            //Console.SetOut(new FlaxLogWriter());
        }

        public override void Write(char value)
        {
            //Internal_WriteStringToLog(value.ToString());
        }

        public override void Write(string s)
        {
            //Internal_WriteStringToLog(s);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void Internal_WriteStringToLog(string s);
    }
}
