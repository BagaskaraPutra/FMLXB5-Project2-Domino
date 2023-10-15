namespace DominoConsole;

public class CardGUI : Card
{
	public int CenterPosX {get; private set;} 
	public int CenterPosY {get; private set;} 
	public int HeadPosX {get; private set;} 
	public int HeadPosY {get; private set;} 
	public int TailPosX {get; private set;}  
	public int TailPosY {get; private set;} 
	public int LengthX {get; private set;}
	public int LengthY {get; private set;}
	private List<List<char>>? _image;
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
		if(card.IsDouble())
		{
			Orientation = OrientationEnum.NORTH;
		}
		else
		{
			Orientation = OrientationEnum.WEST;
		}
		_image = new();
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
	public List<List<char>> GetImage()
	{
		if (Orientation == OrientationEnum.NORTH || Orientation == OrientationEnum.SOUTH)
		{
			_image = StaticVerticalCardImage;
		}
		else
		{
			_image = StaticHorizontalCardImage;
		}
		LengthX = _image.Count;
		LengthY = _image[0].Count;
		CenterPosX = (int)LengthX/2;
		CenterPosY = (int)LengthY/2;
		switch (Orientation)
		{
			case OrientationEnum.NORTH:
			{
				_image[1][2] = Head.ToString().ToCharArray()[0];
				_image[3][2] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.SOUTH:
			{
				_image[3][2] = Head.ToString().ToCharArray()[0];
				_image[1][2] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.EAST:
			{
				_image[1][6] = Head.ToString().ToCharArray()[0];
				_image[1][2] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.WEST:
			{
				_image[1][2] = Head.ToString().ToCharArray()[0];
				_image[1][6] = Tail.ToString().ToCharArray()[0];
				break;
			}
			default: break;
		}
		return _image;
	}
}