using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeGameProject
{
    /// <summary>
    /// Interaction logic for SnakeGame.xaml
    /// </summary>
    public partial class SnakeGame : Window
    {
        private readonly int Rows;
        private readonly int Cols;
        private readonly Image[,] gridImages;
        private GameEngine game;
        private bool gameRuning;
        private bool pause;



        private readonly Dictionary<GridValue, ImageSource> gridValueToImage = new Dictionary<GridValue, ImageSource>()
        {
            {GridValue.EMPTY, Images.Empty },
            {GridValue.SNAKE, Images.Body },
            {GridValue.FOOD, Images.Food }
        };

        private readonly Dictionary<SnakeDirection, int> dirToRotation = new()
        {
            {SnakeDirection.Up, 0 },
            {SnakeDirection.Right, 90 },
            {SnakeDirection.Down, 180 },
            {SnakeDirection.Left, 270 },

        };

        public SnakeGame(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            InitializeComponent();
            gridImages = SetupGrid();
            game = new GameEngine(rows, cols);
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[Rows, Cols];
            GameGrid.Rows = Rows;
            GameGrid.Columns = Cols;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);

                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {game.Score}";
        }

        private void DrawGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    GridValue gridValue = game.GameBoard[r, c];
                    gridImages[r, c].Source = gridValueToImage[gridValue];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await Task.Delay(500);
            await DrawDeadSnake();
            this.Hide();
            GameOver();
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRuning)
            {
                gameRuning = true;
                await RunGame();
                gameRuning = false;
            }

            if (pause && e.Key == Key.Enter)
            {
                await Resume();
            }
        }

        private async void Window_UserInput(object sender, KeyEventArgs e)
        {
            if (game.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                    game.ChangeDirection(SnakeDirection.Up);
                    break;
                case Key.Down:
                    game.ChangeDirection(SnakeDirection.Down);
                    break;
                case Key.Left:
                    game.ChangeDirection(SnakeDirection.Left);
                    break;
                case Key.Right:
                    game.ChangeDirection(SnakeDirection.Right);
                    break;
                case Key.P:
                    Paused();
                    break;
            }
        }

        private async Task GameLoop()
        {
            while (!game.GameOver)
            {

                await Task.Delay(100);
                if (!pause)
                {
                    game.Move();
                }
                Draw();


            }
        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);

            }
        }

        private void GameOver()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        private void DrawSnakeHead()
        {
            ObjectPosition snakeHeadPos = game.SnakeHeadPosition();
            Image image = gridImages[snakeHeadPos.PostionRow, snakeHeadPos.PostionColumn];
            image.Source = Images.Head;
            int rotation = dirToRotation[game.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
            List<ObjectPosition> positions = new List<ObjectPosition>(game.snakeBody);
            ObjectPosition snakeHeadPos = game.SnakeHeadPosition();
            ImageSource sourceBody = Images.DeadBody;
            ImageSource sourceHead = Images.DeadHead;

            for (int i = 0; i < positions.Count; i++)
            {
                ObjectPosition pos = positions[i];

                if (i == 0)
                {
                    gridImages[pos.PostionRow, pos.PostionColumn].Source = sourceHead;
                }
                else
                {
                    gridImages[pos.PostionRow, pos.PostionColumn].Source = sourceBody;
                }

                await Task.Delay(50);
            }
        }

        private void Paused()
        {
            pause = true;
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Press ENTER to resume";
        }
        private async Task Resume()
        {
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            pause = false;
        }
    }
}
