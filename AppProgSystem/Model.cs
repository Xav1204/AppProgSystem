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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AppProgSystem
{
    public class Model
    {
        //variable model
        public string Name;
        public string Source;
        public string Target;
        public string Type;
        public string Extension;
        public string Priorite;
        public string pathSave = "C:\\EasySave\\Save\\Save.json";
        public string pathJournalier = "C:\\EasySave\\Log\\Log_Journalier.json";
        public string pathAvancement = "C:\\EasySave\\Log\\Log_Avancement.json";
        public string pathJournalierXML = "C:\\EasySave\\Log\\Log_Journalier.xml";
        public Thread thr;
        public Task task;
        public bool stop = false;

        //variable intermédiaire
        public static DataGrid set = new DataGrid();
        public static TextBox txt_nom = new TextBox();
        public static TextBox txt_source = new TextBox();
        public static TextBox txt_cible = new TextBox();
        public static TextBox txt_type = new TextBox();
        public static TextBox txt_extension = new TextBox();
        public static TextBox txt_priorite = new TextBox();
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
        public void Journalier(string NameSave, string SourceSave, string TargetSave, int SizeSave, string extension, string priorite, TimeSpan TransfertSave, TimeSpan temps)
        {
            var json = File.ReadAllText(pathSave);
            var List = JsonConvert.DeserializeObject<List<data_Save>>(json);

            foreach (var data in List.Where(x => x.Name == NameSave))
            {
                var jsondata = File.ReadAllText(pathJournalier);
                var list = JsonConvert.DeserializeObject<List<log_journalier>>(jsondata);

                VerifyFile(pathJournalier);

                //strucuture log_journalier
                log_journalier Save = new log_journalier
                {
                    Name = NameSave,
                    Source = SourceSave,
                    Target = TargetSave,
                    Extension = extension,
                    Priorite = priorite,
                    Size = SizeSave.ToString(),
                    FileTransferTime = TransfertSave.ToString(),
                    Time = DateTime.Now,
                    EncryptTime = temps.ToString(),
                };

                if (data.Log == "json")
                {
                    
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
                if (data.Log == "xml")
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
                            Extension = extension,
                            Priorite = priorite,
                            Size = SizeSave.ToString(),
                            FileTransferTime = TransfertSave.ToString(),
                            Time = DateTime.Now,
                            EncryptTime = temps.ToString()
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
                            Extension = extension,
                            Priorite = priorite,
                            Size = SizeSave.ToString(),
                            FileTransferTime = TransfertSave.ToString(),
                            Time = DateTime.Now,
                            EncryptTime = temps.ToString()
                        });

                        stream.Dispose();

                        FileStream streamread = File.OpenRead(pathJournalierXML);

                        var result = (log_journalier)(serializer.Deserialize(streamread));
                    }
                }
            }

               
        }

        //ecrire dans le log avancement les sauvegardes en cours d'exécution
        public void Avancement(string NameSave, string SourceSave, string TargetSave, string State, int FileToCopy, int FileSize, int FileToDo, float Progression)
        {
            {
                VerifyFile(pathAvancement);

                try
                {
                    var jsondata = File.ReadAllText(pathAvancement);
                    var list = JsonConvert.DeserializeObject<List<log_avancement>>(jsondata);

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
                    Thread.Sleep(10);
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
                    items.Add(new Items { Name = data.Name, Source = data.Source, Target = data.Target, Type = data.Type, Extension = data.Extension, Priorite = data.Priorite, Log = data.Log });
                }
                set.ItemsSource = items;
            }
            catch
            {
                pascontent();
            }
            
        }
        
        public CancellationTokenSource _token = null;

        public void Play()
        {
            _token = new CancellationTokenSource();
            CancellationToken token = _token.Token;

            Items items = set.SelectedItem as Items;
            var nom = items.Name;
            
            try
            {
                task = Task.Run(() =>
                {
                    Save Play = new Save();
                    thr = new Thread(new ThreadStart(() => Play.Sauvegarde(nom, token)));
                    thr.Name = nom;
                    thr.IsBackground = true;
                    thr.Start();
                });
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("error");
            }
        }

        public void Pause()
        {
            stop = true;
        }
        public void Resume()
        {
            stop = false;
        }
        public void Stop()
        {
            _token.Cancel();
            _token.Dispose();
        }

        //créer une sauvegarde avec ses paramètres en entrée
        public void Create(string NameSave, string SourceSave, string TargetSave, string TypeSave, string extension, string priorite, string log)
        {
            var result = false;

            if (txt_nom.Text == "" | txt_source.Text == "" | txt_cible.Text == "" | txt_type.Text == ""  | extent.Text == "" )
            {
                pascontent();
            }
            else
            {
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
                    Type = TypeSave,
                    Extension = extension,
                    Priorite = priorite,
                    Log = log
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
                            data.Extension = extension;
                            data.Priorite = priorite;
                            data.Log = log;

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
                    if (txt_nom.Text != "" & txt_extension.Text != "")
                    {
                        var modif = txt_extension.Text;

                        data.Extension = modif;

                        json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                        File.WriteAllText(pathSave, json);
                    }
                    if (txt_nom.Text != "" & txt_priorite.Text != "")
                    {
                        var modif = txt_priorite.Text;

                        data.Priorite = modif;

                        json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                        File.WriteAllText(pathSave, json);
                    }
                    if (txt_nom.Text != "" & extent.Text != "")
                    {
                        var modif = extent.Text;

                        data.Log = modif;

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
            if (txt_nom.Text == "")
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
                    data.Extension = null;
                    data.Priorite = null;
                    data.Log = null;
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

        public class Save : Model
        {
            public void Sauvegarde(string NameSave, CancellationToken token)
            {
                var jsonText = File.ReadAllText(pathSave);
                var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText);

                foreach (var data in Data.Where(x => x.Name == NameSave))
                {
                    Name = data.Name;
                    Source = data.Source;
                    Target = data.Target;
                    Type = data.Type;
                    Extension = data.Extension;
                    Priorite = data.Priorite;
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
                            var chiffre = Extension;
                            var prioritee = Priorite;

                            string[] files = Directory.GetFiles(Source);
                            string[] Files = Directory.GetFiles(Source);

                            int TotalSize = 0;
                            int Size = 0;
                            string state = "Active";
                            float Progression = 0;

                            //début timer pour le temps
                            var sw = Stopwatch.StartNew();

                            //récupérer nombre de fichiers dans le dossier
                            int TotalFiles = Directory.GetFiles(Source, "*.*", SearchOption.TopDirectoryOnly).Length;
                            int FileToDo = TotalFiles;



                            //on recupère la taille en octets du dossier
                            foreach (string F in Files)
                            {
                                try
                                {
                                    
                                    var fileName = Path.GetFileName(F);
                                    var destFile = Path.Combine(Target, fileName);
                                    var prio = Path.GetFileNameWithoutExtension(F);


                                    if (prioritee != null)
                                    {

                                        foreach (string C in Files.Where(x => fileName == prio + prioritee))
                                        {
                                            File.Copy(C, destFile, true);
                                        }
                                    }
                                    if (token.IsCancellationRequested)
                                    {
                                        token.ThrowIfCancellationRequested();
                                    }
                                    File.Copy(F, destFile, true);

                                    TotalSize += F.Length;
                                    Size = TotalSize;
                                    FileToDo--;
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
                                catch
                                {
                                    Thread.Sleep(10);
                                }
                            }
                            try
                            {
                                if (token.IsCancellationRequested)
                                {
                                    token.ThrowIfCancellationRequested();
                                }
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                Process p = new Process();
                                p.StartInfo.FileName = @"C:\EasySave\Cryptosoft\Cryptosoft.exe";
                                string str = source.ToString() + " " + target.ToString() + " " + chiffre.ToString();
                                p.StartInfo.Arguments = str;
                                p.Start();
                                p.WaitForExit();
                                stopwatch.Stop();
                                //fin timer
                                sw.Stop();
                                TimeSpan Timer = sw.Elapsed;
                                TimeSpan temps = stopwatch.Elapsed;
                                try
                                {
                                    Journalier(Name, source, target, Size, Extension, Priorite, Timer, temps);
                                }
                                catch
                                {
                                    Thread.Sleep(500);
                                }
                                content();
                            }
                            catch
                            {
                                MessageBox.Show("Interrupted / Interrompu");
                            }
                        }

                        //si c'est pas complet, c'est différentiel
                        else
                        {
                            var source = Source;
                            var target = Target;
                            var chiffre = Extension;
                            var prioritee = Priorite;
                            string[] files = Directory.GetFiles(Source);
                            string[] Files = Directory.GetFiles(Target);

                            int TotalSize = 0;
                            string state = "Active";

                            int TotalFiles = 0;
                            int Size = 0;

                            foreach (string f in files)
                            {
                                foreach (string F in Files)
                                {
                                    try
                                    {
                                        if (f.Length != F.Length)
                                        {
                                            var fileName = Path.GetFileName(f);
                                            var destFile = Path.Combine(Target, fileName);
                                            var prio = Path.GetFileNameWithoutExtension(f);
                                            if (prioritee != null)
                                            {
                                                foreach (string C in Files.Where(x => fileName == prio + prioritee))
                                                {
                                                    File.Copy(C, destFile, true);
                                                }
                                            }
                                            if (token.IsCancellationRequested)
                                            {
                                                token.ThrowIfCancellationRequested();
                                            }
                                            File.Copy(f, destFile, true);

                                            TotalSize += f.Length - F.Length;
                                            TotalFiles += 1;
                                            Size = TotalSize;
                                        }
                                    }
                                    catch
                                    {
                                        Thread.Sleep(100);
                                    }
                                    
                                }
                            }
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
                            try
                            {
                                if (token.IsCancellationRequested)
                                {
                                    token.ThrowIfCancellationRequested();
                                }
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                Process p = new Process();
                                p.StartInfo.FileName = @"C:\EasySave\Cryptosoft\Cryptosoft.exe";
                                string str = source.ToString() + " " + target.ToString() + " " + chiffre.ToString();
                                p.StartInfo.Arguments = str;
                                p.Start();
                                p.WaitForExit();
                                stopwatch.Stop();
                                //fin timer
                                sw.Stop();
                                TimeSpan Timer = sw.Elapsed;
                                TimeSpan temps = stopwatch.Elapsed;
                                try
                                {
                                    Journalier(Name, source, target, Size, Extension, Priorite, Timer, temps);
                                }
                                catch
                                {
                                    Thread.Sleep(500);
                                }
                                content();
                            }
                            catch
                            {
                                MessageBox.Show("Interrupted / Interrompu");
                            }
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
    }
}