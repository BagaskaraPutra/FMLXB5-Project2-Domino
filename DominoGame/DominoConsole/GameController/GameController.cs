using System.Security.Cryptography;

namespace DominoConsole;

public class GameController
{
	public int NumPlayers { get; private set; }
	public int Round {get; private set; }
	public readonly int MaxNumCardsPerPlayer;
	private static int _firstPlayerIndex;
	private IPlayer _currentPlayer;
	private List<IPlayer> _playersList;
	private Dictionary<IPlayer, List<Card>?> _playerCardDict;
	private Dictionary<IPlayer, int> _playerScoreDict;
	private Dictionary<IPlayer, PlayerStatus> _playerStatusDict;
	private Dictionary<IPlayer, List<KeyValuePair<Card, IdNodeSuit>>> _playerDeckTableCompatible;
	// list of player's cards & open-ended card on the table that are compatible (has same head/tail value)

	private List<Card> _defaultCards;
	// a list of default cards for template only

	private List<Card> _boneyardCards;
	// cards on the table not yet picked by players

	private List<Card> _tableCards;
	// cards on the table already placed by the players

	private HashSet<IdNodeSuit>? _openNodesSet;
	// set of tableCards and their nodes that have open ends (can be placed with a card)
	// TODO: Convert to Hashset<Card> to save memory. 
	// NO, because if only the Card is saved, we need to recheck which nodes are open (node with id=-1)

	private GameStatus _gameStatus;
	public readonly int MaxWinScore;
	// if one of the players reaches this score, then the game is finished

	public GameController(int numPlayers, int maxWinScore)
	{
		_gameStatus = GameStatus.NOTSTARTED;
		NumPlayers = numPlayers;
		MaxWinScore = maxWinScore;
		Round = 1;

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
		_playerDeckTableCompatible = new();
		
		_openNodesSet = new();
		_tableCards = new();
		InitializeDefaultCards();
		_boneyardCards =  _defaultCards.ConvertAll(card => card.DeepCopy()).ToList();
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
		if (_boneyardCards.Count == 0)
		{
			return;
		}
		_playerStatusDict[player] = PlayerStatus.TAKECARD;
		int index = RandomNumberGenerator.GetInt32(0,_boneyardCards.Count);
		_playerCardDict[player].Add(_boneyardCards[index]);
		_boneyardCards.RemoveAt(index);
	}
	public int GetNumberOfCards(IPlayer player)
	{
		return _playerCardDict[player].Count;
	}
	public IPlayer GetFirstPlayer()
	{
		if (Round == 1)
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
			_firstPlayerIndex = Array.IndexOf(sumArray, sumArray.Max());
			// Console.WriteLine("Player {0} has largest sum of head & tail", _playersList[_firstPlayerIndex].GetId());
			_gameStatus = GameStatus.ONGOING;
			_currentPlayer = _playersList[_firstPlayerIndex];
		}
		else
		{
			_firstPlayerIndex++;
			if(_firstPlayerIndex >= NumPlayers)
			{
				_firstPlayerIndex = 0;
			}
		}
		return _playersList[_firstPlayerIndex];
	}
	public bool AddPlayer(IPlayer player)
	{
		if (_playersList.Contains(player))
		{
			return false;
		}
		bool successAddToCardDict 	= _playerCardDict.TryAdd(player, new());
		bool successAddToScore 		= _playerScoreDict.TryAdd(player, 0);
		bool successAddToStatus 	= _playerStatusDict.TryAdd(player, 0);
		bool successAddToDeckTable  = _playerDeckTableCompatible.TryAdd(player, new());
		_playersList.Add(player);
		return successAddToCardDict && successAddToScore && successAddToStatus && successAddToDeckTable;
	}
	public IPlayer GetPlayer(int id)
	{
		return _playersList.FirstOrDefault(p => p.GetId() == id);
	}
	public List<IPlayer> GetPlayers()
	{
		return _playersList;
	}
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
	public PlayerStatus CheckPlayerStatus(IPlayer player)
	{
		return _playerStatusDict[player];
	}
	public void SetPlayerStatus(IPlayer player, PlayerStatus status)
	{
		_playerStatusDict[player] = status;
	}
	public IPlayer GetCurrentPlayer()
	{
		return _currentPlayer;
	}
	public IPlayer GetNextPlayer()
	{
		int currentIndex = _playersList.FindIndex(p => p.Equals(_currentPlayer));
		if (currentIndex == (NumPlayers - 1))
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
		_playerStatusDict[player] = PlayerStatus.SETCARD;
		_tableCards.Add(card);
		_playerCardDict[player].Remove(card);
	}
	public bool PutCard(IPlayer player, Card card, IdNodeSuit targetINS)
	{
		//(DONE): In the GetTargetNodes() method, TryAdd & Add always adds new cards to the HashSet.
		// What happens when the open-ended node is attached with a card from the player's deck?
		// Delete the open-ended IdNodeSuit when adding player's card to the tableCard.
		
		_playerStatusDict[player] = PlayerStatus.SETCARD;
		
		Card targetCard = _tableCards.FirstOrDefault(x => x.GetId()==targetINS.Id);
		targetCard.SetCardIdAtNode(card.GetId(), targetINS.Node);
		
		NodeEnum cardNode = new();
		if(card.IsDouble())
		{
			if(card.Head == targetINS.Suit)
			{
				cardNode = NodeEnum.LEFT;
			}
			else if(card.Tail == targetINS.Suit)
			{
				cardNode = NodeEnum.RIGHT;
			}
		}
		else
		{
			if(card.Head == targetINS.Suit)
			{
				cardNode = NodeEnum.FRONT;
			}
			else if(card.Tail == targetINS.Suit)
			{
				cardNode = NodeEnum.BACK;
			}
		}
		card.SetCardIdAtNode(targetINS.Id, cardNode);
		card.SetParentId(targetINS.Id);
		
		_tableCards.Add(card);
		// _openNodesSet.Remove(targetINS);
		_openNodesSet.Clear();
		// _compatibleList.Remove(new(targetCard,targetINS));
		_playerDeckTableCompatible[player].Clear();
		// TODO: The .Clear() method temporary fixes the duplicate card bug.
		// However, this method may increase computation because when GetDeckTableCompatible() is called,
		// it iterates from the beginning. Compared to .Remove() which only removes the recently placed card.
		_playerCardDict[player].Remove(card);
		return true;
	}
	public HashSet<IdNodeSuit> GetOpenNodes()
	{
		foreach (Card tableCard in _tableCards)
		{
			// Console.WriteLine($"[{tableCard.Head}|{tableCard.Tail}] card id: {tableCard.GetId()}");
			if (tableCard.IsDouble())
			{
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.RIGHT] == -1)
				{
					_openNodesSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.RIGHT, tableCard.Head));
					// Console.WriteLine($"card id: {tableCard.GetId()}, RIGHT, suit: {tableCard.Head}");
				}
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.LEFT] == -1)
				{
					_openNodesSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.LEFT, tableCard.Tail));
					// Console.WriteLine($"card id: {tableCard.GetId()}, LEFT, suit: {tableCard.Tail}");
				}
			}
			else
			{
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.FRONT] == -1)
				{
					_openNodesSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.FRONT, tableCard.Head));
					// Console.WriteLine($"card id: {tableCard.GetId()}, FRONT, suit: {tableCard.Head}");
				}
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.BACK] == -1)
				{
					_openNodesSet.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.BACK, tableCard.Tail));
					// Console.WriteLine($"card id: {tableCard.GetId()}, BACK, suit: {tableCard.Tail}");
				}
			}
		}
		return _openNodesSet;
	}
	public List<KeyValuePair<Card, IdNodeSuit>>
		GetDeckTableCompatible(IPlayer player, HashSet<IdNodeSuit> openNodes)								
	{
		foreach (Card card in _playerCardDict[player])
		{
			foreach (var idNodeSuit in openNodes)
			{
				if(card.Head == idNodeSuit.Suit || card.Tail==idNodeSuit.Suit)
				{
					KeyValuePair<Card, IdNodeSuit> cardInsKvp = new(card, idNodeSuit);
					if (!_playerDeckTableCompatible[player].Contains(cardInsKvp))
					{
						_playerDeckTableCompatible[player].Add(cardInsKvp);
					}	
				}
			}
		}
		return _playerDeckTableCompatible[player];
	}
	public Card GetCardFromId(int id)
	{
		return _defaultCards.FirstOrDefault(x => x.GetId()==id);
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
		Round++;
		foreach (IPlayer player in _playerCardDict.Keys)
		{
			_playerCardDict[player].Clear();
			_playerDeckTableCompatible[player].Clear();
		}
		_openNodesSet.Clear();

		_tableCards.Clear();
		_boneyardCards = _defaultCards.ConvertAll(card => card.DeepCopy()).ToList();
		return true;
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
			if (CheckScore(player) >= MaxWinScore)
			{
				playerWinGame = true;
			}
			winGame = winGame || playerWinGame;
		}
		return winGame;
	}
	public bool IsWinRound()
	{
		bool winRound = false;
		bool[] allNoValidMove = new bool[_playersList.Count];
		// TODO: Waste of memory initialize bool[] everytime
		if (_boneyardCards.Count == 0)
		{
			// no possible moves when boneyardCards is empty
			int i = 0;
			foreach (IPlayer player in _playersList)
			{
				if (GetDeckTableCompatible(player, _openNodesSet).Count == 0)
				{
					allNoValidMove[i] = true;
				}
				i++;
			}
			if (allNoValidMove.All(x => x==true))
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
	public IPlayer GetRoundWinner()
	{
		// Get players' remaining cards
		// Calculate score by by sum of all heads & tails 
		// minus the winner's headTailSum
		int[] headTailSumArray = new int[_playersList.Count];
		int score = 0;
		int i = 0;
		int numCards = 0;
		foreach (var kvp in _playerCardDict)
		{
			foreach(var card in kvp.Value)
			{
				headTailSumArray[i] += card.GetHeadTailSum();
				score += card.GetHeadTailSum();
				numCards++;	
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