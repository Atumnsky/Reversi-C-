using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

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
        public static int circleThick = 10;

        public static Brush White = new SolidBrush(Color.White);
        public static Brush Black = new SolidBrush(Color.Black);

        public static Pen Wcircle = new Pen(Color.White, circleThick);
        public static Pen Bcircle = new Pen(Color.Black, circleThick);

        public static Brush hint = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
        public static bool ShowHint = false;

        public static bool VSAI = false;

        //AI weight
        public static int Weight1;
        public static int Weight2;
        public static int Weight3;
        public static int Weight4;
        public static int Weight5;

        public static void ChangeTheme(string text)
        {
            if (text == "Classic Board")
            {
                Br = 10;
                Bg = 70;
                Bb = 50;

                R = 60;
                G = 179;
                B = 113;

                White = new SolidBrush(Color.White);
                Black = new SolidBrush(Color.Black);

                Wcircle = new Pen(Color.White, circleThick);
                Bcircle = new Pen(Color.Black, circleThick);

            }
            else if (text == "Red & Blue")
            {
                Br = 200;
                Bg = 200;
                Bb = 200;

                R = 140;
                G = 140;
                B = 140;

                White = new SolidBrush(Color.Blue);
                Black = new SolidBrush(Color.Red);

                Wcircle = new Pen(Color.Blue, circleThick);
                Bcircle = new Pen(Color.Red, circleThick);

                hint = new SolidBrush(Color.FromArgb(180, 180, 180));
            }
            else if (text == "Wood")
            {
                Br = 186;
                Bg = 140;
                Bb = 99;

                R = 193;
                G = 154;
                B = 107;

                White = new SolidBrush(Color.FromArgb(238, 213, 174));
                Black = new SolidBrush(Color.FromArgb(76, 43, 32));

                Wcircle = new Pen(Color.FromArgb(238, 213, 174), circleThick);
                Bcircle = new Pen(Color.FromArgb(76, 43, 32), circleThick);
            }
        }

        public static void ChangeLevel(string text)
        {
            if (text == "Easy")
            {
                Weight1 = Weight2 + 2;
                Weight2 = Weight3 + 1;
                Weight3 = Weight4 + 1;
                Weight4 = Weight5 + 1;
                Weight5 = 5;
            }

            else if (text == "Medium")
            {
                Weight1 = Weight2 + 120;
                Weight2 = Weight3 + 70;
                Weight3 = Weight4 + 30;
                Weight4 = Weight5 + 15;
                Weight5 = 3;
            }

            else if (text == "Hard")
            {
                Weight1 = Weight2 + 200;
                Weight2 = Weight3 + 150;
                Weight3 = Weight4 + 40;
                Weight4 = Weight5 + 20;
                Weight5 = 1;
            }
        }
    }
}
