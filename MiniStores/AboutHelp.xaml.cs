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

namespace MiniStores
{
    /// <summary>
    /// Interaction logic for AboutHelp.xaml
    /// </summary>
    public partial class AboutHelp : Window
    {
        public AboutHelp()
        {
            InitializeComponent();

            string fileName = @"C:\Users\we364\source\repos\MiniStores\MiniStores\Resources\Help.rtf";

            TextRange range;
            FileStream fStream;

            if (File.Exists(fileName))
            {
                range = new TextRange(HelpText.Document.ContentStart, HelpText.Document.ContentEnd);
                fStream = new FileStream(fileName, FileMode.Open);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }
        }
    }
}
