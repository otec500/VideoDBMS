using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace VideoDBMS
{
    /// <summary>
    /// Interaction logic for formGenres.xaml
    /// </summary>
    public partial class formGenres : Window
    {
        public formGenres()
        {
            InitializeComponent();
            Refresh();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string mGenre;
            mGenre = txtGenre.Text;

            if (mGenre.Trim() != "")
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT Genres(GenreName) VALUES (@genrename);", conn);
                    cmd.Parameters.Add(new SqlParameter("genrename", mGenre));
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Dodano nowy gatunek.");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                }
                Refresh();
            }
        }

        private void Refresh()
        {
            SqlDataReader dr;
            List<string> dataList = new List<string>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Genres;", conn);
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dataList.Add(dr.GetString(1));
                        }
                       
                    }
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            lsvGenres.ItemsSource = dataList;
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string mGenre;
            if(lsvGenres.SelectedItem == null)
            {
                return;
            }
            mGenre = lsvGenres.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Genres WHERE GenreName=@genrename;", conn);
                cmd.Parameters.Add(new SqlParameter("genrename", mGenre));
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usunięto gatunek.");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                conn.Close();
            }
            Refresh();
        }
    }
}
