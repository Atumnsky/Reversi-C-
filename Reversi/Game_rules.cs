namespace Reversi
{
    public class Game_rules
    {
        public static bool IsValidMove(GameState game, int row, int col, int player)
        {
            //Cannot play in a cell that not empty
            if (game.Board[row, col] != 0)
                return false;

            //Create opponent
            int opponent;
            if (player == 1)
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
                    ; int nextY = y;

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
                    if (IsValidMove(game, row, col, player))
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
}
