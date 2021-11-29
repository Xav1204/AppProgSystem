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

        private void Valider_Button_Click(object sender, RoutedEventArgs e)
        {
            Sauvegarde save = new Sauvegarde();

            if (Anglais.IsChecked == true)
            {
                save.Show();
                this.Close();
            }
            else if(Francais.IsChecked == true)
            {
                save.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Choisissez votre langue / Choose your language !!!");
            }
            
        }
    }
}
