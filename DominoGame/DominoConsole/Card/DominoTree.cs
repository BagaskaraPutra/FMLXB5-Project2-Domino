namespace DominoConsole;

public class DominoTree
{
	private static int previousCardId = -1;
	private static int numCards = 0;
	public Card Root { get; private set; }
	public DominoTree()
	{
		Root = null;
	}
	public DominoTree(Card root)
	{
		Root = root;
	}
	public void PrintInOrder(Card card)
	{
		if (card == null)
		{
			return;
		}
		else
		{
			previousCardId = card.GetId();
		}
		// else
		// {
		// 	previousCardId = card.GetId();
		// 	numCards++;
		// 	// Console.WriteLine($"previousCardId: {previousCardId}");
		// }
		// int total = Enum.GetValues(typeof(NodeEnum)).Length;
		
		NodeEnum[] NodeEnumArray = new NodeEnum[Enum.GetValues(typeof(NodeEnum)).Length];
		int i = 0;
		foreach (NodeEnum node in Enum.GetValues(typeof(NodeEnum)))
		{
			if (card.GetCardIdAtNode(node) != previousCardId)
			{
				NodeEnumArray[i] = node;
			}
			i++;
		}
		
		// if(card.IsDouble())
		// {
		// 	NodeEnumArray[0] = NodeEnum.LEFT;
		// 	NodeEnumArray[1] = NodeEnum.RIGHT;
		// 	NodeEnumArray[2] = NodeEnum.FRONT;
		// 	NodeEnumArray[3] = NodeEnum.BACK;
		// }
		// else
		// {
		// 	NodeEnumArray[0] = NodeEnum.FRONT;
		// 	NodeEnumArray[1] = NodeEnum.BACK;
		// 	NodeEnumArray[2] = NodeEnum.LEFT;
		// 	NodeEnumArray[3] = NodeEnum.RIGHT;
		// }
		
		// All the children except the last 
		// NodeEnumArray = (NodeEnum[])Enum.GetValues(typeof(NodeEnum));
		foreach (NodeEnum node in NodeEnumArray)
		{
			if(node == NodeEnumArray.Last())
			{
				break;
			}
			PrintInOrder(card.GetCardAtNode(node));
		} 

		// Print the current node's data 
		Console.Write($"[{card.Head}|{card.Tail}] ");

		// Last child 
		PrintInOrder(card.GetCardAtNode(NodeEnumArray.Last()));
	}
	/*
	public void PrintInOrder(Card card)
	{
		if (card == null)
		{
			return;
		}
		else
		{
			previousCardId = card.GetId();
			numCards++;
			// Console.WriteLine($"previousCardId: {previousCardId}");
		}
		NodeEnum nodePriority = 0; // = NodeEnum.FRONT;
		int openEndedCount = 0;
		foreach (NodeEnum node in Enum.GetValues(typeof(NodeEnum)))
		{
			if (card.GetCardIdAtNode(node) == -1)
			{
				openEndedCount++;
			}
			else
			{
				if (card.GetCardIdAtNode(node) != previousCardId)
				{
					nodePriority = node;
				}
			}
		}
		if (openEndedCount == 3 && numCards>1)
		{
			Console.Write($"[{card.Head}|{card.Tail}] ");
		}
		else
		{	
			// PrintInOrder(card.GetCardAtNode(nodePriority));
			// Console.Write($"[{card.Head}|{card.Tail}] ");
			if(card.IsDouble())
			{
				PrintInOrder(card.GetCardAtNode(NodeEnum.LEFT));
				Console.Write($"[{card.Head}|{card.Tail}] ");
				PrintInOrder(card.GetCardAtNode(NodeEnum.RIGHT));
			}
			else
			{
				PrintInOrder(card.GetCardAtNode(NodeEnum.FRONT));
				Console.Write($"[{card.Head}|{card.Tail}] ");
				PrintInOrder(card.GetCardAtNode(NodeEnum.BACK));
			}
		}
	}
	*/
}

// public class DominoTreeInt
// {
// 	public int Root{get; private set;}
// 	public DominoTreeInt()
// 	{
// 		Root = -1;
// 	}
// 	public DominoTreeInt(int root)
// 	{
// 		Root = root;
// 	}
// 	public void PrintInOrder(Card card)
// 	{

// 	}
// }
