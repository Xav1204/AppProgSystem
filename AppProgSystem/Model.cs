using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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