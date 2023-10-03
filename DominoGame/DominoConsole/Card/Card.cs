namespace DominoConsole;

public class Card
{
	private int _id;
	private int[] _node; // array of 4 nodes
	public readonly int head;
	public readonly int tail;
	public Card(int id, int head, int tail)
	{
		this._id  = id;
		this.head = head;
		this.tail = tail;
	}
	public bool IsDouble()
	{
		if(head == tail)
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
		return head+tail;
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