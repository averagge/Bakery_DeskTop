using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;

namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для GoodUpdate.xaml
    /// </summary>
    public partial class GoodUpdate : Window
    {
        private Card card;
        private good good;
        private string path;
        private int old_q = 0;
        private goods_in_stock goods_In_Stock = new goods_in_stock();
        private OpenFileDialog opf = new OpenFileDialog();

        public GoodUpdate(Card card)
        {
            InitializeComponent();
            this.card = card;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (bakeryEntities1 db = new bakeryEntities1())
                {
                    if (name.Text == "")
                    {
                        MessageBox.Show("Наименование товара не введено");
                        return;
                    }
                    foreach (goods_in_stock g in db.goods_in_stock)
                    {
                        if (good.id == g.good_id)
                        {
                            old_q = g.quantity;
                        }
                    }
                    foreach (good _good in db.goods)
                    {
                        if (good.id == _good.id)
                        {
                            db.goods.Remove(_good);
                            good = new good();
                            good.name = name.Text;
                            good.price = Convert.ToDecimal(price.Text);
                            if (path != null)
                            {
                                good.image = path;
                            }
                            goods_In_Stock.good_id = good.id;
                            db.goods.Add(good);


                        }
                    }
                    foreach (var item in goodcomponents.Items)
                    {
                        composition composition = new composition();
                        composition.good_id = good.id;
                        foreach (component component in db.components)
                        {
                            if (component.name == item.ToString())
                            {
                                composition.component_id = component.id;
                            }
                        }
                        db.compositions.Add(composition);

                    }
                    goods_In_Stock.quantity = old_q;
                    db.goods_in_stock.Add(goods_In_Stock);

                    db.SaveChanges();
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();

                }

            }
            catch
            {
                MessageBox.Show("Неверный формат ввода");
            }

        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (bakeryEntities1 dbcontext = new bakeryEntities1())
                {
                    foreach (component component in dbcontext.components)
                    {
                        componentslist.Items.Add(component.name);
                    }
                }
                name.Text = card.name.Text;
                price.Text = card.price.Text;
                if (card.image.Source != null)
                {
                    image.Source = card.image.Source;
                    path = image.Source.ToString();
                }


                string[] componentlist = card.composition.Text.Split(',');
                foreach (var item in componentlist)
                {
                    if (item != "")
                    {
                        goodcomponents.Items.Add(item);
                    }
                }
                good = new good();
                good.id = Convert.ToInt32(card.artikul.Text);
                
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
            
        }

        private void componentslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (string item in goodcomponents.Items)
            {
                if (item == componentslist.SelectedItem.ToString())
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
            this.Close();
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
