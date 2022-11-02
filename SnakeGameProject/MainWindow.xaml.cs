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

namespace SnakeGameProject
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

    private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Easy.IsChecked == true)
            {
                SnakeGame snakeGame = new SnakeGame(20, 20);
                this.Hide();
                snakeGame.ShowDialog();
            }

            else if (Medium.IsChecked == true)
            {
                SnakeGame snakeGame = new SnakeGame(15, 15);
                this.Hide();
                snakeGame.ShowDialog();
            }

            else if (Hard.IsChecked == true)
            {
                SnakeGame snakeGame = new SnakeGame(10, 10);
                this.Hide();
                snakeGame.ShowDialog();
            }
        }
    }
}
