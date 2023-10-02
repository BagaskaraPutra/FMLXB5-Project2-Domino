namespace DominoConsole;

public class Card
{
	private int _id;
	private int[] _node; // array of 4 nodes
	public readonly int head;
	public readonly int tail;
	public Card(int head, int tail)
	{
		this.head = head;
		this.tail = tail;
	}
	public bool IsDouble()
	{
		
	}
	public int GetId()
	{
		return _id;
	}
	public Node GetNode(int cardId)
	{
		
	}
}

public enum CardTitle
{
	ZERO,
	ONE,
	TWO,
	THREE,
	FOUR,
	FIVE,
	SIX
}

public enum CardStatus
{
	SETCARD,
	TAKECARD,
	PASS
}

public enum Node
{
	NODE1,
	NODE2,
	NODE3,
	NODE4
}