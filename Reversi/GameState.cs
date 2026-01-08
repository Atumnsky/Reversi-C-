namespace Reversi
{
    public class GameState
    {
        //Empty = 0
        //White = 1
        //Black = 2

        public int Size;

        // Makes the 2D array(board). One cell =  Board[row,column]
        public int[,] Board;

        public int CurrentPlayer;

        public GameState(int size)
        {
            //Creates the board according to the selected Board Size
            Size = size;
            Board = new int[Size, Size]; //new int values starts as 0

            //Random player on start
            Random rnd = new Random();
            CurrentPlayer = rnd.Next(1, 3);
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

        // Tuple for returning points of both Player
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
}
