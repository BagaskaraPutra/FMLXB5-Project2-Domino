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
			Display($"Player {i + 1} please input your name: ");
			string name = Console.ReadLine();
			// TODO: Check for whitespace & no letters input
			Player player = new();
			player.SetId(i + 1);
			player.SetName(name);
			if (gameController.AddPlayer(player))
			{
				DisplayLine($"Player {player.GetId()} ({player.GetName()}) successfully added");
			}
		}

		//Ready? Enter any key to continue ...

		//GameStatus:ONGOING
		IPlayer currentPlayer;
		List<Card> cardsList;
		while (!gameController.IsWinGame())
		{
			// Start round
			do
			{
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
					while (gameController.GetNumberOfCards(player) < gameController.MaxNumCardsPerPlayer);
					gameController.ShowCards(player);
				}

				//	First Player puts first card in the middle
				Display("\n");
				DisplayLine($"Player {currentPlayer.GetId()} ({currentPlayer.GetName()}) starts first!");
				cardsList = gameController.GetPlayerCards(currentPlayer);
				DisplayLine("Here are your available cards in your deck ...");
				DisplayDeckCards(cardsList);
				bool status = false;
				Card putCard = new();
				int cardId;
				do
				{
					Display($"Please enter the card id to be placed on the table ... ");
					status = Int32.TryParse(ReadInput(), out cardId);
					if (cardsList.Any(x => x.GetId() == cardId))
					{
						putCard = cardsList.FirstOrDefault(x => x.GetId() == cardId);
					}
					else
					{
						status = false;
					}
					if (!status)
					{
						DisplayLine("You did not input a valid card id!");
					}
				} while (!status);
				
				gameController.PutCard(currentPlayer, putCard);
				Dictionary<Card, List<Node>> openEndsDict;
				
				while (gameController.CheckGameStatus() == GameStatus.ONGOING)
				{
					Display("\n");
					DisplayLine("Table Cards:");
					DisplayTableCards(gameController.GetTableCards());
					
					currentPlayer = gameController.GetNextPlayer();
					cardsList = gameController.GetPlayerCards(currentPlayer);
					Display("\n");
					DisplayLine($"Player {currentPlayer.GetId()} {currentPlayer.GetName()}'s turn");
					DisplayLine("Here are your available cards in your deck ...");
					DisplayDeckCards(cardsList);
					
					openEndsDict = gameController.GetNodesToPlace();
					// Game Controller gives suggestion to the currentPlayer: 
					// 1. the deck card id that can be placed 
					// 2. and the table card id to be placed adjacent to OR
					//    the position on the table (Left OR Right)
					
					// gameController.PutCard(currentPlayer, deckCardId, tableCardId)	
					
					break; // TODO: Remove this. Only to break while loop & troubleshoot  
				}
				break; // TODO: Remove this. Only to break while loop & troubleshoot
			}
			while (!gameController.IsWinRound());

			break; // TODO: Remove this. Only to break while loop & troubleshoot    

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
		for (i = 0; i < cardsList.Count; i++)
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
		for (i = 0; i < cardsList.Count; i++)
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
		for (i = 0; i < cardsList.Count; i++)
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
	static string? ReadInput()
	{
		return Console.ReadLine();
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
			Display($"[{card.Head}|{card.Tail}]");
		}
		Display("\n");
	}
}