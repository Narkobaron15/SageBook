using ADO.NET_Homework_3.Additional_windows;
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

namespace ADO.NET_Homework_3
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddSageWindow : Window, IAddWindow
    {
        public Sage? Result { get; private set; }
        object? IAddWindow.Result => Result;

        public AddSageWindow(string Title)
        {
            InitializeComponent();

            DescriptionLabel.Content = this.Title = Title;
        }
        public AddSageWindow(Sage Sage, string Title)
            : this(Title)
        {
            this.Result = Sage;

            NameTextBox.Text = Sage.Name;
            BirthdayPicker.SelectedDate = Sage.Birthday;
            SageImg.Source = Sage.GetBitmapImage();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Result ??= new();
            Result.Name = NameTextBox.Text;
            Result.Birthday = BirthdayPicker.SelectedDate;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            DialogResult = false;
        }

        private void SelectImgButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image files|*.jpg;*.jpeg;*.jfif;*.gif;*.png;*.tiff;*.bmp|All files (*.*)|*.*",
                FilterIndex = 1,
                AddExtension = true,
            };

            if (dialog.ShowDialog() == true)
            {
                Result ??= new();

                using (FileStream stream = new(dialog.FileName, FileMode.Open))
                {
                    Result.Image = new byte[stream.Length];
                    stream.Read(Result.Image, 0, (int)stream.Length);
                }

                SageImg.Source = new BitmapImage(new Uri(dialog.FileName)); 
                
            }
        }

    }
}
