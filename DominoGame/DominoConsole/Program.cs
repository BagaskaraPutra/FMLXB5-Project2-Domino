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

				// Reusable local variables
				Card putCard = new();
				IdNodeSuit targetIdNodeSuit = new();
				HashSet<IdNodeSuit> openEnds;
				List<KeyValuePair<Card, IdNodeSuit>> deckTableCompatible;

				FirstPlayerPicksCardId(currentPlayer, cardsList, ref putCard);
				gameController.PutCard(currentPlayer, putCard);

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
					openEnds = gameController.GetTargetNodes();
					deckTableCompatible = gameController.GetDeckTableCompatibleCards(cardsList, openEnds);

					if (deckTableCompatible.Count == 0)
					{
						DisplayLine($"[WARNING!] Player {currentPlayer.GetId()} {currentPlayer.GetName()} doesn't have a valid move.");
						// BUG: When enter this if, the recently placed table card duplicates itself
						do
						{
							gameController.DrawRandomCard(currentPlayer);
							cardsList = gameController.GetPlayerCards(currentPlayer);
							deckTableCompatible = gameController.GetDeckTableCompatibleCards(cardsList, openEnds);
							DisplayLine($"Player {currentPlayer.GetId()} {currentPlayer.GetName()} is drawing card from boneyard pile to the deck ...");
							DisplayDeckCards(cardsList);
							if (gameController.NumBoneyardCards() == 0)
							{
								DisplayLine("No possible moves");
								// TODO: Set CardStatus to pass 
								break;
							}
						}
						while (deckTableCompatible.Count == 0);
					}
					
					DisplayLine("Possible moves:");
					int i = 0;
					foreach (var kvp in deckTableCompatible)
					{
						i++;
						DisplayLine($"{i}. Deck card (id: {kvp.Key.GetId()}): [{kvp.Key.Head}|{kvp.Key.Tail}] put next to -> Table card id: {kvp.Value.Id}, node: {kvp.Value.Node}, suit: {kvp.Value.Suit}");
					}

					bool status = false;
					int moveChoice;
					do
					{
						Display($"Put your card on the table by entering the number (1-{deckTableCompatible.Count}) from the above list ... ");
						status = Int32.TryParse(ReadInput(), out moveChoice);
						if (moveChoice > 0 && moveChoice <= deckTableCompatible.Count)
						{
							putCard = deckTableCompatible[moveChoice - 1].Key;
							targetIdNodeSuit = deckTableCompatible[moveChoice - 1].Value;
						}
						else
						{
							status = false;
						}
						if (!status)
						{
							DisplayLine("You did not input a valid choice!");
						}
					} while (!status);

					// NextPlayerPicksMove(currentPlayer, cardsList, deckTableCompatible, ref putCard);

					// Game Controller gives suggestion to the currentPlayer: 
					// 1. the deck card id that can be placed 
					// 2. and the table card id to be placed adjacent to OR
					//    the position on the table (Left OR Right)

					gameController.PutCard(currentPlayer, putCard, targetIdNodeSuit);

					// break; // TODO: Remove this. Only to break while loop & troubleshoot  
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