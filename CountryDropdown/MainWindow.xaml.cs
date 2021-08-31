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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace CountryDropdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        public MainWindow()
        {
            InitializeComponent();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "select * from continent";
                command = new SqlCommand(sql, connection);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                reader = command.ExecuteReader();
                string continent;
                while (reader.Read())
                {
                    continent = reader["continentname"].ToString();
                    cmbContinent.Items.Add(continent);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void cmbContinent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbCountry.Items.Clear();
            if (cmbContinent.SelectedItem != null)
            {
                string selectedText = cmbContinent.SelectedItem.ToString();

                try
                {
                    string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
                    connection = new SqlConnection(connstr);
                    string sql = "select * from country where continentid in (select continentid from continent where continentname = @text)";
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("text", selectedText);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    reader = command.ExecuteReader();
                    string country;
                    while (reader.Read())
                    {
                        country = reader["countryname"].ToString();
                        cmbCountry.Items.Add(country);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

            }
        }                
        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbState.Items.Clear();
            if (cmbCountry.SelectedItem != null)
            {
                string selectedText = cmbCountry.SelectedItem.ToString();
                try
                {
                    string sql = "select * from State where countryid in (select countryid from country where countryname = @text)";
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("text", selectedText);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    reader = command.ExecuteReader();
                    string state;
                    while (reader.Read())
                    {
                        state = reader["statename"].ToString();
                        cmbState.Items.Add(state);
                    }
                    connection.Close();
                    sql = "select * from Country where countryname = @text";
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("text", selectedText);
                    connection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lblCapital.Content = reader["capital"].ToString();
                        Stream stream = new MemoryStream((byte[])reader["flag"]);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = stream;
                        bi.EndInit();
                        Img1.Source = bi;
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblState.Content = string.Empty;
            if (cmbState.SelectedItem != null)
            {
                string selectedText = cmbState.SelectedItem.ToString();
                try
                {
                    string sql = "select * from State where statename = @text";
                    command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("text", selectedText);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    reader = command.ExecuteReader();
                    string state;
                    while (reader.Read())
                    {
                        state = reader["statecapital"].ToString();
                        lblState.Content = state;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
    
