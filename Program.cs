using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using PA_lab_1;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
          OrderedGenerationSorting();
        //RandomGenerationSorting();
    }

    static public void RandomGenerationSorting()
    {


        FileGenereation filemod = new FileGenereation();
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        filemod.Generation();

        PolyPhase poly = new PolyPhase();
        poly.CleanFolder();
        string[] pathHelp;
        pathHelp = poly.CreateHelpFile(@"C:\Users\nnhf2\Documents\Folder\temporary");
        poly.MergeSeriesUntilOneRemains(@"C:\Users\nnhf2\Documents\Folder\example.txt", pathHelp, true);

        stopwatch.Stop();

        Console.WriteLine($"Time to sort: {stopwatch.Elapsed.TotalSeconds} sec. ");
    }

    static public void OrderedGenerationSorting()
    {
        Random random = new Random();
        FileModification filemod = new FileModification();
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        filemod.FileGeneration();

        PolyPhase poly = new PolyPhase();
        poly.CleanFolder();
        string[] pathHelp;
        pathHelp = poly.CreateHelpFile(@"C:\Users\nnhf2\Documents\Folder\temporary");
        poly.MergeSeriesUntilOneRemains(@"C:\Users\nnhf2\Documents\Folder\example.txt", pathHelp, true);

        stopwatch.Stop();

        Console.WriteLine($"Time to sort: {stopwatch.Elapsed.TotalSeconds} sec. ");
    }

}
