using System;

public class Resource
{

    public string name;

    public float amount;

    public Resource(string resourceName, float startingAmount = 0)
    {
        name = resourceName;
        amount = startingAmount;
    }

}