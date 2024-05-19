using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Window
    {
        private Card card { get; set; }
        private List<Quantity> quantities = new List<Quantity>();
        private int selectedgood { get; set; }
        public Storage()
        {
            InitializeComponent();
        }

        private void goods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            card = (Card)goods.SelectedItem;
            foreach (string item in basket.Items)
            {
                if (item.Split(':')[0].ToString() == card.artikul.Text)
                {
                    return;
                }
            }
            basket.Items.Add(card.artikul.Text + ":" + card.name.Text);
            Quantity _quantity = new Quantity();
            _quantity.Name = "q_" + card.artikul.Text.ToString();
            _quantity.quant.Text = "1";
            quantity.Children.Add(_quantity);
            quantities.Add(_quantity);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedgood != -1)
            {
                foreach (Quantity q in quantities)
                {
                    int index = Convert.ToInt32(q.Name.Split('_')[1]);
                    if (index == selectedgood)
                    {
                        int k = Convert.ToInt32(q.quant.Text);
                        if (k == 1)
                        {
                            
                            foreach(var s in basket.Items)
                            {
                                if (Convert.ToInt32(s.ToString().Split(':')[0]) == index)
                                {
                                    basket.Items.Remove(s);
                                    break;
                                }
                            }
                            quantities.Remove(q);
                            quantity.Children.Remove(q);
                            break;
                        }
                        if (k > 1)
                        {
                            k--;
                            q.quant.Text = k.ToString();
                        }

                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (selectedgood != -1)
            {
                foreach (Quantity q in quantities)
                {
                    int index = Convert.ToInt32(q.Name.Split('_')[1]);
                    if (index == selectedgood)
                    {
                        int k = Convert.ToInt32(q.quant.Text);
                        if (k >= 1)
                        {
                            k++;
                            q.quant.Text = k.ToString();
                        }

                    }
                }
            }
            
        }

        private void basket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(basket.SelectedItem != null)
            {
                selectedgood = Convert.ToInt32(basket.SelectedItem.ToString().Split(':')[0]);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (bakeryEntities1 dbcontext = new bakeryEntities1())
            {
                foreach (good good in dbcontext.goods)
                {
                    Card card = new Card();
                    List<int> componentsid = new List<int>();
                    string goodcomposition = "";
                    foreach (composition composition in good.compositions)
                    {
                        if (composition.good_id == good.id)
                        {
                            componentsid.Add(Convert.ToInt32(composition.component_id));
                        }
                    }

                    foreach (int componentid in componentsid)
                    {
                        foreach (component component in dbcontext.components)
                        {
                            if (component.id == componentid)
                            {
                                goodcomposition += component.name;
                                goodcomposition += ",";

                            }
                        }

                    }
                    card.name.Text = good.name;
                    card.artikul.Text = good.id.ToString();
                    if (good.image != null)
                    {
                        card.image.Source = new BitmapImage(new Uri(good.image));
                    }
                    card.price.Text = Convert.ToString(good.price);
                    card.composition.Text = goodcomposition.TrimEnd(',');
                    goods.Items.Add(card);

                }

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using(bakeryEntities1 db =  new bakeryEntities1())
            {
                foreach(Quantity q in quantities)
                {
                    bool flag = true;
                    int index = Convert.ToInt32(q.Name.Split('_')[1]);
                    string goodname = "";
                    foreach (var s in basket.Items)
                    {
                        if (Convert.ToInt32(s.ToString().Split(':')[0]) == index)
                        {
                            goodname = basket.Items[basket.Items.IndexOf(s)].ToString();
                            break;
                        }
                    }
                    goods_in_stock gis = new goods_in_stock();
                    goods_in_stock old = new goods_in_stock();

                    gis.quantity = Convert.ToInt32(q.quant.Text);
                    gis.good_id = Convert.ToInt32(goodname.Split(':')[0]);
                    foreach(goods_in_stock item in db.goods_in_stock) 
                    {
                        if(item.good_id == gis.good_id)
                        {
                            old = item;
                            gis.quantity += item.quantity;

                            flag = false;
                            break;
                        }
                    }
                    if(!flag)
                    {
                        db.goods_in_stock.Remove(old);
                        db.goods_in_stock.Add(gis);
                        db.SaveChanges();
                    }
                    if (flag)
                    {
                        db.goods_in_stock.Add(gis);
                        db.SaveChanges();
                    }
                    

                }
                Kassa kassa = new Kassa();
                kassa.Show();
                this.Close();
            }
        }
    }
}
