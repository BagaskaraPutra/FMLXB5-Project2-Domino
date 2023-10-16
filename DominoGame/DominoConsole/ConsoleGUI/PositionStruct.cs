namespace DominoConsole;
public struct PositionStruct
{
	public PositionStruct(int x, int y)
	{
		X = x;
		Y = Y;
	}
	public int X {get; set;}
	public int Y {get; set;}
	public static PositionStruct operator + (PositionStruct a, PositionStruct b)
	{
		PositionStruct result = new(0,0);
		result.X = a.X + b.X;
		result.Y = a.Y + b.Y;
		return result;
	}
	public static PositionStruct operator - (PositionStruct a, PositionStruct b)
	{
		PositionStruct result = new(0,0);
		result.X = a.X - b.X;
		result.Y = a.Y - b.Y;
		return result;
	}
}