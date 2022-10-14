using ADO.NET_Homework_3.Additional_windows;
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
        private readonly MyDbContext Context;

        public MainWindow()
        {
            Context = new();
            InitializeComponent();
        }

        ~MainWindow()
        {
            Context.Dispose();
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateDataGrid();

        private void Window_ContentRendered(object sender, EventArgs e) => UpdateDataGrid();

        private void UpdateDataGrid()
        {
            DataGrid1.ItemsSource = TableComboBox.SelectedIndex switch
            {
                0 => Context.Books.ToList(),
                1 => Context.Sages.ToList(),
                2 => Context.BookSage,
                _ => throw new NotImplementedException(),
            };

            try
            {
                if (TableComboBox.SelectedIndex == 0)
                    DataGrid1.Columns[2].Visibility = Visibility.Collapsed;
                else if (TableComboBox.SelectedIndex == 1)
                    DataGrid1.Columns[3].Visibility = DataGrid1.Columns[4].Visibility = Visibility.Collapsed;
            }
            catch { /* ignore */ }
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // tests are needed

            if (DataGrid1.SelectedItem is Sage sage)
                SagePic.Source = sage.GetBitmapImage();
            else SagePic.Source = null;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // open create window

            Window AddWindow = TableComboBox.SelectedIndex switch
            {
                0 => throw new NotImplementedException(),
                1 => new AddSageWindow("Add new sage"),
                2 => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
            
            if (AddWindow.ShowDialog() == true)
            {
                object? result = ((IAddWindow)AddWindow).Result;

                if (result is null) return;
                else if (result is BookSage instance)
                    Context.Books.FirstOrDefault(book => book.Id == instance.BookId)?.Sages.Add(
                    Context.Sages.FirstOrDefault(sage => sage.Id == instance.SageId));
                else
                    Context.Add(result);

                Context.SaveChanges();
                UpdateDataGrid();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // open update window
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem is BookSage instance)
                Context.Books.FirstOrDefault(book => book.Id == instance.BookId)?.Sages.Remove(
                    Context.Sages.FirstOrDefault(sage => sage.Id == instance.SageId));
            else Context.Remove(DataGrid1.SelectedItem);

            Context.SaveChanges();
            UpdateDataGrid();
        }
    }
}
