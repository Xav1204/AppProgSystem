using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;


namespace AppProgSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application_Startup();
        }

        private void Application_Startup()
        {
            Process proc = Process.GetCurrentProcess();
            //check other process with same name
            int count = Process.GetProcesses().Where(p =>
                p.ProcessName == proc.ProcessName).Count();

            if (count > 1)
            {
                MessageBox.Show("Already an instance is running...");
                //shutdown new instance
                App.Current.Shutdown();
            }
        }

        public static string choix = "";
        private void Valider_Button_Click(object sender, RoutedEventArgs e)
        {
            Sauvegarde save = new Sauvegarde();
            Model model = new Model();

            if (Anglais.IsChecked == true)
            {
                save.Show();
                choix = "EN";
                save.langue();
                this.Close();
            }
            else if(Francais.IsChecked == true)
            {
                save.Show();
                choix = "FR";
                save.langue();
                this.Close();
            }
            else
            {
                MessageBox.Show("Choisissez votre langue / Choose your language !!!");
            }
            
        }
        private void Quitter_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
