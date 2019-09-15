using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.IO.Compression;

namespace DictationProcessorLib
{
    public class UploadProcessor
    {
        public string SubFolder { get; set; }
        
        public UploadProcessor(string subFolder)
        {
            this.SubFolder = subFolder;
        }
        public void Process()
        {
            //get metadata file
            var metadataFilePath = Path.Combine(SubFolder, "metadata.json");
            Console.WriteLine($"Reading {metadataFilePath}");
            //extract metadata, including audio file info, from metadata file
            var metadataCollection = GetMetadata(metadataFilePath);
            
            //for each audio file listed in metadata:
            foreach (var metaData in metadataCollection)
            {
                //- get absolute file path
                var audioFilePath = Path.Combine(SubFolder, metaData.File.FileName);
                //- verify file checksum
                var md5Checksum = GetChecksum(audioFilePath);
                if (md5Checksum.Replace("-","").ToLower() != metaData.File.Md5Checksum)
                {
                    throw new Exception("Checksum not verified! File corrupted?");
                }
                //- generate a unique identifier
                var uniqueId = Guid.NewGuid();
                metaData.File.FileName = uniqueId + ".wav";
                var newPath = Path.Combine("../../../../ready_for_transcription", metaData.File.FileName);
                //- compress it
                CreateCompressedFile(audioFilePath, newPath);
                //- create a standalone metadata file
                SaveSingleMetadata(metaData, newPath + ".json");
            }
        }

        private void CreateCompressedFile(string inputFilePath, string outputFilePath)
        {
            outputFilePath += ".gz";
            Console.WriteLine($"Creating {outputFilePath}");
            var inputFileStream = File.Open(inputFilePath, FileMode.Open);
            var outputFileStream = File.Create(outputFilePath);
            var gzipStream = new GZipStream(outputFileStream, CompressionLevel.Optimal);
            inputFileStream.CopyTo(gzipStream);
        }
        private string GetChecksum(string filePath)
        {
            var fileStream = File.Open(filePath, FileMode.Open);
            var md5 = System.Security.Cryptography.MD5.Create();
            var md5Bytes = md5.ComputeHash(fileStream);
            fileStream.Dispose();
            return BitConverter.ToString(md5Bytes);
        }

        private IEnumerable<Metadata> GetMetadata(string metadataFilePath)
        {
            var metadataFileStream = File.Open(metadataFilePath, FileMode.Open);
            var settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssZ")
            };
            var serializer = new DataContractJsonSerializer(typeof(List<Metadata>), settings);
            return (List<Metadata>) serializer.ReadObject(metadataFileStream);
        }
        
        private static void SaveSingleMetadata(Metadata metadata, string metadataFilePath)
        {
            Console.WriteLine($"Creating metadata {metadataFilePath}");
            var metadataFileStream = File.Open(metadataFilePath, FileMode.Create);
            var settings = new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssZ")
            };
            var serializer = new DataContractJsonSerializer(typeof(Metadata), settings);
            serializer.WriteObject(metadataFileStream, metadata);
        }
    }
}