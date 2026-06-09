using System;
using System.Collections.Generic;
using System.Text;

namespace PSFileParse.Auxiliary
{
    internal class BinaryHelper
    {
        public static String GetAnsiString(byte[] data, UInt32 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }


        public static String GetAnsiString(byte[] data, ref UInt32 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }


        public static String GetAnsiString(byte[] data, UInt64 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }


        public static String GetAnsiString(byte[] data, ref UInt64 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.ASCII.GetString(bytes.ToArray());
        }


        public static String GetAnsiString(byte[] data, UInt32 offset, UInt32 length)
        {
            var bytes = new byte[length];

            for (UInt32 i = 0u;  i < bytes.Length; i++)
                bytes[i] = data[offset++];

            return Encoding.ASCII.GetString(bytes);
        }


        public static String GetAnsiString(byte[] data, UInt64 offset, UInt32 length)
        {
            var bytes = new byte[length];

            for (UInt32 i = 0u; i < bytes.Length; i++)
                bytes[i] = data[offset++];

            return Encoding.ASCII.GetString(bytes);
        }


        public static String GetUnicodeString(byte[] data, UInt32 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b0 = data[offset++];
                var b1 = data[offset++];

                if ((b0 == 0) && (b1 == 0))
                    break;

                bytes.Add(b0);
                bytes.Add(b1);
            }

            return Encoding.Unicode.GetString(bytes.ToArray());
        }


        public static String GetUnicodeString(byte[] data, ref UInt32 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b0 = data[offset++];
                var b1 = data[offset++];

                if ((b0 == 0) && (b1 == 0))
                    break;

                bytes.Add(b0);
                bytes.Add(b1);
            }

            return Encoding.Unicode.GetString(bytes.ToArray());
        }


        public static String GetUnicodeString(byte[] data, UInt64 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b0 = data[offset++];
                var b1 = data[offset++];

                if ((b0 == 0) && (b1 == 0))
                    break;

                bytes.Add(b0);
                bytes.Add(b1);
            }

            return Encoding.Unicode.GetString(bytes.ToArray());
        }


        public static String GetUnicodeString(byte[] data, ref UInt64 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b0 = data[offset++];
                var b1 = data[offset++];

                if ((b0 == 0) && (b1 == 0))
                    break;

                bytes.Add(b0);
                bytes.Add(b1);
            }

            return Encoding.Unicode.GetString(bytes.ToArray());
        }


        public static String GetUnicodeString(byte[] data, UInt32 offset, UInt32 length)
        {
            var bytes = new byte[length  * 2];

            for (UInt32 i = 0u; i < bytes.Length; i++)
                bytes[i] = data[offset++];

            return Encoding.Unicode.GetString(bytes);
        }


        public static String GetUnicodeString(byte[] data, UInt64 offset, UInt32 length)
        {
            var bytes = new byte[length * 2];

            for (UInt32 i = 0u; i < bytes.Length; i++)
                bytes[i] = data[offset++];

            return Encoding.Unicode.GetString(bytes);
        }


        public static String GetUTF8String(byte[] data, UInt32 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }


        public static String GetUTF8String(byte[] data, UInt64 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }


        public static String GetUTF8String(byte[] data, ref UInt32 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }



        public static String GetUTF8String(byte[] data, ref UInt64 offset)
        {
            var bytes = new List<byte>();

            while (offset < (UInt32)data.Length)
            {
                var b = data[offset++];

                if (b == 0)
                    break;

                bytes.Add(b);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }


        public static String GetUTF8String(byte[] data, UInt32 offset, UInt32 length)
        {
            var bytes = new byte[length];

            for (UInt32 i = 0u; i < bytes.Length; i++)
                bytes[i] = data[offset++];

            return Encoding.UTF8.GetString(bytes);
        }


        public static String GetUTF8String(byte[] data, UInt64 offset, UInt32 length)
        {
            var bytes = new byte[length];

            for (UInt32 i = 0u; i < bytes.Length; i++)
                bytes[i] = data[offset++];

            return Encoding.UTF8.GetString(bytes);
        }


        public static Int16 ToInt16(byte[] data, UInt32 offset)
        {
            return (Int16)((data[offset + 1] << 8) | data[offset]);
        }


        public static Int16 ToInt16(byte[] data, UInt64 offset)
        {
            return (Int16)((data[offset + 1] << 8) | data[offset]);
        }


        public static Int16 ToInt16Big(byte[] data, UInt32 offset)
        {
            return (Int16)((data[offset] << 8) | data[offset + 1]);
        }


        public static Int16 ToInt16Big(byte[] data, UInt64 offset)
        {
            return (Int16)((data[offset] << 8) | data[offset + 1]);
        }


        public static Int32 ToInt32(byte[] data, UInt32 offset)
        {
            return ((data[offset+3] << 24) |
                (data[offset + 2] << 16) |
                (data[offset + 1] << 8) |
                data[offset]);
        }


        public static Int32 ToInt32(byte[] data, UInt64 offset)
        {
            return ((data[offset + 3] << 24) |
                (data[offset + 2] << 16) |
                (data[offset + 1] << 8) |
                data[offset]);
        }


        public static Int32 ToInt32Big(byte[] data, UInt32 offset)
        {
            return ((data[offset] << 24) |
                (data[offset + 1] << 16) |
                (data[offset + 2] << 8) |
                data[offset + 3]);
        }


        public static Int32 ToInt32Big(byte[] data, UInt64 offset)
        {
            return ((data[offset] << 24) |
                (data[offset + 1] << 16) |
                (data[offset + 2] << 8) |
                data[offset + 3]);
        }


        public static Int64 ToInt64(byte[] data, UInt32 offset)
        {
            var lower = (UInt32)((data[offset + 3] << 24) |
                (data[offset + 2] << 16) |
                (data[offset + 1] << 8) |
                data[offset]);
            var higher = (Int64)((data[offset + 7] << 24) |
                (data[offset + 6] << 16) |
                (data[offset + 5] << 8) |
                data[offset + 4]);
            return ((higher << 32) | lower);
        }


        public static Int64 ToInt64(byte[] data, UInt64 offset)
        {
            var lower = (UInt32)((data[offset + 3] << 24) |
                (data[offset + 2] << 16) |
                (data[offset + 1] << 8) |
                data[offset]);
            var higher = (Int64)((data[offset + 7] << 24) |
                (data[offset + 6] << 16) |
                (data[offset + 5] << 8) |
                data[offset + 4]);
            return ((higher << 32) | lower);
        }


        public static Int64 ToInt64Big(byte[] data, UInt32 offset)
        {
            var lower = (Int64)((data[offset + 4] << 24) |
                (data[offset + 5] << 16) |
                (data[offset + 6] << 8) |
                data[offset + 7]);
            var higher = (UInt32)((data[offset] << 24) |
                (data[offset + 1] << 16) |
                (data[offset + 2] << 8) |
                data[offset + 3]);
            return ((higher << 32) | lower);
        }


        public static Int64 ToInt64Big(byte[] data, UInt64 offset)
        {
            var lower = (Int64)((data[offset + 4] << 24) |
                (data[offset + 5] << 16) |
                (data[offset + 6] << 8) |
                data[offset + 7]);
            var higher = (UInt32)((data[offset] << 24) |
                (data[offset + 1] << 16) |
                (data[offset + 2] << 8) |
                data[offset + 3]);
            return ((higher << 32) | lower);
        }


        public static UInt16 ToUInt16(byte[] data, UInt32 offset)
        {
            return (UInt16)ToInt16(data, offset);
        }


        public static UInt16 ToUInt16(byte[] data, UInt64 offset)
        {
            return (UInt16)ToInt16(data, offset);
        }


        public static UInt16 ToUInt16Big(byte[] data, UInt32 offset)
        {
            return (UInt16)ToInt16Big(data, offset);
        }


        public static UInt16 ToUInt16Big(byte[] data, UInt64 offset)
        {
            return (UInt16)ToInt16Big(data, offset);
        }


        public static UInt32 ToUInt32(byte[] data, UInt32 offset)
        {
            return (UInt32)ToInt32(data, offset);
        }


        public static UInt32 ToUInt32(byte[] data, UInt64 offset)
        {
            return (UInt32)ToInt32(data, offset);
        }


        public static UInt32 ToUInt32Big(byte[] data, UInt32 offset)
        {
            return (UInt32)ToInt32Big(data, offset);
        }


        public static UInt32 ToUInt32Big(byte[] data, UInt64 offset)
        {
            return (UInt32)ToInt32Big(data, offset);
        }


        public static UInt64 ToUInt64(byte[] data, UInt32 offset)
        {
            return (UInt64)ToInt64(data, offset);
        }


        public static UInt64 ToUInt64(byte[] data, UInt64 offset)
        {
            return (UInt64)ToInt64(data, offset);
        }


        public static UInt64 ToUInt64Big(byte[] data, UInt32 offset)
        {
            return (UInt64)ToInt64Big(data, offset);
        }


        public static UInt64 ToUInt64Big(byte[] data, UInt64 offset)
        {
            return (UInt64)ToInt64Big(data, offset);
        }
    }
}
