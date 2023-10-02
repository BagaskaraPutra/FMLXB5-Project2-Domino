using System.Dynamic;

namespace DominoConsole;

public class GameController
{
	public int NumPlayers {get; private set;}
	public int Round;
	//TODO: Is this the current round? 
	// Is it a public property (with get-set methods)?
	
	private IPlayer _currentPlayer;
	private Dictionary<IPlayer,List<Card>?> _playerCardDict;
	private Dictionary<IPlayer,int> _playerScoreDict;
	private List<Card> _tableCard;
	private Deck _deckcard;
	private GameStatus _gameStatus;
	public readonly int WinScore;
	// if one of the players reaches this score, then the game is finished
	
	public GameController(int numPlayers, int winScore)
	{
		_gameStatus = GameStatus.NOTSTARTED;
		NumPlayers 	= numPlayers;
		WinScore 	= winScore;
		
		_playerCardDict  = new();
		_playerScoreDict = new();
	}
	public bool AddPlayer(IPlayer player)
	{
		bool successAddToCardDict 	= _playerCardDict.TryAdd(player,null);
		bool successAddToScore		= _playerScoreDict.TryAdd(player,0);
		return successAddToCardDict && successAddToScore;
	}
	public List<IPlayer> GetPlayer()
	{
		//TODO: Does it return list of players or only one player?
		return null;	
	}
	public bool GetOriention(bool IsDouble)
	{
		//TODO: What is IsDouble?	
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
		return false;	
	}
	public bool PutCard(IPlayer player, Card card, Card target, Node node)
	{
		return false;
	}
	// public Card?[4] GetCards(int CardId)
	// {
	// 	//TODO: What is this? What does [4] signify?
	// 	return null;
	// } 
	public void ShowCard(IPlayer player)
	{

	}
	public List<Card> CardOnTable() //tableCard
	{
		return null;
	}
	public bool ResetRound()
	{
		return false;
	}
	public int CheckScore(IPlayer player)
	{
		return 0;
	} 
}

public enum GameStatus
{
	NOTSTARTED,
	ONGOING,
	ROUNDWIN,
	GAMEWIN
}