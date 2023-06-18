using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymTrainPursuit : MonoBehaviour
{
    // Le prefab de la nourriture
    public GameObject FoodPrefab;

    // Le nombre de nourriture à faire apparaitre sur le terrain
    public int FoodCount = 5;

    // On fait apparaitre la nourriture
    void InitializeFood()
    {
        // On recupère le nombre de nourriture déjà présente sur le terrain
        int foodCount = GameObject.FindGameObjectsWithTag("Food").Length;

        // On fait apparaitre le nombre de nourriture manquant
        foodCount = FoodCount - foodCount;

        // On fait apparaitre le nombre de nourriture restant
        for (int i = 0; i < foodCount; i++)
        {
            // On instancie la nourriture
            GameObject food = Instantiate(FoodPrefab);

            // On lui donne une position aléatoire
            food.transform.position = new Vector3(Random.Range(-22, 22), 0.2f, Random.Range(-22, 22));

            // On ajoute un box collider
            food.AddComponent<BoxCollider>();

            // On change la taille du box collider
            food.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, 0.5f);

            // On l'ajoute à la liste des objets du parent
            food.transform.parent = transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Toutes les minutes on fait apparaitre de la nourriture
        InvokeRepeating("InitializeFood", 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
