

namespace DKit.ThirdPartyPlugins.QECS.Runtime.Core
{
    public class Frame
    {
        public T CreateEntity<T>()where T:Entity,new()
        {
            return new T();
        }
        public void DestoryEntity(Entity e)
        {
        }

        public T[] GetAllEntitys<T>() where T : Entity
        {
            return new T[] { };
        }
    }
}