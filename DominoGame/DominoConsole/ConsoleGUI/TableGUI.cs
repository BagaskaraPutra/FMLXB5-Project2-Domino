namespace DominoConsole;

public class TableGUI
{
	private static readonly int _defaultTableRowSize = 8; //(int)Console.WindowHeight;
	private static readonly int _defaultTableColSize = Console.WindowWidth - 1;
	public int LengthX {get; protected set;}
	public int LengthY {get; protected set;}
	public int CenterX {get; protected set;}
	public int CenterY {get; protected set;}
		
	public List<List<char>> Image {get; set;}
	public TableGUI()
	{
		Image = new(_defaultTableRowSize);
		for (int i = 0; i < _defaultTableRowSize; i++)
		{
			Image.Add(new List<char>(_defaultTableColSize));

			for (int j = 0; j < _defaultTableColSize; j++)
			{
				Image[i].Add(' ');
			}
		}
		LengthX = _defaultTableRowSize;
		LengthY = _defaultTableColSize;
		CenterX = (int)LengthX / 2;
		CenterY = (int)LengthY / 2;
	}
	public void Clear()
	{
		LengthX = Image.Count;
		LengthY = Image[0].Count;
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
		int consoleHeight  = _defaultTableRowSize; //Console.WindowHeight;
		int differenceRows = Math.Abs(LengthX - _defaultTableRowSize); //consoleHeight);
		int consoleWidth   = Console.WindowWidth;
		int differenceCols = Math.Abs(LengthY - consoleWidth);
		
		if(LengthY >= consoleWidth)
		{
			foreach (var tableRowList in Image)
			{
				tableRowList.RemoveRange(consoleWidth-1, differenceCols);
			}
		}
		else
		{
			foreach (var tableRowList in Image)
			{
				for(int i=0; i<differenceCols-1; i++)
				{
					tableRowList.Add(' ');
				}
			}
		}
		LengthY = Image[0].Count;
		
		if(LengthX >= consoleHeight)
		{
			Image.RemoveRange(consoleHeight-1, differenceRows);
		}
		else
		{
			for(int i=0; i<differenceRows-1; i++)
			{
				Image.Add(new ());
				for(int j=0; j<LengthY; j++)
				{
					Image[Image.Count-1].Add(' ');
				}
			}
		}
		// TODO: Resize rows to some default value
		LengthX = Image.Count;
	}
}
