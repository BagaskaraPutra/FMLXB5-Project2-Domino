using DominoConsole;
public class Program
{
	static void Main()
	{
		//GameStatus:NOTSTARTED
		//Input number of players & win score
		GameController gameController = new(numPlayers: 3, winScore: 100);
		
		//Players input id & name
		for (int i = 0; i < gameController.NumPlayers; i++)
		{
			Console.Write("Player {0} please input your name: ", i+1);
			string name = Console.ReadLine();
			// TODO: Check for whitespace & no letters input
			Player player = new();
			player.SetId(i+1);
			player.SetName(name);
			if(gameController.AddPlayer(player))
			{
				Console.WriteLine("Player {0}: {1} successfully added", player.GetId(), player.GetName());
			}
		}
		
		//Ready? Enter any key to continue ...
		
		//GameStatus:ONGOING
		bool winGame = false;
		while(!winGame)
		{
			foreach (IPlayer player in gameController.GetPlayers())
			{
				
				bool playerWinGame = false;
				if (gameController.CheckScore(player) >= gameController.WinScore)
				{
					playerWinGame = true;
				}
				winGame = winGame || playerWinGame;
			}
			// Start round
			// Choose first player to start
			//	Players receive random deck of dominoes
			foreach (IPlayer player in gameController.GetPlayers())
			{
				Console.WriteLine("Player {0} is filling own deck with cards from boneyard ... ", player.GetId());
				do
				{
					gameController.DrawRandomCard(player);
				}
				while(gameController.GetNumberOfCards(player) < gameController.MaxNumCardsPerPlayer);
				// for(int i=0; i<gameController.GetNumberOfCards(player); i++)
				// {
				// 	gameController.DrawRandomCard(player);
				// }
				gameController.ShowCards(player);
			}
			winGame = true; // TODO: Remove this. Only to break while loop & troubleshoot  
			
		//	Check player with largest double domino
		//	Player with largest double domino starts first
		//	Player Put Card  
		//	if no matching card, draw card until one of the sides matches either sides on table
		//	if either Player's card deck is empty ->
		//	round winner
		// 	GameStatus:ROUNDWIN
		//	reset round	
		}
		
		//GameStatus:GAMEWIN
	}
}