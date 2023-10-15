namespace DominoConsole;

public class CardGUI : Card
{
	private List<List<char>>? _cardImage;
	public OrientationEnum Orientation {get; private set;}
	public PositionStruct Position;
	
	public CardGUI(){}
	public CardGUI(Card card)
	{
		_id  = card.GetId();
		ParentId = card.ParentId;
		Head = card.Head;
		Tail = card.Tail;
		_nodeId = new int[Enum.GetValues(typeof(NodeEnum)).Length];
		_nodeId = card.GetCardIdArrayAtNodes();
		Orientation = OrientationEnum.NORTH;
		_cardImage = new();
	}
	public List<List<char>>? VerticalCardImage {get; set; }
	public List<List<char>>? HorizontalCardImage {get; set; }
	public static List<List<char>>? StaticVerticalCardImage {get; private set; }
	public static List<List<char>>? StaticHorizontalCardImage {get; private set; }
	public void UpdateConfig()
	{
		StaticVerticalCardImage = VerticalCardImage;
		StaticHorizontalCardImage = HorizontalCardImage;
	}
	public bool SetOrientation(OrientationEnum orientation)
	{
		Orientation = orientation;
		return true;
	}
	public bool SetPosition(PositionStruct position)
	{
		Position = position;
		return true;
	}
	public List<List<char>> GetCardImage()
	{
		if (Orientation == OrientationEnum.NORTH || Orientation == OrientationEnum.SOUTH)
		{
			_cardImage = StaticVerticalCardImage;
		}
		else
		{
			_cardImage = StaticHorizontalCardImage;
		}
		switch (Orientation)
		{
			case OrientationEnum.NORTH:
				{
					_cardImage[1][2] = Head.ToString().ToCharArray()[0];
					_cardImage[3][2] = Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.SOUTH:
				{
					_cardImage[3][2] = Head.ToString().ToCharArray()[0];
					_cardImage[1][2] = Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.EAST:
				{
					_cardImage[1][6] = Head.ToString().ToCharArray()[0];
					_cardImage[1][2] = Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.WEST:
				{
					_cardImage[1][2] = Head.ToString().ToCharArray()[0];
					_cardImage[1][6] = Tail.ToString().ToCharArray()[0];
					break;
				}
			default: break;
		}
		return _cardImage;
	}
}