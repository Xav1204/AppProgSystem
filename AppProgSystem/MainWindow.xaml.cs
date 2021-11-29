using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
