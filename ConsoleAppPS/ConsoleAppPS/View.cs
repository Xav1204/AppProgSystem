using System;

namespace ConsoleAppPS
{
    class View
    {
        
    }
    //Les classes suivantes instancient les json
    public class data_NBSave
    {
        public int NBSave { get; set; }
    }
    public class data_sequential
    {
        public string ChoixSave { get; set; }
        public int TempsSave { get; set; }
    }
    public class data_Save
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Type { get; set; }
    }
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
