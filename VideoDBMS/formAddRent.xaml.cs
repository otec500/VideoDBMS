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
    /// Interaction logic for formAddRent.xaml
    /// </summary>
    public partial class formAddRent : Window
    {
        struct myOrder
        {
            public string Username { get; set; }
            public string Title { get; set; }
            public string Director { get; set; }
            public int UserId { get; set; }
            public int FilmId { get; set; }
        }

        struct myRent
        {
            public string Username { get; set; }
            public string Title { get; set; }
            public string Director { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int UserId { get; set; }
            public int FilmId { get; set; }
        }

        public formAddRent()
        {
            InitializeComponent();
            RefreshOrders();
            RefreshRents();
            lsvOrders.SelectionMode = SelectionMode.Single;
            lsvRents.SelectionMode = SelectionMode.Single;
        }

        private void RefreshOrders()
        {
            SqlDataReader dr;
            myOrder order;
            List<myOrder> filmList = new List<myOrder>();
            

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT u.Name, f.Title, f.Director, o.UserId, o.FilmId FROM Orders o JOIN Users u ON u.UserId=o.UserId JOIN Films f ON o.FilmId=f.FilmId;", conn);
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            order = new myOrder();
                            order.Username = dr.GetString(0);
                            order.Title = dr.GetString(1);
                            order.Director = dr.GetString(2);
                            order.UserId = dr.GetInt32(3);
                            order.FilmId = dr.GetInt32(4);
                            filmList.Add(order);
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

        private void RefreshOrders(string xUser, string xTitle, string xDirector)
        {
            SqlDataReader dr;
            myOrder order;
            List<myOrder> filmList = new List<myOrder>();


            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT u.Name, f.Title, f.Director, o.UserId, o.FilmId FROM Orders o JOIN Users u ON u.UserId=o.UserId JOIN Films f ON o.FilmId=f.FilmId WHERE u.Name LIKE @username AND f.Title LIKE @title AND f.Director LIKE @director;", conn);
                cmd.Parameters.Add(new SqlParameter("username", "%" + xUser.Trim() + "%"));
                cmd.Parameters.Add(new SqlParameter("title", "%" + xTitle.Trim() + "%"));
                cmd.Parameters.Add(new SqlParameter("director", "%" + xDirector.Trim() + "%"));
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            order = new myOrder();
                            order.Username = dr.GetString(0);
                            order.Title = dr.GetString(1);
                            order.Director = dr.GetString(2);
                            order.UserId = dr.GetInt32(3);
                            order.FilmId = dr.GetInt32(4);
                            filmList.Add(order);
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
    
        private void RefreshRents()
        {
            SqlDataReader dr;
            myRent rent;
            List<myRent> filmList = new List<myRent>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT u.Name, f.Title, f.Director, r.StartDate, r.EndDate, r.UserId, r.FilmId FROM Rents r JOIN Users u ON u.UserId=r.UserId JOIN Films f ON r.FilmId=f.FilmId;", conn);
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            rent = new myRent();
                            rent.Username = dr.GetString(0);
                            rent.Title = dr.GetString(1);
                            rent.Director = dr.GetString(2);
                            rent.StartDate = dr.GetDateTime(3);
                            rent.EndDate = dr.GetDateTime(4);
                            rent.UserId = dr.GetInt32(5);
                            rent.FilmId = dr.GetInt32(6);
                            filmList.Add(rent);
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
            lsvRents.ItemsSource = filmList;
        }

        private void RefreshRents(string xUser, string xTitle, string xDirector)
        {
            SqlDataReader dr;
            myRent rent;
            List<myRent> filmList = new List<myRent>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT u.Name, f.Title, f.Director, r.StartDate, r.EndDate, r.UserId, r.FilmId FROM Rents r JOIN Users u ON u.UserId=r.UserId JOIN Films f ON r.FilmId=f.FilmId WHERE u.Name LIKE @username AND f.Title LIKE @title AND f.Director LIKE @director;", conn);
                cmd.Parameters.Add(new SqlParameter("username", "%" + xUser.Trim() + "%"));
                cmd.Parameters.Add(new SqlParameter("title", "%" + xTitle.Trim() + "%"));
                cmd.Parameters.Add(new SqlParameter("director", "%" + xDirector.Trim() + "%"));
                try
                {
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            rent = new myRent();
                            rent.Username = dr.GetString(0);
                            rent.Title = dr.GetString(1);
                            rent.Director = dr.GetString(2);
                            rent.StartDate = dr.GetDateTime(3);
                            rent.EndDate = dr.GetDateTime(4);
                            rent.UserId = dr.GetInt32(5);
                            rent.FilmId = dr.GetInt32(6);
                            filmList.Add(rent);
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
            lsvRents.ItemsSource = filmList;
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            myOrder order;
            SqlDataReader dr;

            if (lsvOrders.SelectedItems.Count > 0)
            {
                order = (myOrder)(lsvOrders.SelectedItem);
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand("SELECT r.FilmId FROM Rents r WHERE r.FilmId=@filmid;", conn);
                    cmd2.Parameters.Add(new SqlParameter("filmid", order.FilmId));
                    try
                    {
                        dr = cmd2.ExecuteReader();
                        if (dr.HasRows)
                        {
                            MessageBox.Show("Film already rented.");
                            conn.Close();
                            return;

                        }
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    cmd2.Connection.Close();



                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT Rents(UserId, FilmId, StartDate, EndDate) VALUES (@userid, @filmid, @startdate, @enddate);", conn);
                    cmd.Parameters.Add(new SqlParameter("userid", order.UserId));
                    cmd.Parameters.Add(new SqlParameter("filmid", order.FilmId));
                    cmd.Parameters.Add(new SqlParameter("startdate", System.DateTime.Now));
                    cmd.Parameters.Add(new SqlParameter("enddate", System.DateTime.Now.AddDays(30)));


                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Dodano nowe wypozyczenie");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    conn.Close();

                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand("DELETE FROM Orders WHERE UserId=@userid AND FilmId=@filmid;", conn);
                    cmd1.Parameters.Add(new SqlParameter("userid", order.UserId));
                    cmd1.Parameters.Add(new SqlParameter("filmid", order.FilmId));

                    try
                    {
                        cmd1.ExecuteNonQuery();  
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                }
                RefreshRents();
                RefreshOrders();
            }
        }

        private void btnDelOrder_Click(object sender, RoutedEventArgs e)
        {
            myOrder order;

            
            if (lsvOrders.SelectedItems.Count > 0)
            {
                order = (myOrder)(lsvOrders.SelectedItem);
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("DELETE FROM Orders WHERE UserId=@userid AND FilmId=@filmid;", conn);
                    cmd1.Parameters.Add(new SqlParameter("userid", order.UserId));
                    cmd1.Parameters.Add(new SqlParameter("filmid", order.FilmId));

                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                }
                RefreshOrders();
            }

        }

        private void btnDelRent_Click(object sender, RoutedEventArgs e)
        {
            myRent rent;

            
            if (lsvRents.SelectedItems.Count > 0)
            {
                rent = (myRent)(lsvRents.SelectedItem);
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Data Source = MICHAŁ-KOMPUTER\SQLEXPRESS; Initial Catalog = VideoRental; Integrated Security = True;";// Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("DELETE FROM Rents WHERE UserId=@userid AND FilmId=@filmid;", conn);
                    cmd1.Parameters.Add(new SqlParameter("userid", rent.UserId));
                    cmd1.Parameters.Add(new SqlParameter("filmid", rent.FilmId));

                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                }
                RefreshRents();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string Username, Director, Title;

            Username = txtUser.Text;
            Director = txtDirector.Text;
            Title = txtTitle.Text;

            RefreshOrders(Username, Title, Director);
            RefreshRents(Username, Title, Director);
        }
    }
}
