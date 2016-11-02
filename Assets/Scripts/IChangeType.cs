namespace Assets.Scripts
{
    public interface IChangeType
    {
        TypeModel.TypeModelItem TypeItem { get; }
        bool TryChangeType(TypeModel.TypeModelItem typeItem, float autority);
    }
}
