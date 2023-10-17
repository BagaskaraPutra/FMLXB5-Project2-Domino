using System.Text.Json;
using CsvHelper;
using System.Globalization;
using DominoConsole;

namespace Program;

public partial class Program
{
	private static GameController? game;
	private static IPlayer? currentPlayer;
	private static List<Card>? cardsList;
	private static Card? playerCard;
	private static HashSet<IdNodeSuit>? openNodes;
	private static IdNodeSuit targetIdNodeSuit;
	private static List<KeyValuePair<Card, IdNodeSuit>>? deckTableCompatible;
	private static DominoTree? dominoTree;
	private static CardGUI? cardGUI;
	private static TableGUI? tableGUI;
	private static List<CardKinematics>? cardKinematicsLUT;
	static void Main()
	{
		//Load config

		using (var reader = new StreamReader("config/DominoCardKinematicsLookupTable.csv"))
		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			csv.Context.RegisterClassMap<CardKinematicsMap>();
			cardKinematicsLUT = csv.GetRecords<CardKinematics>().ToList();
		}
		dominoTree = new(cardKinematicsLUT);
		// int idxLUT = 0;
		// foreach (var ck in cardKinematicsLUT)
		// {
		// 	idxLUT++;
		// 	Console.WriteLine($"{idxLUT}. P_IsDouble: {ck.ParentIsDouble}, C_IsDouble: {ck.CurrentIsDouble}, P_Node {ck.ParentNode}, C_Node: {ck.CurrentNode}, P_Ori: {ck.ParentOrientation}, C_Ori: {ck.CurrentOrientation}, C_Move: {ck.MoveUntilEdge}");
		// }
		
		using (StreamReader fs = new StreamReader(@"config/cardGUI.json"))
		{
			string cardGUIJson = fs.ReadToEnd();
			cardGUI = JsonSerializer.Deserialize<CardGUI>(cardGUIJson);
		}
		cardGUI.UpdateConfig();
		tableGUI = new();
		
		//GameStatus:NOTSTARTED
		//Input number of players & win score
		game = new(numPlayers: 4, maxWinScore: 100);
		
		//Players input id & name
		for (int i = 0; i < game.NumPlayers; i++)
		{
			bool isBlankOrNoLetters = true;
			string inputName = "";
			do
			{
				Display($"Player {i + 1} please input your name: ");
				inputName = ReadInput();
				if(string.IsNullOrWhiteSpace(inputName))
				{
					DisplayLine("Invalid player name! You cannot leave your name blank");
				}
				else if(!inputName.Any(char.IsLetter))
				{
					DisplayLine("Invalid player name! Your name must contain alphabet letters");
				}
				else
				{
					isBlankOrNoLetters = false;
				}
			}while(isBlankOrNoLetters);
			Player player = new(id: i+1, name: inputName);
			if (game.AddPlayer(player))
			{
				DisplayLine($"Player {player.GetId()} ({player.GetName()}) successfully added");
			}
		}

		//Ready? Enter any key to continue ...

		//GameStatus:ONGOING
		while (!game.IsWinGame())
		{
			// Start round
			do
			{
				DisplayLine($"\n<<<<< ROUND {game.Round} BEGIN! >>>>>");
				
				// Choose first player to start
				currentPlayer = game.GetFirstPlayer();

				//	Players receive random deck of dominoes
				foreach (IPlayer player in game.GetPlayers())
				{
					DisplayLine($"Player {player.GetId()} ({player.GetName()}) is filling own deck with cards from boneyard ... ");
					do
					{
						game.DrawRandomCard(player);
					}
					while (game.GetNumberOfCards(player) < game.MaxNumCardsPerPlayer);
					DisplayDeckCards(game.GetPlayerCards(player));
				}

				//	First Player puts first card in the middle
				Display("\n");
				DisplayLine($"Player {currentPlayer.GetId()} ({currentPlayer.GetName()}) starts first!");
				cardsList = game.GetPlayerCards(currentPlayer);
				DisplayLine("Here are your available cards in your deck ...");
				DisplayDeckCards(cardsList);

				playerCard = new();

				FirstPlayerPicksCardId(currentPlayer, cardsList, ref playerCard);
				game.PutCard(currentPlayer, playerCard);

				// TODO: We have check IsWinRound() here & the outer while loop. Is it redundant?
				// while (gameController.CheckGameStatus() == GameStatus.ONGOING)
				while (!game.IsWinRound())
				{
					dominoTree.UpdateTree(game.GetTableCards());
					
					Display("\n");
					DisplayLine("Table Cards:");
					DisplayTableCards(dominoTree);

					currentPlayer 	= game.GetNextPlayer();
					cardsList 		= game.GetPlayerCards(game.GetCurrentPlayer());
					
					DisplayLine($"Player {currentPlayer.GetId()} {currentPlayer.GetName()}'s turn");
					DisplayLine("Here are your available cards in your deck ...");
					DisplayDeckCards(cardsList);
					
					openNodes 			= game.GetOpenNodes();
					deckTableCompatible = game.GetDeckTableCompatible(currentPlayer, openNodes);

					//	If no matching card, draw card until one of the sides matches either sides on table
					if (deckTableCompatible.Count == 0)
					{
						DisplayLine($"[WARNING!] Player {currentPlayer.GetId()} {currentPlayer.GetName()} doesn't have a valid move.");
						// BUG: When enter this if, the recently placed table card duplicates itself
						do
						{
							game.DrawRandomCard(currentPlayer);
							cardsList 			= game.GetPlayerCards(currentPlayer);
							deckTableCompatible = game.GetDeckTableCompatible(currentPlayer, openNodes);
							if (game.NumBoneyardCards() == 0)
							{
								DisplayLine("[WARNING!] Boneyard card pile is empty! No possible moves");
								game.SetPlayerStatus(currentPlayer, PlayerStatus.PASS);
								break;
							}
							else
							{
								DisplayLine($"Player {currentPlayer.GetId()} {currentPlayer.GetName()} is drawing card from boneyard pile to the deck ...");
								DisplayDeckCards(cardsList);
							}
						}
						while (deckTableCompatible.Count == 0);
					}

					currentPlayer = game.GetCurrentPlayer();
					// Only proceed when CardStatus is not PASS
					if (game.CheckPlayerStatus(currentPlayer) != PlayerStatus.PASS)
					{
						deckTableCompatible = game.GetDeckTableCompatible(currentPlayer, openNodes);
						DisplayLine("Possible moves:");
						int i = 0;
						foreach (var kvp in deckTableCompatible)
						{
							i++;
							// DisplayLine($"{i}. Deck card (id: {kvp.Key.GetId()}): [{kvp.Key.Head}|{kvp.Key.Tail}] put next to -> Table card id: {kvp.Value.Id}, node: {kvp.Value.Node}, suit: {kvp.Value.Suit}");
							// DisplayLine($"{i}. Deck card [{kvp.Key.Head}|{kvp.Key.Tail}] (id: {kvp.Key.GetId()}) put next to -> Table card [{gameController.GetCardFromId(kvp.Value.Id).Head}|{gameController.GetCardFromId(kvp.Value.Id).Tail}] (id: {kvp.Value.Id}) at {kvp.Value.Node} node");
							DisplayLine($"{i}. Deck card [{kvp.Key.Head}|{kvp.Key.Tail}] (id: {kvp.Key.GetId()}) put next to -> Table card [{game.GetCardFromId(kvp.Value.Id).Head}|{game.GetCardFromId(kvp.Value.Id).Tail}] at {kvp.Value.Node} node");
							//TODO: Show {kvp.Value.Node} in TableGUI
						}

						bool status = false;
						int moveChoice;
						do
						{
							Display($"Put your card on the table by entering the number (1-{deckTableCompatible.Count}) from the above list ... ");
							status = Int32.TryParse(ReadInput(), out moveChoice);
							if (moveChoice > 0 && moveChoice <= deckTableCompatible.Count)
							{
								playerCard = deckTableCompatible[moveChoice - 1].Key;
								targetIdNodeSuit = deckTableCompatible[moveChoice - 1].Value;
							}
							else
							{
								DisplayLine("You did not input a valid choice!");
								status = false;
							}
						} while (!status);

						// Game Controller gives suggestion to the currentPlayer: 
						// 1. the deck card id that can be placed 
						// 2. and the table card id to be placed adjacent to OR
						//    the position on the table (Left OR Right)

						game.PutCard(currentPlayer, playerCard, targetIdNodeSuit);
					}
				}
			}
			while (!game.IsWinRound());
			// TODO: We have check IsWinRound() here & the inner while loop. Is it redundant?
			
			// 	GameStatus:ROUNDWIN
			DisplayLine($"\nRound {game.Round} ended!");
			
			// Display players' remaining cards
			foreach (IPlayer player in game.GetPlayers())
			{
				DisplayLine($"Player {player.GetId()} {player.GetName()}'s remaining cards:");
				DisplayDeckCards(game.GetPlayerCards(player));
				Display("\n");
			}
			
			//  Calculate round score
			IPlayer roundWinner = game.GetRoundWinner();
			
			//	if either Player's card deck is empty OR no more valid move -> Round winner
			Display($"[ROUND {game.Round} WINNER!]: ");
			DisplayLine($"Player {roundWinner.GetId()} {roundWinner.GetName()} wins round {game.Round}");
			Display("\n");
			DisplayLine("Cumulative round players' score:");
			foreach (IPlayer player in game.GetPlayers())
			{
				DisplayLine($"Player {player.GetId()} {player.GetName()}'s score: {game.CheckScore(player)}");
			}

			//	Reset round	
			game.ResetRound();
		}

		//GameStatus:GAMEWIN
		DisplayLine("Domino Game Finished! Thank you for playing");
	}
	static void FirstPlayerPicksCardId(IPlayer currentPlayer, List<Card> cardsList, ref Card desiredCard)
	{
		bool status = false;
		do
		{
			Display($"Player {currentPlayer.GetId()} ({currentPlayer.GetName()}), please enter the card id to be placed on the table ... ");
			status = Int32.TryParse(ReadInput(), out int cardId);
			if (cardsList.Any(x => x.GetId() == cardId))
			{
				desiredCard = cardsList.FirstOrDefault(x => x.GetId() == cardId);
			}
			else
			{
				DisplayLine("You did not input a valid card id!");
				status = false;
			}
		} while (!status);
	}
	
}