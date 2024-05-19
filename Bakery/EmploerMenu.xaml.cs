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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для EmploerMenu.xaml
    /// </summary>
    public partial class EmploerMenu : UserControl
    {
        public Window ParentWindow { get; set; }
        public EmploerMenu()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }


        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Storage storage = new Storage();
            if (ParentWindow is Storage)
            {
                return;
                
            }
            storage.Show();
            ParentWindow.Close();
        }

        private void Label_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Kassa kassa = new Kassa();
            if (ParentWindow is Kassa)
            {
                return;
                
            }
            kassa.Show();
            ParentWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ParentWindow = Window.GetWindow(this);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var enter = new Enter();
            enter.Show();
            ParentWindow.Close();
        }
    }
}
