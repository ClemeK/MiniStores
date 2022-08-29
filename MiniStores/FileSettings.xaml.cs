using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace MiniStores
{
    /// <summary>
    /// Interaction logic for FileSettings.xaml
    /// </summary>
    public partial class FileSettings : Window
    {
        bool languageChange = false;
        Dictionary<string, string> ScreenText = new Dictionary<string, string>();

        public FileSettings(Joblog errlog)
        {
            LanguageData AppLanguage = new LanguageData(errlog, GlobalSetting.Language);

            ScreenText = AppLanguage.GetLanguage();

            InitializeComponent();

            // Set-up Screen Language
            this.Title = LookUpTranslation(ScreenText, "Settings");

            lblSLanguage.Content = LookUpTranslation(ScreenText, "Language:");
            lblSRetainP.Content = LookUpTranslation(ScreenText, "RetainP");
            lblSDebug.Content = LookUpTranslation(ScreenText, "Debug:");
            btnSettingSave.Content = LookUpTranslation(ScreenText, "Save");
            btnSettingCancle.Content = LookUpTranslation(ScreenText, "Cancel");

            // Language to use.
            LoadLanguageList(errlog);
            cbSetLang.SelectedValue = GlobalSetting.Language + ".lang";

            // Days to retain Job logs.
            tbSetRetain.Text = GlobalSetting.LogsToKeep.ToString();

            // Debugging On\Off.
            cbSetDebug.Items.Add(LookUpTranslation(ScreenText, "On"));
            cbSetDebug.Items.Add(LookUpTranslation(ScreenText, "Off"));
            cbSetDebug.SelectedValue = GlobalSetting.DebugApp == true ? LookUpTranslation(ScreenText, "On") : LookUpTranslation(ScreenText, "Off");

            btnSettingSave.IsEnabled = false;
        }

        // *****************************************
        // ***
        /// <summary>
        /// TextBox Alert that something MAY have changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSettingChanged(object sender, RoutedEventArgs e)
        {
            SomethingChanged();
        }
        // ***
        /// <summary>
        /// ComboBox Alert that something MAY have changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSettingChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SomethingChanged();
        }
        // ***
        /// <summary>
        ///  Check to see if something has changed on the Setting Screen
        /// </summary>
        private void SomethingChanged()
        {
            bool changed = false;

            if (cbSetLang.SelectedValue != null)
            {
                if (cbSetLang.SelectedValue.ToString() != GlobalSetting.Language)
                {
                    GlobalSetting.Language = cbSetLang.Text;
                    languageChange = true;
                    changed = true;
                }
                else
                {
                    languageChange = false;
                }
            }

            if (tbSetRetain.Text != null)
            {
                if (tbSetRetain.Text != GlobalSetting.LogsToKeep.ToString() && tbSetRetain.Text != "")
                {
                    GlobalSetting.LogsToKeep = int.Parse(tbSetRetain.Text);
                    changed = true;
                }
            }

            if (cbSetDebug.SelectedValue != null)
            {
                bool debuging = cbSetDebug.SelectedValue.ToString() == "On" ? true : false;
                if (debuging != GlobalSetting.DebugApp)
                {
                    GlobalSetting.DebugApp = cbSetDebug.SelectedValue == "On" ? true : false;
                    changed = true;
                }
            }


            if (changed)
            {
                btnSettingSave.IsEnabled = true;
            }

        }
        // ***
        /// <summary>
        /// Save the Global Setting to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettingSave_Clicked(object sender, RoutedEventArgs e)
        {
            GlobalSetting.SaveSettings();

            btnSettingSave.IsEnabled = false;

            if (languageChange == true)
            {
                MessageBox.Show(LookUpTranslation(ScreenText, "ReloadP"), "Mini-Store",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                languageChange = false;
            }
        }
        // ***
        /// <summary>
        /// Close the File Setting screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettingCancle_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // ***
        /// <summary>
        /// Load the Language list into the ComboBox
        /// </summary>
        /// <param name="el">JobLog to write messages too</param>
        private void LoadLanguageList(Joblog el)
        {
            List<string> languages = new List<string>();

            LanguageData AppLang = new LanguageData(el, GlobalSetting.Language);

            languages = AppLang.AvaibleLanguages();

            foreach (var l in languages)
            {
                cbSetLang.Items.Add(l);
            }
        }
        // ***
        /// <summary>
        /// Validate the Number only fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRetainValidation(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Only allow numbers in the textbox
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
