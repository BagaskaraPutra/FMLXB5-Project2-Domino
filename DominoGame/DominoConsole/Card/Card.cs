using System.Dynamic;

namespace DominoConsole;

public class Card
{
	private int _id;
	private int[] _nodeId; // array of 4 nodes consisting of adjacent card id
	private Card?[] _node; // array of 4 nodes consisting of adjacent Card objects
	private Card? _left, _right;
	
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
		_nodeId = new int[Enum.GetValues(typeof(NodeEnum)).Length];
		for(int i=0; i<_nodeId.Length; i++)
		{
			_nodeId[i] = -1;
		}
		_node = new Card[Enum.GetValues(typeof(NodeEnum)).Length];
		for(int i=0; i<_node.Length; i++)
		{
			_node[i] = null;
		}
		_left = _right = null; 
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
	public int[] GetCardIdArrayAtNodes()
	{
		return _nodeId;
	}
	public int GetCardIdAtNode(NodeEnum nodeEnum)
	{
		return _nodeId[(int)nodeEnum];
	}
	public Card GetCardAtNode(NodeEnum nodeEnum)
	{
		return _node[(int)nodeEnum];
	}
	public void SetCardIdAtNode(int cardId, NodeEnum nodeEnum)
	{
		_nodeId[(int)nodeEnum] = cardId;
	}
	public void SetCardAtNode(Card card, NodeEnum nodeEnum)
	{
		_node[(int)nodeEnum] = card;
	}
	public Card GetLeftCard()
	{
		return _left;
	}
	public void SetLeftCard(Card left)
	{
		_left = left;
	}
	public Card GetRightCard()
	{
		return _right;
	}
	public void SetRightCard(Card right)
	{
		_right = right;
	}
	public int GetHeadTailSum()
	{
		return Head+Tail;
	}
}
public struct IdNodeSuit
{
	// A struct consisting of the card's id, node, and its corresponding suit (head/tail) number
	public int Id {get; private set;}
	public NodeEnum Node {get; private set;}
	public int Suit {get; private set;}
	public IdNodeSuit(int id, NodeEnum node, int suit)
	{
		Id   = id;
		Node = node;
		Suit = suit;
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
public enum Orientation
{
	NORTH,
	EAST,
	SOUTH,
	WEST
}
// public enum NodeEnum
// {
// 	FRONT,
// 	RIGHT,
// 	BACK,
// 	LEFT
// }
public enum NodeEnum
{
	FRONT,
	LEFT,
	BACK,
	RIGHT
}