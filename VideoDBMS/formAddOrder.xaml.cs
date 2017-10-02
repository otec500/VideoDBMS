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
    /// Interaction logic for formAddOrder.xaml
    /// </summary>
    public partial class formAddOrder : Window
    {
        int userId;
        public event EventHandler OrderAdded;

        public formAddOrder(int xUserId)
        {
            InitializeComponent();
            userId = xUserId;
            Refresh("", "", "", "");
            lsvFilms.SelectionMode = SelectionMode.Single;
        }

        private void Refresh(string xTitle, string xDirector, string xYear, string xGenre)
        {
            SqlDataReader dr;
            Film mFilm;
            List<Film> filmList = new List<Film>();
            List<string> genreList = new List<string>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT f.Title, f.Director, f.Year, g.GenreName FROM Films f LEFT JOIN Genres g ON f.GenreId=g.GenreId WHERE f.Title LIKE @title AND f.Director LIKE @director AND DATEPART(YEAR, f.Year) LIKE @year AND g.GenreName LIKE @genrename;", conn);
                cmd.Parameters.Add(new SqlParameter("title", "%"+xTitle.Trim()+"%"));
                cmd.Parameters.Add(new SqlParameter("director", "%"+xDirector.Trim()+"%"));
                cmd.Parameters.Add(new SqlParameter("year", "%"+xYear.Trim()+"%"));
                cmd.Parameters.Add(new SqlParameter("genrename", "%"+xGenre.Trim()+"%"));
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
            }
            lsvFilms.ItemsSource = filmList;
        }

        private void OnOrderAdded()
        {
            EventHandler handler = OrderAdded;
            if(handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string mTitle, mDirector, mYear, mGenre;

            mTitle = txtTitle.Text;
            mDirector = txtDirector.Text;
            mYear = txtYear.Text;
            mGenre = txtGenre.Text;

            Refresh(mTitle, mDirector, mYear, mGenre);
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            int mFilmId = 0;
            Film mFilm;
            SqlDataReader dr;

            mFilm = (Film)(lsvFilms.SelectedItem);
            
            if (lsvFilms.SelectedItems.Count >=0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT FilmId FROM Films WHERE Title=@title AND Director=@director AND Year=@year", conn);
                    cmd1.Parameters.Add(new SqlParameter("title", mFilm.Title));
                    cmd1.Parameters.Add(new SqlParameter("director", mFilm.Director));
                    cmd1.Parameters.Add(new SqlParameter("year", mFilm.Year));
                    try
                    {
                        dr = cmd1.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            mFilmId = dr.GetInt32(0);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    cmd1.Connection.Close();
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT Orders(UserId, FilmId) VALUES (@userid, @filmid);", conn);
                    cmd.Parameters.Add(new SqlParameter("userid", userId));
                    cmd.Parameters.Add(new SqlParameter("filmid", mFilmId));
         
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Dodano nowe zamowienie");
                        OnOrderAdded();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                }
                Refresh("", "", "", "");
            }
        }
    }
}
