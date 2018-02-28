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
using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace PreTzeL
{
    /// <summary>
    /// Interaction logic for DbConfigDetails.xaml
    /// </summary>
    public partial class DbConfigDetails : Window
    {
        public DbConfigDetails()
        {
            InitializeComponent();
        }
        public static void Show1(string caption)
        { 
            var msgWindow1 = new DbConfigDetails()
            {
                Title = caption,
                Owner = Application.Current.MainWindow
            };

            msgWindow1.ShowDialog();
        }

        private void textBox_username_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void db_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void textBox_PortNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void textBox_DataSource_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username;
            string password;
            string datasource;
            string db_name;
            string portno;
            if (Class_GlobalVars.database_type == "My SQL")
            {
                MySqlConnectionStringBuilder obj = new MySqlConnectionStringBuilder();
                obj.UserID = textBox_username.Text;
                obj.Password = textBox_Password.Password;
                obj.Server = textBox_DataSource.Text;
                obj.Database = textBox_DBName.Text;
                obj.Port = Convert.ToUInt32(textBox_PortNo.Text);
                //string connStr = "datasource = "+datasource+"; port = "+portno+"; username = "+username+"; password = "+password+";";

                using (MySqlConnection connection = new MySqlConnection(obj.ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        throw new Exception("Connection successfull");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        if (ex.Message == "Connection successfull")
                        {
                            Class_GlobalVars.Client_Conn_string = obj.ConnectionString;
                            this.Close();
                        }
                        return;
                    }

                }
            }

            else if (Class_GlobalVars.database_type == "SQL")
            {
                SqlConnectionStringBuilder obj = new SqlConnectionStringBuilder();
                obj.UserID = textBox_username.Text;
                obj.Password = textBox_Password.Password;
                obj.DataSource = textBox_DataSource.Text;
                obj.InitialCatalog = textBox_DBName.Text;
                obj.IntegratedSecurity = true; /*Integrated Security = SSPI;*/
                using (SqlConnection connection = new SqlConnection(obj.ConnectionString))
                {
                    try
                    {
                        MessageBox.Show(obj.ConnectionString);
                        connection.Open();
                        throw new Exception("Connection successfull");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        if (ex.Message == "Connection successfull")
                        {
                            Class_GlobalVars.Client_Conn_string = obj.ConnectionString;
                            this.Close();
                        }
                        return;
                    }

                }
            }

            else if(Class_GlobalVars.database_type == "Oracle")
            {
                OracleConnectionStringBuilder obj = new OracleConnectionStringBuilder();
                obj.UserID = textBox_username.Text;
                obj.Password = textBox_Password.Password;
                obj.DataSource = textBox_DataSource.Text;
                obj.IntegratedSecurity = true;
                MessageBox.Show(obj.ConnectionString);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
