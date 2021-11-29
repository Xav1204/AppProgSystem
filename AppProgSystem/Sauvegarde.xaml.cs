using System;
using System.Windows;


namespace AppProgSystem
{
    /// <summary>
    /// Logique d'interaction pour Sauvegarde.xaml
    /// </summary>
    public partial class Sauvegarde : Window
    {
        Model model = new Model();

        public Sauvegarde()
        {
            InitializeComponent();
            Model.set = DataRead;
        }
        private void Read_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Read();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Modify_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SequentialSave_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Chiffrer_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
