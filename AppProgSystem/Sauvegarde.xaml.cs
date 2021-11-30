﻿using System;
using System.Windows;


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
        }

        private void Modify_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Modify();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            model.Delete();
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            valeur_nom = Nom.Text;
            model.Save(valeur_nom);
        }

        private void SequentialSave_Button_Click(object sender, RoutedEventArgs e)
        {
            model.SequentialSave();
        }

        private void Chiffrer_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow XavierGrosSexe = new MainWindow();
            XavierGrosSexe.Show();
            this.Close();
            
        }
    }
}
