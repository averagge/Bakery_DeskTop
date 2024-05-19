using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bakery
{
    /// <summary>
    /// Логика взаимодействия для Kassa.xaml
    /// </summary>
    
    public partial class Kassa : Window
    {
        private Card card { get; set; }
        private List <Quantity> quantities = new List <Quantity> ();
        private int selectedgood {  get; set; }
        private decimal sum = 0;

        public Kassa()
        {
            InitializeComponent();
        }

        private void calculateTotal()
        {
            sum = 0;
            using (bakeryEntities1 db = new bakeryEntities1())
            {
                foreach (Quantity q in quantities)
                {
                    int index = Convert.ToInt32(q.Name.Split('_')[1]);

                    int k = Convert.ToInt32(q.quant.Text);
                    foreach (good item in db.goods)
                    {
                        if (item.id == index)
                        {
                            sum += item.price * k;
                            break;
                        }
                    }

                }
                totalprice.Text = Convert.ToString(sum);
            }
        }
        private void goodcomponents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            using(bakeryEntities1 db = new bakeryEntities1())
            {
                foreach(goods_in_stock gis in db.goods_in_stock)
                {
                    if(gis.good_id == Convert.ToInt32(card.artikul.Text))
                    {
                        if(gis.quantity>0 && gis.quantity.ToString()!="")
                        {
                            basket.Items.Add(card.artikul.Text + ":" + card.name.Text);
                            Quantity _quantity = new Quantity();
                            _quantity.Name = "q_" + card.artikul.Text.ToString();
                            _quantity.quant.Text = "1";
                            quantity.Children.Add(_quantity);
                            quantities.Add(_quantity);
                            calculateTotal();
                        }
                        else
                        {
                            MessageBox.Show("Товара нет в наличии");
                        }
                    }
                }
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

        private void basket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (basket.SelectedItem != null)
            {
                selectedgood = Convert.ToInt32(basket.SelectedItem.ToString().Split(':')[0]);
            }
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

                            foreach (var s in basket.Items)
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
                calculateTotal();


            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (selectedgood != -1)
            {
                foreach (Quantity q in quantities)
                {
                    int max = 0;
                    int index = Convert.ToInt32(q.Name.Split('_')[1]);
                    using (bakeryEntities1 db = new bakeryEntities1())
                    {
                        foreach (goods_in_stock gis in db.goods_in_stock)
                        {
                            if (gis.good_id == index)
                            {
                                max = gis.quantity;
                            }
                        }
                    }
                    if (index == selectedgood)
                    {
                        int k = Convert.ToInt32(q.quant.Text);
                        if (k < max)
                        {
                            k++;
                            q.quant.Text = k.ToString();
                        }

                    }
                }
                calculateTotal();

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (bakeryEntities1 db = new bakeryEntities1())
            {

                order order = new order();
                order.total = sum;
                order.user_login = Seans.log;
                order.status = "закрыт";
                if (basket.Items.Count > 0)
                {

                    db.orders.Add(order);
                    db.SaveChanges();
                    foreach (Quantity q in quantities)
                    {
                        int index = Convert.ToInt32(q.Name.Split('_')[1]);

                        goods_in_stock gis = new goods_in_stock();
                        goods_in_stock old = new goods_in_stock();
                        gis.good_id = index;
                        foreach (goods_in_stock item in db.goods_in_stock)
                        {
                            if (item.good_id == gis.good_id)
                            {
                                item.quantity -= Convert.ToInt32(q.quant.Text);
                                old = item;
                                gis.quantity = item.quantity;
                                break;
                            }
                        }
                        db.goods_in_stock.Remove(old);
                        db.goods_in_stock.Add(gis);
                        db.SaveChanges();
                        order_composition oc = new order_composition();
                        oc.order_id = order.id;
                        oc.good_id = index;
                        oc.quantity = Convert.ToInt32(q.quant.Text);
                        db.order_composition.Add(oc);
                        db.SaveChanges();

                    }
                    Kassa kassa = new Kassa();
                    kassa.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Корзина пуста");
                }
                
            }
        }
    }
}
