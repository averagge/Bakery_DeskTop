using System.Windows;
using System.Windows.Controls;


namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для Enter.xaml
    /// </summary>
    public partial class Enter : Window
    {
        private bakeryEntities1 dbcontext;

        public Enter()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (dbcontext = new bakeryEntities1())
            {
                foreach (user user in dbcontext.users)
                {
                    if (login.Text == user.login && Hash.GetHashString(password.Text) == user.password)
                    {
                        if(user.role == "staff")
                        {
                            Seans.log = user.login;
                            Kassa kassa = new Kassa();
                            kassa.Show();
                            this.Close();
                        }
                        if(user.role == "admin")
                        {
                            var mainwindow = new MainWindow();
                            mainwindow.Show();
                            this.Close();
                        }
                        return;
                    }
                }
                MessageBox.Show("Логин или пароль указан неверно!");

            }
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (login.Text.Contains(" "))
            {
                login.Text = login.Text.Replace(" ", "");
                login.SelectionStart = login.Text.Length;
            }
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (password.Text.Contains(" "))
            {
                password.Text = password.Text.Replace(" ", "");
                password.SelectionStart = password.Text.Length;
            }
        }
    }
}
