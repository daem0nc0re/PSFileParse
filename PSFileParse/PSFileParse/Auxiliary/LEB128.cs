using System;

namespace PSFileParse.Auxiliary
{
    internal class LEB128
    {
        // From SLEB128 to Int32
        public static Int32 ToInt32(byte[] data, ref UInt32 offset)
        {
            Int32 ret = 0;
            Int32 shift = 32;

            for (Int32 i = 0; i < 5; i++)
            {
                var b = data[offset++];
                ret |= (Int32)((b & 0x7F) << (i * 7));
                shift -= 7;

                if ((b & 0x80) == 0)
                    break;
            }

            if (shift > 0)
                ret = (ret << shift) >> shift;

            return ret;
        }


        public static Int32 ToInt32(byte[] data, ref UInt64 offset)
        {
            Int32 ret = 0;
            Int32 shift = 32;

            for (Int32 i = 0; i < 5; i++)
            {
                var b = data[offset++];
                ret |= (Int32)((b & 0x7F) << (i * 7));
                shift -= 7;

                if ((b & 0x80) == 0)
                    break;
            }

            if (shift > 0)
                ret = (ret << shift) >> shift;

            return ret;
        }


        // From ULEB128 to UInt32
        public static UInt32 ToUInt32(byte[] data, ref UInt32 offset)
        {
            UInt32 ret = 0;

            for (Int32 i = 0; i < 5; i++)
            {
                var b = data[offset++];
                ret |= (UInt32)((b & 0x7F) << (i * 7));

                if ((b & 0x80) == 0)
                    break;
            }

            return ret;
        }


        public static UInt32 ToUInt32(byte[] data, ref UInt64 offset)
        {
            UInt32 ret = 0;

            for (Int32 i = 0; i < 5; i++)
            {
                var b = data[offset++];
                ret |= (UInt32)((b & 0x7F) << (i * 7));

                if ((b & 0x80) == 0)
                    break;
            }

            return ret;
        }


        public static UInt32 ReadUleb128p1(byte[] data, ref UInt32 offset)
        {
            return (ToUInt32(data, ref offset) - 1);
        }


        // From SLEB128 to Int64
        public static Int64 ToInt64(byte[] data, ref UInt32 offset)
        {
            Int64 ret = 0;
            Int32 shift = 64;

            for (Int32 i = 0; i < 10; i++)
            {
                var b = data[offset++];
                ret |= ((Int64)(b & 0x7F) << (i * 7));
                shift -= 7;

                if ((b & 0x80) == 0)
                    break;
            }

            if (shift > 0)
                ret = (ret << shift) >> shift;

            return ret;
        }


        public static Int64 ToInt64(byte[] data, ref UInt64 offset)
        {
            Int64 ret = 0;
            Int32 shift = 64;

            for (Int32 i = 0; i < 10; i++)
            {
                var b = data[offset++];
                ret |= ((Int64)(b & 0x7F) << (i * 7));
                shift -= 7;

                if ((b & 0x80) == 0)
                    break;
            }

            if (shift > 0)
                ret = (ret << shift) >> shift;

            return ret;
        }


        // From ULEB128 to UInt64
        public static UInt64 ToUInt64(byte[] data, ref UInt32 offset)
        {
            UInt64 ret = 0;

            for (Int32 i = 0; i < 10; i++)
            {
                var b = data[offset++];
                ret |= ((UInt64)(b & 0x7F) << (i * 7));

                if ((b & 0x80) == 0)
                    break;
            }

            return ret;
        }


        public static UInt64 ToUInt64(byte[] data, ref UInt64 offset)
        {
            UInt64 ret = 0;

            for (Int32 i = 0; i < 10; i++)
            {
                var b = data[offset++];
                ret |= ((UInt64)(b & 0x7F) << (i * 7));

                if ((b & 0x80) == 0)
                    break;
            }

            return ret;
        }
    }
}