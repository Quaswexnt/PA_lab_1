using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA_lab_1
{
    internal class FileModification
    {
        static public long segmentSize = 10 * 1024 * 1024;
        static public int totalSegments = 10; 
        static public long totalSize = segmentSize * totalSegments; 

        public long SegmentSize { get { return segmentSize; } }
        public int TotalSegments { get { return totalSegments; } }
        public long TotalSize { get { return totalSize; } }

         public void FileGeneration()
        {
            string filePath = @"C:\Users\nnhf2\Documents\Folder\example.txt";

            Random random = new Random();

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                for (int i = 0; i < totalSegments; i++)
                {
                    Console.WriteLine($"Creating part {i + 1} out of {totalSegments}...");

                    List<int> numbers = GenerateSegment(segmentSize, random);

                    numbers.Sort();

                    WriteSegmentToFile(writer, numbers);
                }
            }

            Console.WriteLine($"File {filePath} has been generated and consists {totalSegments} ordered parts.");
        }

        public List<int> GenerateSegment(long segmentSize, Random random)
        {
            List<int> numbers = new List<int>();
            long currentSize = 0;

            while (currentSize < segmentSize)
            {
                int number = random.Next(1, 100000);

                numbers.Add(number);

                currentSize += Encoding.UTF8.GetByteCount(number.ToString() + Environment.NewLine);
            }

            return numbers;
        }

        public void WriteSegmentToFile(StreamWriter writer, List<int> numbers)
        {
            foreach (int number in numbers)
            {
                writer.WriteLine(number);
            }
        }
    }
}
