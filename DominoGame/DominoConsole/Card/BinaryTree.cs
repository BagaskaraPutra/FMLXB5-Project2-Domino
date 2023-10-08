namespace DominoConsole;

public class BinaryTree<T> where T: class
{
	public T? Root { get; private set; }
	public BinaryTree()
	{
		Root = null;
	}
	public BinaryTree(T root)
	{
		Root = root;
	}
}
