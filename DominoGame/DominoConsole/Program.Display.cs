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
	// static List<List<char>> verticalCardImage = new()
	// {
	// 	new() {'┌','─','─','─','┐'},
	// 	new() {'│',' ','H',' ','│'},
	// 	new() {'│','─','─','─','│'},
	// 	new() {'│',' ','T',' ','│'},
	// 	new() {'└','─','─','─','┘'}
	// };
	// static List<List<char>> horizontalCardImage = new()
	// {
	// 	new(){'┌','─','─','─','─','─','─','─','┐'},
	// 	new(){'│',' ','H',' ','│',' ','T',' ','│'},
	// 	new(){'└','─','─','─','─','─','─','─','┘'}
	// };
	// static List<List<char>> cardImage = new();
	static void DisplayDeckCards(List<Card> cardsList)
	{
		int i;
		Display("        ");
		for (i = 0; i < cardsList.Count; i++)
		{
			Display("┌───┐\t");
		}
		Display("\n");
		Display("        ");
		foreach (Card card in cardsList)
		{
			Display("│ ");
			Display(card.Head);
			Display(" │\t");
		}
		Display("\n");
		Display("        ");
		for (i = 0; i < cardsList.Count; i++)
		{
			Display("│───│\t");
		}
		Display("\n");
		Display("        ");
		foreach (Card card in cardsList)
		{
			Display("│ ");
			Display(card.Tail);
			Display(" │\t");
		}
		Display("\n");
		Display("        ");
		for (i = 0; i < cardsList.Count; i++)
		{
			Display("└───┘\t");
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
	static void DisplayTableCards(DominoTree dominoTree)
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
		if (!dominoTree.GetTableCardsGUI()[0].IsDouble())
		{
			dominoTree.GetTableCardsGUI()[0].SetOrientation(OrientationEnum.WEST);
			dominoTree.GetTableCardsGUI()[0].Position.SetX(centerX - 2);
			dominoTree.GetTableCardsGUI()[0].Position.SetY(centerY - 2);
		}
		else
		{
			dominoTree.GetTableCardsGUI()[0].Position.SetX(centerX - 1);
			dominoTree.GetTableCardsGUI()[0].Position.SetY(centerY - 4);
		}

		foreach (var cardGUI in dominoTree.GetTableCardsGUI())
		{
			//TODO: How to render domino cards on console terminal >:-(

			// Display($"[{card.Head}|{card.Tail}]");
			dominoTree.CalcForwardKinematics(cardGUI.GetId());
			// char[,] cardImage = GetCardImage(card);
			IsExceedBorder(cardGUI);
			Place2D(cardGUI.GetCardImage(), ref windowImage, cardGUI.Position.X, cardGUI.Position.Y);
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
	static void Place2D(List<List<char>> small, ref List<List<char>> big, int offsetX, int offsetY)
	{
		// TODO: For a more consistent kinematics, define the card center's relative position
		// if vertical -> [2][2]
		// else horizontal -> [1][4]
		int smallRows = small.Count;
		int smallCols = small[0].Count;
		int bigRows = big.Count;
		int bigCols = big[0].Count;
		if(bigRows <= smallRows)
		{
			do
			{
				big.Add(new());
			}while(big.Count <= smallRows);
		}
		if(bigCols <= smallCols)
		{
			foreach(var bigRow in big)
			{
				do
				{
					bigRow.Add(' ');
				}while(bigRow.Count <= smallCols);
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
	static bool IsExceedBorder(CardGUI cardGUI)
	{
		int positionX = cardGUI.Position.X;
		int positionY = cardGUI.Position.Y;
		OrientationEnum orientation = cardGUI.Orientation;
		if(cardGUI.Position.Y < 0)
		{		
			cardGUI.SetOrientation(Transform2D.RotateCW(orientation));
			if(cardGUI.IsDouble())
			{
				cardGUI.Position.SetX(positionX - 3);	
			}
			else
			{
				cardGUI.Position.SetX(positionX - 3);
				cardGUI.Position.SetY(positionY + 3);	
			}
			Console.WriteLine("Is exceeds border LEFT");
			return true;
		}
		else if(cardGUI.IsDouble() && cardGUI.Position.Y + 20 > Console.WindowWidth)
		{
			cardGUI.SetOrientation(Transform2D.RotateCW(cardGUI.Orientation));
			cardGUI.Position.SetX(positionX + 5);
			cardGUI.Position.SetY(positionY - 20);
			Console.WriteLine("Is exceeds border RIGHT Double");
			return true;
		}
		else if(!cardGUI.IsDouble() && cardGUI.Position.Y + 6 > Console.WindowWidth)
		{
			cardGUI.SetOrientation(Transform2D.RotateCW(cardGUI.Orientation));
			cardGUI.Position.SetX(positionX + 5);
			cardGUI.Position.SetY(positionY - 6);
			Console.WriteLine("Is exceeds border RIGHT");
			return true;
		}
		else
		{
			return false;
		}
	}
}
