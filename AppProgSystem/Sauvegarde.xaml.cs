using System;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.ComponentModel;

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
        private string valeur_extension;
        private string valeur_priorite;
        private string valeur_log;

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
            Model.txt_priorite = Priorite;
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

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //initialisation de la barre de progression avec le pourcentage de progression
            //progress.Value = e.ProgressPercentage;

            //Affichage de la progression sur un label
            //lb_etat_prog_server.Content = pbstatus1.Value.ToString() + "%";



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
            string search10 = MainWindow.choix + ".Save.Log";
            string search12 = MainWindow.choix + ".Save.Priorite";
            // to make one time at start of code to declare method and delegate
            var js = new Model();
            del_JSON del_js = new del_JSON(js.ExeJS);
            // invoke del_js, output : string
            lbl_nom.Content = del_js.Invoke(pathLangues, search);
            lbl_source.Content = del_js.Invoke(pathLangues, search1);
            lbl_cible.Content = del_js.Invoke(pathLangues, search2);
            lbl_type.Content = del_js.Invoke(pathLangues, search3);
            lbl_priorite.Content = del_js.Invoke(pathLangues, search12);
            lbl_log.Content = del_js.Invoke(pathLangues, search10);
            btn_back.Content = del_js.Invoke(pathLangues, search4);
            btn_create.Content = del_js.Invoke(pathLangues, search5);
            btn_modify.Content = del_js.Invoke(pathLangues, search6);
            btn_delete.Content = del_js.Invoke(pathLangues, search7);
            btn_read.Content = del_js.Invoke(pathLangues, search8);

        }

        private void Click_Data_Play(object sender, RoutedEventArgs e)
        {
            model.Play();
        }
        private void Click_Data_Pause(object sender, RoutedEventArgs e)
        {
            model.Pause();
        }
        private void Click_Data_Stop(object sender, RoutedEventArgs e)
        {

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
            valeur_extension = Extension.Text;
            valeur_priorite = Priorite.Text;
            valeur_log = journalier.Text;

            model.Create(valeur_nom, valeur_source, valeur_cible, valeur_type, valeur_extension, valeur_priorite, valeur_log);

            Nom.Text = "";
            Source.Text = "";
            Cible.Text = "";
            Type.Text = "";
            Extension.Text = "";
            Priorite.Text = "";
            journalier.Text = "";
        }

        private void Modify_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Modify();
            Nom.Text = "";
            Source.Text = "";
            Cible.Text = "";
            Type.Text = "";
            Extension.Text = "";
            Priorite.Text = "";
            journalier.Text = "";
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Delete();
            Nom.Text = "";
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
            
        }
    }
}
