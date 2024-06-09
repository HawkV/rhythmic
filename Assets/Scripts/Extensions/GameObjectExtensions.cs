using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{

    public static List<GameObject> GetDescendantsWithPrefix(this GameObject gameObject, string prefix) {
        var children = new List<GameObject>();

        foreach(Transform child in gameObject.transform) {
            if (child.name.StartsWith(prefix)) {
                children.Add(child.gameObject);       
            }
                
            children.AddRange(child.gameObject.GetDescendantsWithPrefix(prefix)); 
        }

        return children;
    }
}
