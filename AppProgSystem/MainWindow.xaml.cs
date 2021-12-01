using System;
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
