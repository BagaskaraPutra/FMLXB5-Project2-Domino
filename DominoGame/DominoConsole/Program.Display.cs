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
	static char[,] GetCardImage(Card card)
	{
		char[,] cardImage;
		if (card.Orientation == OrientationEnum.NORTH || card.Orientation == OrientationEnum.SOUTH)
		{
			cardImage = new char[5, 5]
			{{' ', '-', '-', '-',' '},
			 {'|', ' ', 'H', ' ','|'},
			 {'|', '-', '-', '-','|'},
			 {'|', ' ', 'T', ' ','|'},
			 {' ', '-', '-', '-',' '}};
		}
		else
		{
			cardImage = new char[3, 9]
			{{' ','-','-','-','-','-','-','-',' '},
			 {'|',' ','H',' ','|',' ','T',' ','|'},
			 {' ','-','-','-','-','-','-','-',' '}};
		}
		switch (card.Orientation)
		{
			case OrientationEnum.NORTH:
				{
					cardImage[1, 2] = card.Head.ToString().ToCharArray()[0];
					cardImage[3, 2] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.SOUTH:
				{
					cardImage[3, 2] = card.Head.ToString().ToCharArray()[0];
					cardImage[1, 2] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.EAST:
				{
					cardImage[1, 6] = card.Head.ToString().ToCharArray()[0];
					cardImage[1, 2] = card.Tail.ToString().ToCharArray()[0];
					break;
				}
			case OrientationEnum.WEST:
				{
					cardImage[1, 2] = card.Head.ToString().ToCharArray()[0];
					cardImage[1, 6] = card.Tail.ToString().ToCharArray()[0];
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
	static void DisplayTableCards(List<Card> tableCards, DominoTree dominoTree)
	{
		int windowRows = (int)Console.WindowHeight;
		int windowCols = Console.WindowWidth - 2;
		char[,] windowImage = new char[windowRows, windowCols];
		for (int i = 0; i < windowRows; i++)
		{
			for (int j = 0; j < windowCols; j++)
			{
				windowImage[i, j] = ' ';
			}
		}

		int centerX = 0, centerY = 0;
		centerX = (int)windowRows / 2;
		centerY = (int)windowCols / 2;
		
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
			char[,] cardImage = GetCardImage(card);
			Place2DArray(in cardImage, ref windowImage, card.Position.X, card.Position.Y);
		}
		Display2DArray(windowImage);
		Display("\n");
	}
	static void Display2DArray<T>(T[,] array)
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
	static void Place2DArray<T>(in T[,] small, ref T[,] big, int offsetX, int offsetY)
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
