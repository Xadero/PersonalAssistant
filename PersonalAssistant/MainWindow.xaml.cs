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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;
using System.IO;

namespace GothicPersonalAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ChooseAssistant_Click(object sender, RoutedEventArgs e)
        {
            if (SelectAssistant.SelectedIndex == 0)
            {
                var gothicAssistant = new GothicPersonalAssistant();
                gothicAssistant.Show();
                this.Hide();
            }
            else
            {
                var classicAssistant = new ClassicPersonalAssistant();
                classicAssistant.Show();
                this.Hide();
            }
        }
    }
}
