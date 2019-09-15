using System;
using System.IO;
using DictationProcessorLib;

namespace DictationProcessorSvc
{
    class Program
    {
        static void Main(string[] args)
        {
            var uploadFolder = "../../../../uploads";
            var fileSystemWatcher = new FileSystemWatcher(uploadFolder, "metadata.json");
            fileSystemWatcher.IncludeSubdirectories = true;
            while (true)
            {
                var result = fileSystemWatcher.WaitForChanged(WatcherChangeTypes.Created);
                Console.WriteLine($"New metadata file {result.Name}");
                var fullMetadataFilePath = Path.Combine(uploadFolder, result.Name);
                var subfolder = Path.GetDirectoryName(fullMetadataFilePath);
                var processor = new UploadProcessor(subfolder);
                processor.Process();
            }
        }
    }
}