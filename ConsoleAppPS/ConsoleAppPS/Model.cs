using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Xml.Serialization;

namespace ConsoleAppPS
{
    class Model
    {
        private string Name;
        private string Source;
        private string Target;
        private string Type;
        private string JsonPath = "C:\\EasySaveConsole\\NBSave.json";
        private string Jsonpath = "C:\\EasySaveConsole\\Save.json";
        private string pathJournalier = "C:\\EasySaveConsole\\Log_Journalier.json";
        private string pathJournalierXML = "C:\\EasySaveConsole\\Log_Journalier.xml";
        private string pathAvancement = "C:\\EasySaveConsole\\Log_Avancement.json";


        public string langueNom;
        public string langueSource;
        public string langueCible;
        public string langueType;
        public string langueChange;
        public string langueValeur;
        public string langueNoCreate;
        public string langueErreur;
        public string languePath;
        public string languelist;
        public string langueadd;
        public string languemodify;
        public string languedelete;
        public string langueread;
        public string languesave;
        public string languemulti;
        public string languequit;
        public string langueoption;
        public string languejournalier;

        //prend en entrée les valeurs du json
        protected void Journalier(string NameSave, string SourceSave, string TargetSave, int SizeSave, TimeSpan TransfertSave)
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

            Console.Write("{0}", languejournalier);
            var choix = Console.ReadLine();

            if(choix == "Json" | choix == "json")
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
            else
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

        //prend en entrée les valeurs du json
        protected void Avancement(string NameSave, string SourceSave, string TargetSave, string State, int FileToCopy, int FileSize, int FileToDo, float Progression)
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
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
        
        //lire le save.json
        protected void read()
        { 
            try
            {
                StreamReader sr = new StreamReader(Jsonpath);
                string jsonString = sr.ReadToEnd();
                if (jsonString != null)
                {
                    data_Save[] m = JsonConvert.DeserializeObject<data_Save[]>(jsonString);
                    Console.WriteLine(jsonString);
                }
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("reading finally succeed.");
            }
        }
        protected void Create(string NameSave, string SourceSave, string TargetSave, string TypeSave)
        {
            Name = NameSave;
            Source = SourceSave;
            Target = TargetSave;
            Type = TypeSave;

            var nbr = new JsonService();

            VerifyFile(JsonPath);
            nbr.NombreSave(JsonPath);

            //si on a 5 sauvegardes, on ne pourra pas en créer de 6ème
            if (JsonService.Nb < 6)
            {                
                VerifyFile(Jsonpath);

                var jsondata = File.ReadAllText(Jsonpath);
                var list = JsonConvert.DeserializeObject<List<data_Save>>(jsondata);

                VerifyFile(pathAvancement);

                var jsondata2 = File.ReadAllText(pathAvancement);
                var list2 = JsonConvert.DeserializeObject<List<log_avancement>>(jsondata);

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
                    File.WriteAllText(Jsonpath, jsondata);

                    jsondata2 = "[" + JsonConvert.SerializeObject(avance, Formatting.Indented) + "]";
                    File.WriteAllText(pathAvancement, jsondata2);
                }

                //si fichier non vide
                else
                { 

                    list.Add(Save);
                    jsondata = JsonConvert.SerializeObject(list, Formatting.Indented);
                    File.WriteAllText(Jsonpath, jsondata);

                    list2.Add(avance);
                    jsondata2 = JsonConvert.SerializeObject(list2, Formatting.Indented);
                    File.WriteAllText(pathAvancement, jsondata2);
                }
            }
            else
            {
                Console.WriteLine("{0}", langueNoCreate);
            }
        }
        protected void Modify()
        {
            VerifyFile(Jsonpath);

            string json = File.ReadAllText(Jsonpath);
            var Data = JsonConvert.DeserializeObject<List<data_Save>>(json);

            VerifyFile(pathAvancement);

            var json2 = File.ReadAllText(pathAvancement);
            var Data2 = JsonConvert.DeserializeObject<List<log_avancement>>(json2);

            Console.Write("{0}", langueNom);
            var search = Console.ReadLine();

            foreach(var data in Data.Where(x => x.Name == search))
            {
                Console.Write("{0}", langueChange);
                var choix = Console.ReadLine();

                //si on change le nom
                if(choix == "name" | choix == "Name" | choix == "nom" | choix == "Nom")
                {                   
                    Console.Write("{0}", langueValeur);
                    var modif = Console.ReadLine();

                    data.Name = modif;

                    json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    File.WriteAllText(Jsonpath, json);

                    //on modifie le nom dans log_avancement
                    foreach(var avancement in Data2.Where(x => x.Name == search))
                    {
                        avancement.Name = modif;
                        json2= JsonConvert.SerializeObject(Data2, Formatting.Indented);
                        File.WriteAllText(pathAvancement, json2);
                    }  
                }

                //si on change la source
                else if (choix == "source" | choix == "Source")
                {
                    Console.Write("{0}", langueValeur);
                    var modif = Console.ReadLine();

                    data.Source = modif;

                    string output = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    File.WriteAllText(Jsonpath, output);
                }

                //si on change la cible
                else if (choix == "target" | choix == "Target" | choix == "cible" | choix == "Cible")
                {
                    Console.Write("{0}", langueValeur);
                    var modif = Console.ReadLine();

                    data.Target = modif;

                    string output = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    File.WriteAllText(Jsonpath, output);
                }

                //si on change le type
                else if (choix == "type" | choix == "Type")
                {
                    Console.Write("{0}", langueValeur);
                    var modif = Console.ReadLine();

                    data.Type = modif;

                    string output = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    File.WriteAllText(Jsonpath, output);
                }
            }
               
        }
        protected  void Delete()
        {
            var nbr = new JsonService();

            VerifyFile(Jsonpath);
            VerifyFile(JsonPath);
            VerifyFile(pathAvancement);
            nbr.DeleteNombreSave(JsonPath);

            var jsonText = File.ReadAllText(Jsonpath);
            var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText);

            var jsonText2 = File.ReadAllText(pathAvancement);
            var Data2 = JsonConvert.DeserializeObject<List<log_avancement>>(jsonText2);

            Console.Write("{0}", langueNom);
            var nameSave = Console.ReadLine();

            //on met tout à null dans le save.json
            foreach (var data in Data.Where(x => x.Name == nameSave))
            {
                data.Source = null;
                data.Target =null;
                data.Type =null;
                data.Name = null;
            }
            jsonText = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Jsonpath, jsonText);

            //on met à null nom dans log_avancement
            foreach (var data2 in Data2.Where(x => x.Name == nameSave))
            {
                data2.Name = null;
            }
            jsonText2 = JsonConvert.SerializeObject(Data2, Formatting.Indented);
            File.WriteAllText(pathAvancement, jsonText2);

        }
        protected void Save(string NameSave)
        {
            var jsonText = File.ReadAllText(Jsonpath);
            var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText);

            foreach(var data in Data.Where(x => x.Name == NameSave))
            {
                Name = data.Name;
                Source = data.Source;
                Target = data.Target;
                Type = data.Type;
            }

            //on vérifie si la cible existe
            if (System.IO.Directory.Exists(Target))
            {
                //on vérifie la source existe
                if (System.IO.Directory.Exists(Source))
                {
                    //si le type est complet
                    if(Type == "complet" | Type == "Complet")
                    {
                        var source = Source;
                        var target = Target;
                        string[] files = System.IO.Directory.GetFiles(Source);
                        string[] Files = System.IO.Directory.GetFiles(Source);

                        int TotalSize = 0;
                        string state = "Active";

                        //on recupère la taille en octets du dossier
                        foreach (string F in Files)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            var fileName = System.IO.Path.GetFileName(F);
                            var destFile = System.IO.Path.Combine(Target, fileName);
                            System.IO.File.Copy(F, destFile, true);
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
                            for(int i = 0; i < TotalFiles; i++ )
                            {
                                FileToDo--;
                                var fileName = System.IO.Path.GetFileName(s);
                                var destFile = System.IO.Path.Combine(Target, fileName);
                                System.IO.File.Copy(s, destFile, true);
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
                        Journalier(Name, source, target, Size, Timer);
                    }

                    //si c'est pas complet, c'est différentiel
                    else
                    {
                        var source = Source;
                        var target = Target;

                        string[] files = System.IO.Directory.GetFiles(Source);
                        string[] Files = System.IO.Directory.GetFiles(Target);

                        int TotalSize = 0;
                        string state = "Active";

                        int TotalFiles = 0;

                        foreach (string f in files)
                        {
                            foreach (string F in Files)
                            {
                                var fileName = System.IO.Path.GetFileName(f);
                                var destFile = System.IO.Path.Combine(Target, fileName);
                                System.IO.File.Copy(f, destFile, true);
                                if(f.Length != F.Length)
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
                                if(s.Length != S.Length)
                                {
                                    for (int i = 0; i < TotalFiles; i++)
                                    {
                                        FileToDo--;
                                        var fileName = System.IO.Path.GetFileName(s);
                                        var destFile = System.IO.Path.Combine(Target, fileName);
                                        System.IO.File.Copy(s, destFile, true);
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
                        Journalier(Name, source, target, Size, Timer);
                    }
                }
                else
                {
                    Console.WriteLine("{0}", languePath);
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(Target);
            }
        }
        protected void SequentialSave()
        {
            var jsonText = File.ReadAllText(Jsonpath);
            var Data = JsonConvert.DeserializeObject<List<data_Save>>(jsonText); 

            //pour chaque sauvegarde dans save.json, on exécute save()
            foreach(var data in Data)
            {
                Save(data.Name);
            }
        }

        //instanciation des langues
        public void Langue(string langue)
        {
            if(langue == "English" | langue == "english")
            {
                langueNom = "Name of the save : ";
                langueSource = "Source of the save : ";
                langueCible = "Target of the save : ";
                langueType = "Type of the save : ";
                langueChange = "What do you want to change ? : ";
                langueValeur = "Value : ";
                langueNoCreate = "You can't create a new Save";
                langueErreur = "\nError : File doesn't exist!";
                languePath = "Source path does not exist!";
                languelist = "Choose an option from the following list:";
                langueadd = "Add";
                languemodify = "Modify";
                languedelete = "Delete";
                langueread = "Read";
                languesave = "Save";
                languemulti = "Sequential Save";
                languequit = "Quit";
                langueoption = "Your option : ";
                languejournalier = "Choose the file type for the daily log :";
            }
            else
            {
                langueNom = "Nom de la sauvegarde : ";
                langueSource = "Source de la sauvegarde : ";
                langueCible = "Cible de la sauvegarde : ";
                langueType = "Type de la sauvegarde : ";
                langueChange = "Que voulez-vous changer ? : ";
                langueValeur = "Valeur : ";
                langueNoCreate = "Vous ne pouvez pas créer une nouvelle sauvegarde";
                langueErreur = "\nErreur :  le fichier n'existe pas ! ";
                languePath = "Source du dossier n'existe pas!";
                languelist = "Choisir une option dans la liste suivante :";
                langueadd = "Ajouter";
                languemodify = "Modifier";
                languedelete = "Supprimer";
                langueread = "Lire";
                languesave = "Sauvegarder";
                languemulti = "Sauvegarde sequentielle";
                languequit = "Quitter";
                langueoption = "Votre option : ";
                languejournalier = "Choissisez le type de fichier pour le log journalier :";
            }
        }

        //déclarer le début du programme
        public void debut()
        {
            Console.WriteLine("{0}", languelist);
            Console.WriteLine("\ta - {0}", langueadd);
            Console.WriteLine("\tm - {0}", languemodify);
            Console.WriteLine("\td - {0}", languedelete);
            Console.WriteLine("\tr - {0}", langueread);
            Console.WriteLine("\ts - {0}", languesave);
            Console.WriteLine("\tp - {0}", languemulti);
            Console.WriteLine("\tq - {0}", languequit);
            Console.Write("{0}", langueoption);
        }

        //déclarer la fin du programme
        public void fin()
        {
            Console.Write("\n Press any key to close the Calculator console app...");
            Console.ReadKey();
        }

        //vérifier si le json existe
        public string VerifyFile(string JsonFileIn)
        {
        BEGIN:
            if (File.Exists(JsonFileIn))
            {
                return JsonFileIn;
            }
            else
            {
                Console.Write("\n{0}", langueErreur);
                goto BEGIN;
            }
        }
    }

    //Classe pour gérer le json de nombre de save
    class JsonService
    {
        public static int Nb;
        public void NombreSave(string JsonFileIn)
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(JsonFileIn));

            Nb = jsonFile["NBSave"];
            Nb++;
            data_NBSave data = new data_NBSave();
            data.NBSave = Nb;
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(JsonFileIn, json);
        }
        public void DeleteNombreSave(string JsonFileIn)
        {
            dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(JsonFileIn));

            Nb = jsonFile["NBSave"];
            Nb--;
            data_NBSave data = new data_NBSave();
            data.NBSave = Nb;
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(JsonFileIn, json);
        }
    }

}
