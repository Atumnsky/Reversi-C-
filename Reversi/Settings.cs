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

    }
}
