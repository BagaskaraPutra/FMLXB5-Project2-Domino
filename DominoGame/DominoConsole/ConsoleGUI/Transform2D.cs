namespace DominoConsole;

public class Transform2D
{
	public static void MoveUntilEdge(ref CardGUI currentCard, CardGUI parentCard, OrientationEnum moveUntilEdge)
	{
		switch (moveUntilEdge)
		{
			case OrientationEnum.NORTH:
			{
				currentCard.Position.SetX(parentCard.Position.X - parentCard.CenterPosX - currentCard.CenterPosX - 1);
				currentCard.Position.SetY(parentCard.Position.Y);
				// Console.WriteLine($"Move NORTH to X: {currentCard.Position.X} {parentCard.Position.X - parentCard.CenterPosX - currentCard.CenterPosX - 1}");
				break;
			}
			case OrientationEnum.EAST:
			{
				currentCard.Position.SetX(parentCard.Position.X);
				currentCard.Position.SetY(parentCard.Position.Y + parentCard.CenterPosY + currentCard.CenterPosY + 1);
				// Console.WriteLine($"Move EAST to Y: {currentCard.Position.Y}");
				break;
			}
			case OrientationEnum.SOUTH:
			{
				currentCard.Position.SetX(parentCard.Position.X + parentCard.CenterPosX + currentCard.CenterPosX + 1);
				currentCard.Position.SetY(parentCard.Position.Y);
				// Console.WriteLine($"Move SOUTH to X: {currentCard.Position.X}");
				break;
			}
			case OrientationEnum.WEST:
			{
				currentCard.Position.SetX(parentCard.Position.X);
				currentCard.Position.SetY(parentCard.Position.Y - parentCard.CenterPosY - currentCard.CenterPosY - 1);
				// Console.WriteLine($"Move WEST to Y: {currentCard.Position.Y}");
				break;
			}
			default: break;
		}
	}
	public static OrientationEnum RotateCW(OrientationEnum orientation)
	{
		// Console.WriteLine("Rotate CW");
		return orientation.Next();
	}
	public static OrientationEnum RotateCCW(OrientationEnum orientation)
	{
		// Console.WriteLine("Rotate CCW");
		return orientation.Previous();
	}
	public static OrientationEnum Rotate180(OrientationEnum orientation)
	{
		// Console.WriteLine("Rotate 180");
		return orientation.Next().Next();
	}
}

public struct PositionStruct{
	public PositionStruct(int x, int y)
	{
		X = x;
		Y = Y;
	}
	public int X {get; private set;}
	public int Y {get; private set;}
	public bool SetX(int x)
	{
		// if (x < 0)
		// {
		// 	return false;
		// }
		X = x;
		return true;
	}
	public bool SetY(int y)
	{
		// if (y < 0)
		// {
		// 	return false;
		// }
		Y = y;
		return true;
	}
}
public enum OrientationEnum
{
	NORTH,
	EAST,
	SOUTH,
	WEST
}

//Source: https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
public static class Extensions
{
	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) + 1;
		return (Arr.Length==j) ? Arr[0] : Arr[j];            
	}
	public static T Previous<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) - 1;
		return (j == -1) ? Arr[Arr.Length-1] : Arr[0];
	}
}