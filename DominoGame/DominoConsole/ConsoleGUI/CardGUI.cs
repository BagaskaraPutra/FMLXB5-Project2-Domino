namespace DominoConsole;

public class CardGUI : Card
{
	public PositionStruct CenterLocal;
	public PositionStruct NorthSuitGlobal {get; private set;} 
	public PositionStruct SouthSuitGlobal {get; private set;}
	public PositionStruct WestSuitGlobal {get; private set;} 
	public PositionStruct EastSuitGlobal {get; private set;}
	public int LengthX {get; private set;}
	public int LengthY {get; private set;}
	private List<List<char>>? _image;
	private static PositionStruct headPosNorth, headPosSouth, headPosWest, headPosEast;
	private static PositionStruct tailPosNorth, tailPosSouth, tailPosWest, tailPosEast;
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
	public List<List<char>> NorthCardImage {get; set; }
	public List<List<char>> SouthCardImage {get; set; }
	public List<List<char>> WestCardImage {get; set; }
	public List<List<char>> EastCardImage {get; set; }
	public static List<List<char>> StaticNorthCardImage {get; private set; }
	public static List<List<char>> StaticSouthCardImage {get; private set; }
	public static List<List<char>> StaticWestCardImage {get; private set; }
	public static List<List<char>> StaticEastCardImage {get; private set; }
	public void UpdateConfig()
	{
		StaticNorthCardImage   	= new (NorthCardImage);
		StaticSouthCardImage 	= new (SouthCardImage);
		StaticWestCardImage 	= new (WestCardImage);
		StaticEastCardImage		= new (EastCardImage);
		headPosNorth = FindCharInImage('H', StaticNorthCardImage);
		tailPosNorth = FindCharInImage('T', StaticNorthCardImage);
		headPosSouth = FindCharInImage('H', StaticSouthCardImage);
		tailPosSouth = FindCharInImage('T', StaticSouthCardImage);
		headPosWest = FindCharInImage('H', StaticWestCardImage);
		tailPosWest = FindCharInImage('T', StaticWestCardImage);
		headPosEast = FindCharInImage('H', StaticEastCardImage);
		tailPosEast = FindCharInImage('T', StaticEastCardImage);
	}
	public bool SetOrientation(OrientationEnum orientation)
	{
		Orientation = orientation;
		RefreshImage();
		return true;
	}
	private PositionStruct FindCharInImage(char element, List<List<char>> _image)
	{
		PositionStruct positionInImage = new(0,0);
		for (int i=0; i < _image.Count; i++)
		{
			for (int j=0; j < _image[i].Count; j++)
			{
				if (_image[i][j].Equals(element))
				{
					positionInImage.X = i;
					positionInImage.Y = j;
					break;
				}
			}
		}
		return positionInImage;
	}
	public void RefreshImage()
	{
		if(_image == null)
		{
			return;
		}
		switch (Orientation)
		{
			case OrientationEnum.NORTH:
			{
				_image = new(StaticNorthCardImage);
				_image[headPosNorth.X][headPosNorth.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosNorth.X][tailPosNorth.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.SOUTH:
			{
				_image = new(StaticSouthCardImage);
				_image[headPosSouth.X][headPosSouth.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosSouth.X][tailPosSouth.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.EAST:
			{
				_image = new(StaticEastCardImage);
				_image[headPosEast.X][headPosEast.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosEast.X][tailPosEast.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.WEST:
			{
				_image = new(StaticWestCardImage);
				_image[headPosWest.X][headPosWest.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosWest.X][tailPosWest.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			default: break;
		}
		UpdateStates();
	}
	public void UpdateStates()
	{
		LengthX = _image.Count;
		LengthY = _image[0].Count;
		CenterLocal.X = (int)LengthX/2;
		CenterLocal.Y = (int)LengthY/2;
		switch(Orientation)
		{
			case OrientationEnum.NORTH:
			{
				NorthSuitGlobal = Position + headPosNorth - CenterLocal;
				SouthSuitGlobal = Position + tailPosNorth - CenterLocal;
				WestSuitGlobal  = Position;
				EastSuitGlobal 	= Position;
				break;
			}
			case OrientationEnum.SOUTH:
			{
				NorthSuitGlobal = Position + tailPosSouth - CenterLocal;
				SouthSuitGlobal = Position + headPosSouth - CenterLocal;
				WestSuitGlobal  = Position;
				EastSuitGlobal 	= Position;
				break;
			}
			case OrientationEnum.WEST:
			{
				NorthSuitGlobal = Position;
				SouthSuitGlobal = Position;
				WestSuitGlobal  = Position + headPosWest - CenterLocal;
				EastSuitGlobal 	= Position + tailPosWest - CenterLocal;
				// Console.WriteLine($"WestSuitGlobal x: {WestSuitGlobal.X} y: {WestSuitGlobal.Y}");
				break;
			}
			case OrientationEnum.EAST:
			{
				NorthSuitGlobal = Position;
				SouthSuitGlobal = Position;
				WestSuitGlobal  = Position + tailPosEast - CenterLocal;
				EastSuitGlobal 	= Position + headPosEast - CenterLocal;
				// Console.WriteLine($"EastSuitGlobal x: {EastSuitGlobal.X} y: {EastSuitGlobal.Y}");
				break;
			}
			default: break;
		}
	}
	public List<List<char>> GetImage()
	{
		return _image;
	}
}