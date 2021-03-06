using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject Prefab;
    private int Size;
    private List<PoolableObject> AvailableObjectsPool;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        AvailableObjectsPool = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size, Transform SpawnPosition)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);
        
        GameObject poolGameObject = new GameObject(Prefab + " Pool");
        pool.CreateObjects(poolGameObject, SpawnPosition);

        return pool;
    }

    private void CreateObjects(GameObject parent, Transform SpawnPosition)
    {
        for (int i = 0; i < Size; i++)
        {
            
            PoolableObject poolableObject = GameObject.Instantiate(Prefab, SpawnPosition.localPosition, SpawnPosition.rotation, SpawnPosition);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        }
    }

    public PoolableObject GetObject()
    {
        PoolableObject instance = AvailableObjectsPool[0];

        AvailableObjectsPool.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnObjectToPool(PoolableObject Object)
    {
        AvailableObjectsPool.Add(Object);
    }
}