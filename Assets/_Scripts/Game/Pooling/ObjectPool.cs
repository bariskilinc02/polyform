using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject PoolParent;

    public List<GameObject> BoxPool;
    public GameObject BoxPrefab;

    public int PoolSize;

    private void Awake()
    {
        CreatePoolObjects();
    }
    public GameObject RemoveObjectFromPool()
    {
        if (BoxPool.Count == 0)
            CreatePoolObjects();

        GameObject extractedObject = BoxPool[0];
        extractedObject.SetActive(true);
        BoxPool.RemoveAt(0);

        return extractedObject;
    }

    public void AddObjectToPool(GameObject addedObject)
    {
        addedObject.transform.parent = PoolParent.transform;
        addedObject.SetActive(false);
        BoxPool.Add(addedObject);
    }

    private void CreatePoolObjects()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject Box = Instantiate(BoxPrefab, PoolParent.transform);
            Box.SetActive(false);
            BoxPool.Add(Box);
        }
    }
}
