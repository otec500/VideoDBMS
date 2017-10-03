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
    /// Interaction logic for formSignup.xaml
    /// </summary>
    public partial class formSignup : Window
    {
        public User CurrentUser;

        public formSignup()
        {
            InitializeComponent();
            CurrentUser = new User();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentUser.Name = txtUsername.Text;
            this.CurrentUser.Password = txtPassword.Text;
            if(datBirthdate.SelectedDate == null)
            {
                this.CurrentUser.BirthDate = System.DateTime.Now;
            }
            else
            {
                this.CurrentUser.BirthDate = (System.DateTime)datBirthdate.SelectedDate;
            }
            this.CurrentUser.Status = 2;

            //string mName, mPassword;
            //DateTime mBirthDate;

            //mName = txtUsername.Text;
            //mPassword = txtPassword.Text;
            //mBirthDate = (System.DateTime)datBirthdate.SelectedDate;

            //using (SqlConnection conn = new SqlConnection())
            //{
            //    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("INSERT Users(Name, Password, BirthDate, StatusId) VALUES (@username, @userpassword, @date, @status);", conn);
            //    cmd.Parameters.Add(new SqlParameter("username", mName));
            //    cmd.Parameters.Add(new SqlParameter("userpassword", mPassword));
            //    cmd.Parameters.Add(new SqlParameter("date", mBirthDate.Date));
            //    cmd.Parameters.Add(new SqlParameter("status", 2));
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //        MessageBox.Show("Dodano nowego użytkownika");
            //    }
            //    catch(SqlException ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    conn.Close();
            //}
            //this.Close();
            this.Close();
        }
    }
}
