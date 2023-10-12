using DominoConsole;
namespace Program;

public partial class Program
{
	static string? ReadInput()
	{
		return Console.ReadLine();
	}
	static void Display<T>(T content)
	{
		Console.Write(content);
	}
	static void DisplayLine<T>(T content)
	{
		Console.WriteLine(content);
	}
	static List<List<char>> verticalCardImage = new()
	{
		new() {' ', '-', '-', '-',' '},
		new() {'|', ' ', 'H', ' ','|'},
		new() {'|', '-', '-', '-','|'},
		new() {'|', ' ', 'T', ' ','|'},
		new() {' ', '-', '-', '-',' '}
	};
	static List<List<char>> horizontalCardImage = new()
	{
		new(){' ','-','-','-','-','-','-','-',' '},
		new(){'|',' ','H',' ','|',' ','T',' ','|'},
		new(){' ','-','-','-','-','-','-','-',' '}
	};
	static List<List<char>> cardImage = new();
	static List<List<char>> GetCardImage(Card card)
	{
		if (card.Orientation == OrientationEnum.NORTH || card.Orientation == OrientationEnum.SOUTH)
		{
			cardImage = verticalCardImage;
		}
		else
		{
			cardImage = horizontalCardImage;
		}
		switch (card.Orientation)
		{
			case OrientationEnum.NORTH:
				{
					cardImage[1][2] = card.Head.ToString().ToCharArray()[0];
					cardImage[3][2] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.SOUTH:
				{
					cardImage[3][2] = card.Head.ToString().ToCharArray()[0];
					cardImage[1][2] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.EAST:
				{
					cardImage[1][6] = card.Head.ToString().ToCharArray()[0];
					cardImage[1][2] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.WEST:
				{
					cardImage[1][2] = card.Head.ToString().ToCharArray()[0];
					cardImage[1][6] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			default: break;
		}
		return cardImage;
	}
	static void DisplayDeckCards(List<Card> cardsList)
	{
		int i;
		Display("        ");
		for (i = 0; i < cardsList.Count; i++)
		{
			Display(" --- \t");
		}
		Display("\n");
		Display("        ");
		foreach (Card card in cardsList)
		{
			Display("| ");
			Display(card.Head);
			Display(" |\t");
		}
		Display("\n");
		Display("        ");
		for (i = 0; i < cardsList.Count; i++)
		{
			Display("|---|\t");
		}
		Display("\n");
		Display("        ");
		foreach (Card card in cardsList)
		{
			Display("| ");
			Display(card.Tail);
			Display(" |\t");
		}
		Display("\n");
		Display("        ");
		for (i = 0; i < cardsList.Count; i++)
		{
			Display(" --- \t");
		}
		Display("\n");
		Display("card id:");
		foreach (Card card in cardsList)
		{
			Display("  ");
			Display(card.GetId());
			Display("\t");
		}
		Display("\n");
	}
	static readonly int _defaultWindowRows = 8; //(int)Console.WindowHeight;
	static readonly int _defaultWindowCols = Console.WindowWidth - 2;
	static List<List<char>> windowImage = new(_defaultWindowRows);
	static void InitializeWindowDisplay()
	{
		for (int i = 0; i < 12; i++)
		{
			windowImage.Add(new List<char>(Console.WindowWidth - 2));

			for (int j = 0; j < _defaultWindowCols; j++)
			{
				windowImage[i].Add(' ');
			}
		}
	}
	static void ResizeWindowToFitTerminal()
	{
		int windowCols = windowImage[0].Count;
		int consoleWidth = Console.WindowWidth;
		int differenceCols = Math.Abs(windowCols - consoleWidth);
		// Console.WriteLine($"windowCols: {windowCols}, consoleWidth: {consoleWidth}");
		if(windowCols >= consoleWidth)
		{
			foreach (var windowRowList in windowImage)
			{
				windowRowList.RemoveRange(consoleWidth-1, differenceCols);
			}
		}
		else
		{
			Console.WriteLine("windowImage columns < consoleWidth");
			foreach (var windowRowList in windowImage)
			{
				for(int i=0; i<differenceCols-1; i++)
				{
					windowRowList.Add(' ');
				}
			}
		}
	}
	static void DisplayTableCards(List<Card> tableCards, DominoTree dominoTree)
	{
		// char[,] windowImage = new char[windowRows, windowCols];
		ResizeWindowToFitTerminal();
		for (int i = 0; i < windowImage.Count; i++)
		{
			for (int j = 0; j < windowImage[i].Count; j++)
			{
				windowImage[i][j] = ' ';
			}
		}
		
		int centerX = 0, centerY = 0;
		centerX = (int)windowImage.Count / 2;
		centerY = (int)windowImage[0].Count / 2;
		if (!tableCards[0].IsDouble())
		{
			tableCards[0].SetOrientation(OrientationEnum.WEST);
			tableCards[0].Position.SetX(centerX - 2);
			tableCards[0].Position.SetY(centerY - 2);
		}
		else
		{
			tableCards[0].Position.SetX(centerX - 1);
			tableCards[0].Position.SetY(centerY - 4);
		}

		foreach (var card in tableCards)
		{
			//TODO: How to render domino cards on console terminal >:-(

			// Display($"[{card.Head}|{card.Tail}]");
			dominoTree.CalcForwardKinematics(card.GetId());
			// char[,] cardImage = GetCardImage(card);
			cardImage = GetCardImage(card);
			Place2D(in cardImage, ref windowImage, card.Position.X, card.Position.Y);
		}
		Display2D(windowImage);
		Display("\n");
	}
	static void Display2D<T>(List<List<T>> listlist)
	{
		for (int i = 0; i < listlist.Count; i++)
		{
			for (int j = 0; j < listlist[i].Count; j++)
			{
				Display(listlist[i][j]);
			}
			Display("\n");
		}
	}
	static void Display2D<T>(T[,] array)
	{
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= array.GetUpperBound(1); j++)
			{
				Display(array[i, j]);
			}
			Display("\n");
		}
	}
	static void Place2D(in List<List<char>> small, ref List<List<char>> big, int offsetX, int offsetY)
	{
		int smallRows = small.Count;
		int smallCols = small[0].Count;
		int bigRows = big.Count;
		int bigCols = big[0].Count;
		if(bigRows < smallRows)
		{
			do
			{
				big.Add(new());
			}while(big.Count < smallRows);
		}
		if(bigCols < smallCols)
		{
			foreach(var bigRow in big)
			{
				do
				{
					bigRow.Add(' ');
				}while(bigRow.Count < smallCols);
			}
		}
		for (int i = 0; i < smallRows; i++)
		{
			for (int j = 0; j < smallCols; j++)
			{
				big[i + offsetX][j + offsetY] = small[i][j];
			}
		}

	}
	static void Place2D<T>(in T[,] small, ref T[,] big, int offsetX, int offsetY)
	{
		for (int i = 0; i < small.GetLength(0); i++)
		{
			for (int j = 0; j < small.GetLength(1); j++)
			{
				big[i + offsetX, j + offsetY] = small[i, j];
			}
		}
	}
}
