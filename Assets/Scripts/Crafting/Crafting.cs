﻿using UnityEngine;
using System.Collections.Generic;
using TMPro;

class Crafting : MonoBehaviour
{

    public Combinations combinations;

    public static Crafting instance;

    public List<CombinationPair> craftedCombos;

    public TextMeshProUGUI recordText;

    public void Start()
    {
        instance = this;
        combinations.FillDictionary();
    }

    public void Craft(Resource resourceOne, Resource resourceTwo)
    {
        CombinableResource one;
        one.micro = resourceOne.micro;
        one.macro = resourceOne.macro;
        one.name = resourceOne.resourceName;
        CombinableResource two;
        two.micro = resourceTwo.micro;
        two.macro = resourceTwo.macro;
        two.name = resourceTwo.resourceName;
        CombinationPair pair;
        pair.a = one;
        pair.b = two;
        GameObject result = combinations.combineResource(pair);
        var spawnPosition = resourceOne.gameObject.transform.position;
        AddToCraftedList(pair);
        Instantiate(result, spawnPosition, result.transform.rotation);
        Destroy(resourceOne.gameObject);
        Destroy(resourceTwo.gameObject);
    }

    public bool CompareCombos(CombinationPair a, CombinationPair b)
    {
        if (a.a.Equals(b.a) && b.b.Equals(a.b))
            return true;
        if (a.a.Equals(b.b) && b.a.Equals(a.b))
            return true;
        return false;
    }   

    public void AddToCraftedList(CombinationPair pair)
    {
        bool hasBeenCrafted = false;
        foreach(CombinationPair crafted in craftedCombos)
        {
            if (CompareCombos(pair, crafted))
                hasBeenCrafted = true;
        }
        if (!hasBeenCrafted)
            craftedCombos.Add(pair);
    }

    private List<CombinationPair> SearchForResource(CombinableResource resource)
    {
        List<CombinationPair> output = new List<CombinationPair>();
        foreach(CombinationPair pair in craftedCombos)
        {
            if (pair.a.Equals(resource) || pair.b.Equals(resource))
                output.Add(pair);
        }
        return output;

    }

    public void DisplayResource(Resource selected)
    {
        CombinableResource resource;
        resource.macro = selected.macro;
        resource.micro = selected.micro;
        resource.name = selected.name;
        List<CombinationPair> combos = SearchForResource(resource);
        foreach(CombinationPair pair in combos)
        {
            recordText.text += GetLineFromPair(ConvertPair(pair));
        }
        recordText.enabled = true;
    }

    public void HideResource()
    {
        recordText.enabled = false;
    }

    private string GetLineFromPair(DisplayableCombinationPair pair)
    {
        string output = pair.a.macro + " " + pair.a.micro + " " + pair.a.name + " and ";
        output += pair.b.macro + " " + pair.b.micro + " " + pair.b.name + " creates ";
        output += combinations.combineResource(pair.data).name + "\n";
        return output;
    }

    public struct DisplayableCombinationPair
    {
        public DisplayableCombinableResource a;
        public DisplayableCombinableResource b;
        public CombinationPair data;
    }

    public struct DisplayableCombinableResource
    {
        public string macro;
        public string micro;
        public string name;
    }

    public DisplayableCombinationPair ConvertPair(CombinationPair pair)
    {
        DisplayableCombinableResource a;
        a.macro = pair.a.macro == 0 ? "Waned" : "Waxed";
        a.micro = pair.a.micro == 0 ? "Stable" : "Flux";
        a.name = pair.a.name;
        DisplayableCombinableResource b;
        b.macro = pair.b.macro == 0 ? "Waned" : "Waxed";
        b.micro = pair.b.micro == 0 ? "Stable" : "Flux";
        b.name = pair.b.name;
        DisplayableCombinationPair output;
        output.a = a;
        output.b = b;
        output.data = pair;
        return output;
    }

}
