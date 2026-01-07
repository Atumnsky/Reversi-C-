namespace Reversi
{
    public class Board
    {
        public static Bitmap Rboard(Size Bsize, GameState game)
        {
            Size bsize = Bsize;

            Bitmap bitmap = new Bitmap(Bsize.Width, Bsize.Height);
            Graphics gr = Graphics.FromImage(bitmap);

            DrawLines(gr);
            if (game != null)
            {
                DrawPieces(gr, game, Bsize);
                if (Settings.ShowHint)
                    DrawHint(gr, game, Bsize);
            }

            void DrawLines(Graphics gr)
            {
                Pen linesColor = new Pen(Color.FromArgb(Settings.r, Settings.g, Settings.b), Settings.thickness);

                Brush blackP = new SolidBrush(Color.Black);
                Brush whiteP = new SolidBrush(Color.White);

                int cellSize = Bsize.Width / Settings.cells;

                int r = cellSize - 10;

                for (int i = 0; i <= Settings.cells; i++)
                {
                    gr.DrawLine(linesColor, cellSize, 0, cellSize, Bsize.Height);
                    gr.DrawLine(linesColor, 0, cellSize, Bsize.Width, cellSize);
                    cellSize += Bsize.Width / Settings.cells;
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
                int cellSize = Bsize.Width / game.Size;
                int x;
                int y;

                //Loop through the board and draw the pieces
                for (int row = 0; row < game.Size; row++)
                {
                    for (int col = 0; col < game.Size; col++)
                    {
                        int value = game.Board[row, col];
                        if (game.Board[row, col] != 0)
                        {
                            x = col * cellSize + ((cellSize * 6) / 100);
                            y = row * cellSize + ((cellSize * 6) / 100);

                            if (value == 1)
                                gr.FillEllipse(Settings.White, x, y, (cellSize * 90) / 100, (cellSize * 90) / 100);
                            else if (value == 2)
                                gr.FillEllipse(Settings.Black, x, y, (cellSize * 90) / 100, (cellSize * 90) / 100);

                        }
                    }
                }
            }

            void DrawHint(Graphics gr, GameState game, Size Bsize)
            {
                int cellSize = Bsize.Width / game.Size;
                int x;
                int y;

                for (int row = 0; row < game.Size; row++)
                {
                    for (int col = 0; col < game.Size; col++)
                    {
                        if (game.Board[row, col] != 0)
                            continue;

                        if (!Game_rules.IsValidMove(game, row, col, game.CurrentPlayer))
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
}
