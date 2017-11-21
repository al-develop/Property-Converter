using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AutoPropToMvvmProp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StringBuilder convertedProperties;
        public MainWindow()
        {
            InitializeComponent();
            convertedProperties = new StringBuilder();
            FillMvvmStyles();
        }

        private void FillMvvmStyles()
        {
            var mvvmStyles = new List<string>()
            {
                "DevExpress",
                "MVVM Light"
            };
            this.cbxStyles.ItemsSource = null;
            this.cbxStyles.ItemsSource = mvvmStyles;
            this.cbxStyles.SelectedIndex = 0;
        }

        private void btnDo_Click(object sender, RoutedEventArgs e)
        {
            convertedProperties = new StringBuilder();

            string[] propTextPerLine = Regex.Split(tbxProp.Text, "\r\n");
            string stylesSelection = cbxStyles.SelectedItem.ToString();
            foreach (string line in propTextPerLine)
            {
                ConvertToMvvmProperty(line, stylesSelection);
            }
            tbxMvvm.Text = convertedProperties.ToString();
            convertedProperties = new StringBuilder();
        }

        private void ConvertToMvvmProperty(string line, string style)
        {
            switch (style.ToLower())
            {
                case "devexpress":
                    ConvertToDXProperty(line);
                    break;

                case "mvvm light":
                    ConvertToLightProperty(line);
                    break;

                default:
                    return;
            }

        }

        private void ConvertToDXProperty(string line)
        {
            if (String.IsNullOrEmpty(line))
                return;

            try
            {
                line = line.TrimStart(' ');
                string[] splittedLine = line.Split(' ');
                string propertyName = splittedLine.ElementAt(2);
                string propertyType = splittedLine.ElementAt(1);
                string backingFieldName = $"_{propertyName.ToLower()}";
                string backingField = $"private {propertyType} {backingFieldName};";

                string getter = $"get {{ return {backingFieldName}; }}";
                string setter = $"set {{ SetProperty(ref {backingFieldName}, value, () => {propertyName}); }}";

                string dxProperty = $"{backingField}\npublic {propertyType} {propertyName}\n{{\n{getter}\n{setter}\n}}";
                convertedProperties.AppendLine(dxProperty);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void ConvertToLightProperty(string line)
        {
            if (String.IsNullOrEmpty(line))
                return;

            try
            {
                line = line.TrimStart(' ');
                string[] splittedLine = line.Split(' ');
                string propertyName = splittedLine.ElementAt(2);
                string propertyType = splittedLine.ElementAt(1);
                string backingFieldName = $"_{propertyName.ToLower()}";
                string backingField = $"private {propertyType} {backingFieldName};";

                string getter = $"get {{ return {backingFieldName}; }}";
                string setter = $"set {{ {backingFieldName} = value;\nRaisePropertyChanged(() => {propertyName}); }}";

                string dxProperty = $"{backingField}\npublic {propertyType} {propertyName}\n{{\n{getter}\n{setter}\n}}";
                convertedProperties.AppendLine(dxProperty);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClip_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateMvvmContent())
                return;

            Clipboard.SetText(this.tbxMvvm.Text);
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateMvvmContent())
                return;

            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog() != true)
                return;

            string savePath = dialog.SelectedPath;
            string file = $"{savePath}\\PropertyConverter.cs";
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            if (File.Exists(file))
            {
                if (MessageBox.Show("File already exists.\nOverwrite?", "File already exists", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                File.Delete(file);
            }

            using (var stream = new StreamWriter($"{savePath}\\PropertyConverter.cs"))
            {
                stream.WriteLine(this.tbxMvvm.Text);
            }
        }

        private bool ValidateMvvmContent()
        {
            return String.IsNullOrEmpty(tbxMvvm.Text);
        }
    }
}