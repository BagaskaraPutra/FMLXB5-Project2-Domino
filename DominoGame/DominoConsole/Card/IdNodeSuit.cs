namespace DominoConsole;

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
