/////////////////////////////////////////////////////////////////////////////////////////////////
//
// IL2C - A translator for ECMA-335 CIL/MSIL to C language.
// Copyright (c) 2016-2019 Kouji Matsui (@kozy_kekyo, @kekyo2)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace IL2C.ILConverters
{
    [TestCase(true, "Boolean", 123, false)]
    [TestCase(false, "Boolean", 123, true)]
    [TestCase((byte)(byte.MaxValue - 1), "Byte", 123, byte.MaxValue)]
    [TestCase((short)(short.MaxValue - 1), "Int16", 123, short.MaxValue)]
    [TestCase(int.MaxValue - 1, "Int32", 123, int.MaxValue)]
    [TestCase(long.MaxValue - 1, "Int64", 123, long.MaxValue)]
    [TestCase((sbyte)(sbyte.MaxValue - 1), "SByte", 123, sbyte.MaxValue)]
    [TestCase((ushort)(ushort.MaxValue - 1), "UInt16", 123, ushort.MaxValue)]
    [TestCase(uint.MaxValue - 1, "UInt32", 123, uint.MaxValue)]
    [TestCase(ulong.MaxValue - 1, "UInt64", 123, ulong.MaxValue)]
    [TestCase(int.MaxValue - 1, "IntPtr", 123, int.MaxValue)]
    [TestCase(uint.MaxValue - 1, "UIntPtr", 123, uint.MaxValue)]
    [TestCase((float)((double)123.45f + (double)3.14159274f), "Single", 123, 123.45f)]
    [TestCase(123.45 + 3.1415926535897931, "Double", 123, 123.45)]
    [TestCase('B', "Char", 123, 'A')]
    [TestCase("ABCD", "String", 123, "ABC")]
    public sealed class Ldarg_1
    {
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern bool Boolean(int arg0, bool value);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern byte Byte(int arg0, byte num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern short Int16(int arg0, short num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern int Int32(int arg0, int num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern long Int64(int arg0, long num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern sbyte SByte(int arg0, sbyte num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern ushort UInt16(int arg0, ushort num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern uint UInt32(int arg0, uint num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern ulong UInt64(int arg0, ulong num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern IntPtr IntPtr(int arg0, IntPtr num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern UIntPtr UIntPtr(int arg0, UIntPtr num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern float Single(int arg0, float num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern double Double(int arg0, double num);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern char Char(int arg0, char ch);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern string String(int arg0, string v);
    }
}
