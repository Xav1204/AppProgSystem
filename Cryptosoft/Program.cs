using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Cryptosoft
{
    class Program
    {
        public static void Main(string[] args)
        {
            string cible = args[1];
            string source = args[0];
            string txt = args[2];
            args = args.Skip(2).ToArray();
            Console.WriteLine(cible);
            Console.WriteLine(source);
            encrypt(source, cible, args);

        }

        public static byte[] EncryptOrDecrypt(byte[] text, byte[] key)
        {
            byte[] xor = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                xor[i] = (byte)(text[i] ^ key[i % key.Length]);
            }
            return xor;
        }

        public static void encrypt(string source, string cible, string[] arguments)
        {

            for (var i = 0; i < arguments.Length; i++)
            {
                string path1 = source;
                string path2 = cible;
                string[] files = Directory.GetFiles(path1, arguments[i]);

                foreach (var f in files)
                {
                    string json;
                    byte[] inputBytes;

                    string inputKey;
                    byte[] key;

                    StreamReader wr = new StreamReader(f);
                    json = wr.ReadToEnd();
                    inputBytes = Encoding.Unicode.GetBytes(json);
                    inputKey = "10011001";
                    key = Encoding.Unicode.GetBytes(inputKey);
                    byte[] text = EncryptOrDecrypt(inputBytes, key);
                    string encryptedStr = Encoding.Unicode.GetString(text);

                    try
                    {
                        File.WriteAllText(System.IO.Path.Combine(path2, System.IO.Path.GetFileName(f)), encryptedStr);
                    }
                    catch
                    {
                        File.Create(System.IO.Path.Combine(path2, System.IO.Path.GetFileName(f)));
                    }
                    finally
                    {
                        File.WriteAllText(System.IO.Path.Combine(path2, System.IO.Path.GetFileName(f)), encryptedStr);
                    }
                }
            }
        }
    }
}
