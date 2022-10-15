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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateDataGrid();

        private void Window_ContentRendered(object sender, EventArgs e) => UpdateDataGrid();

        private void UpdateDataGrid()
        {
            using MyDbContext Context = new();

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

            GC.Collect();
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedItem is Sage sage)
                SagePic.Source = sage.GetBitmapImage();
            else SagePic.Source = null;

            GC.Collect();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Window AddWindow = TableComboBox.SelectedIndex switch
            {
                0 => new AddBookWindow("Add new book"),
                1 => new AddSageWindow("Add new sage"),
                2 => new AddBookSageWindow(),
                _ => throw new NotImplementedException(),
            };
            
            if (AddWindow.ShowDialog() == true)
            {
                object? result = ((IAddWindow)AddWindow).Result;
                using MyDbContext Context = new();

                if (result is null) return;
                else if (result is BookSage instance)
                {
                    Book? book = Context.Books.FirstOrDefault(book => book.Id == instance.BookId);
                    Sage? sage = Context.Sages.FirstOrDefault(sage => sage.Id == instance.SageId);

                    book?.Sages.Add(sage);
                }
                else Context.Add(result);

                Context.SaveChanges();
                UpdateDataGrid();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            object selected = DataGrid1.SelectedItem;
            Window? AddWindow = null;

            if (selected is Book Book)
                AddWindow = new AddBookWindow(Book, "Update a book");
            else if (selected is Sage Sage)
                AddWindow = new AddSageWindow(Sage, "Update a sage");
            else if (selected is BookSage bs)
                AddWindow = new UpdateBookSageWindow(bs);
            else
            {
                MessageBox.Show("Select an item to update", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (AddWindow?.ShowDialog() == true)
            {
                object? result = ((IAddWindow)AddWindow).Result;
                using MyDbContext Context = new();

                if (result is null) return;
                else if (result is BookSage instance)
                {
                    #pragma warning disable CS8602, CS8634, CS8604, CS8622

                    Sage? sagePrev = Context.Sages.Where(x => x.Id == ((BookSage)selected).SageId).FirstOrDefault();
                    Context.Entry(sagePrev).Collection(x => x.Books).Load();
                    Book? bookPrev = Context.Books.Where(x => x.Id == ((BookSage)selected).BookId).FirstOrDefault();
                    bookPrev?.Sages.Remove(sagePrev);

                    Sage? sageCurrent = Context.Sages.Where(x => x.Id == instance.SageId).FirstOrDefault();
                    Context.Entry(sageCurrent).Collection(x => x.Books).Load();
                    Book? bookCurrent = Context.Books.Where(x => x.Id == instance.BookId).FirstOrDefault();

                    sageCurrent?.Books.Add(bookCurrent);

                    #pragma warning restore CS8602, CS8634, CS8604, CS8622
                }
                else Context.Update(result);

                Context.SaveChanges();
                UpdateDataGrid();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using MyDbContext Context = new();

                if (DataGrid1.SelectedItem is BookSage instance)
                {
                    #pragma warning disable CS8602, CS8634, CS8604, CS8622

                    Book? book = Context.Books.FirstOrDefault(book => book.Id == instance.BookId);
                    Context.Entry(book).Collection(x => x.Sages).Load();
                    Sage? sage = book?.Sages.FirstOrDefault(sage => sage.Id == instance.SageId);

                    book?.Sages.Remove(sage);

                    #pragma warning restore CS8602, CS8634, CS8604, CS8622
                }
                else Context.Remove(DataGrid1.SelectedItem);

                Context.SaveChanges();
                UpdateDataGrid();
            }
            catch { /* ignore */ }
        }
    }
}
