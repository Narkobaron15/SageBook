using ADO.NET_Homework_3.Model;
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
using System.Windows.Shapes;

namespace ADO.NET_Homework_3.Additional_windows
{
    /// <summary>
    /// Interaction logic for AddBookSageWindow.xaml
    /// </summary>
    public partial class AddBookSageWindow : Window, IAddWindow
    {
        public BookSage? Result { get; private set; }
        object? IAddWindow.Result => Result;

        public AddBookSageWindow()
        {
            InitializeComponent();

            // fill tables and dropdowns

            // updatebooksagewindow will be new window
            // contains two datagrids and dropdowns for new value.

            using MyDbContext Context = new();

            BookComboBox.ItemsSource = Context.Books.ToList();
            BookDataGrid.ItemsSource = Context.Books.ToList();
            SageComboBox.ItemsSource = Context.Sages.ToList();
            SageDataGrid.ItemsSource = Context.Sages.ToList();
            BookSageDataGrid.ItemsSource = Context.BookSage;

            Result = null;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (OKButton.IsEnabled)
            {
                Result = new BookSage(((Book)BookComboBox.SelectedItem).Id, ((Sage)SageComboBox.SelectedItem).Id);
                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;

        private void SelectionChanged(object sender, SelectionChangedEventArgs e) 
            => OKButton.IsEnabled = !SelectedBookSageExists() &&
                BookComboBox.SelectedItem is not null &&
                SageComboBox.SelectedItem is not null;

        private bool SelectedBookSageExists()
        {
            using MyDbContext Context = new();

            if (Context.BookSage.Where(
                x => x.SageId == ((Sage)SageComboBox.SelectedItem ?? new()).Id &&
                x.BookId == ((Book)BookComboBox.SelectedItem ?? new()).Id)
                .Any())
            {
                MessageBox.Show("Already contains such item", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            return false;
        }

        private void BookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => BookComboBox.SelectedItem = BookDataGrid.SelectedItem;

        private void SageDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => SageComboBox.SelectedItem = SageDataGrid.SelectedItem;
    }
}