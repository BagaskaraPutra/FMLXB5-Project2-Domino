namespace DominoConsole;

public class CardGUI : Card
{
	public PositionStruct CenterLocal;
	public PositionStruct HeadNorthCenterLocal {get; set;}
	public PositionStruct HeadSouthCenterLocal {get; set;}
	public PositionStruct HeadWestCenterLocal {get; set;}
	public PositionStruct HeadEastCenterLocal {get; set;}
	private static PositionStruct _headNorthCenterLocal;
	private static PositionStruct _headSouthCenterLocal;
	private static PositionStruct _headWestCenterLocal;
	private static PositionStruct _headEastCenterLocal;
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
	private static List<List<char>> _headNorthImage;
	private static List<List<char>> _headSouthImage;
	private static List<List<char>> _headWestImage; 
	private static List<List<char>> _headEastImage; 
	public void UpdateConfig()
	{
		_headNorthImage   	= new (HeadNorthImage);
		_headSouthImage 	= new (HeadSouthImage);
		_headWestImage 	= new (HeadWestImage);
		_headEastImage		= new (HeadEastImage);
		headPosNorthLocal = FindCharInImage('H', _headNorthImage);
		tailPosNorthLocal = FindCharInImage('T', _headNorthImage);
		headPosSouthLocal = FindCharInImage('H', _headSouthImage);
		tailPosSouthLocal = FindCharInImage('T', _headSouthImage);
		headPosWestLocal = FindCharInImage('H', _headWestImage);
		tailPosWestLocal = FindCharInImage('T', _headWestImage);
		headPosEastLocal = FindCharInImage('H', _headEastImage);
		tailPosEastLocal = FindCharInImage('T', _headEastImage);
		_headNorthCenterLocal = HeadNorthCenterLocal;
		_headSouthCenterLocal = HeadSouthCenterLocal;
		_headWestCenterLocal  = HeadWestCenterLocal;
		_headEastCenterLocal  = HeadEastCenterLocal;
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
				_image = new(_headNorthImage);
				_image[headPosNorthLocal.X][headPosNorthLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosNorthLocal.X][tailPosNorthLocal.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.SOUTH:
			{
				_image = new(_headSouthImage);
				_image[headPosSouthLocal.X][headPosSouthLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosSouthLocal.X][tailPosSouthLocal.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.EAST:
			{
				_image = new(_headEastImage);
				_image[headPosEastLocal.X][headPosEastLocal.Y] = Head.ToString().ToCharArray()[0];
				_image[tailPosEastLocal.X][tailPosEastLocal.Y] = Tail.ToString().ToCharArray()[0];
				break;
			}
			case OrientationEnum.WEST:
			{
				_image = new(_headWestImage);
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
				CenterLocal = _headNorthCenterLocal;
				NorthSuitGlobal = Position + headPosNorthLocal - _headNorthCenterLocal;
				SouthSuitGlobal = Position + tailPosNorthLocal - _headNorthCenterLocal;
				WestSuitGlobal  = Position;
				EastSuitGlobal 	= Position;
				NorthEdgeToCenterLength = Math.Abs(_headNorthCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(_headNorthImage.Count - 1 - _headNorthCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(_headNorthCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(_headNorthImage[0].Count - 1 - _headNorthCenterLocal.Y);
				break;
			}
			case OrientationEnum.SOUTH:
			{
				CenterLocal = _headSouthCenterLocal;
				NorthSuitGlobal = Position + tailPosSouthLocal - _headSouthCenterLocal;
				SouthSuitGlobal = Position + headPosSouthLocal - _headSouthCenterLocal;
				WestSuitGlobal  = Position;
				EastSuitGlobal 	= Position;
				NorthEdgeToCenterLength = Math.Abs(_headSouthCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(_headSouthImage.Count - 1 - _headSouthCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(_headSouthCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(_headSouthImage[0].Count - 1 - _headSouthCenterLocal.Y);
				break;
			}
			case OrientationEnum.WEST:
			{
				CenterLocal = _headWestCenterLocal;
				NorthSuitGlobal = Position;
				SouthSuitGlobal = Position;
				WestSuitGlobal  = Position + headPosWestLocal - _headWestCenterLocal;
				EastSuitGlobal 	= Position + tailPosWestLocal - _headWestCenterLocal;
				NorthEdgeToCenterLength = Math.Abs(_headWestCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(_headWestImage.Count - 1 - _headWestCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(_headWestCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(_headWestImage[0].Count - 1 - _headWestCenterLocal.Y);
				break;
			}
			case OrientationEnum.EAST:
			{
				CenterLocal = _headEastCenterLocal;
				NorthSuitGlobal = Position;
				SouthSuitGlobal = Position;
				WestSuitGlobal  = Position + tailPosEastLocal - _headEastCenterLocal;
				EastSuitGlobal 	= Position + headPosEastLocal - _headEastCenterLocal;
				NorthEdgeToCenterLength = Math.Abs(_headEastCenterLocal.X);
				SouthEdgeToCenterLength = Math.Abs(_headEastImage.Count - 1 - _headEastCenterLocal.X);
				WestEdgeToCenterLength  = Math.Abs(_headEastCenterLocal.Y);
				EastEdgeToCenterLength  = Math.Abs(_headEastImage[0].Count - 1 - _headEastCenterLocal.Y);
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