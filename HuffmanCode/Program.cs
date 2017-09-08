using HuffmanCode.Decoders;
using HuffmanCode.Decoders.Base;
using System;
using System.Configuration;
using System.Text;

namespace HuffmanCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес файла для кодировки");
            string fileName = Console.ReadLine();

            DateTime startEncode = DateTime.Now;

            Encoder encoder = new Encoder(fileName);
            encoder.Encode();

            TimeSpan tsEncode = DateTime.Now - startEncode;

            DisplayCodeInformation(encoder);
            
            encoder.WriteToFile("myFile.bin");

            DateTime startDecode = DateTime.Now;

            string decoderType = ConfigurationManager.AppSettings.Get("DecoderType");
            IDecoder decoder = DecoderCreator.Create(encoder.CodeToChar, "myFile.bin", encoder.TextLength, decoderType);

            decoder.Decode();
            TimeSpan tsDecode = DateTime.Now - startDecode;
            Console.WriteLine("Время выполнения кодировки/декодировки: " + tsEncode.ToString(@"ss\.ffff") + "/" + tsDecode.ToString(@"ss\.ffff"));

            decoder.SaveToText("text.txt", Encoding.GetEncoding(1251));

            Console.ReadLine();
        }

        static void DisplayCodeInformation(Encoder encoder)
        {
            Console.WriteLine(encoder.CharToCode.Count);
            foreach (var kvp in encoder.CharToCode)
                Console.WriteLine(kvp.Key + ": " + kvp.Value.CodeToString());
        }
    }
}
