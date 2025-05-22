using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for SqlWindow.xaml
    /// </summary>
    public partial class SqlWindow : Window
    {
        public SqlWindow()
        {
            InitializeComponent();
        }
        private void MnuSql_Click(object sender, RoutedEventArgs e)
        {
            var window = new SqlWindow();
            window.ShowDialog();
        }
        private SqlConnection GetConnection()
        {
            SqlConnection conn;
            try
            {
                //string connectionString = "Trusted_Connection=True;";
                string connectionString = $@"Server={TxtServer.Text};";
                connectionString += $"Database={TxtDb.Text}";
                connectionString += "user id = sa;";
                connectionString += "Password = pxl;";
                var conn1 = "Server=PC5G;Database=Test;User id=sa;Password=pxl";
                conn = new SqlConnection(conn1);
                conn.Open();
                conn.Close();
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string connectionString = "Trusted_Connection=True;";
                string connectionString = $@"Server={TxtServer.Text};";
                connectionString += $"Database={TxtDb.Text}";
                connectionString += "user id = sa;";
                connectionString += "Password = pxl;";

                var conn1 = "Server=PC5G;Database=Test;User id=sa;Password=pxl";

                SqlConnection conn = new SqlConnection(conn1);
                conn.Open();
                conn.Close();
                MessageBox.Show("connection ok!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnGetData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmd = $"Select * from {TxtTable.Text}";
                SqlCommand sql = new SqlCommand(cmd, GetConnection());
                SqlDataAdapter da = new SqlDataAdapter(sql);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DgdSql.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string commandText = $"Select * from {TxtTable.Text}";
                SqlCommand sql = new SqlCommand(commandText, GetConnection());
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                DgdSql.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
