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
    /// Interaction logic for formAdminPanel.xaml
    /// </summary>
    public partial class formAdminPanel : Window
    {
        Admin _admin;

        public formAdminPanel()
        {
            InitializeComponent();
            RefreshStats();
        }
        public formAdminPanel(User xUser)
        {
            InitializeComponent();
            _admin = new Admin(xUser);
            lblName.Content = _admin.Name;
            RefreshStats();
        }

        private void RefreshStats()
        {
            SqlDataReader dr;
            int UsersNum = 0;
            int FilmsNum = 0;
            int RentsNum = 0;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("select (select count(*) from Users) as count1, (select count(*) from Films) as count2, (select count(*) from Rents) as count3;", conn);
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UsersNum = dr.GetInt32(0);
                            FilmsNum = dr.GetInt32(1);
                            RentsNum = dr.GetInt32(2);
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
            lblUsersNum.Content = UsersNum.ToString();
            lblFilmsNum.Content = FilmsNum.ToString();
            lblRentsNum.Content = RentsNum.ToString();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGenresEdit_Click(object sender, RoutedEventArgs e)
        {
            formGenres form = new formGenres();
            form.Show();
        }

        private void btnFilms_Click(object sender, RoutedEventArgs e)
        {
            formAddFilm form = new formAddFilm();
            form.Show();
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            formAddRent form = new formAddRent();
            form.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new MainWindow().Show();
        }

        private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            User mUser;
            SqlDataReader dr;

            formSignup window = new formSignup();
            window.ShowDialog();
            mUser = window.CurrentUser;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                conn.Open();

                SqlCommand cmdIfUserExist = new SqlCommand("SELECT UserId FROM Users WHERE name=@username;", conn);
                cmdIfUserExist.Parameters.Add(new SqlParameter("username", mUser.Name));
                try
                {
                    dr = cmdIfUserExist.ExecuteReader();
                    if (dr.HasRows)
                    {
                        MessageBox.Show("Nazwa użytkownika jest już zajęta.");
                        conn.Close();
                        return;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cmdIfUserExist.Connection.Close();
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT Users(Name, Password, BirthDate, StatusId) VALUES (@username, @userpassword, @date, @status);", conn);
                cmd.Parameters.Add(new SqlParameter("username", mUser.Name));
                cmd.Parameters.Add(new SqlParameter("userpassword", mUser.Password));
                cmd.Parameters.Add(new SqlParameter("date", mUser.BirthDate.Date));
                cmd.Parameters.Add(new SqlParameter("status", 1));

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Dodano nowego użytkownika");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                conn.Close();
            }
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            formChangePassword form = new formChangePassword(_admin);
            form.Show();
        }
    }
}
