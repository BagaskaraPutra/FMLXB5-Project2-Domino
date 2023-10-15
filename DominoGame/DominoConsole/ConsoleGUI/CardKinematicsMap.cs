
using CsvHelper;
using CsvHelper.Configuration;
namespace DominoConsole;

public class CardKinematicsMap : ClassMap<CardKinematics>
{
	public CardKinematicsMap()
	{
		Map(m => m.ParentIsDouble).Name("Parent IsDouble");
		Map(m => m.CurrentIsDouble).Name("Current IsDouble");
		Map(m => m.ParentNode).Name("Parent node");
		Map(m => m.CurrentNode).Name("Current node");
		Map(m => m.CurrentIsDouble).Name("Current IsDouble");
		Map(m => m.ParentOrientation).Name("Parent orientation");
		Map(m => m.CurrentOrientation).Name("Current orientation");
		Map(m => m.CurrentOffsetX).Name("Current offset x");
		Map(m => m.CurrentOffsetY).Name("Current offset y");
	}
}
