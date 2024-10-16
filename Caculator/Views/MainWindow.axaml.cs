using Avalonia.Controls;
using Caculator.ViewModels;

namespace Caculator.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}