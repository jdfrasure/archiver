using System;
using System.IO;
using System.Linq;


using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

// Directory Structure is E:\csvdatay\MACADDR\YYYY-MM-DD
// Date currently matches the date on the folder.

class MainClass
{
    public static void Main(string[] args)
    {
        if (!args.Any())
        {
            Console.WriteLine("Example: program Directory");
        }
        else
        {
            var archivedir = args[0];
            // var daysold = "60";

            string[] dirs = Directory.GetDirectories(archivedir); // something like e:\csvdatay\ 
            foreach (string directory1 in dirs)
            {
                string[] dirname = directory1.Split(Path.DirectorySeparatorChar);

                //This is for E:\csvDatay
                string filename = (dirname[2] + ".zip");
                //string filename = (dirname[5] + ".zip");
                Console.WriteLine();
                Console.WriteLine("ZIP Filename: " + filename);

                // Find the files that are n days old

                string[] files = Directory.GetFiles(directory1, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastWriteTime < DateTime.Now.AddDays(-60))
                        Console.WriteLine("test: " + file);
                    // Add Compression stuff here

                }
            }
        }
    }

}