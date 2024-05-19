using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;


namespace Bakery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Card card { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (goods.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }
            var goodupdate = new GoodUpdate(card);
            goodupdate.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var goodinsert = new GoodInsert();
            goodinsert.Show();
            this.Close();
        }

        private void goods_Loaded(object sender, RoutedEventArgs e)
        {
            using (bakeryEntities1 dbcontext = new bakeryEntities1())
            {
                foreach (good good in dbcontext.goods)
                {
                    Card card = new Card();
                    List<int> componentsid = new List<int>();
                    string goodcomposition = "";
                    foreach(composition composition in good.compositions)
                    {
                        if(composition.good_id == good.id)
                        {
                            componentsid.Add(Convert.ToInt32(composition.component_id));
                        }
                    }

                    foreach (int componentid in componentsid)
                    {
                        foreach(component component in dbcontext.components)
                        {
                            if(component.id == componentid)
                            {
                                goodcomposition += component.name;
                                goodcomposition += ",";

                            }
                        }

                    }
                    card.name.Text = good.name;
                    card.artikul.Text = good.id.ToString();
                    if(good.image != null)
                    {
                        card.image.Source = new BitmapImage(new Uri(good.image));
                    }
                    card.price.Text = Convert.ToString(good.price);
                    card.composition.Text= goodcomposition.TrimEnd(',');
                    goods.Items.Add(card);
                }

            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.card = (Card)goods.SelectedItem;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (goods.SelectedItem == null)
                {
                    MessageBox.Show("Выберите товар");
                    return;
                }
                using (bakeryEntities1 db = new bakeryEntities1())
                {
                    foreach (good _good in db.goods)
                    {
                        if (_good.id == Convert.ToInt32(card.artikul.Text))
                        {
                            db.goods.Remove(_good);
                        }
                    }
                    db.SaveChanges();
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
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
