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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoDBMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
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

                SqlCommand cmdIfUserExist = new SqlCommand("SELECT UserId FROM Users WHERE name=@username;",conn);
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
                catch(SqlException ex)
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            User mUser;
            mUser = Login();

            if (mUser.Status == 2)
            {
                formUserPanel userPanel = new formUserPanel(mUser);
                userPanel.Show();
                this.Close();
            }
            else if(mUser.Status == 1)
            {
                formAdminPanel adminPanel = new formAdminPanel(mUser);
                adminPanel.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Logowanie nie powiodło się.");
            }
        }

        private User Login()
        {
            string mName, mPassword;
            SqlDataReader dr;
            User mUser;

            mName = txtName.Text;
            mPassword = txtPassword.Text;

            mUser = new User();
            mUser.Id = -1;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE name=@username AND password=@userpassword;", conn);
                cmd.Parameters.Add(new SqlParameter("username", mName));
                cmd.Parameters.Add(new SqlParameter("userpassword", mPassword));
      
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        mUser.Id = dr.GetInt32(0);
                        mUser.Name = dr.GetString(1);
                        mUser.Password = dr.GetString(2);
                        mUser.BirthDate = dr.GetDateTime(3);
                        mUser.Status = dr.GetInt32(4);
                    }
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                } 
            }
            return mUser;
        }
    }
}
