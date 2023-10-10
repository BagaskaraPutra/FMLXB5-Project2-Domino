using System.Data;
using System.Dynamic;

namespace DominoConsole;

public class GameController
{
	public int NumPlayers { get; private set; }
	public int Round;
	//TODO: Is this the current round? 
	// Is it a public property (with get-set methods)?
	public readonly int MaxNumCardsPerPlayer;

	private IPlayer _currentPlayer;
	private List<IPlayer> _playersList;
	private Dictionary<IPlayer, List<Card>?> _playerCardDict;
	private Dictionary<IPlayer, int> _playerScoreDict;
	private Dictionary<IPlayer, CardStatus> _playerStatusDict;

	private List<Card> _defaultCards;
	// a list of default cards for template only

	private List<Card> _boneyardCards;
	// cards on the table not yet picked by players

	private List<Card> _tableCards;
	// cards on the table already placed by the players

	private BinaryTree<Card> _tableTree;
	// binary tree of card on the table

	// private Deck _deckcard;
	// cards on each player's deck
	// TODO: Redundant because already in _playerCardDict

	private HashSet<IdNodeSuit>? _openEndsSet;
	// dictionary of tableCards and their nodes that have open ends (can be placed with a card)

	private List<KeyValuePair<Card, IdNodeSuit>> _compatibleList;
	
	
	private Random _random;

	private GameStatus _gameStatus;
	public readonly int WinScore;
	// if one of the players reaches this score, then the game is finished

	public GameController(int numPlayers, int winScore)
	{
		_gameStatus = GameStatus.NOTSTARTED;
		NumPlayers = numPlayers;
		WinScore = winScore;

		if (numPlayers > 2)
		{
			MaxNumCardsPerPlayer = 5;
		}
		else
		{
			MaxNumCardsPerPlayer = 7;
		}

		_playersList = new();
		_playerCardDict = new();
		_playerScoreDict = new();
		_playerStatusDict = new();
		_openEndsSet = new();

		_tableCards = new();
		_compatibleList = new();
		InitializeDefaultCards();
		_boneyardCards = new List<Card>(_defaultCards);
		_random = new Random();
	}

	private void InitializeDefaultCards()
	{
		_defaultCards = new();
		int id = 0;
		for (int h = 0; h < 7; h++)
		{
			for (int t = 0; t <= h; t++)
			{
				_defaultCards.Add(new Card(id, h, t));
				// Console.WriteLine("id: {0},\t head: {1},\t tail: {2}", id,h,t);
				id++;
			}
		}
	}
	public void DrawRandomCard(IPlayer player)
	{
		_playerStatusDict[player] = CardStatus.TAKECARD;
		int index = _random.Next(_boneyardCards.Count);
		// TODO: Unhandled exception. System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
		_playerCardDict[player].Add(_boneyardCards[index]);
		_boneyardCards.RemoveAt(index);
	}
	public int GetNumberOfCards(IPlayer player)
	{
		return _playerCardDict[player].Count;
	}
	public IPlayer GetFirstPlayer()
	{
		int[] sumArray = new int[NumPlayers];
		int i = 0;
		foreach (IPlayer player in _playersList)
		{
			DrawRandomCard(player);
			int sum = _playerCardDict[player].FirstOrDefault().GetHeadTailSum();
			sumArray[i] = sum;
			i++;
		}
		int p = Array.IndexOf(sumArray, sumArray.Max());
		// Console.WriteLine("Player {0} has largest sum of head & tail", _playersList[p].GetId());
		_gameStatus = GameStatus.ONGOING;
		_currentPlayer = _playersList[p];
		return _playersList[p];
	}
	public bool AddPlayer(IPlayer player)
	{
		bool successAddToCardDict = _playerCardDict.TryAdd(player, new());
		bool successAddToScore = _playerScoreDict.TryAdd(player, 0);
		bool successAddToStatus = _playerStatusDict.TryAdd(player, 0);
		_playersList.Add(player);
		return successAddToCardDict && successAddToScore && successAddToStatus;
	}
	public IPlayer GetPlayer(int id)
	{
		return _playersList.FirstOrDefault(n => n.GetId() == id);
	}
	public List<IPlayer> GetPlayers()
	{
		return _playersList;
	}
	public bool GetOrientation(bool IsDouble)
	{
		//TODO: What is IsDouble?	
		// This checks the possible orientation of the card that is going to be placed
		return false;
	}
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
	// public CardStatus CheckSetCardStatus()
	public CardStatus CheckPlayerStatus(IPlayer player)
	{
		return _playerStatusDict[player];
	}
	public void SetPlayerStatus(IPlayer player, CardStatus status)
	{
		_playerStatusDict[player] = status;
	}
	// public bool SetNextTurn(IPlayer player, List<Card> cardHand)
	// {
	// 	//TODO: What does this method do? Why does it need the cardHand parameter?
	// 	// Answer: cardHand is redundant because it is already contained in _playerCardDict
	// 	return false;
	// }
	public IPlayer GetCurrentPlayer()
	{
		return _currentPlayer;
	}
	public IPlayer GetNextPlayer()
	{
		int currentIndex = _playersList.FindIndex(a => a.Equals(_currentPlayer));
		if (currentIndex == (_playersList.Count - 1))
		{
			_currentPlayer =  _playersList[0];
			return _currentPlayer;
		}
		else
		{
			_currentPlayer = _playersList[currentIndex + 1];
			return _currentPlayer;
		}
	}
	public void PutCard(IPlayer player, Card card)
	{
		_playerStatusDict[player] = CardStatus.SETCARD;
		_tableCards.Add(card);
		_playerCardDict[player].Remove(card);
	}
	public bool PutCard(IPlayer player, Card card, IdNodeSuit targetINS)
	{
		//(DONE): In the GetTargetNodes() method, TryAdd & Add always adds new cards to the HashSet.
		// What happens when the open-ended node is attached with a card from the player's deck?
		// Delete the open-ended IdNodeSuit when adding player's card to the tableCard.
		
		_playerStatusDict[player] = CardStatus.SETCARD;
		_tableCards.Add(card);
		Card targetCard = _tableCards.FirstOrDefault(x => x.GetId()==targetINS.Id);
		targetCard.SetCardIdAtNode(card.GetId(), targetINS.Node);
		NodeEnum cardNode = new();
		if(card.IsDouble())
		{
			if(card.Head == targetINS.Id)
			{
				cardNode = NodeEnum.LEFT;
			}
			else if(card.Tail == targetINS.Id)
			{
				cardNode = NodeEnum.RIGHT;
			}
		}
		else
		{
			if(card.Head == targetINS.Id)
			{
				cardNode = NodeEnum.FRONT;
			}
			else if(card.Tail == targetINS.Id)
			{
				cardNode = NodeEnum.BACK;
			}
		}
		_tableCards.FirstOrDefault(x => x==card).SetCardIdAtNode(targetINS.Id, cardNode);
		_openEndsSet.Remove(targetINS);
		// _compatibleList.Remove(new(targetCard,targetINS));
		_compatibleList.Clear(); 
		// TODO: The .Clear() method temporary fixes the duplicate card bug.
		// However, this method may increase computation because when GetDeckTableCompatibleCards() is called,
		// it iterates from the beginning. Compared to .Remove() which only removes the recently placed card.
		_playerCardDict[player].Remove(card);
		return true;
	}
	public Card?[] GetAdjacentCards(int cardId)
	{
		//TODO: What is this? What does [4] signify?
		//Answer: Card?[4] is the array of cards that are connected to the current card (which has the id: CardId)
		Card currentCard = _tableCards.FirstOrDefault(x => x.GetId() == cardId);
		int[] cardIdAtNodes = currentCard.GetCardIdArrayAtNodes();
		Card?[] cards = new Card[cardIdAtNodes.Length];
		for (int i = 0; i < cardIdAtNodes.Length; i++)
		{
			cards[i] = _tableCards.FirstOrDefault(x => x.GetId() == i);
		}
		return cards;
	}
	public HashSet<IdNodeSuit> GetTargetNodes()
	{
		//(DONE): Change to HashSet<IdNodeSuit> because it is unique
		// which signifies the open-ended card, its available node, and its available head/tail number.
		// Available head/tail (suit) number is to make it easier for player's card to check
		foreach (Card tableCard in _tableCards)
		{
			if (tableCard.IsDouble())
			{
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.RIGHT] == -1)
				{
					_openEndsSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.RIGHT, tableCard.Head));
					// Console.WriteLine($"card id: {tableCard.GetId()}, RIGHT, suit: {tableCard.Head}");
				}
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.LEFT] == -1)
				{
					_openEndsSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.LEFT, tableCard.Tail));
					// Console.WriteLine($"card id: {tableCard.GetId()}, LEFT, suit: {tableCard.Tail}");
				}
			}
			else
			{
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.FRONT] == -1)
				{
					_openEndsSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.FRONT, tableCard.Head));
					// Console.WriteLine($"card id: {tableCard.GetId()}, FRONT, suit: {tableCard.Head}");
				}
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.BACK] == -1)
				{
					_openEndsSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.BACK, tableCard.Tail));
					// Console.WriteLine($"card id: {tableCard.GetId()}, BACK, suit: {tableCard.Tail}");
				}
			}
		}
		return _openEndsSet;
	}
	public List<KeyValuePair<Card, IdNodeSuit>>
		GetDeckTableCompatibleCards(List<Card> cardsList,
				HashSet<IdNodeSuit> openEndsSet)
	{
		foreach (Card card in cardsList)
		{
			foreach (var idNodeSuit in openEndsSet)
			{
				if(card.Head == idNodeSuit.Suit || card.Tail==idNodeSuit.Suit)
				{
					KeyValuePair<Card, IdNodeSuit> cardInsKvp = new(card, idNodeSuit);
					if (!_compatibleList.Contains(cardInsKvp))
					{
						_compatibleList.Add(cardInsKvp);
					}	
				}
			}
		}
		return _compatibleList;
	}
	public Card GetCardFromId(int id)
	{
		return _defaultCards.FirstOrDefault(x => x.GetId()==id);
	}
	public void ShowCards(IPlayer player)
	{
		// TODO: Maybe delete this function because UI is handled in Program.cs, not GameController
		// For now, it is a helper function for debugging
		foreach (Card card in _playerCardDict[player])
		{
			Console.WriteLine("id: {0},\t head: {1},\t tail: {2}", card.GetId(), card.Head, card.Tail);
		}
		// var chosenCard = _playerCardDict[player].Where(n => n.GetId() == id);
		// Console.WriteLine("{0},{1},{2}",id,chosenCard.FirstOrDefault().head, chosenCard.FirstOrDefault().tail);	
	}
	public List<Card> GetPlayerCards(IPlayer player)
	{
		return _playerCardDict[player];
	}
	public List<Card> GetTableCards()
	{
		return _tableCards;
	}
	public bool ResetRound()
	{
		return false;
	}
	public int CheckScore(IPlayer player)
	{
		return _playerScoreDict[player];
	}
	public bool IsWinGame()
	{
		bool winGame = false;
		foreach (IPlayer player in _playersList)
		{
			bool playerWinGame = false;
			if (CheckScore(player) >= WinScore)
			{
				playerWinGame = true;
			}
			winGame = winGame || playerWinGame;
		}
		return winGame;
	}
	public bool IsWinRound()
	{
		//TODO: Check for: 
		//1. no possible moves when boneyardCards is empty OR
		//2. empty deck (DONE)
		bool winRound = false;
		if (_boneyardCards.Count == 0)
		{
			// no possible moves when boneyardCards is empty
			bool[] allNoValidMove = new bool[_playersList.Count];
			int i = 0;
			foreach (IPlayer player in _playersList)
			{
				if (GetDeckTableCompatibleCards(_playerCardDict[player], _openEndsSet).Count == 0)
				{
					allNoValidMove[i] = true;
				}
				i++;
			}
			if (allNoValidMove.All(x => x==false))
			{
				winRound = true;
			}
		}
		else
		{
			foreach (IPlayer player in _playersList)
			{
				bool playerWinRound = false;
				// no cards remaining in player's deck
				if (_playerCardDict[player].Count == 0)
				{
					winRound = true;
					break;
				}
				winRound = winRound || playerWinRound;
			}
		}
		if (winRound)
		{
			_gameStatus = GameStatus.ROUNDWIN;
		}
		return winRound;
	}
	public IPlayer CalculateRoundScore()
	{
		// Get players' remaining cards
		// Calculate score by by sum of all heads & tails 
		// minus the winner's headTailSum
		int[] headTailSumArray = new int[_playersList.Count];
		int score = 0;
		int i = 0;
		foreach (var kvp in _playerCardDict)
		{
			foreach(var card in kvp.Value)
			{
				headTailSumArray[i] = card.GetHeadTailSum();
				score += headTailSumArray[i];	
			}
			i++;
		}
		// winner is player with the least headTailSum 
		int roundWinnerIdx = Array.IndexOf(headTailSumArray, headTailSumArray.Min());
			
		// Give score to round winner player
		_playerScoreDict[_playersList[roundWinnerIdx]] += score - headTailSumArray[roundWinnerIdx];
		return _playersList[roundWinnerIdx];	
	}
	public int NumBoneyardCards()
	{
		return _boneyardCards.Count;
	}
}

public enum GameStatus
{
	NOTSTARTED,
	ONGOING,
	ROUNDWIN,
	GAMEWIN
}

public enum CardStatus
{
	SETCARD,
	TAKECARD,
	PASS
}