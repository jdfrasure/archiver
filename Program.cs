using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

// Directory Structure is E:\csvdatay\MACADDR\YYYY-MM-DD
// Date currently matches the date on the folder.

class MainClass
{
    public static void Main(string[] args)
    {
        if (!args.Any())
        {
            Console.WriteLine("Example: Archiver c:\\csvDatay");
        }
        else
        {
            var archivedir = args[0];
            string[] dirs = Directory.GetDirectories(archivedir); // something like e:\csvdatay\ 
            foreach (string directory1 in dirs)
            {
                string[] dirname = directory1.Split(Path.DirectorySeparatorChar);

                //This is for E:\csvDatay
                string tgzfn = (@"c:\temp\" + dirname[2] + ".tgz");
                string adir = directory1;
                Console.WriteLine("Creating " + tgzfn);
                CreateTarGZ(tgzfn, adir);
            }
        }
    }

    public static void CreateTarGZ(string tgzFilename, string sourceDirectory)
    {
        Console.WriteLine("Running CreateTarGZ process");
        Stream outStream = File.Create(tgzFilename);
        Stream gzoStream = new GZipOutputStream(outStream);
        TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

        // Note that the RootPath is currently case sensitive and must be forward slashes e.g. "c:/temp"
        // and must not end with a slash, otherwise cuts off first char of filename
        // This is scheduled for fix in next release
        tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
        if (tarArchive.RootPath.EndsWith("/"))
            tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);


        AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);

        tarArchive.Close();
    }


    public static void AddDirectoryFilesToTar(TarArchive tarArchive, string sourceDirectory, bool recurse)
    {

        // Optionally, write an entry for the directory itself.
        // Specify false for recursion here if we will add the directory's files individually.
        //
        Console.WriteLine("AddDirectoryFilestoTar");

        TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
        tarArchive.WriteEntry(tarEntry, false);

        // Write each file to the tar.
        //
        string[] filenames = Directory.GetFiles(sourceDirectory);
        foreach (string filename in filenames)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.LastWriteTime < DateTime.Now.AddDays(-60))
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tarArchive.WriteEntry(tarEntry, true);
            Console.WriteLine("Writing to tar file: " + filename);
        }

        if (recurse)
        {
            string[] directories = Directory.GetDirectories(sourceDirectory);
            foreach (string directory in directories)
                AddDirectoryFilesToTar(tarArchive, directory, recurse);
        }
    }


}
