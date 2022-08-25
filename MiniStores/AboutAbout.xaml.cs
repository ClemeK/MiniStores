using System.Collections.Generic;
using System.Windows;


namespace MiniStores
{
    public partial class AboutAbout : Window
    {
        Dictionary<string, string> ScreenText = new Dictionary<string, string>();

        public AboutAbout(Joblog errlog)
        {
            LanguageData AppLanguage = new LanguageData(errlog, GlobalSetting.Language);

            ScreenText = AppLanguage.GetLanguage();

            InitializeComponent();

            this.Title = LookUpTranslation(ScreenText, "About");

            lblATitle.Content = LookUpTranslation(ScreenText, "Title:");
            lblTitle.Content = "MiniStore";

            lblADescription.Content = LookUpTranslation(ScreenText, "Description:");
            tbDescription.Text = "This is a inventory application for hobbies.";

            lblAProduct.Content = LookUpTranslation(ScreenText, "Product:");
            lblProduct.Content = "MiniStore - Inventory application";

            lblACopyright.Content = LookUpTranslation(ScreenText, "Copyright:");
            lblCopyright.Content = "Copyright © 2022 - Kelvin Clements";

            lblAVersion.Content = LookUpTranslation(ScreenText, "Version:");
            lblVersion.Content = "1.0.*";
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
