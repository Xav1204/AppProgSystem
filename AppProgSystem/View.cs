using System;
using System.ComponentModel;

namespace AppProgSystem
{
    public class View { }
    public class data_Save
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public string Priorite { get; set; }
        public string Log { get; set; }
    }


    public class Items : data_Save
    {
        public int progress{ get; set;}
    }
    public class log_journalier
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Size { get; set; }
        public string Extension { get; set; }
        public string Priorite { get; set; }
        public string FileTransferTime { get; set; }
        public DateTime Time { get; set; }
        public string EncryptTime { get; set; }
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
