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

				Card putCard = new();
				IdNodeSuit targetIdNodeSuit = new();
				FirstPlayerPicksCardId(currentPlayer, cardsList, ref putCard);
				gameController.PutCard(currentPlayer, putCard);

				Dictionary<Card, HashSet<IdNodeSuit>> deckTableCompatible;

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

					deckTableCompatible = gameController.GetDeckTableCompatibleCards(cardsList, gameController.GetTargetNodes());
					if (deckTableCompatible.Count == 0)
					{
						// gameController.DrawRandomCard(currentPlayer) until tableCard is empty
						// if (tableCards is empty):
						DisplayLine("No possible moves");
					}
					else
					{
						List<KeyValuePair<Card, IdNodeSuit>> deckTableList = new();
						DisplayLine("Possible moves:");
						int i = 0;
						foreach (var kvp in deckTableCompatible)
						{
							foreach (var values in kvp.Value)
							{
								i++;
								deckTableList.Add(new(kvp.Key, values));
								// DisplayLine($"{i}. Deck card: [{kvp.Key.Head}|{kvp.Key.Tail}] put to LEFT/RIGHT");
								DisplayLine($"{i}. Deck card: [{kvp.Key.Head}|{kvp.Key.Tail}] put next to Table card id: {values.Id}, node: {values.Node}, suit: {values.Suit}");
							}
						}

						bool idStatus = false;
						int moveChoice;
						do
						{
							Display($"Put your card on the table by entering the number (1-{i}) from the above list ... ");
							idStatus = Int32.TryParse(ReadInput(), out moveChoice);
							if (moveChoice > 0 && moveChoice <= i)
							{
								putCard = cardsList.FirstOrDefault(x => x == deckTableList[moveChoice-1].Key);
								targetIdNodeSuit  = deckTableList[moveChoice-1].Value;
							}
							else
							{
								idStatus = false;
							}
							if (!idStatus)
							{
								DisplayLine("You did not input a valid choice!");
							}
						} while (!idStatus);
					}

					// NextPlayerPicksMove(currentPlayer, cardsList, deckTableCompatible, ref putCard);

					// Game Controller gives suggestion to the currentPlayer: 
					// 1. the deck card id that can be placed 
					// 2. and the table card id to be placed adjacent to OR
					//    the position on the table (Left OR Right)

					gameController.PutCard(currentPlayer, putCard, targetIdNodeSuit);	

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
	static void FirstPlayerPicksCardId(IPlayer currentPlayer, List<Card> cardsList, ref Card putCard)
	{
		bool status = false;
		int cardId;
		do
		{
			Display($"Player {currentPlayer.GetId()} ({currentPlayer.GetName()}), please enter the card id to be placed on the table ... ");
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
	}
	// static void NextPlayerPicksMove(IPlayer currentPlayer, 
	// 								List<Card> cardsList, 
	// 								Dictionary<Card, HashSet<IdNodeSuit>>  deckTableCompatible,
	// 								ref Card putCard)
	// {	
	// 	bool status = false;
	// 	bool idStatus = false;
	// 	int cardId;
	// 	do
	// 	{
	// 		Display($"Place your card by entering the number from the following list of options ... ");
	// 		int i = 0;
	// 		Card tableCard;
	// 		foreach(var kvp in deckTableCompatible)
	// 		{
	// 			foreach (var values in kvp.Value)
	// 			{
	// 				i++;
	// 				// DisplayLine($"Deck: id {kvp.Key.GetId()} -> Table id: {values.Id}, node: {values.Node}, suit: {values.Suit}");
	// 				tableCard = 
	// 				DisplayLine($"{i}. Deck card: [{kvp.Key.Head}|{kvp.Key.Tail}] put next to Table card: [] ");
	// 			}
	// 		}
	// 		idStatus = Int32.TryParse(ReadInput(), out cardId);
	// 		if (cardsList.Any(x => x.GetId() == cardId))
	// 		{
	// 			putCard = cardsList.FirstOrDefault(x => x.GetId() == cardId);
	// 		}
	// 		else
	// 		{
	// 			idStatus = false;
	// 		}
	// 		// if (openEndsDict.ContainsValue())
	// 		if (!idStatus)
	// 		{
	// 			DisplayLine("You did not input a valid card id!");
	// 		}
	// 	} while (!idStatus);
	// }
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
			// How to check which card is in the left side, which one is on the right side
			Display($"[{card.Head}|{card.Tail}]");
		}
		Display("\n");
	}
}