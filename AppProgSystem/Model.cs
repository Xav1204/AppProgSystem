using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace AppProgSystem
{
    public class Model
    {
        public static DataGrid set = new DataGrid();
        public void Read()
        {
            StreamReader r = new StreamReader(@"C:\EasySave\Save\Save.json");
            string json = r.ReadToEnd();
            List<data_Save> table = JsonConvert.DeserializeObject<List<data_Save>>(json);
            List<Items> items = new List<Items>();
            foreach (var data in table)
            {
                items.Add(new Items { Name = data.Name, Source = data.Source, Target = data.Target, Type = data.Type });
            }
            set.ItemsSource = items;
        }
    }
}