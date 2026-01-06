using DemoAttempt3.Data;
using System.Windows;

namespace DemoAttempt3
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PwdBox.Password;
            PwdBox.Password = "";

            Models.User user = DbHelper.Authorize(login, password);
            
            login = null;
            password = null;

            MainWindow wnd = new MainWindow(user);
            wnd.Closed += (s, args) => this.Show();
            this.Hide();
            wnd.Show();
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wnd = new MainWindow(null);
            wnd.Closed += (s, args) => this.Show();
            this.Hide();
            wnd.Show();
        }
    }
}
