using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Entities;

namespace ServerEngine.PacketEngine
{
    public class Packet
    {
        private byte[] BufferData;
        public int Index = 0;
        public int Length = 0;

        /// <summary>
        /// Creates a new buffer
        /// </summary>
        /// <param name="Size">Size of buffer</param>
        public Packet(uint Size)
        {
            try
            {
                BufferData = new byte[Size];
            }
            catch
            {
                BufferData = new byte[Size * 2]; //whatever
            }
            Length = BufferData.GetLength(0);
        }
        /// <summary>
        /// Creates a new buffer
        /// </summary>
        /// <param name="ArrayOfBytes">The byte array the buffer should use</param>
        public Packet(byte[] ArrayOfBytes)
        {
            BufferData = new byte[ArrayOfBytes.Length];
            ArrayOfBytes.CopyTo(BufferData, 0);
            Length = BufferData.Length;
        }
        /// <summary>
        /// Resizes the buffer
        /// </summary>
        /// <param name="Deltasize"></param>
        private void Resize(int Deltasize)
        {
            byte[] newbuffer = new byte[Length + Deltasize];
            BufferData.CopyTo(newbuffer, 0);
            BufferData = newbuffer;
            Length = BufferData.Length;
        }
        /// <summary>
        /// Returns the buffer data
        /// </summary>
        /// <returns>Buffer data</returns>
        public byte[] GetBuffer()
        {
            return BufferData;
        }
        /// <summary>
        /// Returns the buffer data in a byte array
        /// </summary>
        /// <returns>The buffer data</returns>
        public byte[] GetWrittenBuffer()
        {
            try
            {
                if (Index > Length)
                {
                    byte[] result = new byte[Index - 1];
                    System.Buffer.BlockCopy(BufferData, 0, result, 0, Index - 1);
                    return result;
                }
                else
                {
                    byte[] result = new byte[Index];
                    System.Buffer.BlockCopy(BufferData, 0, result, 0, Index);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new byte[1];
            }
        }

        public byte[] GetWrittenBuffer(PacketIds packetid)
        {
            Packet p = new Packet((uint)this.Index + 4);
            p.WriteHeader(packetid, this);
            return p.GetWrittenBuffer();
        }

        /// <summary>
        /// Sets the index to the given position
        /// </summary>
        /// <param name="nIndex">New index</param>
        /// <returns>True if the operation completed</returns>
        public bool setIndex(int nIndex)
        {
            if (Index > Length) return false;
            Index = nIndex;
            return true;
        }
        /// <summary>
        /// Sets the index to current pos + new index
        /// </summary>
        /// <param name="nIndex">New index</param>
        /// <returns>True if operation completed</returns>
        public bool addIndex(int nIndex)
        {
            return setIndex(Index + nIndex);
        }
        /// <summary>
        /// Sets the index to 0
        /// </summary>
        public void resetIndex()
        {
            setIndex(0);
        }

        //Write methods

        private void WriteHeader(PacketIds so, Packet p)
        {
            WriteShort((short)so);
            WriteShort(p.Index);
            WriteArray(p.GetWrittenBuffer());
        }

        /// <summary>
        /// Writes a byte to the buffer
        /// </summary>
        /// <param name="Byte">The byte to write</param>
        public void WriteByte(byte Byte)
        {
            if (Index >= Length) Resize((Index - Length) + 1);
            BufferData[Index] = Byte;
            ++Index;
        }

        /// <summary>
        /// Writes a byte to the buffer
        /// </summary>
        /// <param name="Byte">The byte to write</param>
        public void WriteByte(int Int)
        {
            byte Byte = (byte)Int;
            if (Index >= Length) Resize((Index - Length) + 1);
            BufferData[Index] = Byte;
            ++Index;
        }

        /// <summary>
        /// Writes a short (Int16) to the buffer
        /// </summary>
        /// <param name="ShortInteger">The short to write</param>
        public void WriteShort(short ShortInteger)
        {
            if (Index + 2 >= Length) Resize((Index - Length) + 2);
            WriteByte((byte)(ShortInteger & 0xFF));
            WriteByte((byte)((((ushort)ShortInteger) >> 8) & 0xFF));
        }

        /// <summary>
        /// Writes a short (Int16) to the buffer
        /// </summary>
        /// <param name="ShortInteger">The short to write</param>
        public void WriteShort(int Int)
        {
            short ShortInteger = (short)Int;
            if (Index + 2 >= Length) Resize((Index - Length) + 2);
            WriteByte((byte)(ShortInteger & 0xFF));
            WriteByte((byte)((((ushort)ShortInteger) >> 8) & 0xFF));
        }

        public void WriteShort1(short ShortInteger)
        {
            if (Index + 2 >= Length) Resize((Index - Length) + 2);
            WriteByte((byte)((((ushort)ShortInteger) >> 8) & 0xFF));
            WriteByte((byte)(ShortInteger & 0xFF));
        }

        /// <summary>
        /// Writes an integer to the buffer
        /// </summary>
        /// <param name="Integer">The int you want to write</param>
        public void WriteInt(int Integer)
        {
            if (Index + 4 >= Length) Resize((Index - Length) + 4);
            WriteByte((byte)(Integer & 0xFF));
            WriteByte((byte)((((uint)Integer) >> 8) & 0xFF));
            WriteByte((byte)((((uint)Integer) >> 16) & 0xFF));
            WriteByte((byte)((((uint)Integer) >> 24) & 0xFF));

        }

        public void WriteLong(long Integer)
        {
            WriteByte((byte)(Integer & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 8) & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 16) & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 24) & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 32) & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 40) & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 48) & 0xFF));
            WriteByte((byte)((((ulong)Integer) >> 56) & 0xFF));
        }

        /// <summary>
        /// Writes a byte n number of times to the buffer
        /// </summary>
        /// <param name="Pad">The byte you want to pad</param>
        /// <param name="nLength">The number of bytes to write</param>
        public void BytePad(byte Pad, int nLength)
        {
            if ((Index + nLength) >= Length) Resize(nLength);
            for (int i = 0; i < nLength; i++)
            {
                BufferData[Index + i] = Pad;
            }
            Index += nLength;
        }
        /// <summary>
        /// Writes a string to the buffer
        /// </summary>
        /// <param name="String">The string you want to write</param>
        public void WriteString(string String)
        {
            byte[] test = Encoding.Unicode.GetBytes(String);
            WriteShort(test.Length + 2);
            if ((Index + test.Length) >= Length) Resize(test.Length);
            for (int i = 0; i < test.Length; i++)
            {
                BufferData[Index + i] = test[i];
            }
            Index += test.Length;
            WriteShort(0);
        }
        /// <summary>
        /// Writes a string preceeded by a short containing its length
        /// </summary>
        /// <param name="String">The string you want to write</param>
        public void WriteStringLen(string String)
        {
            if ((Index + String.Length + 2) >= Length) Resize(String.Length + 2);
            WriteShort((short)String.Length);
            WriteString(String);
        }
        /// <summary>
        /// Writes a padded left string to the buffer
        /// </summary>
        /// <param name="String">The string you want to write</param>
        /// <param name="Pad">The char you want to pad</param>
        /// <param name="PadLength">The number of chars you want to pad</param>
        public void WriteStringPaddedLeft(string String, char Pad, int PadLength)
        {
            String.PadLeft(PadLength, Pad);
            if ((Index + String.Length) >= Length) Resize(String.Length);
            WriteString(String);
        }
        /// <summary>
        /// Writes a padded right string to the buffer
        /// </summary>
        /// <param name="String">The string you want to write</param>
        /// <param name="Pad">The char you want to pad</param>
        /// <param name="PadLength">The number of chars you want to pad</param>
        public void WriteStringPaddedRight(string String, char Pad, int PadLength)
        {
            String.PadRight(PadLength, Pad);
            if ((Index + String.Length) >= Length) Resize(String.Length);
            WriteString(String);
        }
        /// <summary>
        /// Writes an array of bytes to the buffer
        /// </summary>
        /// <param name="Array">The array you want to write</param>
        public void WriteArray(byte[] Array)
        {
            if ((Index + Array.Length) >= Length) Resize(Array.Length);
            for (int i = 0; i < Array.Length; i++)
            {
                BufferData[Index + i] = Array[i];
            }
            Index += Array.Length;
        }
        /// <summary>
        /// Write a hex string to the buffer
        /// </summary>
        /// <param name="HexString">The string containing the hex chain</param>
        public void WriteHexString(string HexString)
        {
            int _out;
            byte[] _byte = HexEncoding.GetBytes(HexString, out _out);
            WriteArray(_byte);
        }

        //Read methods

        /// <summary>
        /// Reads a byte from the buffer
        /// </summary>
        /// <returns>The read byte or 0 if failed</returns>
        public byte ReadByte()
        {
            try
            {
                byte result = BufferData[Index];
                ++Index;
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ++Index;
                return 0;
            }
        }

        /// <summary>
        /// Reads a byte at a certain position in the buffer
        /// </summary>
        /// <param name="pos">The position to read from</param>
        /// <returns>The read byte or 0 if failed</returns>
        public byte ReadByte(int pos)
        {
            int TempIndex = Index;
            Index = pos;
            byte result = ReadByte();
            Index = TempIndex;
            return result;
        }
        /// <summary>
        /// Reads an unsigned short from the buffer
        /// </summary>
        /// <returns>The read ushort</returns>
        public ushort ReadUShort()
        {
            try
            {
                ushort result = (ushort)((BufferData[Index]) + (BufferData[Index + 1] * 256));
                Index += 2;
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Index += 2;
                return 0;
            }
        }
        /// <summary>
        /// Reads a short from the buffer
        /// </summary>
        /// <returns>The read short</returns>
        public short ReadShort()
        {
            int byte1, byte2;
            byte1 = ReadByte();
            byte2 = ReadByte();
            return (short)((byte2 << 8) + byte1);
        }
        public short ReadShort2()
        {
            int byte1, byte2;
            byte1 = ReadByte();
            byte2 = ReadByte();
            return (short)((byte1 << 8) + byte2);
        }
        /// <summary>
        /// Reads a short from the buffer at the given position
        /// </summary>
        /// <param name="pos">Position to read at</param>
        /// <returns>The read short</returns>
        public short ReadShort(int pos)
        {
            int TempIndex = Index;
            Index = pos;
            short result = ReadShort();
            Index = TempIndex;
            return result;
        }


        /// <summary>
        /// Reads an integer from the buffer
        /// </summary>
        /// <returns>The read int</returns>
        public int ReadInt()
        {
            int byte1 = ReadByte();
            int byte2 = ReadByte();
            int byte3 = ReadByte();
            int byte4 = ReadByte();



            return (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1;
        }
        /// <summary>
        /// Reads an unsigned int from the buffer at the given position
        /// </summary>
        /// <param name="Pos">The position to read at</param>
        /// <returns>The read uint</returns>
        public int ReadInt(int Pos)
        {
            int TempIndex = Index;
            Index = Pos;
            int result = ReadInt();
            Index = TempIndex;
            return result;
        }

        /// <summary>
        /// Reads a string based on length before the string(thats how its used in xiah, aka short length, string text)
        /// </summary>
        /// <returns>The read string</returns>
        public string ReadString()
        {
            try
            {
                int lenght = ReadShort() - 2;
                byte[] newbuf = new byte[lenght];
                for (int i = 0; i < lenght; i++)
                {
                    newbuf[i] = BufferData[Index + i];
                }
                Index += lenght + 2;

                string test = Encoding.Unicode.GetString(newbuf);
                return test;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads a string that is as long as the provided length from the buffer
        /// </summary>
        /// <param name="_length">Length of the string to be read</param>
        /// <returns>The read string</returns>
        public string ReadString(int _length)
        {
            try
            {
                char[] newbuf = new char[_length];
                for (int i = 0; i < _length; i++)
                {
                    newbuf[i] = (char)BufferData[Index + i];
                }
                Index += _length;
                return new string(newbuf);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads a string that is as long as the provided length from the buffer
        /// </summary>
        /// <param name="_length">Length of the string to be read</param>
        /// <returns>The read string</returns>
        public string ReadStringFrom0(int _length)
        {
            try
            {
                ArrayList a = new ArrayList();
                for (int i = 0; i < _length; i++)
                {
                    if (BufferData[Index + i] != 0x00)
                        a.Add((char)BufferData[Index + i]);
                }
                Index += _length;
                char[] newbuf = new char[a.Count];
                for (int i = 0; i < a.Count; i++)
                    newbuf[i] = (char)a[i];
                return new string(newbuf);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return string.Empty;
            }
        }
        /// <summary>
        /// Reads a string using the short length preceding the string from the buffer
        /// </summary>
        /// <returns>The read string</returns>
        public string ReadStringFromLength()
        {
            int _length = ReadShort();
            return ReadString(_length);
        }
        /// <summary>
        /// Reads a string using the short length preceding the string from the buffer at the given position
        /// </summary>
        /// <param name="Pos">The position to read at</param>
        /// <returns>The read string</returns>
        public string ReadStringFromLength(int Pos)
        {
            int TempIndex = Index;
            Index = Pos;
            string result = ReadStringFromLength();
            Index = TempIndex;
            return result;
        }
        /// <summary>
        /// Reads a string that is as long as the provided length from the buffer at the given position
        /// </summary>
        /// <param name="Pos">The position to read at</param>
        /// <param name="nLength">The length of the string to be read</param>
        /// <returns>The read string</returns>
        public string ReadStringSpecify(int Pos, int nLength)
        {
            int TempIndex = Index;
            Index = Pos;
            string result = ReadString(nLength);
            Index = TempIndex;
            return result;
        }

        /// <summary>
        /// Pads the packet
        /// </summary>
        /// <param name="b"></param>
        /// <param name="amount"></param>
        public void Pad(byte b, int amount)
        {
            for (int i = 0; i < amount; i++)
                WriteByte(b);
        }

        /// <summary>
        /// Encrypts the given packet
        /// </summary>
        /// <param name="p">The packet to encrypt</param>
        public static Packet Encrypt(Packet p, int key)
        {
            byte[] buffer = p.GetBuffer();
            int packetLength = buffer.Length - 4;
            string value;
            int Lenght;
            byte AL, BL;
            long CX, CL, DL;

            value = buffer[3].ToString("x2");
            value += buffer[2].ToString("x2");
            Lenght = Int32.Parse(value, NumberStyles.HexNumber);

            AL = (byte)key;
            BL = buffer[0];
            CX = Lenght + 4;
            CL = CX;
            DL = CL;

            DL = (byte)(DL + AL);
            BL = (byte)(BL + DL); // First encrypted byte
            buffer[0] = BL;

            DL = (byte)(BL); // DL = Packet first byte
            DL = (byte)(DL + CL); // ADD DL, CL
            DL = (byte)(AL + DL); // ADD DL, AL (gotta parse 1)
            DL = (byte)(DL + buffer[1]);
            buffer[1] = (byte)DL;


            for (int i = 0; i < packetLength; i++)
            {
                BL = (byte)(CL);
                BL = (byte)(DL + BL);
                BL = (byte)(AL + BL);
                BL = (byte)(BL + buffer[i + 4]);

                buffer[i + 4] = BL;
                DL = BL;
            }

            return new Packet(buffer);
        }

        public static Packet Normal(Packet p)
        {
            byte[] data = p.GetBuffer();
            return new Packet(data);
        }

        /// <summary>
        /// Decrypts the given packet 
        /// </summary>
        /// <param name="p">The packet to decrypt</param>
        public static Packet Decrypt(Packet p, int key)
        {
            byte[] data = p.GetBuffer();
            int packetLength = data.Length - 4;
            string value;
            int Lenght;
            byte BL, DL, CL;
            long AL;

            value = data[3].ToString("x2");
            value += data[2].ToString("x2");
            Lenght = Int32.Parse(value, NumberStyles.HexNumber);

            BL = (byte)key;
            CL = data[0];
            AL = Lenght + 4;

            DL = CL;
            DL = (byte)(DL - AL);
            DL = (byte)(DL - BL);
            data[0] = DL;
            DL = data[1];

            BL = DL;
            BL = (byte)(BL - AL);
            BL = (byte)(BL - CL);
            BL = (byte)(BL - key);
            data[1] = BL;

            for (int a = 0; a < Lenght; a++)
            {
                BL = data[a + 4];
                BL = (byte)(BL - AL);
                BL = (byte)(BL - DL);
                BL = (byte)(BL - key);
                DL = data[a + 4];
                data[a + 4] = BL;
            }
            return new Packet(data);
        }

        public byte[] ReadBytes(int amt)
        {
            byte[] buffer = new byte[amt];
            Array.Copy(this.GetBuffer(), Index, buffer, 0, amt);
            Index += amt;
            return buffer;
        }

        public void Skip(int amt)
        {
            Index += amt;
        }

        public void Skip()
        {
            Index += 1;
        }
    }
    /// <summary>
    /// Hex encoding class
    /// </summary>
    class HexEncoding
    {
        /* External code from http://www.codeproject.com/KB/recipes/hexencoding.aspx */
        /* Author = neilck http://www.codeproject.com/script/Membership/Profiles.aspx?mid=375133 */
        public static bool IsHexDigit(Char c)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }

        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }

        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = string.Empty;
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { newString[j], newString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }
        /* End external code */
    }
}
