namespace DominoConsole;

public class CardKinematics
{
	public bool ParentIsDouble {get; set; }
	public bool CurrentIsDouble {get; set; }
	public NodeEnum ParentNode {get; set; }
	public NodeEnum CurrentNode {get; set; }
	public OrientationEnum ParentOrientation {get; set; }
	public OrientationEnum CurrentOrientation {get; set; }
	public int CurrentOffsetX {get; set; }
	public int CurrentOffsetY {get; set; }
}
