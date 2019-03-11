using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MnistDatasetParser
{
    class Program
    {
        const string trainImageFilePath = @"Data/train-images.idx3-ubyte";
        const string trainLabelFilePath = @"Data/train-labels.idx1-ubyte";
        const string testImageFilePath = @"Data/t10k-images.idx3-ubyte";
        const string testLabelFilePath = @"Data/t10k-labels.idx1-ubyte";

        const string trainFileOutputPath = @"train.csv";
        const string testFileOutputPath = @"test.csv";

        static byte[] intBytes = new byte[4];

        static int ReadInt32(FileStream file)
        {
            file.Read(intBytes, 0, 4);
            Array.Reverse(intBytes);
            return BitConverter.ToInt32(intBytes, 0);
        }

        static void ParseMnistDataset(string imageFile, string labelFile, string outFile)
        {
            List<int> trainLablesIndex = new List<int>();

            using (var file = File.Open(labelFile, FileMode.Open))
            {
                int magicNumber = ReadInt32(file);

                int length = ReadInt32(file);

                int i = 0;
                while (file.CanRead && i < length)
                {
                    i++;
                    trainLablesIndex.Add(file.ReadByte());
                }
            }


            using (var file = File.Open(imageFile, FileMode.Open))
            {
                using (var reader = new StreamReader(file))
                {
                    int magicNumber = ReadInt32(file);

                    int length = ReadInt32(file);

                    int rowNumber = ReadInt32(file);
                    int columnNumber = ReadInt32(file);

                    using (var outputFile = File.OpenWrite(outFile))
                    {
                        using (var writer = new StreamWriter(outputFile))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                writer.Write(trainLablesIndex[i]);

                                for (int m = 0; m < columnNumber; m++)
                                {
                                    for (int n = 0; n < rowNumber; n++)
                                    {
                                        writer.Write(",");
                                        writer.Write(file.ReadByte());
                                    }
                                }

                                writer.WriteLine();
                                writer.Flush();
                            }
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            string projectFolder = @"..\..\";
            ParseMnistDataset(Path.Combine(projectFolder, trainImageFilePath),
                Path.Combine(projectFolder, trainLabelFilePath), 
                trainFileOutputPath);

            ParseMnistDataset(Path.Combine(projectFolder, testImageFilePath),
                Path.Combine(projectFolder, testLabelFilePath), 
               testFileOutputPath);
            
        }
    }
}
