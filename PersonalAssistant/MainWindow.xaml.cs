using PersonalAssistant.Service.Interfaces;
using System.Windows;

namespace PersonalAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly ISoundService _soundService;
        public readonly ISpeechRecognizerService _speechRecognizerService;
        public readonly IAssistantService _assistantService;
        public static MainWindow AppWindow;

        public MainWindow(ISoundService soundService, ISpeechRecognizerService speechRecognizerService, IAssistantService assistantService)
        {
            AppWindow = this;
            _soundService = soundService;
            _speechRecognizerService = speechRecognizerService;
            _assistantService = assistantService;
            InitializeComponent();
        }

        private void ChooseAssistant_Click(object sender, RoutedEventArgs e)
        {
            if (SelectAssistant.SelectedIndex == 0)
            {
                var gothicAssistant = new GothicPersonalAssistant(_soundService, _speechRecognizerService);
                gothicAssistant.Show();
                this.Hide();
            }
            else
            {
                var classicAssistant = new ClassicPersonalAssistant(_speechRecognizerService, _assistantService);
                classicAssistant.Show();
                this.Hide();
            }
        }
    }
}
