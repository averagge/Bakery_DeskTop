using Microsoft.Win32;
using System;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;
namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для GoodInsert.xaml
    /// </summary>
    public partial class GoodInsert : Window
    {
        private string path;
        private OpenFileDialog opf = new OpenFileDialog() { Filter = "Image files(*.jpg, *.jpeg)|*.jpg;*.jpeg", Multiselect = false };
        public GoodInsert()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (bakeryEntities1 dbcontext = new bakeryEntities1())
                {
                    if (name.Text == "")
                    {
                        MessageBox.Show("Наименование товара не введено");
                        return;
                    }

                    good good = new good();
                    good.price = Convert.ToDecimal(price.Text);
                    good.name = name.Text;
                    good.image = path;

                    foreach (var item in goodcomponents.Items)
                    {
                        composition composition = new composition();
                        composition.good_id = good.id;
                        foreach (component component in dbcontext.components)
                        {
                            if(component.name == item.ToString())
                            {
                                composition.component_id = component.id;
                            }
                        }
                        dbcontext.compositions.Add(composition);

                    }
                    dbcontext.goods.Add(good);
                    goods_in_stock gis = new goods_in_stock();
                    gis.quantity = 0;
                    gis.good_id = good.id;
                    dbcontext.goods_in_stock.Add(gis);

                    dbcontext.SaveChanges();


                }
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Неверный формат ввода");
            }

        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            using(bakeryEntities1 dbcontext = new bakeryEntities1())
            {
                foreach(component component in dbcontext.components)
                {
                    componentslist.Items.Add(component.name);
                }
            }
        }
        
        private void componentslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (string item in goodcomponents.Items)
            {
                if(item==componentslist.SelectedItem.ToString())
                {
                    return;
                }
            }
            goodcomponents.Items.Add(componentslist.SelectedItem.ToString());
        }

        private void goodcomponents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            goodcomponents.Items.Remove(goodcomponents.SelectedItem);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (opf.ShowDialog() == true)
            {
                path = opf.FileName;
                image.Source = new BitmapImage(new Uri(path));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void price_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (price.Text.Contains("-"))
            {
                price.Text = price.Text.Replace("-", "");
                price.SelectionStart = price.Text.Length;
            }
        }
    }
}
