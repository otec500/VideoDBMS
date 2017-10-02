using System;
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
    /// Interaction logic for formAddFilm.xaml
    /// </summary>
    public partial class formAddFilm : Window
    {
        public formAddFilm()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            SqlDataReader dr;
            Film mFilm;
            List<Film> filmList = new List<Film>();
            List<string> genreList = new List<string>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT f.Title, f.Director, f.Year, g.GenreName FROM Films f LEFT JOIN Genres g ON f.GenreId=g.GenreId;", conn);
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            mFilm = new Film();
                            mFilm.Title = dr.GetString(0);
                            mFilm.Director = dr.GetString(1);
                            mFilm.Year = dr.GetDateTime(2);
                            mFilm.Genre = dr.GetString(3);
                            filmList.Add(mFilm);
                        }
                        
                    }
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cmd.Connection.Close();
                conn.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT GenreName FROM Genres", conn);
                try
                {
                    dr = cmd2.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            genreList.Add(dr.GetString(0));
                        }
                    }
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            cmbGenre.ItemsSource = genreList;
            lsvFilms.ItemsSource = filmList;
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string mTitle, mDirector, mGenre;
            int mGenreId;
            DateTime mYear;
            SqlDataReader dr;

            mTitle = txtTitle.Text;
            mDirector = txtDirector.Text;
            mYear = (DateTime)datYear.SelectedDate;
            mGenre = (string)cmbGenre.SelectedItem;
            mGenreId = 0;

            if (mTitle.Trim() != "")
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT GenreId FROM Genres WHERE GenreName=@genrename", conn);
                    cmd1.Parameters.Add(new SqlParameter("genrename", mGenre));
                    try
                    {
                        dr = cmd1.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            mGenreId = dr.GetInt32(0);
                        }
                    }
                    catch(SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    cmd1.Connection.Close();
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT Films(Title, Director, Year, GenreId) VALUES (@title, @director, @year, @genreid);", conn);
                    cmd.Parameters.Add(new SqlParameter("title", mTitle));
                    cmd.Parameters.Add(new SqlParameter("director", mDirector));
                    cmd.Parameters.Add(new SqlParameter("year", mYear.Date));
                    cmd.Parameters.Add(new SqlParameter("genreid", mGenreId));
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Dodano nowy film.");
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Film mFilm;
            if (lsvFilms.SelectedItem == null)
            {
                return;
            }
            mFilm = (Film)lsvFilms.SelectedItem;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Films WHERE Title=@title AND Director=@director;", conn);
                cmd.Parameters.Add(new SqlParameter("title", mFilm.Title));
                cmd.Parameters.Add(new SqlParameter("director", mFilm.Director));
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Usunięto film.");
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
