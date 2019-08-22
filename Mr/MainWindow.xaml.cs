using Mr.Game;
using System.Windows;

namespace Mr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MineField vm = new MineField(50, 30, 1000);

            this.Width = vm.FieldWidht + 50;
            this.Height = vm.FieldHeight + 60;

            this.DataContext = vm;
        }
    }
}
