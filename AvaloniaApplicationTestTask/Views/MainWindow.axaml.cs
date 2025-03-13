using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using AvaloniaApplicationTestTask.Services.Interfaces;

namespace AvaloniaApplicationTestTask.Views
{
    public partial class MainWindow : Window
    {
        private readonly IDatabaseService _dbService;
        private readonly IGraphGenerator _graphGenerator;
        private static readonly string graphPath = Path.Combine(Directory.GetCurrentDirectory(), "graph.png");

        public MainWindow(IDatabaseService dbService, IGraphGenerator graphGenerator)
        {
            InitializeComponent();
            _dbService = dbService;
            _graphGenerator = graphGenerator;

            Opened += async (_, _) => await LoadLastState();

            saveButton.Click += async (_, _) => await SaveState();
            generateGraphButton.Click += (_, _) => GenerateAndDisplayGraph();
        }

        private async Task LoadLastState()
        {
            var settings = await _dbService.LoadData();

            slider.Value = settings.SliderValue;
            numericInput.Value = settings.NumericValue;
            checkBox.IsChecked = settings.CheckboxState;
            radioA.IsChecked = settings.SelectedRadio == "A";
            radioB.IsChecked = settings.SelectedRadio == "B";
            radioC.IsChecked = settings.SelectedRadio == "C";

            var comboItems = await _dbService.LoadComboBoxItems();
            comboBox.ItemsSource = comboItems;
            comboBox.SelectedItem = comboItems.Contains(settings.SelectedCombo) ? settings.SelectedCombo : comboItems.FirstOrDefault();

            LoadGraphImage();
        }

        private async Task SaveState()
        {
            var settings = new Models.SettingsModel
            {
                SliderValue = (int)slider.Value,
                NumericValue = numericInput.Value ?? 0,
                CheckboxState = checkBox.IsChecked ?? false,
                SelectedRadio = GetSelectedRadio(),
                SelectedCombo = comboBox.SelectedItem?.ToString() ?? "None"
            };

            await _dbService.SaveData(settings);
        }

        private string GetSelectedRadio()
        {
            if (radioA.IsChecked == true) return "A";
            if (radioB.IsChecked == true) return "B";
            return "C";
        }

        private async void GenerateAndDisplayGraph()
        {
            await Task.Run(() =>
            {
                string newGraphPath = _graphGenerator.GenerateGraph();
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    graphImage.Source = new Bitmap(newGraphPath);
                });
            });
        }

        private void LoadGraphImage()
        {
            if (File.Exists(graphPath))
            {
                graphImage.Source = new Bitmap(graphPath);
            }
        }
    }
}
