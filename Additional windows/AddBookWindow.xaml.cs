using ADO.NET_Homework_3.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window, IAddWindow
    {
        public Book? Result { get; private set; }
        object? IAddWindow.Result => Result;

        public AddBookWindow(string Title)
        {
            InitializeComponent();
            DescriptionLabel.Content = this.Title = Title;
        }
        public AddBookWindow(Book Book, string Title)
            : this(Title)
        {
            this.Result = Book;
            TitleTextBox.Text = Book.Title;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Result ??= new();
            Result.Title = TitleTextBox.Text;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            DialogResult = false;
        }

    }
}
