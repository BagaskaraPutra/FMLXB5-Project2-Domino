namespace DominoConsole;

public class GameController
{
	public int Round;
	//TODO: Is this the current round? 
	// Is it a public property (with get-set methods)?
	
	private IPlayer _currentPlayer;
	private Dictionary<IPlayer,List<Card>> _playerCardDict;
	private Dictionary<IPlayer,int> _playerScore;
	private List<Card> _tableCard;
	private Deck _deckcard;
	private GameStatus _gameStatus;
	private int _winScore;
	// if one of the players reaches this score, then the game is finished
	
	public GameController(int numPlayers, int winScore)
	{
		_gameStatus = GameStatus.NOTSTARTED;
	}
	public bool AddPlayer(IPlayer player)
	{
		return false;
	}
	public List<IPlayer> GetPlayer()
	{
		//TODO: Does it return list of players or only one player?	
	}
	public bool GetOriention(IsDouble)
	{
		//TODO: What is IsDouble?	
	}
	public GameStatus CheckGameStatus()
	{
		return _gameStatus;
	}
	public CardStatus CheckSetCardStatus()
	{
		
	}
	public bool SetNextTurn(IPlayer player, List<Card> cardHand)
	{
		
	}
	public bool PutCard(IPlayer player, Card card, Card target, Node node)
	{
		
	}
	public Card?[4] GetCards(int CardId)
	{
		//TODO: What is this? What does [4] signify?
	} 
	public void ShowCard(IPlayer player)
	{
		
	}
	public List<Card> CardOnTable() //tableCard
	{
		
	}
	public bool ResetRound()
	{
		
	}
	public int CheckScore(IPlayer player)
	{
		
	} 
}

public enum GameStatus
{
	NOTSTARTED,
	ONGOING,
	ROUNDWIN,
	GAMEWIN
}