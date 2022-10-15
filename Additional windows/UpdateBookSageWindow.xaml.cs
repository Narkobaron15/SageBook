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
    /// Interaction logic for UpdateBookSageWindow.xaml
    /// </summary>
    public partial class UpdateBookSageWindow : Window, IAddWindow
    {
        public BookSage? Result { get; private set; }
        object? IAddWindow.Result => Result;

        public UpdateBookSageWindow(BookSage bookSage)
        {
            InitializeComponent();

            using MyDbContext Context = new();

            BookComboBox.ItemsSource = Context.Books.ToList();
            SageComboBox.ItemsSource = Context.Sages.ToList();

            BookComboBox.SelectedItem = Context.Books.Where(x => x.Id == bookSage.BookId).FirstOrDefault();
            SageComboBox.SelectedItem = Context.Sages.Where(x => x.Id == bookSage.SageId).FirstOrDefault();

            Result = null;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
            => OKButton.IsEnabled = BookComboBox.SelectedItem is not null && SageComboBox.SelectedItem is not null;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            using MyDbContext Context = new();

            if (Context.BookSage.Where(
                x => x.SageId == ((Sage)SageComboBox.SelectedItem ?? new()).Id &&
                x.BookId == ((Book)BookComboBox.SelectedItem ?? new()).Id)
                .Any())
            {
                MessageBox.Show("Already contains such item", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Result = new BookSage(((Book)BookComboBox.SelectedItem).Id, ((Sage)SageComboBox.SelectedItem).Id);
                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
