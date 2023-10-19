using DominoConsole;
using Moq;
namespace DominoConsole.Test;

public class GameControllerTest
{
	private GameController _cut;
	private Mock<IPlayer> _player;
	private Mock<ICard> _card;
	
	public GameControllerTest()
	{
		_cut = new(3, 100);	
	}
	
	[Fact]
	public void AddPlayer_PlayerNumberEqualsExpected_ThreePlayers()
	{
		_cut.AddPlayer(new Player(id: 1, name: "Alpha"));
		_cut.AddPlayer(new Player(id: 2, name: "Beta"));
		_cut.AddPlayer(new Player(id: 3, name: "Charlie"));
		Assert.Equal(3, _cut.GetPlayers().Count);
	}
	[Fact]
	public void AddPlayer_PlayerNumberEqualsExpected_FourPlayers()
	{
		_cut = new(4, 100);
		_cut.AddPlayer(new Player(id: 1, name: "Alpha"));
		_cut.AddPlayer(new Player(id: 2, name: "Bravo"));
		_cut.AddPlayer(new Player(id: 3, name: "Charlie"));
		_cut.AddPlayer(new Player(id: 4, name: "Delta"));
		Assert.Equal(4, _cut.GetPlayers().Count);
	}
	[Fact]
	public void AddPlayer_PlayerNumberNotEqualsExpected_OnePlayerTryDuplicate()
	{
		Player alphaPlayer = new Player(id: 1, name: "Alpha");
		_cut.AddPlayer(alphaPlayer);
		_cut.AddPlayer(alphaPlayer);
		_cut.AddPlayer(new Player(id: 1, name: "Alpha"));
		Console.WriteLine($"Max number of players: {_cut.MaxNumPlayers}");
		foreach (var player in _cut.GetPlayers())
		{
			Console.WriteLine($"{player.GetId()}: {player.GetName()}");
		}
		Assert.NotEqual(3, _cut.GetPlayers().Count);
	}
	// [Fact]
	// public void AddCards_CardsAddedSuccessfullyReturnsTrue_PlayerExists()
	// {
	// 	_card.Setup(c => c.SetStatus(It.IsAny<CardStatus>()));

	// 	bool result = _cut.AddCards(_player.Object, _card.Object);

	// 	Assert.IsTrue(result);
	// 	_card.Verify(c => c.SetStatus(It.IsAny<CardStatus>()), Times.Once);
	// }
}