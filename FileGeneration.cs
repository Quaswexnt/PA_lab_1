using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA_lab_1
{
    internal class FileGenereation
    {
        public void Generation()
        {
            
            string filePath = @"C:\Users\nnhf2\Documents\Folder\example.txt";
            long fileSize = 100*1024*1024; 
            
            Random random = new Random();

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                long currentSize = 0;

                while (currentSize < fileSize)
                {
                    
                    int number = random.Next(1, 100000);
   
                    string numberStr = number.ToString();
                    writer.WriteLine(numberStr);
                    currentSize += Encoding.UTF8.GetByteCount(numberStr + Environment.NewLine);
                }
            }

            Console.WriteLine($"File {filePath} has been created .");
        }
    }
}
