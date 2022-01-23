using UnityEngine;

class Crafting : MonoBehaviour
{

    public Combinations combinations;

    public static Crafting instance;

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
        one.name = resourceOne.name;
        CombinableResource two;
        two.micro = resourceTwo.micro;
        two.macro = resourceTwo.macro;
        two.name = resourceTwo.name;
        CombinationPair pair;
        pair.a = one;
        pair.b = two;
        GameObject result = combinations.combineResource(pair);
        var spawnTransform = resourceOne.gameObject.transform;
        Destroy(resourceOne.gameObject);
        Destroy(resourceTwo.gameObject);
        Instantiate(result, spawnTransform);
    }

}
