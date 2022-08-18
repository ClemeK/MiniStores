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

        public FileSettings(Joblog errlog, GlobalSetting gs1)
        {

            InitializeComponent();

            // Language to use.
            LoadLanguageList(errlog);
            cbSetLang.SelectedValue = GlobalSetting.Language + ".lang";

            // Days to retain Job logs.
            tbSetRetain.Text = GlobalSetting.LogsToKeep.ToString();

            // Debugging On\Off.
            cbSetDebug.Items.Add("On");
            cbSetDebug.Items.Add("Off");
            cbSetDebug.SelectedValue = GlobalSetting.DebugApp == true ? "On" : "Off";

            btnSettingSave.IsEnabled = false;
        }

        // *****************************************
        // ***
        private void tbSettingChanged(object sender, RoutedEventArgs e)
        {
            SomethingChanged();
        }
        // ***
        private void cbSettingChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SomethingChanged();
        }
        // ***
        private void SomethingChanged()
        {
            bool changed = false;

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

            if (tbSetRetain.Text != GlobalSetting.LogsToKeep.ToString() && tbSetRetain.Text != "")
            {
                GlobalSetting.LogsToKeep = int.Parse(tbSetRetain.Text);
                changed = true;
            }

            bool debuging = cbSetDebug.SelectedValue == "On" ? true : false;
            if (debuging != GlobalSetting.DebugApp)
            {
                GlobalSetting.DebugApp = cbSetDebug.SelectedValue == "On" ? true : false;
                changed = true;
            }


            if (changed)
            {
                btnSettingSave.IsEnabled = true;
            }

        }
        // ***
        private void btnSettingSave_Clicked(object sender, RoutedEventArgs e)
        {
            GlobalSetting.SaveSettings();

            btnSettingSave.IsEnabled = false;

            if (languageChange == true)
            {
                MessageBox.Show("Re-Start Required for the Language change to take place.", "Mini-Store",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                languageChange = false;
            }
        }
        // ***
        private void btnSettingCancle_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
        // ***
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
        private void tbRetainValidation(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Only allow numbers in the textbox
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
