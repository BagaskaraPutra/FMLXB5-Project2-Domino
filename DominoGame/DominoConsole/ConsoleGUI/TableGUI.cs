namespace DominoConsole;

public class TableGUI
{
	private static readonly int _defaultWindowRowSize = 8; //(int)Console.WindowHeight;
	private static readonly int _defaultWindowColSize = Console.WindowWidth - 2;
	public int LengthX {get; protected set;}
	public int LengthY {get; protected set;}
	public int CenterX {get; protected set;}
	public int CenterY {get; protected set;}
		
	public List<List<char>> Image {get; set;}
	public TableGUI()
	{
		Image = new(_defaultWindowRowSize);
		for (int i = 0; i < _defaultWindowRowSize; i++)
		{
			Image.Add(new List<char>(_defaultWindowColSize));

			for (int j = 0; j < _defaultWindowColSize; j++)
			{
				Image[i].Add(' ');
			}
		}
		LengthX = _defaultWindowRowSize;
		LengthY = _defaultWindowColSize;
		CenterX = (int)LengthX / 2;
		CenterY = (int)LengthY / 2;
	}
	public void Clear()
	{
		for (int i = 0; i < LengthX; i++)
		{
			for (int j = 0; j < LengthY; j++)
			{
				Image[i][j] = ' ';
			}
		}
		CenterX = (int)LengthX / 2;
		CenterY = (int)LengthY / 2;
	}
	public void ResizeToFitTerminal()
	{
		LengthX = Image.Count;
		LengthY = Image[0].Count;
		int consoleWidth = Console.WindowWidth;
		int differenceCols = Math.Abs(LengthY - consoleWidth);
		// Console.WriteLine($"windowCols: {windowCols}, consoleWidth: {consoleWidth}");
		if(LengthY >= consoleWidth)
		{
			foreach (var windowRowList in Image)
			{
				windowRowList.RemoveRange(consoleWidth-1, differenceCols);
			}
		}
		else
		{
			Console.WriteLine("windowImage columns < consoleWidth");
			foreach (var windowRowList in Image)
			{
				for(int i=0; i<differenceCols-1; i++)
				{
					windowRowList.Add(' ');
				}
			}
		}
		LengthX = Image.Count;
		LengthY = Image[0].Count;
	}
}
