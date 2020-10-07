/*
 * The MIT License (MIT)
 *
 * Modified: Copyright(c) 2020 - 2020 Kapdap
 * Original: Copyright(c) 2013 - 2020 Christopher Serr and Sergey Papushin
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * https://github.com/LiveSplit/LiveSplit/blob/master/LiveSplit/LiveSplit.Core/ComponentUtil/ProcessExtensions.cs
 */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SRTPluginProviderRECVX.Utilities
{
    public enum StringEnumType
    {
        AutoDetect,
        ASCII,
        UTF8,
        UTF16
    }

    public static class ExtensionMethods
    {
        public static bool Is64Bit(this Process process)
        {
            bool procWow64;
            NativeWrappers.IsWow64Process(process.Handle, out procWow64);
            if (Environment.Is64BitOperatingSystem && !procWow64)
                return true;
            return false;
        }

        public static bool IsRunning(this Process process)
        {
            int exitCode = 0;
            return NativeWrappers.GetExitCodeProcess(process.Handle, ref exitCode) && exitCode == 259;
        }

        public static int ExitCode(this Process process)
        {
            int exitCode = 0;
            NativeWrappers.GetExitCodeProcess(process.Handle, ref exitCode);
            return exitCode;
        }

        public static T ReadValue<T>(this Process process, IntPtr addr, bool swap = false, T default_ = default)
            where T : struct
        {
            T value;

            if (!process.ReadValue(addr, out value, swap))
                value = default_;

            return value;
        }

        public static bool ReadValue<T>(this Process process, IntPtr addr, out T value, bool swap = false)
            where T : struct
        {
            byte[] bytes;

            object val;
            value = default;

            Type type = typeof(T);
            type = type.IsEnum ? Enum.GetUnderlyingType(type) : type;

            int size = type == typeof(bool) ? 1 : Marshal.SizeOf(type);

            if (!ReadBytes(process, addr, size, out bytes, swap))
                return false;

            val = ResolveToType(bytes, type);
            value = (T)val;

            return true;
        }

        public static byte[] ReadBytes(this Process process, IntPtr addr, int size, bool swap = false)
        {
            byte[] bytes;

            if (!process.ReadBytes(addr, size, out bytes, swap))
                return new byte[size];

            return bytes;
        }

        public static bool ReadBytes(this Process process, IntPtr addr, int size, out byte[] value, bool swap = false)
        {
            var bytes = new byte[size];
            IntPtr read = IntPtr.Zero;

            value = null;

            if (!NativeWrappers.ReadProcessMemory(process.Handle, addr, bytes, size, out read))
                return false;

            if (swap)
                Array.Reverse(bytes);

            value = bytes;

            return true;
        }

        public static string ReadString(this Process process, IntPtr addr, int size, bool swap = false, string default_ = null)
        {
            string str;

            if (!process.ReadString(addr, size, out str, swap))
                return default_;

            return str;
        }

        public static string ReadString(this Process process, IntPtr addr, StringEnumType type, int size, bool swap = false, string default_ = null)
        {
            string str;

            if (!process.ReadString(addr, type, size, out str, swap))
                return default_;

            return str;
        }

        public static bool ReadString(this Process process, IntPtr addr, int size, out string value, bool swap = false) =>
            ReadString(process, addr, StringEnumType.AutoDetect, size, out value, swap);

        public static bool ReadString(this Process process, IntPtr addr, StringEnumType type, int size, out string value, bool swap = false)
        {
            var bytes = new byte[size];
            IntPtr read = IntPtr.Zero;

            value = null;

            if (!NativeWrappers.ReadProcessMemory(process.Handle, addr, bytes, size, out read))
                return false;

            if (swap)
                Array.Reverse(bytes);

            if (type == StringEnumType.AutoDetect)
                if (read.ToInt64() >= 2 && bytes[1] == '\x0')
                    value = Encoding.Unicode.GetString(bytes);
                else
                    value = Encoding.UTF8.GetString(bytes);
            else if (type == StringEnumType.UTF8)
                value = Encoding.UTF8.GetString(bytes);
            else if (type == StringEnumType.UTF16)
                value = Encoding.Unicode.GetString(bytes);
            else
                value = Encoding.ASCII.GetString(bytes);

            return true;
        }

        private static object ResolveToType(byte[] bytes, Type type)
        {
            object val;

            if (type == typeof(int))
                val = BitConverter.ToInt32(bytes, 0);
            else if (type == typeof(uint))
                val = BitConverter.ToUInt32(bytes, 0);
            else if (type == typeof(float))
                val = BitConverter.ToSingle(bytes, 0);
            else if (type == typeof(double))
                val = BitConverter.ToDouble(bytes, 0);
            else if (type == typeof(byte))
                val = bytes[0];
            else if (type == typeof(bool))
                if (bytes == null)
                    val = false;
                else
                    val = (bytes[0] != 0);
            else if (type == typeof(short))
                val = BitConverter.ToInt16(bytes, 0);
            else // probably a struct
            {
                var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

                try { val = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type); }
                finally { handle.Free(); }
            }

            return val;
        }
    }
}