namespace Reversi
{
    public class Reversi_AI
    {
        public static int MoveWeight(GameState game, int row, int col)
        {
            int end = game.Size - 1;

            //Corner has max weight
            if ((row == 0 && col == 0) || (row == 0 && col == end) || (row == end && col == 0) || (row == end && col == end))
                return Settings.Weight1;
            //Edges has 2nd most weight
            else if (row == 0 || row == end || col == 0 || col == end)
                return Settings.Weight2;
            //Closer to edge has more weight
            else if (row == 1 || row == end - 1 || col == 1 || col == end - 1)
                return Settings.Weight3;
            else if (row == 2 || row == end - 2 || col == 2 || col == end - 2)
                return Settings.Weight4;
            //Inner moves has the least weight
            else
                return Settings.Weight5;
        }
        public static async void AIMove(GameState game, UI ui)
        {
            if (game == null)
                return;

            //Async used for simulating response delay
            Random time = new Random();
            int delay = time.Next(400, 1000);
            await Task.Delay(delay);
            if (game == null)
                return;
            if (game.CurrentPlayer != 2)
                return;

            //Sum up the weights, and pick a random number in the weights
            //Count total weight
            int totalWeight = 0;
            for (int row = 0; row < game.Size; row++)
            {
                for (int col = 0; col < game.Size; col++)
                {
                    if (Game_rules.IsValidMove(game, row, col, 2))
                        totalWeight += MoveWeight(game, row, col);
                }
            }

            if (totalWeight == 0)
                return;

            Random rnd = new Random();
            int Move = rnd.Next(totalWeight);

            //AI play
            int i = 0;
            for (int row = 0; row < game.Size; row++)
            {
                for (int col = 0; col < game.Size; col++)
                {
                    if (Game_rules.IsValidMove(game, row, col, 2))
                    {
                        //If the number is less than the weight of lets say edge and greater than the weight of inner, it will pick the edge
                        i += MoveWeight(game, row, col);

                        if (Move < i)
                        {
                            game.Board[row, col] = 2;
                            Game_rules.FlipPieces(game, row, col);

                            await Task.Delay(100);

                            game.CurrentPlayer = 1;
                            ui.PPanel.Invalidate();
                            Window.RedrawBoard();
                            Window.CheckGameEnd();

                            if (!Game_rules.MovePossible(game, 1))
                            {
                                game.CurrentPlayer = 2;
                                AIMove(game, ui);
                            }

                            return;
                        }
                    }
                }
            }
        }
    }
}
