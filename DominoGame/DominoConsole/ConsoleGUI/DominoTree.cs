namespace DominoConsole;

public class DominoTree
{
	private int _parentId;
	private List<CardGUI> _tableCardsGUI;
	private CardGUI _currentCard;
	private CardGUI _parentCard;
	private readonly List<CardKinematics> _cardKinematicsLUT;
	private CardKinematics? _desiredCardKinematics;
	private int _numCardsSinceWestBorder;
	private int _numCardsSinceEastBorder;
	public DominoTree(List<CardKinematics> lookupTable)
	{
		_tableCardsGUI = new();
		_currentCard = new();
		_parentCard = new();
		_cardKinematicsLUT = lookupTable;
		_desiredCardKinematics = new();
	}
	public void UpdateTree(List<ICard> tableCards)
	{
		// Console.WriteLine("Update tree!");
		foreach (var card in tableCards)
		{
			if (!_tableCardsGUI.Any(x => x.GetId() == card.GetId()))
			{
				_tableCardsGUI.Add(new CardGUI(card));
				// Console.WriteLine($"Added new card: [{card.Head}|{card.Tail}]");	
			}
		}
	}
	public List<CardGUI> GetTableCardsGUI()
	{
		return _tableCardsGUI;
	}
	public void CalcForwardKinematics(int id)
	{
		_currentCard = _tableCardsGUI.FirstOrDefault(x => x.GetId() == id);
		if (_currentCard == null)
		{
			return;
		}
		_currentCard.RefreshImage();

		_parentId   = _currentCard.ParentId;
		_parentCard = _tableCardsGUI.FirstOrDefault(x => x.GetId() == _parentId);
		if (_parentId == -1)
		{
			return;
		}
		_parentCard.RefreshImage();
		
		_desiredCardKinematics = _cardKinematicsLUT.FirstOrDefault(x =>
								x.ParentIsDouble 	== _parentCard.IsDouble() &&
								x.CurrentIsDouble 	== _currentCard.IsDouble() &&
								x.ParentNode 		== _parentCard.GetNode(_currentCard.GetId()) &&
								x.CurrentNode 		== _currentCard.GetNode(_parentId) &&
								x.ParentOrientation == _parentCard.Orientation
								);
		if (_desiredCardKinematics == null)
		{
			Console.WriteLine("Not yet implemented in lookup table");
			return;
		}
		// Console.WriteLine($"desired orientation: {_desiredCardKinematics.CurrentOrientation}");
		_currentCard.SetOrientation(_desiredCardKinematics.CurrentOrientation);
		Transform2D.MoveUntilEdge(ref _currentCard, _parentCard, _desiredCardKinematics.MoveDirection);
		_currentCard.UpdateStates();
		// _currentCard.Position.X = (_parentCard.Position.X + _desiredCardKinematics.CurrentOffsetX);
		// _currentCard.Position.Y = (_parentCard.Position.Y + _desiredCardKinematics.CurrentOffsetY);

		int positionX = _currentCard.Position.X;
		int positionY = _currentCard.Position.Y;
		OrientationEnum orientation = _currentCard.Orientation;
		if (positionY - _currentCard.WestEdgeToCenterLength <= 0)
		{
			if (!_currentCard.IsDouble())
			{
				_currentCard.SetOrientation(Transform2D.RotateCW(orientation));	
			}
			Transform2D.MoveUntilEdge(ref _currentCard, _parentCard, OrientationEnum.NORTH);
			_currentCard.Position.Y = _parentCard.WestSuitGlobal.Y;
			_numCardsSinceWestBorder++;
			// TODO: Save the card id that hits the border, then count its number of child cards
			// if more than 2 children, then rotate the 3rd clockwise
			// Console.WriteLine($"Is exceeds border WEST, _parentCard.WestSuitGlobal.Y: {_parentCard.WestSuitGlobal.Y}");
		}
		else if (positionY + _currentCard.EastEdgeToCenterLength + 1 >= Console.WindowWidth)
		{
			if (!_currentCard.IsDouble())
			{
				_currentCard.SetOrientation(Transform2D.RotateCW(_currentCard.Orientation));	
			}
			Transform2D.MoveUntilEdge(ref _currentCard, _parentCard, OrientationEnum.SOUTH);
			_currentCard.Position.Y = _parentCard.EastSuitGlobal.Y;
			_numCardsSinceEastBorder++;
			// Console.WriteLine($"Is exceeds border EAST, _parentCard.EastSuitGlobal.Y: {_parentCard.EastSuitGlobal.Y}");
		}
		// else if (positionX - _currentCard.NorthEdgeToCenterLength <= 0 && _numCardsSinceWestBorder >= 2) //-_currentCard.LengthX)
		// {
		// 	if (!_currentCard.IsDouble())
		// 	{
		// 		_currentCard.SetOrientation(Transform2D.RotateCW(orientation));	
		// 	}
		// 	Transform2D.MoveUntilEdge(ref _currentCard, _parentCard, OrientationEnum.EAST);
		// 	_currentCard.Position.X = _parentCard.NorthSuitGlobal.X;
		// 	Console.WriteLine($"[{_currentCard.Head}|{_currentCard.Tail}] exceeds border NORTH, [{_parentCard.Head}|{_parentCard.Tail}].NorthSuitGlobal.X: {_parentCard.NorthSuitGlobal.X}");
		// }
		// else if (positionX + _currentCard.SouthEdgeToCenterLength + 1 >= TableGUI.DefaultMaxTableRowSize && _numCardsSinceEastBorder >= 2)
		// {
		// 	if (!_currentCard.IsDouble())
		// 	{
		// 		_currentCard.SetOrientation(Transform2D.RotateCW(_currentCard.Orientation));	
		// 	}
		// 	Transform2D.MoveUntilEdge(ref _currentCard, _parentCard, OrientationEnum.WEST);
		// 	_currentCard.Position.X = _parentCard.SouthSuitGlobal.X;
		// 	Console.WriteLine($"[{_currentCard.Head}|{_currentCard.Tail}] exceeds border SOUTH, [{_parentCard.Head}|{_parentCard.Tail}].SouthSuitGlobal.X: {_parentCard.SouthSuitGlobal.X}");
		// }
		// else
		// {
		// 	_numCardsSinceEastBorder = 0;
		// 	_numCardsSinceWestBorder = 0;
		// }
		_currentCard.UpdateStates();
		// Console.WriteLine($"parent card [{_parentCard.Head}|{_parentCard.Tail}] IsDouble: {_parentCard.IsDouble()}, \t node: {_parentCard.GetNode(_currentCard.GetId())}, \t orientation: {_parentCard.Orientation} \t x: {_parentCard.Position.X} \t y: {_parentCard.Position.Y}");
		// Console.WriteLine($"current card [{_currentCard.Head}|{_currentCard.Tail}] IsDouble: {_currentCard.IsDouble()}, \t node: {_currentCard.GetNode(_parentCard.GetId())}, \t orientation: {_currentCard.Orientation} \t x: {_currentCard.Position.X} \t y: {_currentCard.Position.Y}");
	}
	public void MoveAllSouth(int offsetX)
	{
		foreach (var card in _tableCardsGUI)
		{
			int currentPosX = card.Position.X;
			card.Position.X = currentPosX + offsetX;
		}
	}
	public void MoveAllEast(int offsetY)
	{
		foreach (var card in _tableCardsGUI)
		{
			int currentPosY = card.Position.Y;
			card.Position.Y = currentPosY + offsetY;
		}
	}
}