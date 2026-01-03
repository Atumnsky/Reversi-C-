
using System.Diagnostics;
using System.Xml.Schema;

namespace Reversi
{
    public class Settings
    {
        // Window size.
        public static int Wwidth = 1000;
        public static int Wheight = 800;

        // Board.
        // Board size is dependent on the Window size
        public static int cells = 6;
        public static int thickness = 5;

        // UI sizes.
        public static int Uheight = 100;
        public static int ButWidth = 110;
        public static int ButHeight = 40;
        public static int FontSize = 10;

        // RGB for the background.
        public static int Br;
        public static int Bg;
        public static int Bb;

        // RGB for the board.
        // classic board green : (60,179,113)
        public static int R;
        public static int G;
        public static int B;

        // RGB for the lines.
        public static int r = 0;
        public static int g = 0;
        public static int b = 0;

        //Piece color
        public static Brush White = new SolidBrush(Color.White);
        public static Brush Black = new SolidBrush(Color.Black);

        public static Brush hint = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
        public static bool ShowHint = false;

        public static bool VSAI = false;
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
            MinimumSize = ClientSize;
            Text = "Reversi C#";

            ui = new UI(new Font("Arial Black", Settings.FontSize), 
                        new Font("Arial Black", Settings.FontSize),
                        ClientSize
                        );


            Controls.Add(ui.UIPanel);
            Controls.Add(ui.PPanel);

            ui.FieldSizeBox.SelectedItem = "6x6";
            ui.ColorBox.SelectedItem = "Classic Board";
            

            ui.NewGameClicked += StartNewGame;
            ui.FieldSizeChanged += ChangeFieldSize;
            ui.ColorChanged += ChangeColor;
            ui.HintClicked += HintON;
            ui.AIClicked += VSAIOn;

            CenterUI();

            labelLocation = new Point((Settings.Wwidth/2)-((Settings.Wheight-Settings.Uheight)/2), Settings.Uheight-20);

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

            ChangeColor(null, EventArgs.Empty);

            ui.UIPanel.Parent = background;
            ui.UIPanel.BackColor = Color.Transparent;

            ui.PPanel.Parent = background;
            ui.PPanel.BackColor = Color.Transparent;
            ui.PPanel.Paint += DrawP;
        }

        int r = 100;
        void DrawPlayers(PaintEventArgs pea)
        {
            Font Arial = new Font("Arial Black", 30);

            Graphics g = pea.Graphics;

            int panelWidth = ui.PPanel.Width;
            int panelHeight = ui.PPanel.Height;

            int sideWidth = panelWidth / 7;

            int leftX = (sideWidth - r) / 2;
            int rightX = panelWidth - sideWidth + (sideWidth - r) / 2;

            int centerY = (panelHeight - r) / 2;

            g.FillEllipse(Settings.White, leftX, centerY, r, r);
            g.FillEllipse(Settings.Black, rightX, centerY, r, r);

            int WhiteScore = 0;
            int BlackScore = 0;

            if (game!=null)
            {
                var scores = game.CountPoints();
                WhiteScore = scores.WhiteP;
                BlackScore = scores.BlackP;
            }
            string textW = WhiteScore.ToString();
            string textB = BlackScore.ToString();

            SizeF leftSize = g.MeasureString(textW, Arial);
            SizeF rightSize = g.MeasureString(textB, Arial);

            float textWX = leftX + r / 2f - leftSize.Width / 2f;
            float textWY = centerY + r / 2f - leftSize.Height / 2f;

            float textBX = rightX + r / 2f - rightSize.Width / 2f;
            float textBY = centerY + r / 2f - rightSize.Height / 2f;

            g.DrawString(textW, Arial, Settings.Black, textWX, textWY);
            g.DrawString(textB, Arial, Settings.White, textBX, textBY);
        }

        void DrawP(object s, PaintEventArgs pea)
        {
            ui.PPanel.Location = new Point(0,0);
            DrawPlayers(pea);
        }

        void CenterUI()
        {
            if (ui == null)
                return;
            ui.UIPanel.Location = new Point((ClientSize.Width-ui.UIPanel.Width)/2, Settings.Uheight / 10);
        }
        void StartNewGame(object sender, EventArgs e)
        {
            game = new GameState(Settings.cells);
            ui.PPanel.Invalidate();
            RedrawBoard();
            if (Settings.VSAI && game.CurrentPlayer == 2)
                AIMove();
        }
        void HintON(object sender, EventArgs e)
        {
            Settings.ShowHint = !Settings.ShowHint;
            RedrawBoard();
        }
        void VSAIOn(object sender, EventArgs e)
        {
            Settings.VSAI = ui.VSAIOn;
        }
        async void AIMove()
        {
            if (game == null) 
                return;

            //Async used for simulating delay
            Random time = new Random();
            int delay = time.Next(250, 1000);
            await Task.Delay(delay);

            if (game.CurrentPlayer != 2)
                return;

            //Count total legal moves
            int legalMoves = 0;
            for (int row = 0; row < game.Size; row++)
            {
                for (int col = 0; col < game.Size; col++)
                {
                    if (Game_rules.IsValidMove(game, row, col))
                        legalMoves++;
                }
            }

            if (legalMoves==0)
                return;

            Random rnd = new Random();
            int Move = rnd.Next(legalMoves);

            //AI play
            int i = 0;
            for (int row = 0; row < game.Size; row++)
            {
                for (int col = 0;col < game.Size; col++)
                {
                    if (Game_rules.IsValidMove(game, row, col))
                    {
                        if (i == Move)
                        {
                            game.Board[row, col] = 2;
                            Game_rules.FlipPieces(game, row, col);

                            game.CurrentPlayer = 1;
                            ui.PPanel.Invalidate();
                            RedrawBoard();
                            CheckGameEnd();
                            return;
                        }
                        i++;
                    }
                }
            }

        }
        void ChangeFieldSize(object sender, EventArgs ea)
        {
            //Parse the number from the Box for FieldSize
            string text = ui.FieldSizeBox.SelectedItem.ToString();
            Settings.cells = int.Parse(text.Split('x')[0]);
        }
        void ChangeColor(object sender, EventArgs e)
        {
            string text = ui.ColorBox.SelectedItem.ToString();

            if(text == "Classic Board")
            {
                Settings.Br = 10;
                Settings.Bg = 70;
                Settings.Bb = 50;

                Settings.R = 60;
                Settings.G = 179;
                Settings.B = 113;

                Settings.White = new SolidBrush(Color.White);
                Settings.Black = new SolidBrush(Color.Black);
            }
            else if (text == "Red VS Blue")
            {
                Settings.Br = 200;
                Settings.Bg = 200;
                Settings.Bb = 200;

                Settings.R = 140;
                Settings.G = 140;
                Settings.B = 140;

                Settings.White = new SolidBrush(Color.Blue);
                Settings.Black = new SolidBrush(Color.Red);

                Settings.hint = new SolidBrush(Color.FromArgb(180, 180, 180));
            }
            else if (text == "Wood")
            {
                Settings.Br = 186;
                Settings.Bg = 140;
                Settings.Bb = 99;

                Settings.R = 193;
                Settings.G = 154;
                Settings.B = 107;

                Settings.White = new SolidBrush(Color.FromArgb(238,213,174));
                Settings.Black = new SolidBrush(Color.FromArgb(76,43,32));
            }

            background.BackColor = Color.FromArgb(Settings.Br, Settings.Bg, Settings.Bb);
            label.BackColor = Color.FromArgb(Settings.R, Settings.G, Settings.B);
            RedrawBoard();
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

                Game_rules.FlipPieces(game, row, col);

                //CurrentPlayer swtiched after click
                game.CurrentPlayer++;
                if (game.CurrentPlayer > 2)
                    game.CurrentPlayer = 1;

                ui.PPanel.Invalidate();
                
                //Skip if the current player has not move possible
                if (!Game_rules.MovePossible(game,game.CurrentPlayer))
                {
                    game.CurrentPlayer++;
                    if(game.CurrentPlayer > 2)
                        game.CurrentPlayer = 1;
                }

                if (Settings.VSAI && game.CurrentPlayer == 2)
                    AIMove();

                RedrawBoard();
                CheckGameEnd();
            }
            
        }

        void CheckGameEnd()
        {
            if (game == null)
                return;

            bool WhiteHasMove = Game_rules.MovePossible(game, 1);
            bool BlackHasMove = Game_rules.MovePossible(game, 2);

            if (!WhiteHasMove && !BlackHasMove)
                ShowWinner();
        }

        void ShowWinner()
        {
            var scores = game.CountPoints();

            string message;
            if (scores.WhiteP > scores.BlackP)
                message = $"White wins!\n\n White: {scores.WhiteP} \n Black: {scores.BlackP}";
            else if (scores.WhiteP < scores.BlackP)
                message = $"Black wins! \n\n White: {scores.WhiteP} \n Black: {scores.BlackP}";
            else
                message = $"It's a draw! \n\n White: {scores.WhiteP} \n Black: {scores.BlackP}";

            MessageBox.Show(message, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                int z = ClientSize.Height - Settings.Uheight;
                background.Size = ClientSize;
                label.Location = new Point((ClientSize.Width - (ClientSize.Height-Settings.Uheight)) / 2, Settings.Uheight-20);
                label.Size = new Size(z,z);

                r = ClientSize.Width/10;

                ui.PPanel.Size = ClientSize;
                ui.PPanel.Invalidate();

                if (label.Location.X < ((ClientSize.Width / 7) - 100)/2 + r+20)
                {
                    int sideSpace = ClientSize.Width / 7;

                    int maxBoardWidth = ClientSize.Width - sideSpace*2;
                    int maxBoardHeight = ClientSize.Height - Settings.Uheight;

                    int boardSize = Math.Min(maxBoardWidth, maxBoardHeight);

                    label.Location = new Point(sideSpace + (maxBoardWidth - boardSize)/2, Settings.Uheight - 20);
                    label.Size = new Size(boardSize, boardSize);
                }

                label.Image = Board.Rboard(label.ClientSize, game);
            }
            if(ui != null)
                ui.PPanel.Invalidate();
            CenterUI();
            base.OnResize(e);
        }
    }

    // Board draws everything
    public class Board
    {
        public static Bitmap Rboard(Size Bsize, GameState game)
        {
            Size bsize = Bsize;

            Bitmap bitmap = new Bitmap(Bsize.Width,Bsize.Height);
            Graphics gr = Graphics.FromImage(bitmap);

            DrawLines(gr);
            if (game != null)
            {
                DrawPieces(gr, game, Bsize);
                if(Settings.ShowHint)
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
                                gr.FillEllipse(Settings.White, x, y, (cellSize*90)/100, (cellSize * 90) / 100);
                            else if (value ==2)
                                gr.FillEllipse(Settings.Black, x, y, (cellSize * 90) / 100, (cellSize * 90) / 100);

                        }
                    }
                }
            }

            void DrawHint(Graphics gr, GameState game, Size Bsize)
            {
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

                        gr.FillEllipse(Settings.hint, x, y, (cellSize * 90) / 100, (cellSize * 90) / 100);

                    }
                }
            }
            return bitmap;
        }
    }

    public class UI
    {
        public Panel UIPanel;
        public Panel PPanel;

        public Button NewGameButton;
        public Button HintButton;
        public Button VSAIButton;
        public ComboBox FieldSizeBox;
        public ComboBox ColorBox;

        public EventHandler NewGameClicked;
        public EventHandler FieldSizeChanged;
        public EventHandler ColorChanged;
        public EventHandler HintClicked;
        public EventHandler AIClicked;

        Point mouse;
        public bool HintOn = false;
        public bool VSAIOn = false;

        public UI(Font font1,Font font2, Size ppSize)
        {
            UIPanel = new Panel();
            UIPanel.Size = new Size(120*6, Settings.Uheight);

            PPanel = new Panel();
            PPanel.Size = ppSize;

            NewGameButton = new Button();
            NewGameButton.Text = "New Game";
            NewGameButton.Size = new Size(120, UIPanel.Size.Height/3);
            NewGameButton.Location = new Point(0,0);
            NewGameButton.Font = font1;
            NewGameButton.Click += OnNewGameClicked;

            int space = NewGameButton.Size.Width / 4;

            VSAIButton = new Button();
            VSAIButton.Text = "Fake AI";
            VSAIButton.Font = font1;
            VSAIButton.Size = NewGameButton.Size;
            VSAIButton.Location = new Point(NewGameButton.Location.X+NewGameButton.Width+space,0);

            VSAIButton.Click += ONAIClicked;
            
            HintButton = new Button();
            HintButton.Text = "Moves Hint";
            HintButton.Font = font1;
            HintButton.Size = NewGameButton.Size;
            HintButton.Location = new Point(VSAIButton.Location.X + VSAIButton.Width + space, 0);

            HintButton.Click += OnHintClicked;

            FieldSizeBox = new ComboBox();
            FieldSizeBox.Location = new Point(HintButton.Location.X + HintButton.Width + space,0);
            FieldSizeBox.Font = font2;

            FieldSizeBox.Items.Add("4x4");
            FieldSizeBox.Items.Add("6x6");
            FieldSizeBox.Items.Add("8x8");
            FieldSizeBox.Items.Add("10x10");

            FieldSizeBox.SelectedIndexChanged += OnFieldSizeChanged;

            ColorBox = new ComboBox();
            ColorBox.Location = new Point(FieldSizeBox.Location.X + FieldSizeBox.Width + space,0);
            ColorBox.Font = font2;

            ColorBox.Items.Add("Classic Board");
            ColorBox.Items.Add("Red VS Blue");
            ColorBox.Items.Add("Wood");

            ColorBox.SelectedIndexChanged += OnColorChanged;

            void OnColorChanged(object sender, EventArgs e)
            {
                if(ColorChanged != null)
                    ColorChanged(sender, e);
            }

            void OnHintClicked(object s, EventArgs ea)
            {
                HintOn = !HintOn;
                if (HintOn)
                    HintButton.BackColor = Color.LimeGreen;
                else
                    HintButton.BackColor = Color.White;

                if (HintClicked != null)
                    HintClicked(s, ea);
            }

            void ONAIClicked (object s, EventArgs ea)
            {
                VSAIOn = !VSAIOn;

                if (VSAIOn)
                    VSAIButton.BackColor = Color.Orange;
                else
                    VSAIButton.BackColor = Color.White;

                if (AIClicked != null)
                    AIClicked(s, ea);
            }

            UIPanel.Controls.Add(NewGameButton);
            UIPanel.Controls.Add(FieldSizeBox);
            UIPanel.Controls.Add(ColorBox);
            UIPanel.Controls.Add(HintButton);
            UIPanel.Controls.Add(VSAIButton);


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

        // Tuple for returning 2 things
        public (int WhiteP, int BlackP) CountPoints()
        {
            int WhiteP = 0;
            int BlackP = 0;

            //Loop through the board and count the pieces
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (Board[row, col] == 1)
                        WhiteP++;
                    else if (Board[row, col] == 2)
                        BlackP++;
                }
            }

            return (WhiteP, BlackP);
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

        public static bool MovePossible(GameState game, int player)
        {
            //check if the opponent has legal move left
            int previousPlayer = game.CurrentPlayer;
            game.CurrentPlayer = player;

            for (int row = 0; row < game.Size; row++)
            {
                for (int col = 0; col < game.Size; col++)
                {
                    if (IsValidMove(game, row, col))
                    {
                        //Switch the player back
                        game.CurrentPlayer = previousPlayer;
                        return true;
                    }
                }
            }
            //return false if no move left for opponent
            game.CurrentPlayer = previousPlayer;
            return false;
        }
    }

    public class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Window());
        }

    }
}