using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace MiniStores
{
    /// <summary>
    /// Interaction logic for AboutHelp.xaml
    /// </summary>
    public partial class AboutHelp : Window
    {
        Dictionary<string, string> ScreenText = new Dictionary<string, string>();

        public AboutHelp(Joblog errlog)
        {
            LanguageData AppLanguage = new LanguageData(errlog, GlobalSetting.Language);

            ScreenText = AppLanguage.GetLanguage();

            InitializeComponent();

            this.Title = LookUpTranslation(ScreenText, "Help");

            lblHTitle.Content = LookUpTranslation(ScreenText, "MiniStores");
            lblHSubTitle.Content = LookUpTranslation(ScreenText, "HelpSubP");

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
        // ***
        /// <summary>
        /// Fetch's the correct Language Phrase from the Dictionary
        /// </summary>
        /// <param name="dic">Dictionary to use</param>
        /// <param name="lookUpKey">Key to look up</param>
        /// <returns></returns>
        public string LookUpTranslation(Dictionary<string, string> dic, string lookUpKey)
        {
            bool worked = dic.TryGetValue(lookUpKey, out string? output);

            if (worked)
            {
                return output;
            }
            else
            {
                return "Unknown (" + lookUpKey + ")";
            }
        }

    }
}
