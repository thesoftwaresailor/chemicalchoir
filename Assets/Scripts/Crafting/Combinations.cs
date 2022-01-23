﻿
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combinations")]
class Combinations : ScriptableObject
{
    public Dictionary<CombinationPair, GameObject> combinations;

    public List<DictionaryKeyValue> keyValues;

    public GameObject defaultResource;

    public GameObject combineResource(CombinationPair input)
    {

        if (combinations.TryGetValue(input, out GameObject output))
        {
            return output;
        } else
        {
            var temp = input.a;
            input.a = input.b;
            input.b = temp;
            if (combinations.TryGetValue(input, out GameObject output2))
                return output2;
        }
        return defaultResource;
    }

    public void FillDictionary()
    {
        combinations = new Dictionary<CombinationPair, GameObject>();
        foreach(DictionaryKeyValue pair in keyValues)
        {
            combinations.Add(pair.key, pair.value);
        }
    }
}

[System.Serializable]
public struct DictionaryKeyValue
{
    public CombinationPair key;
    public GameObject value;
}

[System.Serializable]
public struct CombinableResource
{
    public int macro;
    public int micro;
    public string name;
}

[System.Serializable]
public struct CombinationPair
{
    public CombinableResource a;
    public CombinableResource b;
}

