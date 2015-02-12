using System;
using System.Diagnostics;
using System.Text;

namespace FileSystemAssignment
{
    class Program
    {
        private static readonly int Blocksize = 8192;
        private static readonly int NBlocks = 32 * 131072;
        private static readonly string chars = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~'";

        static void Main(string[] args)
        {
            //Set and start timer
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            //Stream writer
            System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\vegaholt\\Desktop\\32GB_File.txt");

            //Generate random block
            var rnd = new Random();
            var charString = GetRandomCharacters(rnd);

            //Write block to file
            for (var i = 0; i < NBlocks; i++)
            {
                file.WriteLine(charString);
            }

            //Close file and stop timer
            file.Close();
            stopWatch.Stop();
           
            Console.WriteLine("Timer in ms - " + stopWatch.ElapsedMilliseconds);
            Console.Read();
        }

        
        public static string GetRandomCharacters(Random rng)
        {
            StringBuilder builder = new StringBuilder();
            for (var i = 0; i < Blocksize; i++)
            {
                int index = rng.Next(chars.Length);
                builder.Append(chars[index]);            
            }
            return builder.ToString();
        }
    }
}
