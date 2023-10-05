namespace DominoConsole;

public class Card
{
	private int _id;
	private int[] _node; // array of 4 nodes
	public int Head {get; private set;}
	public int Tail {get; private set;}
	public Card()
	{
		_id = -1;
	}
	public Card(int id, int head, int tail)
	{
		_id  = id;
		Head = head;
		Tail = tail;
		_node = new int[Enum.GetValues(typeof(Node)).Length];
		for(int i=0; i<_node.Length; i++)
		{
			_node[i] = -1;
		} 
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
	// public Node GetNode(int cardId)
	// {
	// 	return Node.NODE1;
	// }
	public int[] GetCardIdAtNodes()
	{
		return _node;
	}
	public void SetCardIdAtNode(int cardId, Node node)
	{
		_node[(int)node] = cardId;
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

// public enum Node
// {
// 	NODE1,
// 	NODE2,
// 	NODE3,
// 	NODE4
// }

public enum Node
{
	NORTH,
	EAST,
	SOUTH,
	WEST
}