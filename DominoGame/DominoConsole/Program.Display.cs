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
			// dominoTree.GetTableCardsGUI()[0].SetOrientation(OrientationEnum.WEST);
			dominoTree.GetTableCardsGUI()[0].Position.SetX(tableGUI.CenterX - 2);
			dominoTree.GetTableCardsGUI()[0].Position.SetY(tableGUI.CenterY - 2);
		}
		else
		{
			dominoTree.GetTableCardsGUI()[0].Position.SetX(tableGUI.CenterX - 1);
			dominoTree.GetTableCardsGUI()[0].Position.SetY(tableGUI.CenterY - 4);
		}

		foreach (var cardGUI in dominoTree.GetTableCardsGUI())
		{
			//TODO: How to render domino cards on console terminal >:-(

			// Display($"[{card.Head}|{card.Tail}]");
			dominoTree.CalcForwardKinematics(cardGUI.GetId());
			IsExceedsBorder(cardGUI);
			// PlaceSmallIntoBig2D(cardGUI.GetImage(), ref windowImage, cardGUI.Position.X, cardGUI.Position.Y);
			PlaceCardCenterIntoWindow(cardGUI, ref tableGUI, cardGUI.Position.X, cardGUI.Position.Y);
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
	static void PlaceCardCenterIntoWindow(CardGUI cardGUI, ref TableGUI tableGUI, int smallCenterX, int smallCenterY)
	{
		// TODO: For a more consistent kinematics, define the card center's relative position
		// if vertical -> [2][2]
		// else horizontal -> [1][4]
		List<List<char>> cardImage = cardGUI.GetImage();
		if(tableGUI.LengthX <= tableGUI.LengthY)
		{
			do
			{
				tableGUI.Image.Add(new());
			}while(tableGUI.LengthX <= cardGUI.LengthX);
		}
		if(tableGUI.LengthY <= cardGUI.LengthY)
		{
			foreach(var imageRows in tableGUI.Image)
			{
				do
				{
					imageRows.Add(' ');
				}while(tableGUI.LengthY <= cardGUI.LengthY);
			}
		}
		for (int i = 0; i < cardGUI.LengthX; i++)
		{
			for (int j = 0; j < cardGUI.LengthY; j++)
			{
				tableGUI.Image[i + smallCenterX][j + smallCenterY] = cardImage[i][j];
			}
		}
	}
	static void PlaceSmallIntoBig2D(List<List<char>> small, ref List<List<char>> big, int offsetX, int offsetY)
	{
		int smallRowSize = small.Count;
		int smallColSize = small[0].Count;
		int bigRowSize = big.Count;
		int bigColSize = big[0].Count;
		if(bigRowSize <= smallRowSize)
		{
			do
			{
				big.Add(new());
			}while(big.Count <= smallRowSize);
		}
		if(bigColSize <= smallColSize)
		{
			foreach(var bigRows in big)
			{
				do
				{
					bigRows.Add(' ');
				}while(bigRows.Count <= smallColSize);
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
	static bool IsExceedsBorder(CardGUI cardGUI)
	{
		// TODO: Still fails when exceeds border
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
