
using System.Diagnostics;

namespace Reversi
{
    public class Settings
    {
        // Window size.
        public static int Wwidth = 800;
        public static int Wheight = 800;

        // Board size.
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


    public class Window : Form
    {
        Point labelLocation;
        Label label;
        Label background;

        Font Arial = new Font("Arial Black", 10, FontStyle.Regular);
        Button newGame;
        ComboBox Field;

        public Window()
        {
            ClientSize = new Size(Settings.Wwidth, Settings.Wheight);
            Text = "Reversi C#";

            labelLocation = new Point((Settings.Wwidth/2)-((Settings.Wheight-Settings.Uheight)/2), Settings.Uheight);

            ////////////////////////// UI controls
            //New Game
            newGame = new Button();
            newGame.Location = new Point(labelLocation.X, Settings.Uheight-130);
            newGame.Text = "New Game";
            newGame.Font = Arial;
            newGame.Size = new Size(100, 30);
            Controls.Add(newGame);

            // Field Size
            Field = new ComboBox();
            Field.Location = new Point(labelLocation.X + 120, Settings.Uheight - 128);
            Field.Text = "Field Size";
            Field.Font = Arial;
            Field.Items.Add("4x4");
            Field.Items.Add("6x6");
            Field.Items.Add("8x8");
            Field.Items.Add("10x10");
            Controls.Add(Field);

            //

            ///////////////////////////////
            // Reversi board
            label = new Label();
            label.Location = labelLocation;
            label.Size = new Size(Settings.Wheight-Settings.Uheight, Settings.Wheight-Settings.Uheight);
            label.BackColor = Color.FromArgb(Settings.R,Settings.G, Settings.B);
            Controls.Add(label);
            label.Image = Board.Rboard(label.ClientSize);

            // Window background
            background = new Label();
            background.Size = ClientSize;
            background.BackColor = Color.FromArgb(Settings.Br,Settings.Bg,Settings.Bb);
            Controls.Add(background);

        }
        public void FieldSize()
        {
            if (Field.SelectedIndex > -1)
            {
                Settings.cells = 8;
                Invalidate();
            }
        }

        // Resize logic
        protected override void OnResize(EventArgs e)
        {
            if (background != null && ClientSize.Height-Settings.Uheight > 100)
            {
                background.Size = ClientSize;
                label.Location = new Point((ClientSize.Width - (ClientSize.Height-Settings.Uheight)) / 2, Settings.Uheight);
                label.Size = new Size(ClientSize.Height-Settings.Uheight,ClientSize.Height-Settings.Uheight);

                newGame.Location = new Point((ClientSize.Width - (ClientSize.Height - Settings.Uheight)) / 2, Settings.Uheight - 130);
                Field.Location = new Point(((ClientSize.Width - (ClientSize.Height - Settings.Uheight)) / 2)+120, Settings.Uheight - 128);

                if (ClientSize.Width < ClientSize.Height - Settings.Uheight)
                {
                    label.Size = new Size(ClientSize.Width, ClientSize.Width);
                    label.Location = new Point((ClientSize.Width-label.Width)/2,Settings.Uheight);

                    newGame.Location = new Point((ClientSize.Width - label.Width) / 2, Settings.Uheight - 130);
                    Field.Location = new Point(((ClientSize.Width - label.Width) / 2)+120, Settings.Uheight - 128);
                }

                label.Image = Board.Rboard(label.ClientSize);
            }
            base.OnResize(e);
        }
    }

    // zonder variabelen static maken
    public class Board
    {
        public static Bitmap Rboard(Size Bsize)
        {
            Bitmap bitmap = new Bitmap(Bsize.Width,Bsize.Height);
            Graphics gr = Graphics.FromImage(bitmap);

            DrawLines(gr);
        
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

                //gr.FillEllipse(blackP, (Bsize.Width / 2)+5, (Bsize.Height / 2)+5, r, r);
            
            }

            return bitmap;
        }
    }

    public class UI
    {
        public static void StartGame(object sender, EventArgs ea)
        {
            //Board.Rboard();
        }
    }

    class Pieces
    {
        public void DrawP(object s, PaintEventArgs pea, int x, int y)
        {
            Size cell_Size = new Size(Settings.Wheight - Settings.Uheight, Settings.Wheight - Settings.Uheight);
            int r = cell_Size.Width - 10;

            Brush blackP = new SolidBrush(Color.Black);
            Brush whiteP = new SolidBrush(Color.White);

            Graphics g = pea.Graphics;

            g.FillEllipse(whiteP, x/2, y/2, r, r);
        }
    }

    class Game_logic
    { 
        
    }

    class Program
    {
        static void Main()
        {

            Application.Run(new Window());
        }

    }
    
}