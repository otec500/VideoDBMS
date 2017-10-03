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
    /// Interaction logic for formChangePassword.xaml
    /// </summary>
    public partial class formChangePassword : Window
    {
        User _user;

        public formChangePassword(User xUser)
        {
            InitializeComponent();
            this._user = xUser;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNew.Text;
            string oldPassword = txtOld.Text;

            if(oldPassword != _user.Password)
            {
                MessageBox.Show("Wrong password. Confirm your old password.");
                return;
            }
            if (newPassword == "")
            {
                MessageBox.Show("Set new password.");
                return;
            }
 
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Users SET Password=@password WHERE UserId=@userid;", conn);
                cmd.Parameters.Add(new SqlParameter("password", newPassword));
                cmd.Parameters.Add(new SqlParameter("userid", _user.Id));
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password changed.");
                   
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cmd.Connection.Close();
                conn.Close();
            }
            this.Close();
        }
    }
}
