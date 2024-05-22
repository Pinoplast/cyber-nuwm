string filePath = "file.txt";
string originalContent = File.ReadAllText(filePath);

string encodedData = "0001000000010111011100110010001111001001101001";
//HammingEncoder.EncodeToHamming(originalContent);
string decodedData = HammingEncoder.DecodeFromHamming(encodedData);

Console.WriteLine($"Source: {originalContent}");
Console.WriteLine($"Encoded: {encodedData}");
Console.WriteLine($"Restored: {decodedData}");
