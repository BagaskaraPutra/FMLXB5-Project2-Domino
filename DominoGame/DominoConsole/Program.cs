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
			Display($"Player {i+1} please input your name: ");
			string name = Console.ReadLine();
			// TODO: Check for whitespace & no letters input
			Player player = new();
			player.SetId(i+1);
			player.SetName(name);
			if(gameController.AddPlayer(player))
			{
				DisplayLine($"Player {player.GetId()} ({player.GetName()}) successfully added");
			}
		}
		
		//Ready? Enter any key to continue ...
		
		//GameStatus:ONGOING
		bool winGame = false;
		IPlayer currentPlayer;
		List<Card> cardsList;
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
			currentPlayer = gameController.GetFirstPlayer();
			
			//	Players receive random deck of dominoes
			foreach (IPlayer player in gameController.GetPlayers())
			{
				DisplayLine($"Player {player.GetId()} ({player.GetName()}) is filling own deck with cards from boneyard ... ");
				do
				{
					gameController.DrawRandomCard(player);
				}
				while(gameController.GetNumberOfCards(player) < gameController.MaxNumCardsPerPlayer);
				gameController.ShowCards(player);
			}
			
			//	First Player puts first card in the middle
			Display("\n");
			DisplayLine($"Player {currentPlayer.GetId()} ({currentPlayer.GetName()}) starts first!");
			cardsList = gameController.GetPlayerCards(currentPlayer);
			DisplayLine("Here are your available cards in your deck ...");
			DisplayDeckCards(cardsList);
			DisplayLine($"Please enter the card id to be placed on the table ...");
			
			/*
			while(gameController.CheckGameStatus() == GameStatus.ONGOING)
			{
				currentPlayer = gameController.GetCurrentPlayer();
				// gameController.ShowCards(currentPlayer);
				gameController.GetNextPlayer();
				// gameController.PutCard(currentPlayer, )	
				if(gameController.GetTableCards().Count <= 0)
				{
					break;
				}
			}
			*/
			
			winGame = true; // TODO: Remove this. Only to break while loop & troubleshoot    
			
			//	if no matching card, draw card until one of the sides matches either sides on table
			//	if either Player's card deck is empty ->
			//	round winner
			// 	GameStatus:ROUNDWIN
			//	reset round	
		}
		
		//GameStatus:GAMEWIN
	}
	static void DisplayDeckCards(List<Card> cardsList)
	{
		int i;
		Display("        ");
		for (i=0; i<cardsList.Count; i++)
		{
			Display(" --- \t");
		}
		Display("\n");
		Display("        ");
		foreach (Card card in cardsList)
		{
			Display("| ");
			Display(card.Head);
			Display(" |\t");
		}
		Display("\n");
		Display("        ");
		for (i=0; i<cardsList.Count; i++)
		{
			Display("|---|\t");
		}
		Display("\n");
		Display("        ");
		foreach (Card card in cardsList)
		{
			Display("| ");
			Display(card.Tail);
			Display(" |\t");
		}
		Display("\n");
		Display("        ");
		for (i=0; i<cardsList.Count; i++)
		{
			Display(" --- \t");
		}
		Display("\n");
		Display("card id:");
		foreach (Card card in cardsList)
		{
			Display("  ");
			Display(card.GetId());
			Display("\t");
		}
		Display("\n");
	}
	static void Display<T>(T content)
	{
		Console.Write(content);
	}
	static void DisplayLine<T>(T content)
	{
		Console.WriteLine(content);
	}
	static void DisplayTableCards(List<Card> tableCards)
	{
		foreach (var card in tableCards)
		{
			//TODO: How to render domino cards on console terminal >:-(
		}
	}
}