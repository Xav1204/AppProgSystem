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
            valeur_nom = Nom.Text;
            model.Save(valeur_nom);

            Nom.Text = "";
        }

        private void SequentialSave_Button_Click(object sender, RoutedEventArgs e)
        {
            model.SequentialSave();

            Nom.Text = "";
        }

        private void Chiffrer_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Encrypt();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
            
        }
    }
}
