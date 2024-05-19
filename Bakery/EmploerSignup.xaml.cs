using System;

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;



namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для EmploerSignup.xaml
    /// </summary>
    public partial class EmploerSignup : Window
    {
        private bakeryEntities1 _dbcontext;
        private string pattern = "89018765432";
        private string pattern1 = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public EmploerSignup()
        {
            InitializeComponent();
        }
        bool IsPhoneNumber(string input, string pattern)
        {
            if (input.Length != pattern.Length) return false;

            for (int i = 0; i < input.Length; ++i)
            {
                bool ith_character_ok;
                if (Char.IsDigit(pattern, i))
                    ith_character_ok = Char.IsDigit(input, i);
                else
                    ith_character_ok = (input[i] == pattern[i]);

                if (!ith_character_ok) return false;
            }
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (_dbcontext = new bakeryEntities1())
                {
                    if(!IsPhoneNumber(phone.Text, pattern))
                    {
                        MessageBox.Show("Неверный телефон");
                        return;
                    }

                    try
                    {
                        if (Regex.IsMatch(email.Text, pattern1))
                        {
                        }
                        else
                        {
                            MessageBox.Show("Неверная почта");
                            return;

                        }

                    }
                    catch
                    {
                        MessageBox.Show("Неверно введена почта");
                        return;
                    }
                    try
                    {
                        if (Convert.ToInt32(age.Text) < 18 || Convert.ToInt32(age.Text) > 80)
                        {
                            MessageBox.Show("Недопустимый возраст");
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Недопустимый возраст");
                    }
                    
                    user user = new user();
                    user.login = login.Text;
                    user.password = Hash.GetHashString(password.Text);
                    user.email = email.Text;
                    user.phone = phone.Text;
                    user.role = "staff";
                    staff staff = new staff();
                    staff.login = login.Text;
                    staff.name = name.Text;
                    staff.surname = surname.Text;
                    staff.age = Convert.ToInt32(age.Text);
                    _dbcontext.users.Add(user);
                    _dbcontext.staffs.Add(staff);
                    _dbcontext.SaveChanges();
                    var emploer = new Emploer();
                    emploer.Show();
                    this.Close();

                }
            }
            catch
            {
                MessageBox.Show("Неверный формат данных");
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

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (name.Text.Contains(" "))
            {
                name.Text = name.Text.Replace(" ", "");
                name.SelectionStart = name.Text.Length;
            }
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (email.Text.Contains(" "))
            {
                email.Text = email.Text.Replace(" ", "");
                email.SelectionStart = email.Text.Length;
            }
        }

        private void age_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (age.Text.Contains(" "))
            {
                age.Text = age.Text.Replace(" ", "");
                age.SelectionStart = age.Text.Length;
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var mw = new Emploer();
            mw.Show();
            Close();
        }

        private void surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (surname.Text.Contains(" "))
            {
                surname.Text = surname.Text.Replace(" ", "");
                surname.SelectionStart = surname.Text.Length;
            }
        }
    }
}
