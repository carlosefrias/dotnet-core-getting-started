using System.IO;
using System.Runtime.InteropServices;
using DictationProcessorLib;

namespace DictationProcessorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = 0;
            //iterate through subfolders of /mnt/uploads
            foreach (var subfolder in Directory.GetDirectories("../../../../uploads"))
            {
                var uploadProcessor = new UploadProcessor(subfolder);
                uploadProcessor.Process();
                i++;
            }

            var message = $"{i} uploads were processed";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                GtkHelper.DisplayAlert(message);
            }
        }
    }
}