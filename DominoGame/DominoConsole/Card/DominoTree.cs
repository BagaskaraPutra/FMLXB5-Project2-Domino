namespace DominoConsole;

public class DominoTree
{
	private int _parentId;
	private List<Card> _tableCards;
	private Card _currentCard;
	private Card _parentCard;
	private readonly List<CardKinematics> _cardKinematicsLUT;
	private CardKinematics _desiredCardKinematics;
	public DominoTree(List<Card> cards, List<CardKinematics> lookupTable)
	{
		_tableCards = cards;
		_currentCard = new();
		_parentCard = new();
		_cardKinematicsLUT = lookupTable;
		_desiredCardKinematics = new();
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
			_desiredCardKinematics = _cardKinematicsLUT.FirstOrDefault(x =>
									x.ParentIsDouble == _parentCard.IsDouble() &&
									x.CurrentIsDouble == _currentCard.IsDouble() &&
									x.ParentNode == _parentCard.GetNode(_currentCard.GetId()) &&
									x.CurrentNode == _currentCard.GetNode(_parentId) &&
									x.ParentOrientation == _parentCard.Orientation
									);
			if (_desiredCardKinematics == null)
			{
				Console.WriteLine("Not yet implemented in lookup table");
				return;
			}
			// Console.WriteLine($"desired orientation: {_desiredCardKinematics.CurrentOrientation}");
			_currentCard.SetOrientation(_desiredCardKinematics.CurrentOrientation);
			_currentCard.Position.SetX(_parentCard.Position.X + _desiredCardKinematics.CurrentOffsetX);
			_currentCard.Position.SetY(_parentCard.Position.Y + _desiredCardKinematics.CurrentOffsetY);
			Console.WriteLine($"parent card [{_parentCard.Head}|{_parentCard.Tail}] IsDouble: {_parentCard.IsDouble()}, \t node: {_parentCard.GetNode(_currentCard.GetId())}, \t orientation: {_parentCard.Orientation} \t x: {_parentCard.Position.X} \t y: {_parentCard.Position.Y}");
			Console.WriteLine($"current card [{_currentCard.Head}|{_currentCard.Tail}] IsDouble: {_currentCard.IsDouble()}, \t node: {_currentCard.GetNode(_parentCard.GetId())}, \t orientation: {_currentCard.Orientation} \t x: {_currentCard.Position.X} \t y: {_currentCard.Position.Y}");
			// Console.WriteLine($"parent  card [{_parentCard.Head}|{_parentCard.Tail}] position x: {_parentCard.Position.X} \t y: {_parentCard.Position.Y}");
			// Console.WriteLine($"current card [{_currentCard.Head}|{_currentCard.Tail}] position x: {_currentCard.Position.X} \t y: {_currentCard.Position.Y}");
		}
	}
}