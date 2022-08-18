using System.Windows;


namespace MiniStores
{
    public partial class AboutAbout : Window
    {
        public AboutAbout()
        {
            InitializeComponent();

            lblTitle.Content = "MiniStore";
            tbDescription.Text = "This is a inventory application for hobbies.";
            lblProduct.Content = "MiniStore - Inventory application";
            lblCopyright.Content = "Copyright © 2022 - Kelvin Clements";
            lblVersion.Content = "1.0.*";
        }

    }
}
