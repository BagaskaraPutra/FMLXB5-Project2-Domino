using NLog;
using System.Security.Cryptography;

namespace DominoConsole;

public class GameController
{
	private static Logger logger = LogManager.GetCurrentClassLogger();
	public int Round {get; private set; }
	public int MaxNumPlayers { get; private set; }
	public readonly int MaxNumCardsPerPlayer;
	private static int _firstPlayerIndex;
	private IPlayer _currentPlayer;
	private List<IPlayer> _playersList;
	private Dictionary<IPlayer, List<ICard>?> _playerCardDict;
	private Dictionary<IPlayer, int> _playerScoreDict;
	private Dictionary<IPlayer, PlayerStatus> _playerStatusDict;
	private Dictionary<IPlayer, List<KeyValuePair<ICard, IdNodeSuit>>> _playerDeckTableCompatible;
	// list of player's cards & open-ended card on the table that are compatible (has same head/tail value)

	private List<ICard> _defaultCards;
	// a list of default cards for template only

	private List<ICard> _boneyardCards;
	// cards on the table not yet picked by players

	private List<ICard> _tableCards;
	// cards on the table already placed by the players

	private HashSet<IdNodeSuit>? _openNodes;
	// set of tableCards and their nodes that have open ends (can be placed with a card)
	// TODO: Convert to Hashset<ICard> to save memory. 
	// NO, because if only the Card is saved, we need to recheck which nodes are open (node with id=-1)

	private GameStatus _gameStatus;
	public readonly int MaxWinScore;
	// if one of the players reaches this score, then the game is finished

	public GameController(int maxNumPlayers, int maxWinScore)
	{
		_gameStatus = GameStatus.NOTSTARTED;
		MaxNumPlayers = maxNumPlayers;
		MaxWinScore = maxWinScore;
		Round = 1;

		if (maxNumPlayers > 2 && maxNumPlayers <= 4)
		{
			MaxNumCardsPerPlayer = 5;
		}
		else if(maxNumPlayers == 2)
		{
			MaxNumCardsPerPlayer = 7;
		}
		else
		{
			logger.Fatal("Invalid number of players! Number of players should be between 2-4");
			throw new Exception("Invalid number of players! Number of players should be between 2-4");
		}

		_playersList = new();
		_playerCardDict = new();
		_playerScoreDict = new();
		_playerStatusDict = new();
		_playerDeckTableCompatible = new();
		
		_openNodes = new();
		_tableCards = new();
		InitializeDefaultCards();
		_boneyardCards =  _defaultCards.ConvertAll(card => card.DeepCopy()).ToList();
		
		logger.Info("Domino GameController initialized with {int} players", MaxNumPlayers);
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
				logger.Trace("Created card id: {id}, head: {h}, tail: {t}", id,h,t);
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
		logger.Trace($"Player {player.GetId()} ({player.GetName()}) is drawing card [{_boneyardCards[index].Head}|{_boneyardCards[index].Tail}] with id {_boneyardCards[index].GetId()}");
		_boneyardCards.RemoveAt(index);
	}
	public int GetNumberOfCards(IPlayer player)
	{
		return _playerCardDict[player].Count;
	}
	public ICard GetFirstPlayerCandidateCard(IPlayer player)
	{
		if(Round != 1) 
		{
			return null;
		}
		DrawRandomCard(player);
		return _playerCardDict[player].FirstOrDefault();
	}
	public IPlayer GetFirstPlayer()
	{
		if (Round == 1)
		{
			int[] sumArray = new int[MaxNumPlayers];
			int i = 0;
			foreach (IPlayer player in _playersList)
			{
				sumArray[i] = _playerCardDict[player].FirstOrDefault().GetHeadTailSum();;
				i++;
			}
			_firstPlayerIndex = Array.IndexOf(sumArray, sumArray.Max());
			logger.Info("Player {0} has largest sum of head & tail", _playersList[_firstPlayerIndex].GetId());
			_currentPlayer = _playersList[_firstPlayerIndex];
		}
		else
		{
			_firstPlayerIndex++;
			if(_firstPlayerIndex >= MaxNumPlayers)
			{
				_firstPlayerIndex = 0;
			}
		}
		_gameStatus = GameStatus.ONGOING;
		return _playersList[_firstPlayerIndex];
	}
	public bool AddPlayer(IPlayer player)
	{
		if (_playersList.Any(p => p.GetId() == player.GetId() || p.GetName() == player.GetName()))
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
		if (currentIndex == (MaxNumPlayers - 1))
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
	public void PutCard(IPlayer player, ICard card)
	{
		_playerStatusDict[player] = PlayerStatus.SETCARD;
		_tableCards.Add(card);
		_playerCardDict[player].Remove(card);
	}
	public bool PutCard(IPlayer player, ICard card, IdNodeSuit targetINS)
	{
		//(DONE): In the GetTargetNodes() method, TryAdd & Add always adds new cards to the HashSet.
		// What happens when the open-ended node is attached with a card from the player's deck?
		// Delete the open-ended IdNodeSuit when adding player's card to the tableCard.
		
		_playerStatusDict[player] = PlayerStatus.SETCARD;
		
		ICard targetCard = _tableCards.FirstOrDefault(x => x.GetId()==targetINS.Id);
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
		logger.Trace($"Player {player.GetId()} ({player.GetName()}) is putting card [{card.Head}|{card.Tail}] at [{targetCard.Head}|{targetCard.Tail}] at {cardNode}");
		
		_tableCards.Add(card);
		// _openNodes.Remove(targetINS);
		_openNodes.Clear();
		// _playerDeckTableCompatible[player].Remove(new(targetCard,targetINS));
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
			logger.Debug($"Scanning through table cards ... Found: [{tableCard.Head}|{tableCard.Tail}] with id: {tableCard.GetId()}");
			if (tableCard.IsDouble())
			{
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.RIGHT] == -1)
				{
					_openNodes.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.RIGHT, tableCard.Head));
					logger.Debug($"Found open-ended node at: card id: {tableCard.GetId()}, RIGHT, suit: {tableCard.Head}");
				}
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.LEFT] == -1)
				{
					_openNodes.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.LEFT, tableCard.Tail));
					logger.Debug($"Found open-ended node at: card id: {tableCard.GetId()}, LEFT, suit: {tableCard.Tail}");
				}
			}
			else
			{
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.FRONT] == -1)
				{
					_openNodes.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.FRONT, tableCard.Head));
					logger.Debug($"Found open-ended node at: card id: {tableCard.GetId()}, FRONT, suit: {tableCard.Head}");
				}
				if (tableCard.GetCardIdArrayAtNodes()[(int)NodeEnum.BACK] == -1)
				{
					_openNodes.Add(new IdNodeSuit(tableCard.GetId(), NodeEnum.BACK, tableCard.Tail));
					logger.Debug($"Found open-ended node at: card id: {tableCard.GetId()}, BACK, suit: {tableCard.Tail}");
				}
			}
		}
		return _openNodes;
	}
	public List<KeyValuePair<ICard, IdNodeSuit>>
		GetDeckTableCompatible(IPlayer player, HashSet<IdNodeSuit> openNodes)								
	{
		foreach (var card in _playerCardDict[player])
		{
			foreach (var idNodeSuit in openNodes)
			{
				if(card.Head == idNodeSuit.Suit || card.Tail==idNodeSuit.Suit)
				{
					KeyValuePair<ICard, IdNodeSuit> cardInsKvp = new(card, idNodeSuit);
					if (!_playerDeckTableCompatible[player].Contains(cardInsKvp))
					{
						_playerDeckTableCompatible[player].Add(cardInsKvp);
					}	
				}
			}
		}
		return _playerDeckTableCompatible[player];
	}
	public ICard GetCardFromId(int id)
	{
		return _defaultCards.FirstOrDefault(x => x.GetId()==id);
	}
	public List<ICard> GetPlayerCards(IPlayer player)
	{
		return _playerCardDict[player];
	}
	public List<ICard> GetTableCards()
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
		_openNodes.Clear();

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
				if (GetDeckTableCompatible(player, _openNodes).Count == 0)
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