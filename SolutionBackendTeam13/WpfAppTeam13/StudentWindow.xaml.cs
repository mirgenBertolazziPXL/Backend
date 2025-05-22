using ClassLib13.Business;
using ClassLib13.Data.Framework;
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
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DgdStudent.ItemsSource = Students.List();
            LoadData();
        }
        private void LoadData()
        {
            DgdStudent.ItemsSource = null;
            DgdStudent.ItemsSource = Students.List();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            StackNewStudent.Visibility = Visibility.Hidden;
        }
        private void MnuNewStudent_Click(object sender, RoutedEventArgs e)
        {
            StackNewStudent.Visibility = Visibility.Visible;
        }
        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (TxtFirstName.Text.Length > 0 && TxtLastName.Text.Length > 0)
            {
                Students.Add(TxtFirstName.Text, TxtLastName.Text);
                LoadData();
                StackNewStudent.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Incorrect data!");
            }
        }
        private void BtnGetFrameworkData_Click(object sender, RoutedEventArgs e)
        {
            SelectResult result = Students.GetStudents();
            if (result.Succeeded)
            {
                DgdStudent.ItemsSource = null;
                DgdStudent.ItemsSource = result.DataTable.DefaultView;
            }
        }
        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            InsertResult result = Students.Add(TxtFirstName.Text, TxtLastName.Text);
        }


    }
}
