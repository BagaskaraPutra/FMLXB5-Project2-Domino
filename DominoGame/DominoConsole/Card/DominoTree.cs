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
		// TODO: Try without recursion
		_currentCard = _tableCards.FirstOrDefault(x => x.GetId() == id);
		if (_currentCard == null)
		{
			return;
		}

		_parentId = _currentCard.ParentId;
		_parentCard = _tableCards.FirstOrDefault(x => x.GetId() == _parentId);

		if (_parentId != -1)
		{
			if (_parentCard.IsDouble())
			{
				if (_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.LEFT)
				{
					// if currentCard is attached to parentCard's left
					if (_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
					{
						// if the currentCard's front/head is attached
						_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));
						Console.WriteLine("parent Double LEFT current Single FRONT Rotate CW");
					}
					else if (_currentCard.GetNode(_parentId) == NodeEnum.BACK)
					{
						_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));
						Console.WriteLine("parent Double LEFT current Single BACK Rotate CCW");
					}
					_currentCard.Position.SetX(_parentCard.Position.X + 1);
					_currentCard.Position.SetY(_parentCard.Position.Y - 5);
				}
				else if (_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.RIGHT)
				{
					// if currentCard is attached to parentCard's right:
					if (_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
					{
						// if the currentCard's front/head is attached
						_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));
						Console.WriteLine("parent Double RIGHT current Single FRONT Rotate CCW");
					}
					else if (_currentCard.GetNode(_parentId) == NodeEnum.BACK)
					{
						_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));
						Console.WriteLine("parent Double RIGHT current Single BACK Rotate CW");
					}
					_currentCard.Position.SetX(_parentCard.Position.X + 1);
					_currentCard.Position.SetY(_parentCard.Position.Y + 5);
				}
			}
			else
			{
				// parentCard is not double
				if (_currentCard.IsDouble())
				{
					// current card is double
					if (_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.FRONT)
					{
						if (_currentCard.GetNode(_parentId) == NodeEnum.RIGHT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));

							Console.WriteLine("parent Single FRONT current Double RIGHT Rotate CW");
						}
						else if (_currentCard.GetNode(_parentId) == NodeEnum.LEFT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));
							Console.WriteLine("parent Single FRONT current Double LEFT Rotate CCW");
						}
					}
					else if (_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.BACK)
					{
						if (_currentCard.GetNode(_parentId) == NodeEnum.RIGHT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCW(_parentCard.Orientation));
							Console.WriteLine("parent Single BACK current Double RIGHT Rotate CW");
						}
						else if (_currentCard.GetNode(_parentId) == NodeEnum.LEFT)
						{
							_currentCard.SetOrientation(Transform2D.RotateCCW(_parentCard.Orientation));
							Console.WriteLine("parent Single BACK current Double LEFT Rotate CCW");
						}
					}
				}
				else
				{
					// current card is not double
					if (_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.FRONT)
					{
						// if currentCard is attached to parentCard's front
						if (_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
						{
							// if the currentCard's front/head is attached
							_currentCard.SetOrientation(Transform2D.Rotate180(_parentCard.Orientation));
							Console.WriteLine("parent Single FRONT current Single FRONT Rotate 180");
						}
						else if (_currentCard.GetNode(_parentId) == NodeEnum.BACK)
						{
							_currentCard.SetOrientation(_parentCard.Orientation);
							Console.WriteLine("parent Single FRONT current Single BACK");
						}
					}
					else if (_parentCard.GetNode(_currentCard.GetId()) == NodeEnum.BACK)
					{
						if (_currentCard.GetNode(_parentId) == NodeEnum.FRONT)
						{
							// if the currentCard's front/head is attached
							_currentCard.SetOrientation(_parentCard.Orientation);
							Console.WriteLine("parent Single BACK current Single FRONT");
						}
						else if (_currentCard.GetNode(_parentId) == NodeEnum.BACK)
						{
							_currentCard.SetOrientation(Transform2D.Rotate180(_parentCard.Orientation));
							Console.WriteLine("parent Single BACK current Single BACK Rotate 180");
						}
					}
					switch (_parentCard.Orientation)
					{
						case OrientationEnum.NORTH:
							_currentCard.Position.SetX(_parentCard.Position.X + 5);
							_currentCard.Position.SetY(_parentCard.Position.Y);
							break;
						case OrientationEnum.EAST:
							_currentCard.Position.SetX(_parentCard.Position.X);
							_currentCard.Position.SetY(_parentCard.Position.Y - 5);
							break;
						case OrientationEnum.SOUTH:
							_currentCard.Position.SetX(_parentCard.Position.X - 5);
							_currentCard.Position.SetY(_parentCard.Position.Y);
							break;
						case OrientationEnum.WEST:
							_currentCard.Position.SetX(_parentCard.Position.X);
							_currentCard.Position.SetY(_parentCard.Position.Y + 5);
							break;
						default: break;
					}
				}
			}
			Console.WriteLine($"parent position x: {_parentCard.Position.X} \t y: {_parentCard.Position.Y}");
			Console.WriteLine($"current position x: {_currentCard.Position.X} \t y: {_currentCard.Position.Y}");
		}

		// int openEndCount = 0;
		// foreach (NodeEnum node in Enum.GetValues(typeof(NodeEnum)))
		// {
		// 	if(_currentCard.GetCardIdAtNode(node) == -1)
		// 	{
		// 		openEndCount++;
		// 	}
		// }
		// if(openEndCount >= 3)
		// {
		// 	// Console.WriteLine("Open ended");
		// 	return;
		// }

		// foreach (NodeEnum node in Enum.GetValues(typeof(NodeEnum)))
		// {
		// 	if(_currentCard == null)
		// 	{
		// 		return;
		// 	}
		// 	if (_currentCard.GetCardIdAtNode(node) != -1)
		// 	{
		// 		CalcForwardKinematics(_currentCard.GetCardIdAtNode(node));	
		// 	}
		// }
	}
}