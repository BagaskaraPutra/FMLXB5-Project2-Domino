namespace DominoConsole;

public interface ICard
{
    int ParentId { get; protected set; }
    int Head { get; protected set; }
    int Tail { get; protected set; }
    Card DeepCopy();
    bool IsDouble();
    int GetId();
    NodeEnum GetNode(int cardId);
    int[] GetCardIdArrayAtNodes();
    void SetCardIdAtNode(int cardId, NodeEnum nodeEnum);
    void SetParentId(int id);
    int GetHeadTailSum();
}