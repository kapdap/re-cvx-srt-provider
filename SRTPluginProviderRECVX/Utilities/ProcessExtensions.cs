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
    using SizeT = UIntPtr;

    public enum ReadStringType
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
            WinAPI.IsWow64Process(process.Handle, out procWow64);
            if (Environment.Is64BitOperatingSystem && !procWow64)
                return true;
            return false;
        }

        public static bool IsRunning(this Process process)
        {
            int exitCode = 0;
            return WinAPI.GetExitCodeProcess(process.Handle, ref exitCode) && exitCode == 259;
        }

        public static int ExitCode(this Process process)
        {
            int exitCode = 0;
            WinAPI.GetExitCodeProcess(process.Handle, ref exitCode);
            return exitCode;
        }

        public static bool ReadValue<T>(this Process process, IntPtr addr, out T val, bool swapBytes = false) where T : struct
        {
            var type = typeof(T);
            type = type.IsEnum ? Enum.GetUnderlyingType(type) : type;

            val = default(T);
            object val2;
            if (!ReadValue(process, addr, type, out val2, swapBytes))
                return false;

            val = (T)val2;

            return true;
        }

        public static bool ReadValue(Process process, IntPtr addr, Type type, out object val, bool swapBytes = false)
        {
            byte[] bytes;

            val = null;
            int size = type == typeof(bool) ? 1 : Marshal.SizeOf(type);
            if (!ReadBytes(process, addr, size, out bytes, swapBytes))
                return false;

            val = ResolveToType(bytes, type);

            return true;
        }

        public static bool ReadBytes(this Process process, IntPtr addr, int count, out byte[] val, bool swapBytes = false)
        {
            var bytes = new byte[count];

            SizeT read;
            val = null;
            if (!WinAPI.ReadProcessMemory(process.Handle, addr, bytes, (SizeT)bytes.Length, out read)
                || read != (SizeT)bytes.Length)
                return false;

            if (swapBytes)
                Array.Reverse(bytes);

            val = bytes;

            return true;
        }

        public static bool ReadPointer(this Process process, IntPtr addr, out IntPtr val, bool swapBytes = false)
        {
            return ReadPointer(process, addr, process.Is64Bit(), out val, swapBytes);
        }

        public static bool ReadPointer(this Process process, IntPtr addr, bool is64Bit, out IntPtr val, bool swapBytes = false)
        {
            var bytes = new byte[is64Bit ? 8 : 4];

            SizeT read;
            val = IntPtr.Zero;
            if (!WinAPI.ReadProcessMemory(process.Handle, addr, bytes, (SizeT)bytes.Length, out read)
                || read != (SizeT)bytes.Length)
                return false;

            if (swapBytes)
                Array.Reverse(bytes);

            val = is64Bit ? (IntPtr)BitConverter.ToInt64(bytes, 0) : (IntPtr)BitConverter.ToUInt32(bytes, 0);

            return true;
        }

        public static bool ReadString(this Process process, IntPtr addr, int numBytes, out string str, bool swapBytes = false)
        {
            return ReadString(process, addr, ReadStringType.AutoDetect, numBytes, out str, swapBytes);
        }

        public static bool ReadString(this Process process, IntPtr addr, ReadStringType type, int numBytes, out string str, bool swapBytes = false)
        {
            var sb = new StringBuilder(numBytes);
            if (!ReadString(process, addr, type, sb, swapBytes))
            {
                str = string.Empty;
                return false;
            }

            str = sb.ToString();

            return true;
        }

        public static bool ReadString(this Process process, IntPtr addr, StringBuilder sb, bool swapBytes = false)
        {
            return ReadString(process, addr, ReadStringType.AutoDetect, sb, swapBytes);
        }

        public static bool ReadString(this Process process, IntPtr addr, ReadStringType type, StringBuilder sb, bool swapBytes = false)
        {
            var bytes = new byte[sb.Capacity];
            SizeT read;
            if (!WinAPI.ReadProcessMemory(process.Handle, addr, bytes, (SizeT)bytes.Length, out read)
                || read != (SizeT)bytes.Length)
                return false;

            if (swapBytes)
                Array.Reverse(bytes);

            if (type == ReadStringType.AutoDetect)
            {
                if (read.ToUInt64() >= 2 && bytes[1] == '\x0')
                    sb.Append(Encoding.Unicode.GetString(bytes));
                else
                    sb.Append(Encoding.UTF8.GetString(bytes));
            }
            else if (type == ReadStringType.UTF8)
                sb.Append(Encoding.UTF8.GetString(bytes));
            else if (type == ReadStringType.UTF16)
                sb.Append(Encoding.Unicode.GetString(bytes));
            else
                sb.Append(Encoding.ASCII.GetString(bytes));

            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '\0')
                {
                    sb.Remove(i, sb.Length - i);
                    break;
                }
            }

            return true;
        }

        public static T ReadValue<T>(this Process process, IntPtr addr, bool swapBytes = false, T default_ = default(T)) where T : struct
        {
            T val;
            if (!process.ReadValue(addr, out val, swapBytes))
                val = default_;
            return val;
        }

        public static byte[] ReadBytes(this Process process, IntPtr addr, int count, bool swapBytes = false)
        {
            byte[] bytes;
            if (!process.ReadBytes(addr, count, out bytes, swapBytes))
                return new byte[count];
            return bytes;
        }

        public static IntPtr ReadPointer(this Process process, IntPtr addr, bool swapBytes = false, IntPtr default_ = default(IntPtr))
        {
            IntPtr ptr;
            if (!process.ReadPointer(addr, out ptr, swapBytes))
                return default_;
            return ptr;
        }

        public static string ReadString(this Process process, IntPtr addr, int numBytes, bool swapBytes = false, string default_ = null)
        {
            string str;
            if (!process.ReadString(addr, numBytes, out str, swapBytes))
                return default_;
            return str;
        }

        public static string ReadString(this Process process, IntPtr addr, ReadStringType type, int numBytes, bool swapBytes = false, string default_ = null)
        {
            string str;
            if (!process.ReadString(addr, type, numBytes, out str, swapBytes))
                return default_;
            return str;
        }

        private static object ResolveToType(byte[] bytes, Type type)
        {
            object val;

            if (type == typeof(int))
            {
                val = BitConverter.ToInt32(bytes, 0);
            }
            else if (type == typeof(uint))
            {
                val = BitConverter.ToUInt32(bytes, 0);
            }
            else if (type == typeof(float))
            {
                val = BitConverter.ToSingle(bytes, 0);
            }
            else if (type == typeof(double))
            {
                val = BitConverter.ToDouble(bytes, 0);
            }
            else if (type == typeof(byte))
            {
                val = bytes[0];
            }
            else if (type == typeof(bool))
            {
                if (bytes == null)
                    val = false;
                else
                    val = (bytes[0] != 0);
            }
            else if (type == typeof(short))
            {
                val = BitConverter.ToInt16(bytes, 0);
            }
            else // probably a struct
            {
                var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                try
                {
                    val = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
                }
                finally
                {
                    handle.Free();
                }
            }

            return val;
        }
    }
}