using ADO.NET_Homework_3.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace ADO.NET_Homework_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using MyDbContext context = new();

            //_ = new Button() { Content = "" };
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using MyDbContext c = new();

            DataGrid1.ItemsSource = TableComboBox.SelectedIndex switch
            {
                0 => c.Books.ToList(),
                1 => c.Sages.ToList(),
                2 => c.BookSage,
                _ => throw new NotImplementedException(),
            };
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedItem is Sage sage)
                SagePic.Source = sage.GetBitmapImage();
        }
    }
}
