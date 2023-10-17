namespace DominoConsole;

public class CardGUI : Card
{
	public PositionStruct CenterLocal;
	public PositionStruct HeadNorthCenterLocal {get; set;}
	public PositionStruct HeadSouthCenterLocal {get; set;}
	public PositionStruct HeadWestCenterLocal {get; set;}
	public PositionStruct HeadEastCenterLocal {get; set;}
	public static PositionStruct StaticHeadNorthCenterLocal {get; private set;}
	public static PositionStruct StaticHeadSouthCenterLocal {get; private set;}
	public static PositionStruct StaticHeadWestCenterLocal {get; private set;}
	public static PositionStruct StaticHeadEastCenterLocal {get; private set;}
	public PositionStruct NorthEdgeGlobal {get; private set;} 
	public PositionStruct SouthEdgeGlobal {get; private set;}
	public PositionStruct WestEdgeGlobal {get; private set;} 
	public PositionStruct EastEdgeGlobal {get; private set;}
	public PositionStruct NorthSuitGlobal {get; private set;} 
	public PositionStruct SouthSuitGlobal {get; private set;}
	public PositionStruct WestSuitGlobal {get; private set;} 
	public PositionStruct EastSuitGlobal {get; private set;}
	public int NorthEdgeToCenterLength {get; private set;} 
	public int SouthEdgeToCenterLength {get; private set;}
	public int WestEdgeToCenterLength {get; private set;} 
	public int EastEdgeToCenterLength {get; private set;}
	public int LengthX {get; private set;}
	public int LengthY {get; private set;}
	private List<List<char>>? _image;
	private static PositionStruct headPosNorthLocal, headPosSouthLocal, headPosWestLocal, headPosEastLocal;
	private static PositionStruct tailPosNorthLocal, tailPosSouthLocal, tailPosWestLocal, tailPosEastLocal;
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
	public List<List<char>> HeadNorthImage {get; set; }
	public List<List<char>> HeadSouthImage {get; set; }
	public List<List<char>> HeadWestImage {get; set; }
	public List<List<char>> HeadEastImage {get; set; }
	public static List<List<char>> StaticHeadNorthImage {get; private set; }
	public static List<List<char>> StaticHeadSouthImage {get; private set; }
	public static List<List<char>> StaticHeadWestImage {get; private set; }
	public static List<List<char>> StaticHeadEastImage {get; private set; }
	public void UpdateConfig()
	{
		StaticHeadNorthImage   	= new (HeadNorthImage);
		StaticHeadSouthImage 	= new (HeadSouthImage);
		StaticHeadWestImage 	= new (HeadWestImage);
		StaticHeadEastImage		= new (HeadEastImage);
		headPosNorthLocal = FindCharInImage('H', StaticHeadNorthImage);
		tailPosNorthLocal = FindCharInImage('T', StaticHeadNorthImage);
		headPosSouthLocal = FindCharInImage('H', StaticHeadSouthImage);
		tailPosSouthLocal = FindCharInImage('T', StaticHeadSouthImage);
		headPosWestLocal = FindCharInImage('H', StaticHeadWestImage);
		tailPosWestLocal = FindCharInImage('T', StaticHeadWestImage);
		headPosEastLocal = FindCharInImage('H', StaticHeadEastImage);
		tailPosEastLocal = FindCharInImage('T', StaticHeadEastImage);
		StaticHeadNorthCenterLocal = HeadNorthCenterLocal;
		StaticHeadSouthCenterLocal = HeadSouthCenterLocal;
		StaticHeadWestCenterLocal  = HeadWestCenterLocal;
		StaticHeadEastCenterLocal  = HeadEastCenterLocal;
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
				_image = new(StaticHeadNorthImage);
				_image[headPosNorthLocal.X][headPosNorthLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosNorthLocal.X][tailPosNorthLocal.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.SOUTH:
			{
				_image = new(StaticHeadSouthImage);
				_image[headPosSouthLocal.X][headPosSouthLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosSouthLocal.X][tailPosSouthLocal.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.EAST:
			{
				_image = new(StaticHeadEastImage);
				_image[headPosEastLocal.X][headPosEastLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosEastLocal.X][tailPosEastLocal.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.WEST:
			{
				_image = new(StaticHeadWestImage);
				_image[headPosWestLocal.X][headPosWestLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosWestLocal.X][tailPosWestLocal.Y] = Tail.ToString().ToCharArray()[0];
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
		// CenterLocal.X = (int)LengthX/2;
		// CenterLocal.Y = (int)LengthY/2;
		switch(Orientation)
		{
			case OrientationEnum.NORTH:
			{
				CenterLocal = StaticHeadNorthCenterLocal;
				NorthSuitGlobal = Position + headPosNorthLocal - StaticHeadNorthCenterLocal;
				SouthSuitGlobal = Position + tailPosNorthLocal - StaticHeadNorthCenterLocal;
				WestSuitGlobal  = Position;
				EastSuitGlobal 	= Position;
				NorthEdgeToCenterLength = Math.Abs(StaticHeadNorthCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(StaticHeadNorthImage.Count - 1 - StaticHeadNorthCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(StaticHeadNorthCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(StaticHeadNorthImage[0].Count - 1 - StaticHeadNorthCenterLocal.Y);
				break;
			}
			case OrientationEnum.SOUTH:
			{
				CenterLocal = StaticHeadSouthCenterLocal;
				NorthSuitGlobal = Position + tailPosSouthLocal - StaticHeadSouthCenterLocal;
				SouthSuitGlobal = Position + headPosSouthLocal - StaticHeadSouthCenterLocal;
				WestSuitGlobal  = Position;
				EastSuitGlobal 	= Position;
				NorthEdgeToCenterLength = Math.Abs(StaticHeadSouthCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(StaticHeadSouthImage.Count - 1 - StaticHeadSouthCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(StaticHeadSouthCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(StaticHeadSouthImage[0].Count - 1 - StaticHeadSouthCenterLocal.Y);
				break;
			}
			case OrientationEnum.WEST:
			{
				CenterLocal = StaticHeadWestCenterLocal;
				NorthSuitGlobal = Position;
				SouthSuitGlobal = Position;
				WestSuitGlobal  = Position + headPosWestLocal - StaticHeadWestCenterLocal;
				EastSuitGlobal 	= Position + tailPosWestLocal - StaticHeadWestCenterLocal;
				NorthEdgeToCenterLength = Math.Abs(StaticHeadWestCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(StaticHeadWestImage.Count - 1 - StaticHeadWestCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(StaticHeadWestCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(StaticHeadWestImage[0].Count - 1 - StaticHeadWestCenterLocal.Y);
				break;
			}
			case OrientationEnum.EAST:
			{
				CenterLocal = StaticHeadEastCenterLocal;
				NorthSuitGlobal = Position;
				SouthSuitGlobal = Position;
				WestSuitGlobal  = Position + tailPosEastLocal - StaticHeadEastCenterLocal;
				EastSuitGlobal 	= Position + headPosEastLocal - StaticHeadEastCenterLocal;
				NorthEdgeToCenterLength = Math.Abs(StaticHeadEastCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(StaticHeadEastImage.Count - 1 - StaticHeadEastCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(StaticHeadEastCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(StaticHeadEastImage[0].Count - 1 - StaticHeadEastCenterLocal.Y);
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