using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstantiateBehaviour
{
    public static GameObject InstantiateGameObject(GameObject prefab, Transform parent, Vector3 position)
    {
        GameObject newObject = Object.Instantiate(prefab, parent);
        newObject.transform.position = position;
        return newObject;
    }

    public static GameObject InstantiateGameObjectLocal(GameObject prefab, Transform parent, Vector3 position)
    {
        GameObject newObject = Object.Instantiate(prefab, parent);
        newObject.transform.localPosition = position;
        return newObject;
    }
}
