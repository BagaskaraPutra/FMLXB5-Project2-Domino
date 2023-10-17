namespace DominoConsole;
public struct LengthStruct
{
	private int _x, _y;
	public LengthStruct(int x, int y)
	{
		X = x;
		Y = y;
	}
	public int X 
	{
		get => _x; 
		set {if (value < 0) _x = 0;
			else _x = value;}
	}
	public int Y 
	{
		get => _y; 
		set {if (value < 0) _y = 0;
			else _y = value;}
	}
	public static LengthStruct operator + (LengthStruct a, LengthStruct b)
	{
		LengthStruct result = new(0,0);
		result.X = a.X + b.X;
		result.Y = a.Y + b.Y;
		return result;
	}
	public static LengthStruct operator - (LengthStruct a, LengthStruct b)
	{
		LengthStruct result = new(0,0);
		result.X = a.X - b.X;
		result.Y = a.Y - b.Y;
		return result;
	}
}