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
}
