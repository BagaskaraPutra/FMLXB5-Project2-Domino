namespace DominoConsole;

public class Card
{
	protected int _id;
	protected int[] _nodeId; // array of 4 nodes consisting of adjacent card id
	public int ParentId {get; protected set;}
	
	public int Head {get; protected set;}
	public int Tail {get; protected set;}
	public Card()
	{
		_id = -1;
		ParentId = -1;
		_nodeId = new int[Enum.GetValues(typeof(NodeEnum)).Length];
		for(int i=0; i<_nodeId.Length; i++)
		{
			_nodeId[i] = -1;
		}
	}
	public Card(int id, int head, int tail)
	{
		_id  = id;
		ParentId = -1;
		Head = head;
		Tail = tail;
		_nodeId = new int[Enum.GetValues(typeof(NodeEnum)).Length];
		for(int i=0; i<_nodeId.Length; i++)
		{
			_nodeId[i] = -1;
		}
	}
	public Card DeepCopy()
	{
		return new Card(_id, Head, Tail);
	}
	public bool IsDouble()
	{
		if(Head == Tail)
		{
			return true;
		}
		return false;
	}
	public int GetId()
	{
		return _id;
	}
	public NodeEnum GetNode(int cardId)
	{
		NodeEnum matchedNode = NodeEnum.FRONT;
		foreach (NodeEnum node in Enum.GetValues(typeof(NodeEnum)))
		{
			if(_nodeId[(int)node] == cardId)
			{
				matchedNode = node;
			}
		}
		// Console.WriteLine($"Current card [{Head}|{Tail}] matchedNode: {matchedNode} to cardId {cardId}");
		return matchedNode;
	}
	public int[] GetCardIdArrayAtNodes()
	{
		return _nodeId;
	}
	public void SetCardIdAtNode(int cardId, NodeEnum nodeEnum)
	{
		_nodeId[(int)nodeEnum] = cardId;
	}
	public void SetParentId(int id)
	{
		ParentId = id;
		// Console.WriteLine($"Set card {_id} parent's id to: {id}");
	}
	public int GetHeadTailSum()
	{
		return Head+Tail;
	}
}