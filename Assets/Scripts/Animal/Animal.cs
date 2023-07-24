using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    // Soif de l'animal
    public int Thirst = 100;

    // Faim de l'animal
    public int Hunger = 100;

    // Il mange
    public void Eat()
    {
        Hunger = 100;
    }

    // Il boit
    public void Drink()
    {
        Thirst = 100;
    }

    public void ReduceThirst()
    {
        Thirst--;
    }

    public void ReduceHunger()
    {
        Hunger--;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ReduceThirst", 0, 5);
        InvokeRepeating("ReduceHunger", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
