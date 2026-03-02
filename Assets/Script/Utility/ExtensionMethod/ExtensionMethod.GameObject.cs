using UnityEngine;

public static partial class ExtensionMethod
{
    public static string GetFullName (this GameObject go)
    {
        string name = go.name;
        while (go.transform.parent != null)
        {

            go = go.transform.parent.gameObject;
            name = go.name + " / " + name;
        }
        return name;
    }
}