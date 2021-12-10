using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace AppProgSystem
{
    public class Model
    {
        public delegate String del_JSON(string path, string search);

        //variable model
        private string Name;
        private string Source;
        private string Target;
        private string Type;
        private string pathSave = "C:\\EasySave\\Save\\Save.json";
        private string pathJournalier = "C:\\EasySave\\Log\\Log_Journalier.json";
        private string pathAvancement = "C:\\EasySave\\Log\\Log_Avancement.json";
        private string pathJournalierXML = "C:\\EasySave\\Log\\Log_Journalier.xml";

        //variable intermédiaire
        public static DataGrid set = new DataGrid();
        public static TextBox txt_nom = new TextBox();
        public static TextBox txt_source = new TextBox();
        public static TextBox txt_cible = new TextBox();
        public static TextBox txt_type = new TextBox();
        public static TextBox txt_extension = new TextBox();
        public static ComboBox extent = new ComboBox();

        public void pascontent()
        {
            string pascontent = char.ConvertFromUtf32(0x1F624);
            MessageBox.Show(pascontent + pascontent + pascontent + pascontent + pascontent + pascontent + pascontent);
        }

        public void content()
        {
            string content = char.ConvertFromUtf32(0x1F601);
            MessageBox.Show(content + content + content + content + content + content + content);
        }
        //verifier si fichier existe
        public string VerifyFile(string JsonFileIn)
        {
        BEGIN:
            if (File.Exists(JsonFileIn))
            {
                return JsonFileIn;
            }
            else
            {
                pascontent();
                goto BEGIN;
            }
        }

        //ecrire dans le log journalier les sauvegardes exécutées
        public void Journalier(string NameSave, string SourceSave, string TargetSave, int SizeSave, TimeSpan TransfertSave, string Temps)
        {
            VerifyFile(pathJournalier);

            //strucuture log_journalier
            log_journalier Save = new log_journalier
            {
                Name = NameSave,
                Source = SourceSave,
                Target = TargetSave,
                Size = SizeSave.ToString(),
                FileTransferTime = TransfertSave.ToString(),
                Time = DateTime.Now
            };

            var choix = extent.Text;

            if (choix == "json")
            {
                var jsondata = File.ReadAllText(pathJournalier);
                var list = JsonConvert.DeserializeObject<List<log_journalier>>(jsondata);

                //si le log journalier est vide
                if (list == null)
                {
                    jsondata = "[" + JsonConvert.SerializeObject(Save, Formatting.Indented) + "]";
                    File.WriteAllText(pathJournalier, jsondata);
                }

                //si le log journalier est non vide
                else
                {
                    list.Add(Save);
                    jsondata = JsonConvert.SerializeObject(list, Formatting.Indented);
                    File.WriteAllText(pathJournalier, jsondata);
                }
            }
            if(choix == "xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(log_journalier));

                try
                {
                    FileStream stream = File.OpenWrite(pathJournalierXML);
                    serializer.Serialize(stream, new log_journalier()
                    {
                        Name = NameSave,
                        Source = SourceSave,
                        Target = TargetSave,
                        Size = SizeSave.ToString(),
                        FileTransferTime = TransfertSave.ToString(),
                        Time = DateTime.Now
                    });

                    stream.Dispose();

                    FileStream streamread = File.OpenRead(pathJournalierXML);

                    var result = (log_journalier)(serializer.Deserialize(streamread));
                }
                catch
                {
                    FileStream stream = File.OpenWrite(pathJournalierXML);
                    serializer.Serialize(stream, new log_journalier()
                    {
                        Name = NameSave,
                        Source = SourceSave,
                        Target = TargetSave,
                        Size = SizeSave.ToString(),
                        FileTransferTime = TransfertSave.ToString(),
                        Time = DateTime.Now
                    });

                    stream.Dispose();

                    FileStream streamread = File.OpenRead(pathJournalierXML);

                    var result = (log_journalier)(serializer.Deserialize(streamread));
                }
            }
        }

        //ecrire dans le log avancement les sauvegardes en cours d'exécution
        public void Avancement(string NameSave, string SourceSave, string TargetSave, string State, int FileToCopy, int FileSize, int FileToDo, float Progression)
        {
            {
                VerifyFile(pathAvancement);

                var jsondata = File.ReadAllText(pathAvancement);
                var list = JsonConvert.DeserializeObject<List<log_avancement>>(jsondata);

                try
                {
                    //update le log_avancement le temps du save
                    foreach (var data in list.Where(x => x.Name == NameSave))
                    {
                        data.Source = SourceSave;
                        data.Target = TargetSave;
                        data.State = State;
                        data.progression = Progression.ToString();
                        data.TotalFilesToCopy = FileToCopy.ToString();
                        data.TotalFilesSize = FileSize.ToString();
                        data.NbFilesLeftToDo = FileToDo.ToString();
                        jsondata = JsonConvert.SerializeObject(list, Formatting.Indented);
                        File.WriteAllText(pathAvancement, jsondata);
                    }
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        //lire données json dans datagrid
        public void Read()
        {
            StreamReader r = new StreamReader(pathSave);
            string json = r.ReadToEnd();
            List<data_Save> table = JsonConvert.DeserializeObject<List<data_Save>>(json);
            List<Items> items = new List<Items>();
            try
            {
                foreach(var data in table)
                {
                    items.Add(new Items { Name = data.Name, Source = data.Source, Target = data.Target, Type = data.Type });
                }
                set.ItemsSource = items;
            }
            catch
            {
                pascontent();
            }
            
        }

        //créer une sauvegarde avec ses paramètres en entrée
        public void Create(string NameSave, string SourceSave, string TargetSave, string TypeSave)
        {
            var result = false;

            if (txt_nom.Text == "" & txt_source.Text == "" & txt_cible.Text == "" & txt_type.Text == "")
            {
                pascontent();
            }
            else
            {
                Name = NameSave;
                Source = SourceSave;
                Target = TargetSave;
                Type = TypeSave;

                VerifyFile(pathSave);
                VerifyFile(pathAvancement);

                var jsondata = File.ReadAllText(pathSave);
                var list = JsonConvert.DeserializeObject<List<data_Save>>(jsondata);

                var jsondata2 = File.ReadAllText(pathAvancement);
                var list2 = JsonConvert.DeserializeObject<List<log_avancement>>(jsondata2);

                //strucuture save.json
                data_Save Save = new data_Save
                {
                    Name = NameSave,
                    Source = SourceSave,
                    Target = TargetSave,
                    Type = TypeSave
                };

                //structure log_avancement
                log_avancement avance = new log_avancement
                {
                    Name = NameSave,
                    Source = "",
                    Target = "",
                    State = "END",
                    progression = "0",
                    TotalFilesToCopy = "0",
                    TotalFilesSize = "0",
                    NbFilesLeftToDo = "0"
                };

                //si fichier vide
                if (list == null)
                {
                    jsondata = "[" + JsonConvert.SerializeObject(Save, Formatting.Indented) + "]";
                    File.WriteAllText(pathSave, jsondata);

                    jsondata2 = "[" + JsonConvert.SerializeObject(avance, Formatting.Indented) + "]";
                    File.WriteAllText(pathAvancement, jsondata2);
                    MessageBox.Show("Save created");
                }

                else
                {
                    foreach (var data in list)
                    {
                        if (data.Name == null)
                        {
                            result = true;
                        }
                    }

                    if (result == true)
                    {
                        foreach (var data in list.Where(x => x.Name == null))
                        {
                            data.Name = NameSave;
                            data.Source = SourceSave;
                            data.Target = TargetSave;
                            data.Type = TypeSave;

                            jsondata = JsonConvert.SerializeObject(list, Formatting.Indented);
                            File.WriteAllText(pathSave, jsondata);

                            if (data.Name == NameSave)
                            {
                                break;
                            }
                        }

                        foreach (var data in list2.Where(x => x.Name == null))
                        {
                            data.Name = NameSave;

                            jsondata2 = JsonConvert.SerializeObject(list2, Formatting.Indented);
                            File.WriteAllText(pathAvancement, jsondata2);

                            if (data.Name == NameSave)
                            {
                                break;
                            }
                        }
                        content();
                    }
                    //si fichier non vide
                    else
                    {
                        list.Add(Save);
                        jsondata = JsonConvert.SerializeObject(list, Formatting.Indented);
                        File.WriteAllText(pathSave, jsondata);

                        list2.Add(avance);
                        jsondata2 = JsonConvert.SerializeObject(list2, Formatting.Indented);
                        File.WriteAllText(pathAvancement, jsondata2);
                        content();

                    }
                }   
            }
        }

        //modifier une sauvegarde selon les champs renseignés
        public void Modify()
        {
            if (txt_nom.Text == "")
            {
                pascontent();
            }
            else
            {
                VerifyFile(pathSave);
                VerifyFile(pathAvancement);

                string json = File.ReadAllText(pathSave);
                var Data = JsonConvert.DeserializeObject<List<data_Save>>(json);

                var search = txt_nom.Text;

                foreach (var data in Data.Where(x => x.Name == search))
                {
                    //si on change la source
                    if (txt_nom.Text != "" & txt_source.Text != "")
                    {
                        var modif = txt_source.Text;

                        data.Source = modif;

                        json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                        File.WriteAllText(pathSave, json);
                    }

                    //si on change la cible
                    if (txt_nom.Text != "" & txt_cible.Text != "")
                    {
                        var modif = txt_cible.Text;

                        data.Target = modif;

                        json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                        File.WriteAllText(pathSave, json);
                    }

                    //si on change le type
                    if (txt_nom.Text != "" & txt_type.Text != "")
                    {
                        var modif = txt_type.Text;

                        data.Type = modif;

                        json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                        File.WriteAllText(pathSave, json);
                    }
                }
                content();
            }
        }

        //supprimer sauvegarde
        public void Delete()
        {
            if (txt_nom.Text == "" & extent.Text == "")
            {
                pascontent();
            }
            else
            {
                VerifyFile(pathSave);
                VerifyFile(pathAvancement);

                var jsonText = File.ReadAllText(pathSave);
                var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText);

                var jsonText2 = File.ReadAllText(pathAvancement);
                var Data2 = JsonConvert.DeserializeObject<List<log_avancement>>(jsonText2);

                var nameSave = txt_nom.Text;

                //on met tout à null dans le save.json
                foreach (var data in Data.Where(x => x.Name == nameSave))
                {
                    data.Source = null;
                    data.Target = null;
                    data.Type = null;
                    data.Name = null;
                }
                jsonText = JsonConvert.SerializeObject(Data, Formatting.Indented);
                File.WriteAllText(pathSave, jsonText);

                //on met à null nom dans log_avancement
                foreach (var data2 in Data2.Where(x => x.Name == nameSave))
                {
                    data2.Name = null;
                }
                jsonText2 = JsonConvert.SerializeObject(Data2, Formatting.Indented);
                File.WriteAllText(pathAvancement, jsonText2);

                content();
            } 
        }

        public void Save(string NameSave)
        {
            if(txt_nom.Text == "")
            {
                pascontent();
            }
            else
            {
                var jsonText = File.ReadAllText(pathSave);
                var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText);

                foreach (var data in Data.Where(x => x.Name == NameSave))
                {
                    Name = data.Name;
                    Source = data.Source;
                    Target = data.Target;
                    Type = data.Type;
                }

                //on vérifie si la cible existe
                if (Directory.Exists(Target))
                {
                    //on vérifie la source existe
                    if (Directory.Exists(Source))
                    {
                        //si le type est complet
                        if (Type == "complet" | Type == "Complet")
                        {
                            var source = Source;
                            var target = Target;
                            string[] files = Directory.GetFiles(Source);
                            string[] Files = Directory.GetFiles(Source);

                            int TotalSize = 0;
                            string state = "Active";

                            //on recupère la taille en octets du dossier
                            foreach (string F in Files)
                            {
                                // Use static Path methods to extract only the file name from the path.
                                var fileName = Path.GetFileName(F);
                                var destFile = Path.Combine(Target, fileName);
                                File.Copy(F, destFile, true);
                                TotalSize += F.Length;
                            }
                            int Size = TotalSize;
                            float Progression = 0;
                            //début timer pour le temps
                            var sw = Stopwatch.StartNew();
                            //récupérer nombre de fichiers dans le dossier
                            int TotalFiles = Directory.GetFiles(Source, "*.*", SearchOption.TopDirectoryOnly).Length;
                            int FileToDo = TotalFiles;

                            foreach (string s in files)
                            {
                                for (int i = 0; i < TotalFiles; i++)
                                {
                                    FileToDo--;
                                    var fileName = Path.GetFileName(s);
                                    var destFile = Path.Combine(Target, fileName);
                                    File.Copy(s, destFile, true);
                                    // quand on a plus de fichiers à copier, on met tout à zéro
                                    if (FileToDo == 0)
                                    {
                                        Source = "";
                                        Target = "";
                                        state = "END";
                                        TotalFiles = 0;
                                        TotalSize = 0;
                                        FileToDo = 0;
                                        Progression = 0;
                                        Avancement(Name, Source, Target, state, TotalFiles, TotalSize, FileToDo, Progression);
                                    }
                                    Avancement(Name, Source, Target, state, TotalFiles, TotalSize, FileToDo, Progression);
                                }
                            }
                            //fin timer
                            sw.Stop();
                            TimeSpan Timer = sw.Elapsed;
                            Environment.SetEnvironmentVariable("Name", NameSave);
                            var temps = Environment.GetEnvironmentVariable("Temps");
                            cryptosoft();
                            Journalier(Name, source, target, Size, Timer, temps);
                            content();
                        }

                        //si c'est pas complet, c'est différentiel
                        else
                        {
                            var source = Source;
                            var target = Target;

                            string[] files = Directory.GetFiles(Source);
                            string[] Files = Directory.GetFiles(Target);

                            int TotalSize = 0;
                            string state = "Active";

                            int TotalFiles = 0;

                            foreach (string f in files)
                            {
                                foreach (string F in Files)
                                {
                                    var fileName = Path.GetFileName(f);
                                    var destFile = Path.Combine(Target, fileName);
                                    File.Copy(f, destFile, true);
                                    if (f.Length != F.Length)
                                    {
                                        TotalSize += f.Length - F.Length;
                                        TotalFiles += 1;
                                    }
                                }
                            }
                            int Size = TotalSize;
                            float Progression = 0;
                            var sw = Stopwatch.StartNew();
                            int FileToDo = TotalFiles;

                            foreach (string s in files)
                            {
                                foreach (string S in Files)
                                {
                                    if (s.Length != S.Length)
                                    {
                                        for (int i = 0; i < TotalFiles; i++)
                                        {
                                            FileToDo--;
                                            var fileName = Path.GetFileName(s);
                                            var destFile = Path.Combine(Target, fileName);
                                            File.Copy(s, destFile, true);
                                            if (FileToDo == 0)
                                            {
                                                Source = "";
                                                Target = "";
                                                state = "END";
                                                TotalFiles = 0;
                                                TotalSize = 0;
                                                FileToDo = 0;
                                                Progression = 0;
                                                Avancement(Name, Source, Target, state, TotalFiles, TotalSize, FileToDo, Progression);
                                            }
                                            Avancement(Name, Source, Target, state, TotalFiles, TotalSize, FileToDo, Progression);
                                        }
                                    }
                                }
                            }
                            sw.Stop();
                            TimeSpan Timer = sw.Elapsed;
                            Environment.SetEnvironmentVariable("Name", NameSave);
                            var temps = Environment.GetEnvironmentVariable("Temps");
                            cryptosoft();
                            Journalier(Name, source, target, Size, Timer, temps);
                            content();
                        }
                    }
                    else
                    {
                        pascontent();
                    }
                }
                else
                {
                    Directory.CreateDirectory(Target);
                }
            }  
        }
        public void SequentialSave()
        {
            if (txt_nom.Text == "")
            {
                pascontent();
            }
            else
            {
                var jsonText = File.ReadAllText(pathSave);
                var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText);

                //pour chaque sauvegarde dans save.json, on exécute save()
                foreach (var data in Data)
                {
                    Save(data.Name);
                }
            }
        }

        public string ExeJS(string pathLangues, string search)
        {
            var Jservice = new Model();
            return Jservice.ReadJsonFile(VerifyFiles(pathLangues), search);
        }
        private string VerifyFiles(string path)
        {
        BEGIN:
            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                //in some case just break the execution
                pascontent();
                goto BEGIN;
            }
        }
        public string ReadJsonFile(string path, string search)
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            //Searching in JSON File support multiple parameters
            return jsonFile.SelectToken(search);
        }

        public void cryptosoft()
        {
            Process myprocess = Process.Start("C:\\EasySave\\Cryptosoft\\Cryptosoft.exe");
            myprocess.WaitForExit();
        }
    }
}