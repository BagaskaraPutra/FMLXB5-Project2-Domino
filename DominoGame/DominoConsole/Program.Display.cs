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
		if (!dominoTree.GetTableCardsGUI()[0].IsDouble())
		{
			dominoTree.GetTableCardsGUI()[0].Position.X = tableGUI.CenterX;
			dominoTree.GetTableCardsGUI()[0].Position.Y = tableGUI.CenterY;
		}
		else
		{
			dominoTree.GetTableCardsGUI()[0].Position.X = tableGUI.CenterX;
			dominoTree.GetTableCardsGUI()[0].Position.Y = tableGUI.CenterY;
		}

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
		int northBorder = cardGUI.Position.X - cardGUI.CenterLocal.X - 1;
		int southBorder = cardGUI.Position.X + cardGUI.CenterLocal.X + 1;
		if (northBorder < 0)
		{
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
			Console.WriteLine($"Is exceeds NORTH border by {Math.Abs(northBorder)} cells");
		}
		else if (southBorder > tableGUI.LengthX)
		{
			for (int i = 0; i <= Math.Abs(southBorder - tableGUI.LengthX)+1; i++)
			{
				List<char> columns = new();
				for (int j=0; j<tableGUI.Image[0].Count; j++)
				{
					columns.Add(' ');
				}
				tableGUI.Image.Add(columns);
			}
			Console.WriteLine($"Is exceeds SOUTH border by {Math.Abs(southBorder - tableGUI.LengthX)} cells");
		}
		int setPosX;
		int setPosY;
		for (int i = 0; i < cardGUI.LengthX; i++)
		{
			for (int j = 0; j < cardGUI.LengthY; j++)
			{
				setPosX = i + cardGUI.Position.X - cardGUI.CenterLocal.X;
				// if (setPosX < 0) setPosX = 0;
				// else if (setPosX >= tableGUI.LengthX-1) setPosX = tableGUI.LengthX-1;
				setPosY = j + cardGUI.Position.Y - cardGUI.CenterLocal.Y;
				// if (setPosY < 0) setPosY = 0;
				// else if (setPosY >= tableGUI.LengthX-1) setPosY = tableGUI.LengthY-1;
				tableGUI.Image
				[setPosX]
				[setPosY]
				= cardImage[i][j];
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
