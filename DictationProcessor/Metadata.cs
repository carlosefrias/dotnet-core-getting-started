using System;
using System.Collections.Generic;
namespace DictationProcessor
{
    public class Metadata
    {
        public string Practitioner {get; set;}
        public string Patient {get; set;}
        public DateTime DateRecorded {get; set;}
        public List<string> Tags {get; set;}
        public AudioFile File {get; set;}
    }
}