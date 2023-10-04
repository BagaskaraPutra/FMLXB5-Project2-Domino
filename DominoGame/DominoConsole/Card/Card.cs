namespace DominoConsole;

public class Card
{
	private int _id;
	private int[] _node; // array of 4 nodes
	public readonly int Head;
	public readonly int Tail;
	public Card(int id, int head, int tail)
	{
		this._id  = id;
		this.Head = head;
		this.Tail = tail;
	}
	public bool IsDouble()
	{
		if(Head == Tail)
		{
			return true;
		}
		else
		{
			return false;	
		}
	}
	public int GetId()
	{
		return _id;
	}
	public Node GetNode(int cardId)
	{
		return Node.NODE1;
	}
	public int GetHeadTailSum()
	{
		return Head+Tail;
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

public enum Node
{
	NODE1,
	NODE2,
	NODE3,
	NODE4
}