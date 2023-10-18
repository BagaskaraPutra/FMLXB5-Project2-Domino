using DominoConsole;
namespace DominoConsole.Test;

public class UnitTest1
{
	GameController? game;
	IPlayer? currentPlayer;
	List<Card>? cardsList;
	Card? playerCard;
	HashSet<IdNodeSuit>? openNodes;
	IdNodeSuit targetIdNodeSuit;
	List<KeyValuePair<Card, IdNodeSuit>>? deckTableCompatible;
	DominoTree? dominoTree;
	CardGUI? cardGUI;
	TableGUI? tableGUI;
	List<CardKinematics>? cardKinematicsLUT;
	[Fact]
	public void AddPlayerSuccessTest()
	{
		game = new(3, 100);
		game.AddPlayer(new Player(id: 1, name: "Alpha"));
		game.AddPlayer(new Player(id: 2, name: "Beta"));
		game.AddPlayer(new Player(id: 3, name: "Charlie"));
		Assert.Equal(3, game.GetPlayers().Count);
	}
	[Fact]
	public void AddPlayerFailTest()
	{
		game = new(3, 100);
		game.AddPlayer(new Player(id: 1, name: "Alpha"));
		game.AddPlayer(new Player(id: 1, name: "Alpha"));
		game.AddPlayer(new Player(id: 3, name: "Charlie"));
		foreach(var player in game.GetPlayers())
		{
			Console.WriteLine($"{player.GetId()}: {player.GetName()}");
		}
		Assert.NotEqual(3, game.GetPlayers().Count);
	}
}