using System;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес файла для кодировки");
            string fileName = Console.ReadLine();
            StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(1251));
            string textToEncode = sr.ReadToEnd();
            sr.Close();

            Encoder encoder = new Encoder(textToEncode);
            encoder.Encode();

            Console.WriteLine(encoder.CharToCode.Count + " " + textToEncode.Length);
            foreach (var kvp in encoder.CharToCode)
                Console.WriteLine(kvp.Key + ": " + kvp.Value.CodeToString());

            encoder.WriteToFile("myFile.bin");

            Decoder decoder = new Decoder(encoder.CodeToChar, "myFile.bin", encoder.TextLength);
            decoder.Decode();
            decoder.SaveToText("text.txt", Encoding.GetEncoding(1251));
            
            Console.ReadLine();
        }
    }
}
