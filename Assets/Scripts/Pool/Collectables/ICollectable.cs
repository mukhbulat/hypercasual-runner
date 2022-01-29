namespace Pool.Collectables
{
    public interface ICollectable : IPoolable
    {
        public void Obtain();

        public ICollectable Initialize(int index);
        public int GetNumberOfTypesOfCollectables();
    }
}