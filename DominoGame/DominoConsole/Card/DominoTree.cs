namespace DominoConsole;

public class DominoTree
{
	private int _parentId;
	private List<Card> _tableCards;
	private Card _currentCard;
	private Card _parentCard;
	public DominoTree(List<Card> cards)
	{
		_tableCards = cards;
		_currentCard = new();
		_parentCard = new();
	}
	public void CalcForwardKinematics(int id)
	{
		if (id == -1)
			return;
			
		_currentCard = _tableCards.FirstOrDefault(x => x.GetId()==id);
		_parentId    = _currentCard.ParentId;
		_parentCard  = _tableCards.FirstOrDefault(x => x.GetId()==_parentId);
		if (_parentId != -1)
		{
			if(_parentCard.IsDouble())
			{
				if(_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.LEFT)
				{
					// if currentCard is attached to parentCard's left
					if(_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
					{
						// if the currentCard's front/head is attached
						_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));	
					}
					else if(_currentCard.GetNode(_parentId) == NodeEnum.BACK)
					{
						_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));	
					}
				}
				else if(_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.RIGHT)
				{
					// if currentCard is attached to parentCard's right:
					if(_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
					{
						// if the currentCard's front/head is attached
						_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));	
					}
					else if(_currentCard.GetNode(_parentId) == NodeEnum.BACK)
					{
						_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));	
					}
				}
			}
			else
			{
				// parentCard is not double
				if(_currentCard.IsDouble())
				{
					// current card is double
					if(_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.FRONT)
					{
						if(_currentCard.GetNode(_parentId) == NodeEnum.RIGHT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));
						}
						else if(_currentCard.GetNode(_parentId) == NodeEnum.LEFT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));
						}
					}
					else if(_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.BACK)
					{
						if(_currentCard.GetNode(_parentId) == NodeEnum.RIGHT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));
						}
						else if(_currentCard.GetNode(_parentId) == NodeEnum.LEFT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));
						}
					}
				}
				else
				{
					// current card is not double
					if(_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.FRONT)
					{
						// if currentCard is attached to parentCard's front
						if(_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
						{
							// if the currentCard's front/head is attached
							_currentCard.SetOrientation(Transform2D.Rotate180(_parentCard.Orientation));
						}
						else if(_currentCard.GetNode(_parentId) == NodeEnum.BACK)
						{
							_currentCard.SetOrientation(_parentCard.Orientation);	
						}
					}
					else if(_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.BACK)
					{
						if(_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
						{
							// if the currentCard's front/head is attached
							_currentCard.SetOrientation(_parentCard.Orientation);
						}
						else if(_currentCard.GetNode(_parentId) == NodeEnum.BACK)
						{
							_currentCard.SetOrientation(Transform2D.Rotate180(_parentCard.Orientation));	
						}
					}
				}
			}
		}
		
		foreach (NodeEnum node in Enum.GetValues(typeof(NodeEnum)))
		{
			CalcForwardKinematics(_currentCard.GetCardIdAtNode(node));
		}
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
	*/
}