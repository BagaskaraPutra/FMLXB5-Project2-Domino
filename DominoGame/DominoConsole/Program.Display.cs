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
	static void DisplayLineCenter<T>(T content)
	{
		string s = content.ToString();
		Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
		Console.WriteLine(s);
	}
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
	static void DisplayTableCards(DominoTree dominoTree)
	{
		tableGUI.ResizeToFitTerminal();
		tableGUI.Clear();
		
		dominoTree.GetTableCardsGUI()[0].Position.X = tableGUI.CenterX;
		dominoTree.GetTableCardsGUI()[0].Position.Y = tableGUI.CenterY;

		foreach (var cardGUI in dominoTree.GetTableCardsGUI())
		{
			// Display($"[{card.Head}|{card.Tail}]");
			dominoTree.CalcForwardKinematics(cardGUI.GetId());
			PlaceCardCenterIntoTable(cardGUI, ref tableGUI, dominoTree);
		}
		Display2D(tableGUI.Image);
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
	static void PlaceCardCenterIntoTable(CardGUI cardGUI, ref TableGUI tableGUI, DominoTree dominoTree)
	{
		List<List<char>> cardImage = cardGUI.GetImage();
		int northBorder = cardGUI.Position.X - cardGUI.NorthEdgeToCenterLength - 1;
		int southBorder = cardGUI.Position.X + cardGUI.SouthEdgeToCenterLength + 1;
		if (northBorder < 0)
		{
			// Console.WriteLine($"Is exceeds NORTH border by {Math.Abs(northBorder)} cells.");
			// Console.WriteLine($"tableGUI rows: {tableGUI.Image.Count} cols: {tableGUI.Image[0].Count}. tableGUI LengthX: {tableGUI.LengthX} LengthY: {tableGUI.LengthY}");
			for (int i = 0; i < Math.Abs(northBorder); i++)
			{
				List<char> columns = new();
				for (int j=0; j<tableGUI.Image[0].Count; j++)
				{
					columns.Add(' ');
				}
				tableGUI.Image.Insert(0, columns);
			}
			dominoTree.MoveAllSouth(Math.Abs(northBorder));
		}
		else if (southBorder > tableGUI.LengthX)
		{
			// Console.WriteLine($"Is exceeds SOUTH border by {Math.Abs(southBorder - tableGUI.LengthX)} cells.");	
			// Console.WriteLine($"tableGUI rows: {tableGUI.Image.Count} cols: {tableGUI.Image[0].Count}. tableGUI LengthX: {tableGUI.LengthX} LengthY: {tableGUI.LengthY}");
			for (int i = 0; i <= Math.Abs(southBorder - tableGUI.LengthX)+1; i++)
			{
				List<char> columns = new();
				for (int j=0; j<tableGUI.Image[0].Count; j++)
				{
					columns.Add(' ');
				}
				tableGUI.Image.Add(columns);
			}
		}
		tableGUI.UpdateStates();
		
		int setPosX, setPosY;
		for (int i = 0; i < cardGUI.LengthX; i++)
		{
			for (int j = 0; j < cardGUI.LengthY; j++)
			{
				setPosX = i + cardGUI.Position.X - cardGUI.CenterLocal.X;
				setPosY = j + cardGUI.Position.Y - cardGUI.CenterLocal.Y;
				tableGUI.Image[setPosX][setPosY] = cardImage[i][j];
			}
		}
	}
	static void PlaceSmallIntoBig2D(List<List<char>> small, ref List<List<char>> big, int offsetX, int offsetY)
	{
		int smallRowSize = small.Count;
		int smallColSize = small[0].Count;
		int bigRowSize = big.Count;
		int bigColSize = big[0].Count;
		if (bigRowSize <= smallRowSize)
		{
			do
			{
				big.Add(new());
			} while (big.Count <= smallRowSize);
		}
		if (bigColSize <= smallColSize)
		{
			foreach (var bigRows in big)
			{
				do
				{
					bigRows.Add(' ');
				} while (bigRows.Count <= smallColSize);
			}
		}
		for (int i = 0; i < smallRowSize; i++)
		{
			for (int j = 0; j < smallColSize; j++)
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
