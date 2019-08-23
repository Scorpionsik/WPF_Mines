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
            MineField vm = new MineField(30, 16, 99);

            this.Width = vm.FieldWidht + 30;
            this.Height = vm.FieldHeight + 100;

            this.DataContext = vm;
        }
    }
}
