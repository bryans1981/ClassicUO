using System.Runtime.InteropServices;
using System.Text;

namespace BrowserHost.Services;

internal static class BrowserClilocParser
{
    public static BrowserClilocReadResult ParseFirstEntry(string path, byte[] bytes)
    {
        byte[] decoded = bytes.Length >= 4 && bytes[3] == 0x8E ? BwtDecompressShim.Decompress(bytes) : bytes;

        using BinaryReader reader = new BinaryReader(new MemoryStream(decoded, writable: false), Encoding.UTF8, leaveOpen: false);
        int header1 = reader.ReadInt32();
        short header2 = reader.ReadInt16();
        int firstEntryNumber = 0;
        byte firstEntryFlag = 0;
        short firstEntryTextLength = 0;
        string firstEntryText = string.Empty;

        if (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            firstEntryNumber = reader.ReadInt32();
            firstEntryFlag = reader.ReadByte();
            firstEntryTextLength = reader.ReadInt16();

            if (firstEntryTextLength < 0)
            {
                throw new InvalidDataException("cliloc entry length was negative.");
            }

            byte[] textBytes = reader.ReadBytes(firstEntryTextLength);

            if (textBytes.Length != firstEntryTextLength)
            {
                throw new EndOfStreamException("cliloc file ended before the first entry text was fully read.");
            }

            firstEntryText = Encoding.UTF8.GetString(textBytes);
        }

        return new BrowserClilocReadResult
        {
            Path = path,
            Exists = true,
            Length = bytes.Length,
            Header1 = header1,
            Header2 = header2,
            FirstEntryNumber = firstEntryNumber,
            FirstEntryFlag = firstEntryFlag,
            FirstEntryTextLength = firstEntryTextLength,
            FirstEntryText = firstEntryText
        };
    }
}

internal static class BwtDecompressShim
{
    public static byte[] Decompress(byte[] buffer)
    {
        using BinaryReader reader = new BinaryReader(new MemoryStream(buffer, writable: false));
        _ = reader.ReadUInt32();
        uint len = 0;
        byte firstChar = reader.ReadByte();

        Span<ushort> table = stackalloc ushort[256 * 256];
        BuildTable(table, firstChar);

        byte[] list = new byte[reader.BaseStream.Length - 4];
        int i = 0;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            int currentValue = firstChar;
            ushort value = table[currentValue];

            if (currentValue > 0)
            {
                do
                {
                    table[currentValue] = table[currentValue - 1];
                } while (--currentValue > 0);
            }

            table[0] = value;
            list[i++] = (byte) value;
            firstChar = reader.ReadByte();
        }

        return InternalDecompress(list, len);
    }

    private static void BuildTable(Span<ushort> table, byte startValue)
    {
        int index = 0;
        byte firstByte = startValue;
        byte secondByte = 0;

        for (int i = 0; i < 256 * 256; i++)
        {
            ushort value = (ushort)(firstByte + (secondByte << 8));
            table[index++] = value;
            firstByte++;

            if (firstByte == 0)
            {
                secondByte++;
            }
        }

        table.Sort();
    }

    private static byte[] InternalDecompress(Span<byte> input, uint len)
    {
        Span<char> symbolTable = stackalloc char[256];
        Span<char> frequency = stackalloc char[256];
        Span<int> partialInput = stackalloc int[256 * 3];
        partialInput.Clear();

        for (int i = 0; i < 256; i++)
        {
            symbolTable[i] = (char)i;
        }

        input.Slice(0, 1024).CopyTo(MemoryMarshal.AsBytes(partialInput));

        int sum = 0;

        for (int i = 0; i < 256; i++)
        {
            sum += partialInput[i];
        }

        if (len == 0)
        {
            len = (uint)sum;
        }

        if (sum != len)
        {
            return Array.Empty<byte>();
        }

        byte[] output = new byte[len];
        int count = 0;
        int nonZeroCount = 0;

        for (int i = 0; i < 256; i++)
        {
            if (partialInput[i] != 0)
            {
                nonZeroCount++;
            }
        }

        Frequency(partialInput, frequency);

        for (int i = 0, m = 0; i < nonZeroCount; ++i)
        {
            byte freq = (byte)frequency[i];
            symbolTable[input[m + 1024]] = (char)freq;
            partialInput[freq + 256] = m + 1;
            m += partialInput[freq];
            partialInput[freq + 512] = m;
        }

        byte value = (byte)symbolTable[0];

        if (len != 0)
        {
            do
            {
                ref int firstValRef = ref partialInput[value + 256];
                output[count] = value;

                if (firstValRef >= partialInput[value + 512])
                {
                    if (nonZeroCount-- > 0)
                    {
                        ShiftLeft(symbolTable, nonZeroCount);
                        value = (byte)symbolTable[0];
                    }
                }
                else
                {
                    char index = (char)input[firstValRef + 1024];
                    firstValRef++;

                    if (index != 0)
                    {
                        ShiftLeft(symbolTable, index);
                        symbolTable[(byte)index] = (char)value;
                        value = (byte)symbolTable[0];
                    }
                }

                count++;
            } while (count < len);
        }

        return output;
    }

    private static void Frequency(Span<int> input, Span<char> output)
    {
        Span<int> temp = stackalloc int[256];
        input.Slice(0, temp.Length).CopyTo(temp);

        for (int i = 0; i < 256; i++)
        {
            uint value = 0;
            byte index = 0;

            for (int j = 0; j < 256; j++)
            {
                if (temp[j] > value)
                {
                    index = (byte)j;
                    value = (uint)temp[j];
                }
            }

            if (value == 0)
            {
                break;
            }

            output[i] = (char)index;
            temp[index] = 0;
        }
    }

    private static void ShiftLeft(Span<char> input, int max)
    {
        for (int i = 0; i < max; ++i)
        {
            input[i] = input[i + 1];
        }
    }
}
