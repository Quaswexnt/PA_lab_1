using System;

internal class PolyPhase
{
    private int helpAmountPoly = 4;
    
    public int ReadHelpFiles { get { return helpAmountPoly; } }

    public void CleanFolder()
    {
        string directoryPath = @"C:\Users\nnhf2\Documents\Folder\temporary";

        try
        {
            string[] files = Directory.GetFiles(directoryPath);

            
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public string[] CreateHelpFile(string helpDirectoryPath)
    {
        string[] filePaths = new string[ReadHelpFiles];


        for (int i = 0; i < ReadHelpFiles; i++)
        {
            string fileName = $"helpFile{i + 1}.txt";
            string filePath = Path.Combine(helpDirectoryPath, fileName);
            using (FileStream fileStream = File.Create(filePath)) { }
           

            filePaths[i] = filePath;
        }


        return filePaths;
    }

    private List<int> GetPerfectSequence(List<int> a)
    {
        List<int> newA = new List<int>();
        for (int i = 0; i < a.Count - 1; i++)
        {
            newA.Add(a[i + 1] + a[0]);
        }
        newA.Add(0);

        return newA;
    }

    private List<int> SeriesCounter(List<int> a)
    {
        List<int> d = new List<int>();
        for (int k = 0; k < a.Count - 1; k++)
        {
            d.Add(a[k + 1] - a[k] + a[0]);
        }
        d.Add(0);

        return d;
    }

    public List<int> GetDistributedSeries(string mainPath, string[] helpFilePaths, bool ascending)
    {
        if (!File.Exists(mainPath) || helpFilePaths.Any(path => !File.Exists(path)))
        {
            return null;
        }

        List<int> a = new List<int>();
        List<int> d = new List<int>();

        int readHelpFiles = helpFilePaths.Length;
        for (int i = 0; i < readHelpFiles - 1; i++)
        {
            a.Add(1);
            d.Add(1);
        }
        a.Add(0);
        d.Add(0);

        List<int> fictionalSeries = new List<int>();
        int currentIndex = 0;
        List<int> input = new List<int>();

        try
        {
            var fileContent = File.ReadAllLines(mainPath);
            
            foreach (var line in fileContent)
            {
                if (int.TryParse(line, out int number))
                {
                    input.Add(number);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        while (d.Any(x => x > 0) && currentIndex < input.Count)
        {
            for (int i = 0; i < d.Count; i++)
            {
                using (StreamWriter sw = new StreamWriter(helpFilePaths[i], true))
                {
                    while (d[i] > 0 && currentIndex < input.Count)
                    {
                        int seriesStartIndex = currentIndex;

                        if (ascending)
                        {
                            while (currentIndex < input.Count - 1 && input[currentIndex] <= input[currentIndex + 1])
                            {
                                currentIndex++;
                            }
                        }
                        else
                        {
                            while (currentIndex < input.Count - 1 && input[currentIndex] >= input[currentIndex + 1])
                            {
                                currentIndex++;
                            }
                        }

                        if (ascending)
                        {
                            for (int j = seriesStartIndex; j <= currentIndex; j++)
                            {
                                sw.WriteLine(input[j]);
                                
                            }
                        }
                        else
                        {
                            for (int j = currentIndex; j >= seriesStartIndex; j--)
                            {
                                sw.WriteLine(input[j]);
                                
                            }
                        }

                        currentIndex++;
                        d[i]--;

                        for (int k = 0; k < d.Count; k++)
                        {
                            fictionalSeries.Add(d[k]);
                        }
                    }

                    if (d[i] == 0 && i == d.Count - 1)
                    {
                        d = SeriesCounter(a);
                        a = GetPerfectSequence(a);
                    }
                }
            }
        }

        int countToRemove = fictionalSeries.Count - 4;
        if (countToRemove > 0)
        {
            fictionalSeries.RemoveRange(0, countToRemove);
        }

        return fictionalSeries;
    }

    public void MergeSeriesUntilOneRemains(string mainPath, string[] helpFilePath, bool ascending)
    {
        List<int> fictionalSeries = GetDistributedSeries(mainPath, helpFilePath, ascending);

        int minFictionalSeries = fictionalSeries[0];
        for (int i = 0; i < fictionalSeries.Count - 1; i++)
        {
            if (minFictionalSeries > fictionalSeries[i])
            {
                minFictionalSeries = fictionalSeries[i];
            }
        }
        for (int i = 0; i < fictionalSeries.Count - 1; i++)
        {
            fictionalSeries[i] -= minFictionalSeries;
        }


        while (helpFilePath.Count(f => new FileInfo(f).Length > 0) > 1)
        {
            for (int i = 0; i < helpFilePath.Length; i++)
            {
                if (new FileInfo(helpFilePath[i]).Length == 0)
                {
                    
                    var filesToMerge = new List<string>();
                    foreach (var file in helpFilePath.Where(f => new FileInfo(f).Length > 0))
                    {
                        int index = Array.IndexOf(helpFilePath, file);
                        if (fictionalSeries[index] > 0)
                        {
                            fictionalSeries[index]--;
                        }
                        else
                        {
                            filesToMerge.Add(file);
                            if (filesToMerge.Count == 2) break;
                        }
                    }

                    if (filesToMerge.Count < 2)
                        continue;

                    MergeSeriesOnce(filesToMerge.ToArray(), helpFilePath[i], ascending);
                    foreach (string file in filesToMerge)
                    {
                        File.WriteAllText(file, "");
                        
                    }
                }
            }
        }

        string finalFile = helpFilePath.FirstOrDefault(f => new FileInfo(f).Length > 0);
        foreach (string file in helpFilePath)
        {
            if (file != finalFile && new FileInfo(file).Length == 0)
            {
                
                
            }
        }
    }
    public void MergeSeriesOnce(string[] inputFiles, string outputFile, bool ascending)
    {
        List<int>[] files = inputFiles.Select(file => File.ReadAllLines(file).Select(int.Parse).ToList()).ToArray();
        
        List<int> outputList = new List<int>();

        foreach (var file in files)
        {
            outputList = Merge(outputList, ExtractFullSeries(file, ascending), ascending);
        }

        File.WriteAllLines(outputFile, outputList.Select(x => x.ToString()));
        
    }

    private List<int> ExtractFullSeries(List<int> file, bool ascending)
    {
        if (ascending)
        {
            return file.OrderBy(x => x).ToList();
        }
        else
        {
            return file.OrderByDescending(x => x).ToList();
        }
    }

    private List<int> Merge(List<int> list1, List<int> list2, bool ascending)
    {
        List<int> merged = new List<int>();
        int i = 0, j = 0;

        while (i < list1.Count && j < list2.Count)
        {
            if ((ascending && list1[i] <= list2[j]) || (!ascending && list1[i] >= list2[j]))
            {
                merged.Add(list1[i++]);
            }
            else
            {
                merged.Add(list2[j++]);
            }
        }

        merged.AddRange(list1.Skip(i));
        merged.AddRange(list2.Skip(j));

        return merged;
    }
}