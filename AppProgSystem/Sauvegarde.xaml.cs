using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace AppProgSystem
{
    /// <summary>
    /// Logique d'interaction pour Sauvegarde.xaml
    /// </summary>
    public partial class Sauvegarde : Window
    {
        private string valeur_nom;
        private string valeur_source;
        private string valeur_cible;
        private string valeur_type;

        Model model = new Model();

        public Sauvegarde()
        {
            InitializeComponent();
            Model.set = DataRead;
            Model.txt_nom = Nom;
            Model.txt_source = Source;
            Model.txt_cible = Cible;
            Model.txt_type = Type;
            Model.txt_extension = Extension;
            Model.extent = journalier;
        }

        public delegate String del_JSON(string path, string search);

        public void freeze()
        {
            Console.WriteLine(CheckProcess());
            if (CheckProcess())
            {
                HandlingProcess();
            }
        }
        public static bool CheckProcess()
        {
            return System.Diagnostics.Process.GetProcessesByName("notepad").Length != 0;
        }
        public static void HandlingProcess()
        {
            Process[] allProcessus = Process.GetProcesses();
            Model process = new Model();
            //Check if notepad process is already running.
            foreach (Process unProcessus in allProcessus)
            {
                if (unProcessus.ProcessName == "notepad")
                {
                    process.pascontent();
                    unProcessus.WaitForExit();
                }
            }
            process.content();
        }

        public void langue()
        {
            string pathLangues = "C:\\EasySave\\Langues\\Langues.json";
            string search = MainWindow.choix + ".Save.Name";
            string search1 = MainWindow.choix + ".Save.Source";
            string search2 = MainWindow.choix + ".Save.Target";
            string search3 = MainWindow.choix + ".Save.Type";
            string search4 = MainWindow.choix + ".Interface.Back";
            string search5 = MainWindow.choix + ".Interface.Add";
            string search6 = MainWindow.choix + ".Interface.Modify";
            string search7 = MainWindow.choix + ".Interface.Delete";
            string search8 = MainWindow.choix + ".Interface.Read";
            string search9 = MainWindow.choix + ".Interface.Execute";
            string search10 = MainWindow.choix + ".Interface.Sequential";
            string search11 = MainWindow.choix + ".Interface.Encrypt";
            // to make one time at start of code to declare method and delegate
            var js = new Model();
            del_JSON del_js = new del_JSON(js.ExeJS);
            // invoke del_js, output : string
            lbl_nom.Content = del_js.Invoke(pathLangues, search);
            lbl_source.Content = del_js.Invoke(pathLangues, search1);
            lbl_cible.Content = del_js.Invoke(pathLangues, search2);
            lbl_type.Content = del_js.Invoke(pathLangues, search3);
            btn_back.Content = del_js.Invoke(pathLangues, search4);
            btn_create.Content = del_js.Invoke(pathLangues, search5);
            btn_modify.Content = del_js.Invoke(pathLangues, search6);
            btn_delete.Content = del_js.Invoke(pathLangues, search7);
            btn_read.Content = del_js.Invoke(pathLangues, search8);
            btn_save.Content = del_js.Invoke(pathLangues, search9);
            btn_ssave.Content = del_js.Invoke(pathLangues, search10);
        }


        private void Read_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Read();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            valeur_nom = Nom.Text;
            valeur_source = Source.Text;
            valeur_cible = Cible.Text;
            valeur_type = Type.Text;

            model.Create(valeur_nom, valeur_source, valeur_cible, valeur_type);

            Nom.Text = "";
            Source.Text = "";
            Cible.Text = "";
            Type.Text = "";
        }

        private void Modify_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Modify();
            Nom.Text = "";
            Source.Text = "";
            Cible.Text = "";
            Type.Text = "";
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Delete();
            Nom.Text = "";
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            freeze();
            valeur_nom = Nom.Text;
            model.Save(valeur_nom);

            Nom.Text = "";
            journalier.Text = "";
        }

        private void SequentialSave_Button_Click(object sender, RoutedEventArgs e)
        {
            freeze();
            model.SequentialSave();

            Nom.Text = "";
            journalier.Text = "";
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
            
        }
    }
}
