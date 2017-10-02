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
        }
        public formAdminPanel(User xUser)
        {
            InitializeComponent();
            _admin = new Admin(xUser);
            lblName.Content = _admin.Name;
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

        }
    }
}
