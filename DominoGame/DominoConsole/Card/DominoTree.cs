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
			_currentCard.SetOrientation(_desiredCardKinematics.CurrentOrientation);
			_currentCard.Position.SetX(_parentCard.Position.X + _desiredCardKinematics.CurrentOffsetX);
			_currentCard.Position.SetY(_parentCard.Position.Y + _desiredCardKinematics.CurrentOffsetY);
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