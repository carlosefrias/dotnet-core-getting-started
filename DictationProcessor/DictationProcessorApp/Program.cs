using System.IO;
using DictationProcessorLib;

namespace DictationProcessorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //iterate through subfolders of /mnt/uploads
            foreach (var subfolder in Directory.GetDirectories("../../../../uploads"))
            {
                var uploadProcessor = new UploadProcessor(subfolder);
                uploadProcessor.Process();
            }
        }
    }
}