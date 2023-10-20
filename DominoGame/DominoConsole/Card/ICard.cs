namespace DominoConsole;

public interface ICard
{
    int ParentId { get; }
    int Head { get; }
    int Tail { get; }
    ICard DeepCopy();
    bool IsDouble();
    int GetId();
    NodeEnum GetNode(int cardId);
    int[] GetCardIdArrayAtNodes();
    void SetCardIdAtNode(int cardId, NodeEnum nodeEnum);
    void SetParentId(int id);
    int GetHeadTailSum();
}