
using System.Diagnostics;

namespace Reversi
{
    public class Settings
    {
        // Window size.
        public static int Wwidth = 800;
        public static int Wheight = 800;

        // Board.
        // Board size is dependent on the Window size
        public static int cells = 6;
        public static int thickness = 5;

        // UI sizes.
        public static int Uheight = 150;

        // RGB for the background.
        public static int Br = 10;
        public static int Bg = 70;
        public static int Bb = 50;

        // RGB for the board.
        // classic board green : (60,179,113)
        public static int R = 60;
        public static int G = 179;
        public static int B = 113;

        // RGB for the lines.
        public static int r = 0;
        public static int g = 0;
        public static int b = 0;
    }

    public class Window : Form //Window inherit from Form class
    {
        UI ui;
        GameState game;


        Point labelLocation;
        Label label;
        Label background;

        public Window()
        {
            ClientSize = new Size(Settings.Wwidth, Settings.Wheight);
            Text = "Reversi C#";

            ui = new UI(new Font("Arial Black", 10), new Point(100, Settings.Uheight - 130));

            Controls.Add(ui.NewGameButton);
            Controls.Add(ui.FieldSizeBox);

            ui.FieldSizeBox.SelectedItem = "6x6";

            ui.NewGameClicked += StartNewGame;
            ui.FieldSizeChanged += ChangeFieldSize;

            labelLocation = new Point((Settings.Wwidth/2)-((Settings.Wheight-Settings.Uheight)/2), Settings.Uheight);

            // Reversi board
            label = new Label();
            label.Location = labelLocation;
            label.Size = new Size(Settings.Wheight-Settings.Uheight, Settings.Wheight-Settings.Uheight); 
            label.BackColor = Color.FromArgb(Settings.R,Settings.G, Settings.B);
            Controls.Add(label);
            label.Image = Board.Rboard(label.ClientSize, null);

            label.MouseClick += BoardClicked;

            // Window background
            background = new Label();
            background.Size = ClientSize;
            background.BackColor = Color.FromArgb(Settings.Br,Settings.Bg,Settings.Bb);
            Controls.Add(background);

        }

        void StartNewGame(object sender, EventArgs e)
        {
            game = new GameState(Settings.cells);
            RedrawBoard();
        }
        void ChangeFieldSize(object sender, EventArgs e)
        {
            //Parse the number from the Box for FieldSize
            string text = ui.FieldSizeBox.SelectedItem.ToString();
            Settings.cells = int.Parse(text.Split('x')[0]);
        }
        void BoardClicked(object sender, MouseEventArgs m)
        {
            if (game == null) return;

            //Scaling the game into a small grid
            int cellSize = label.Width / game.Size;

            //Scaling the mouse to the small grid
            int col = m.X / cellSize;
            int row = m.Y / cellSize;

            //Return if the mouse is not inside the board
            if (col < 0 || col >= game.Size) return;
            if (row < 0 || row >= game.Size) return;

            if (Game_rules.IsValidMove(game, row, col))
            {
                //The clicked cell gets the CurrentPlayer number
                game.Board[row, col] = game.CurrentPlayer;

                //CALL FLIP FUNCTION???
                Game_rules.FlipPieces(game, row, col);

                //CurrentPlayer swtiched after click
                game.CurrentPlayer++;
                if (game.CurrentPlayer > 2)
                    game.CurrentPlayer = 1;

                RedrawBoard();
            }
            
        }
        void RedrawBoard()
        {
            if (game == null)
                label.Image = Board.Rboard(label.ClientSize, null);
            else
                label.Image = Board.Rboard(label.ClientSize, game);
        }
        
        // Resize logic
        protected override void OnResize(EventArgs e)
        {
            if (background != null && ClientSize.Height-Settings.Uheight > 100)
            {
                background.Size = ClientSize;
                label.Location = new Point((ClientSize.Width - (ClientSize.Height-Settings.Uheight)) / 2, Settings.Uheight);
                label.Size = new Size(ClientSize.Height-Settings.Uheight,ClientSize.Height-Settings.Uheight);

                ui.NewGameButton.Location = new Point((ClientSize.Width - (ClientSize.Height - Settings.Uheight)) / 2, Settings.Uheight - 130);
                ui.FieldSizeBox.Location = new Point(((ClientSize.Width - (ClientSize.Height - Settings.Uheight)) / 2)+120, Settings.Uheight - 128);

                if (ClientSize.Width < ClientSize.Height - Settings.Uheight)
                {
                    label.Size = new Size(ClientSize.Width, ClientSize.Width);
                    label.Location = new Point((ClientSize.Width-label.Width)/2,Settings.Uheight);

                    ui.NewGameButton.Location = new Point((ClientSize.Width - label.Width) / 2, Settings.Uheight - 130);
                    ui.FieldSizeBox.Location = new Point(((ClientSize.Width - label.Width) / 2)+120, Settings.Uheight - 128);
                }

                label.Image = Board.Rboard(label.ClientSize, game);
            }
            base.OnResize(e);
        }
    }

    // Board draws everything
    public class Board
    {
        public static Bitmap Rboard(Size Bsize, GameState game)
        {
            Bitmap bitmap = new Bitmap(Bsize.Width,Bsize.Height);
            Graphics gr = Graphics.FromImage(bitmap);

            DrawLines(gr);
            if (game != null)
            {
                DrawPieces(gr, game, Bsize);
                DrawHint(gr, game, Bsize);
            }
        
            void DrawLines(Graphics gr)
            {
                Pen linesColor = new Pen(Color.FromArgb(Settings.r,Settings.g,Settings.b), Settings.thickness);

                Brush blackP = new SolidBrush(Color.Black);
                Brush whiteP = new SolidBrush(Color.White);

                int cellSize = Bsize.Width / Settings.cells;

                int r = cellSize - 10;

                for (int i = 0; i <= Settings.cells; i++)
                {
                    gr.DrawLine(linesColor, cellSize,0, cellSize,Bsize.Height);
                    gr.DrawLine(linesColor, 0, cellSize, Bsize.Width, cellSize);
                    cellSize += Bsize.Width /Settings.cells;
                }

                // Drawing the borders.
                gr.DrawLine(linesColor, 0, Settings.thickness / 3, Bsize.Width, Settings.thickness / 3);
                gr.DrawLine(linesColor, Settings.thickness / 3, 0, Settings.thickness / 3, Bsize.Height);
                gr.DrawLine(linesColor, Bsize.Width - Settings.thickness / 2, 0, Bsize.Height - Settings.thickness / 2, Bsize.Height);
                gr.DrawLine(linesColor, 0, Bsize.Height - Settings.thickness / 2, Bsize.Width - Settings.thickness / 2, Bsize.Height - Settings.thickness / 2);
            }

            void DrawPieces(Graphics gr, GameState game, Size Bsize)
            {
                Brush W = new SolidBrush(Color.White);
                Brush B = new SolidBrush(Color.Black);

                //Scaling the game into a "small grid"
                int cellSize = Bsize.Width/game.Size;
                int x;
                int y;

                //Loop through the board and draw the pieces
                for (int row = 0; row < game.Size; row++)
                {
                    for (int col = 0; col < game.Size; col++)
                    {
                        int value = game.Board[row, col];
                        if (game.Board[row,col]!=0)
                        {
                            x = col * cellSize + ((cellSize * 6) / 100);
                            y = row * cellSize + ((cellSize * 6) / 100);
                            
                            if (value ==1)
                                gr.FillEllipse(W, x, y, (cellSize*90)/100, (cellSize * 90) / 100);
                            else if (value ==2)
                                gr.FillEllipse(B, x, y, (cellSize * 90) / 100, (cellSize * 90) / 100);

                        }
                    }
                }
            }

            void DrawHint(Graphics gr, GameState game, Size Bsize)
            {
                Brush hint = new SolidBrush(Color.FromArgb(128, 128, 128, 128));

                int cellSize = Bsize.Width/game.Size;
                int x;
                int y;

                for (int row = 0; row < game.Size; row++)
                {
                    for (int col = 0; col < game.Size; col++)
                    {
                        if (game.Board[row, col] != 0)
                            continue;

                        if (!Game_rules.IsValidMove(game, row, col))
                            continue;

                        x = col * cellSize + ((cellSize * 6) / 100);
                        y = row * cellSize + ((cellSize * 6) / 100);

                        gr.FillEllipse(hint, x, y, (cellSize * 90) / 100, (cellSize * 90) / 100);

                    }
                }
            }
            return bitmap;
        }
    }

    public class UI
    {
        public Button NewGameButton;
        public ComboBox FieldSizeBox;

        public EventHandler NewGameClicked;
        public EventHandler FieldSizeChanged;

        Point mouse;
        public UI(Font font, Point UILocation)
        {
            NewGameButton = new Button();
            NewGameButton.Text = "New Game";
            NewGameButton.Size = new Size(100, 30);
            NewGameButton.Location = UILocation;
            NewGameButton.Font = font;

            NewGameButton.Click += OnNewGameClicked;

            FieldSizeBox = new ComboBox();
            FieldSizeBox.Location = new Point(UILocation.X + 120, UILocation.Y + 2);
            FieldSizeBox.Font = font;

            FieldSizeBox.Items.Add("4x4");
            FieldSizeBox.Items.Add("6x6");
            FieldSizeBox.Items.Add("8x8");
            FieldSizeBox.Items.Add("10x10");

            FieldSizeBox.SelectedIndexChanged += OnFieldSizeChanged;

            void OnNewGameClicked(object sender, EventArgs e)
            {
                if (NewGameClicked != null)
                    NewGameClicked(sender, e);
            }

            void OnFieldSizeChanged(object sender, EventArgs e)
            {
                if(FieldSizeChanged != null)
                    FieldSizeChanged(sender, e);
            }

        }
    }

    public class GameState
    {
        //Empty = 0
        //White = 1
        //Black = 2

        public int Size;

        // Makes the 2D array(board). One cell =  Board[row,column]
        public int [,] Board;

        public int CurrentPlayer;

        public GameState(int size)
        {
            //Creates the board according to the selected Board Size
            Size = size;
            Board = new int[Size, Size]; //new int values starts as 0

            //Random player on start
            Random rnd = new Random();
            CurrentPlayer = rnd.Next(1,3);
            InitBoard();
        }
        
        void InitBoard() //Starting position of the game
        {
            int middle = Size / 2;

            //White
            Board[middle - 1, middle - 1] = 1;
            Board[middle, middle] = 1;

            //Black
            Board[middle - 1, middle] = 2;
            Board[middle, middle - 1] = 2;
        }

    }
    public class Game_rules
    {
        public static bool IsValidMove( GameState game, int row, int col)
        {
            //Cannot play in a cell that not empty
            if (game.Board[row, col] != 0)
                return false;

            //Create opponent
            int opponent;
            if (game.CurrentPlayer == 1)
                opponent = 2;
            else
                opponent = 1;

            //Check 8 directions of a piece
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    //A direction is invalid if:
                    int x = col + dx;
                    int y = row + dy;

                    //its outside the board
                    if (x < 0 || x >= game.Size || y < 0 || y >= game.Size) 
                        continue;

                    //its your own piece or 0
                    if (game.Board[y, x] != opponent)
                        continue;

                    //Check if we can sandwitch the opponent
                    while (true)
                    {
                        x += dx;
                        y += dy;

                        //Stop if its outside the board
                        if (x < 0 || x >= game.Size || y < 0 || y >= game.Size)
                            break;

                        //Stop if we encounter a empty piece
                        if (game.Board[y, x] == 0)
                            break;

                        //Valid if we encouter own piece again
                        if (game.Board[y, x] == game.CurrentPlayer)
                            return true;
                    }
                }
            }

            return false;
        }

        public static void FlipPieces(GameState game, int row, int col)
        {
            //Create opponent
            int opponent;
            if (game.CurrentPlayer == 1)
                opponent = 2;
            else
                opponent = 1;

            //Check 8 directions of the placed piece
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int x = col + dx;
                    int y = row + dy;
                    //A direction is invalid if:
                    //its outside the board
                    if (x < 0 || x >= game.Size || y < 0 || y >= game.Size)
                        continue;
                    //its your own piece or 0
                    if (game.Board[y, x] != opponent)
                        continue;

                    int nextX = x;
;                   int nextY = y;

                    while (true)
                    {
                        nextX += dx;
                        nextY += dy;

                        //Continue if the direction outside the board
                        if (nextX < 0 || nextX >= game.Size || nextY < 0 || nextY >= game.Size)
                            break;

                        //Continue if we encounter a empty piece
                        if (game.Board[nextY, nextX] == 0)
                            break;

                        //Flip pieces between
                        if (game.Board[nextY, nextX] == game.CurrentPlayer)
                        {
                            int flipX = x;
                            int flipY = y;

                            while (flipX != nextX || flipY != nextY)
                            {
                                game.Board[flipY, flipX] = game.CurrentPlayer;
                                flipX += dx;
                                flipY += dy;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    public class Program
    {
        static void Main()
        {

            Application.Run(new Window());
        }

    }
}