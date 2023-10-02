namespace DominoConsole;

public class Player : IPlayer
{
	private int _id;
	private string _name;
	public string GetName()
	{
		return _name;
	}
	public void SetName(string name)
	{
		_name = name;
	}
	public int GetId()
	{
		return _id;
	}
	public void SetId(int id)
	{
		_id = id;
	}
}
