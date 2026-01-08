using System.Diagnostics;

namespace Reversi
{
    public class Window : Form //Window inherit from Form class
    {
        static UI ui;

        static GameState game;

        Point labelLocation;
        static Label label;
        Label background;

        public  Window()
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
            ui.DifficultyBox.SelectedItem = "Easy";


            ui.NewGameClicked += StartNewGame;
            ui.FieldSizeChanged += ChangeFieldSize;
            ui.ColorChanged += ChangeColor;
            ui.HintClicked += HintON;
            ui.AIClicked += VSAIOn;
            ui.HelpClicked += ShowHelp;
            ui.DifficultyChanged += ChangeDifficulty;
            ChangeDifficulty(null, EventArgs.Empty);

            CenterUI();

            labelLocation = new Point((Settings.Wwidth / 2) - ((Settings.Wheight - Settings.Uheight) / 2), Settings.Uheight - 20);

            // Reversi board
            label = new Label();
            label.Location = labelLocation;
            label.Size = new Size(Settings.Wheight - Settings.Uheight, Settings.Wheight - Settings.Uheight);
            label.BackColor = Color.FromArgb(Settings.R, Settings.G, Settings.B);
            Controls.Add(label);
            label.Image = Board.Rboard(label.ClientSize, null);

            label.MouseClick += BoardClicked;


            // Window background
            background = new Label();
            background.Size = ClientSize;
            background.BackColor = Color.FromArgb(Settings.Br, Settings.Bg, Settings.Bb);
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

            if (game != null)
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

            if (game != null)
            {
                int bigger = 25;
                int R = r + bigger;

                if (game.CurrentPlayer == 1)
                    g.DrawEllipse(Settings.Wcircle, leftX - bigger / 2, centerY - bigger / 2, R, R);
                else if (game.CurrentPlayer == 2)
                    g.DrawEllipse(Settings.Bcircle, rightX - bigger / 2, centerY - bigger / 2, R, R);
            }
        }

        void DrawP(object s, PaintEventArgs pea)
        {
            ui.PPanel.Location = new Point(0, 0);
            DrawPlayers(pea);
        }

        void CenterUI()
        {
            if (ui == null)
                return;
            ui.UIPanel.Location = new Point((ClientSize.Width - ui.UIPanel.Width) / 2, Settings.Uheight / 10);
        }

        void StartNewGame(object sender, EventArgs e)
        {
            if (game != null)
            {
                game = null;
                ChangeFieldSize(null, new EventArgs());
            }

            game = new GameState(Settings.cells);
            ui.PPanel.Invalidate();
            RedrawBoard();
            if (Settings.VSAI && game.CurrentPlayer == 2)
                Reversi_AI.AIMove(game, ui);
        }

        void ShowHelp(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://bitflap.app/vspocket/articles/reversi-rules-complete-guide/", UseShellExecute = true });
        }

        void HintON(object sender, EventArgs e)
        {
            Settings.ShowHint = !Settings.ShowHint;
            RedrawBoard();
        }

        void VSAIOn(object sender, EventArgs e)
        {
            Settings.VSAI = ui.VSAIOn;
            if (game != null && game.CurrentPlayer == 2)
                Reversi_AI.AIMove(game, ui);
        }

        void ChangeFieldSize(object sender, EventArgs ea)
        {
            if (game != null)
                return;
            //Parse the number from the Box for FieldSize
            if (ui.FieldSizeBox.SelectedItem == null) return;
            string text = ui.FieldSizeBox.SelectedItem.ToString()!;
            Settings.cells = int.Parse(text.Split('x')[0]);
        }
        
        void ChangeColor(object sender, EventArgs e)
        {
            if (ui.ColorBox.SelectedItem == null) return;
            string text = ui.ColorBox.SelectedItem.ToString()!;

            Settings.ChangeTheme(text);

            if (text == "Classic Board")
                ui.HelpButton.ForeColor = Color.White;
            else if (text == "Red & Blue")
                ui.HelpButton.ForeColor = Color.Blue;
            else if (text == "Wood")
                ui.HelpButton.ForeColor = Color.FromArgb(76, 43, 32);

            ui.HelpButton.BackColor = Color.FromArgb(Settings.Br, Settings.Bg, Settings.Bb);
            background.BackColor = Color.FromArgb(Settings.Br, Settings.Bg, Settings.Bb);
            label.BackColor = Color.FromArgb(Settings.R, Settings.G, Settings.B);

            RedrawBoard();
        }

        void ChangeDifficulty(object sender, EventArgs e)
        {
            if (ui.DifficultyBox.SelectedItem == null) return;
            string text = ui.DifficultyBox.SelectedItem.ToString()!;
            Settings.ChangeLevel(text);
        }

        void BoardClicked(object sender, MouseEventArgs m)
        {
            if (game == null) return;
            if (ui.VSAIOn && game.CurrentPlayer == 2) return;

            //Scaling the game into a small grid
            int cellSize = label.Width / game.Size;

            //Scaling the mouse to the small grid
            int col = m.X / cellSize;
            int row = m.Y / cellSize;

            //Return if the mouse is not inside the board
            if (col < 0 || col >= game.Size) return;
            if (row < 0 || row >= game.Size) return;

            if (Game_rules.IsValidMove(game, row, col, game.CurrentPlayer))
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
                if (!Game_rules.MovePossible(game, game.CurrentPlayer))
                {
                    game.CurrentPlayer++;
                    if (game.CurrentPlayer > 2)
                        game.CurrentPlayer = 1;

                }

                if (Settings.VSAI && game.CurrentPlayer == 2)
                    Reversi_AI.AIMove(game, ui);

                RedrawBoard();
                CheckGameEnd();
            }

        }

        public static void CheckGameEnd()
        {
            if (game == null)
                return;

            bool WhiteHasMove = Game_rules.MovePossible(game, 1);
            bool BlackHasMove = Game_rules.MovePossible(game, 2);

            if (!WhiteHasMove && !BlackHasMove)
                ShowWinner();
        }

        static async void ShowWinner()
        {
            var scores = game.CountPoints();

            if (ui.ColorBox.SelectedItem == null) ui.ColorBox.SelectedItem = "Classic Board";
            string text = ui.ColorBox.SelectedItem.ToString()!;
            string message;
            string w = "White"; string b = "Black";

            if (text == "Classic Board")
            { w = "White"; b = "Black"; }

            else if (text == "Red & Blue")
            { w = "Blue"; b = "Red"; }

            else if (text == "Wood")
            { w = "Birch"; b = "Dark Oak"; }


            if (scores.WhiteP > scores.BlackP)
                message = $"{w} wins!\n\n {w}: {scores.WhiteP} \n {b}: {scores.BlackP}";

            else if (scores.WhiteP < scores.BlackP)
                message = $"{b} wins! \n\n {w}: {scores.WhiteP} \n {b}: {scores.BlackP}";

            else
                message = $"It's a draw! \n\n {w}: {scores.WhiteP} \n {b}: {scores.BlackP}";

            MessageBox.Show(message, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await Task.Delay(100);
            game = null;
        }

        public static void RedrawBoard()
        {
            if (game == null)
                label.Image = Board.Rboard(label.ClientSize, null);
            else
                label.Image = Board.Rboard(label.ClientSize, game);
        }

        // Resize logic
        protected override void OnResize(EventArgs e)
        {
            if (background != null && ClientSize.Height - Settings.Uheight > 100)
            {
                int z = ClientSize.Height - Settings.Uheight;
                background.Size = ClientSize;
                label.Location = new Point((ClientSize.Width - (ClientSize.Height - Settings.Uheight)) / 2, Settings.Uheight - 20);
                label.Size = new Size(z, z);

                r = ClientSize.Width / 10;

                ui.PPanel.Size = ClientSize;
                ui.PPanel.Invalidate();
                ui.HelpButton.Location = new Point(ClientSize.Width - 110, ClientSize.Height - 40);

                if (label.Location.X < ((ClientSize.Width / 7) - 100) / 2 + r + 20)
                {
                    int sideSpace = ClientSize.Width / 7;

                    int maxBoardWidth = ClientSize.Width - sideSpace * 2;
                    int maxBoardHeight = ClientSize.Height - Settings.Uheight;

                    int boardSize = Math.Min(maxBoardWidth, maxBoardHeight);

                    label.Location = new Point(sideSpace + (maxBoardWidth - boardSize) / 2, Settings.Uheight - 20);
                    label.Size = new Size(boardSize, boardSize);
                }

                label.Image = Board.Rboard(label.ClientSize, game);
            }
            if (ui != null)
                ui.PPanel.Invalidate();
            CenterUI();
            base.OnResize(e);
        }
    }
}

