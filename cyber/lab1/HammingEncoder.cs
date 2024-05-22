
using System.Text;

static class HammingEncoder
{
    public static string EncodeToHamming(string input)
    {
        string binaryData = ConvertToBinary(input);
        string hammingEncoded = ApplyHammingEncoding(binaryData);
        return hammingEncoded;
    }

    public static string DecodeFromHamming(string input)
    {
        string correctedData = CorrectHammingErrors(input);
        string originalText = ConvertToString(correctedData);
        return originalText;
    }

    private static string ConvertToBinary(string text)
    {
        return string.Concat(text.Select(c => Convert.ToString(c, 2).PadLeft(8, '0')));
    }

    private static string ConvertToString(string binaryData)
    {
        var byteList = new List<byte>();
        for (int i = 0; i < binaryData.Length; i += 8)
        {
            byteList.Add(Convert.ToByte(binaryData.Substring(i, 8), 2));
        }
        return Encoding.ASCII.GetString(byteList.ToArray());
    }

    private static int CalculateControlBits(int length)
    {
        for (int p = 0; ; p++)
        {
            if (Math.Pow(2, p) >= length + p + 1)
            {
                return p;
            }
        }
    }

    private static List<int> GetControlBitPositions(int bit, int length)
    {
        var positions = new List<int>();
        int step = bit + 1;
        for (int i = bit; i < length; i += 2 * step)
        {
            for (int j = i; j < i + step && j < length; j++)
            {
                positions.Add(j);
            }
        }
        return positions;
    }

    private static string ApplyHammingEncoding(string data)
    {
        int dataLength = data.Length;
        int controlBitsCount = CalculateControlBits(dataLength);
        int totalLength = dataLength + controlBitsCount;
        var encodedData = new char[totalLength];

        for (int i = 0, j = 0; i < totalLength; i++)
        {
            if (IsPowerOfTwo(i + 1))
            {
                encodedData[i] = '0';
            }
            else
            {
                encodedData[i] = data[j++];
            }
        }

        for (int i = 0; i < controlBitsCount; i++)
        {
            int controlBitPosition = (int)Math.Pow(2, i) - 1;
            var positions = GetControlBitPositions(controlBitPosition, totalLength);
            int parity = positions.Select(p => encodedData[p] - '0').Sum() % 2;
            encodedData[controlBitPosition] = parity.ToString()[0];
        }

        return new string(encodedData);
    }

    private static string CorrectHammingErrors(string data)
    {
        int dataLength = data.Length;
        int controlBitsCount = CalculateControlBits(dataLength);
        var correctedData = data.ToCharArray();
        int errorPosition = 0;

        for (int i = 0; i < controlBitsCount; i++)
        {
            int controlBitPosition = (int)Math.Pow(2, i) - 1;
            var positions = GetControlBitPositions(controlBitPosition, dataLength);
            int parity = positions.Select(p => correctedData[p] - '0').Sum() % 2;

            if (parity != 0)
            {
                errorPosition += controlBitPosition + 1;
            }
        }

        if (errorPosition > 0)
        {
            correctedData[errorPosition - 1] = correctedData[errorPosition - 1] == '0' ? '1' : '0';
        }

        var decodedData = new StringBuilder();
        for (int i = 0; i < dataLength; i++)
        {
            if (!IsPowerOfTwo(i + 1))
            {
                decodedData.Append(correctedData[i]);
            }
        }

        return decodedData.ToString();
    }

    private static bool IsPowerOfTwo(int x)
    {
        return (x & (x - 1)) == 0;
    }
}
