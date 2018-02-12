namespace Assets.Data
{
    public interface IDataManager<T>
    {
        T Load();
        void Save(T item);
    }
}
