using System.Dynamic;

namespace DominoConsole;

public class GameController
{
	public int NumPlayers {get; private set;}
	public int Round;
	//TODO: Is this the current round? 
	// Is it a public property (with get-set methods)?
	public readonly int MaxNumCardsPerPlayer;
	
	private IPlayer _currentPlayer;
	private List<IPlayer> _playersList;
	private Dictionary<IPlayer,List<Card>?> _playerCardDict;
	private Dictionary<IPlayer,int> _playerScoreDict;
	
	private List<Card> _defaultCards;
	// a list of default cards for template only
	
	private List<Card> _boneyardCards;
	// cards on the table not yet picked by players
	
	private List<Card> _tableCards;
	// cards on the table already placed by the players
	
	// private Deck _deckcard;
	// cards on each player's deck
	// TODO: Redundant because already in _playerCardDict
	
	private Random _random;
	
	private GameStatus _gameStatus;
	public readonly int WinScore;
	// if one of the players reaches this score, then the game is finished
	
	public GameController(int numPlayers, int winScore)
	{
		_gameStatus = GameStatus.NOTSTARTED;
		NumPlayers 	= numPlayers;
		WinScore 	= winScore;
		
		if(numPlayers > 2)
		{
			MaxNumCardsPerPlayer = 5;
		}
		else
		{
			MaxNumCardsPerPlayer = 7;
		}
		
		_playersList 	 = new();
		_playerCardDict  = new();
		_playerScoreDict = new();
		
		_tableCards = new();
		InitializeDefaultCards();
		_boneyardCards = new List<Card> (_defaultCards);
		_random = new Random();
	}
	
	private void InitializeDefaultCards()
	{
		_defaultCards = new();
		int id = 0;
		for (int h=0; h<7; h++)
		{
			for(int t=0; t<=h; t++)
			{
				_defaultCards.Add(new Card(id,h,t));
				// Console.WriteLine("id: {0},\t head: {1},\t tail: {2}", id,h,t);
				id++;
			}
		}
	}
	public void DrawRandomCard(IPlayer player)
	{
		int index = _random.Next(_boneyardCards.Count);
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
		bool successAddToCardDict 	= _playerCardDict.TryAdd(player,new());
		bool successAddToScore		= _playerScoreDict.TryAdd(player,0);
		_playersList.Add(player);
		return successAddToCardDict && successAddToScore;
	}
	public IPlayer GetPlayer(int id)
	{
		var filteredPlayer = _playersList.Where(n => n.GetId() == id);
		return filteredPlayer.FirstOrDefault();	
	}
	public List<IPlayer> GetPlayers()
	{
		return _playersList;	
	}
	public bool GetOriention(bool IsDouble)
	{
		//TODO: What is IsDouble?	
		// This checks the possible orientation of the card that is going to be placed
		return false;
	}
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
	public CardStatus CheckSetCardStatus()
	{
		return CardStatus.SETCARD;
	}
	public bool SetNextTurn(IPlayer player, List<Card> cardHand)
	{
		//TODO: What does this method do? Why does it need the cardHand parameter?
		// Answer: cardHand is redundant because it is already contained in _playerCardDict
		return false;	
	}
	public IPlayer GetCurrentPlayer()
	{
		return _currentPlayer;	
	}
	public IPlayer GetNextPlayer()
	{
		int currentIndex = _playersList.FindIndex(a => a.Equals(_currentPlayer));
		return _playersList[currentIndex+1];
	}
	public bool PutCard(IPlayer player, Card card, Card target, Node node)
	{
		return false;
	}
	public Card?[] GetAdjacentCards(int CardId)
	{
		//TODO: What is this? What does [4] signify?
	 	//Answer: Card?[4] is the array of cards that are connected to the current card (which has the id: CardId)
		return null;
	}
	public void ShowCards(IPlayer player)
	{
		// TODO: Maybe delete this function because UI is handled in Program.cs, not GameController
		// For now, it is a helper function for debugging
		foreach (Card card in _playerCardDict[player])
		{
			Console.WriteLine("id: {0},\t head: {1},\t tail: {2}",card.GetId(),card.Head, card.Tail);	
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
}

public enum GameStatus
{
	NOTSTARTED,
	ONGOING,
	ROUNDWIN,
	GAMEWIN
}