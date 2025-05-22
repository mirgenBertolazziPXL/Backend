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
using System.Windows.Shapes;

namespace WpfAppTeam13
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }
        private void SqlMenu_Click(object sender, RoutedEventArgs e)
        {
            var window = new SqlWindow();
            window.ShowDialog();
        }

        private void StudentMenu_Click(object sender, RoutedEventArgs e)
        {
            var window = new StudentWindow();
            window.ShowDialog();
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
