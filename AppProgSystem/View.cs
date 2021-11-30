using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AppProgSystem
{
    public class View { }
    public class data_Save
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Type { get; set; }
    }
    public class Items : data_Save { }
    public class log_journalier
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Size { get; set; }
        public string FileTransferTime { get; set; }
        public DateTime Time { get; set; }
    }
    public class log_avancement
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string State { get; set; }
        public string progression { get; set; }
        public string TotalFilesToCopy { get; set; }
        public string TotalFilesSize { get; set; }
        public string NbFilesLeftToDo { get; set; }
    }
}
