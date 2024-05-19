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

namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для Emploer.xaml
    /// </summary>
    public partial class Emploer : Window
    {
        private Emploers emploers;
        public Emploer()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var signup = new EmploerSignup();
            signup.Show();
            this.Close();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            emploers = (Emploers)stafflist.SelectedItem;
        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            using (bakeryEntities1 dbcontext = new bakeryEntities1())
            {
                foreach (staff staff in dbcontext.staffs)
                {
                    Emploers emploers = new Emploers();
                    foreach (user user in dbcontext.users)
                    {
                        if(user.login==staff.login && user.role=="staff")
                        {
                            emploers.login.Text = user.login;
                            emploers.name.Text = staff.name;
                            emploers.surname.Text = staff.surname;
                            emploers.phone.Text = user.phone;
                            emploers.email.Text = user.email;
                            stafflist.Items.Add(emploers);

                        }
                    }
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (bakeryEntities1 db = new bakeryEntities1())
                {
                    if (stafflist.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите сотрудника");
                        return;
                    }
                    foreach (staff staff in db.staffs)
                    {
                        if (staff.login == emploers.login.Text)
                        {
                            db.staffs.Remove(staff);
                        }
                    }
                    foreach (user user in db.users)
                    {
                        if (user.login == emploers.login.Text)
                        {
                            db.users.Remove(user);
                        }
                    }
                    db.SaveChanges();
                    var emploer = new Emploer();
                    emploer.Show();
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
