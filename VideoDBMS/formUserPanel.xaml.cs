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
    /// Interaction logic for formUserPanel.xaml
    /// </summary>
    public partial class formUserPanel : Window
    {
        User _user;

        public formUserPanel()
        {
            InitializeComponent();
        }
        public formUserPanel(User xUser)
        {
            InitializeComponent();
            _user = new User(xUser);
            lblName.Content = _user.Name;
            lblBirthDate.Content = _user.BirthDate.Date;
            RefreshOrders();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            formAddOrder form = new formAddOrder(_user.Id);
            form.OrderAdded += On_formAddOrderAdded;
            form.Show();
           
        }

        private void On_formAddOrderAdded(object sender, EventArgs e)
        {
            RefreshOrders();
        }

        private void RefreshOrders()
        {
            SqlDataReader dr;
            Film mFilm;
            List<Film> filmList = new List<Film>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT f.Title, f.Director, f.Year FROM Orders o LEFT JOIN Films f ON o.FilmId=f.FilmId WHERE o.UserId=@userid;", conn);
                cmd.Parameters.Add(new SqlParameter("userid",_user.Id ));
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
            }
            lsvOrders.ItemsSource = filmList;
        }
    }
}
