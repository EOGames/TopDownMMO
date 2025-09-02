using UnityEngine;
using System.Collections.Generic;

public static class JsonHelper
{
    // Convert {"id1":{...},"id2":{...}} to [{...},{...}]
    public static string DictionaryToArrayJson(string dictJson)
    {
        // Remove outer braces and wrap inner values as array
        var trimmed = dictJson.Trim();
        if (trimmed.StartsWith("{") && trimmed.EndsWith("}"))
        {
            trimmed = trimmed.Substring(1, trimmed.Length - 2); // remove {}
        }

        // Split by "}," to separate objects
        trimmed = "[" + trimmed.Replace("},", "}|") + "]";
        string[] parts = trimmed.Substring(1, trimmed.Length - 2).Split('|');

        for (int i = 0; i < parts.Length; i++)
        {
            if (!parts[i].EndsWith("}")) parts[i] += "}";
        }

        return "[" + string.Join(",", parts) + "]";
    }

    public static T[] FromJson<T>(string json)
    {
        return JsonUtility.FromJson<Wrapper<T>>( "{\"players\":" + json + "}" ).players;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] players;
    }
}