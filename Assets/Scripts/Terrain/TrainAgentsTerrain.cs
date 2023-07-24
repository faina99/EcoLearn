using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAgentsTerrain : MonoBehaviour
{
    // Le prefab de la nourriture
    public GameObject FoodPrefab;

    // Le nombre de nourriture à faire apparaitre sur le terrain
    public int FoodCount = 15;

    // Permet de faire apparaitre la nourriture
    void InitializeFood()
    {   
        // On recupère le nombre de nourriture déjà présente sur le terrain
        int foodCountCurrent = GameObject.FindGameObjectsWithTag("Food").Length;

        // On fait apparaitre le nombre de nourriture manquant
        foodCountCurrent = FoodCount - foodCountCurrent;

        // On fait apparaitre le nombre de nourriture restant
        for (int i = 0; i < foodCountCurrent; i++)
        {

            // On recupère la taille du parent
            float sizeX = 9;
            float sizeZ = 9;

            // On instancie la nourriture
            GameObject food = Instantiate(FoodPrefab);

            // On ajoute le tag Food
            food.tag = "Food";

            // On lui donne une position aléatoire
            food.transform.position = new Vector3(Random.Range(-sizeX + 1, sizeX - 1), 0.2f, Random.Range(-sizeZ + 1, sizeZ - 1));

            // On ajoute un box collider
            food.AddComponent<BoxCollider>();

            // On change la taille du box collider
            food.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, 0.5f);

            // On l'ajoute à la liste des objets du parent
            food.transform.parent = transform;
        }

        // Si de la nouritture est dans un objet avec un tag "Wall" on la détruit
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in walls)
        {
            // On recupère les objets dans le collider
            Collider[] hitColliders = Physics.OverlapBox(wall.transform.position, wall.transform.localScale / 2);

            // On détruit la nourriture
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Food")
                {
                    Destroy(hitCollider.gameObject);
                }
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        // Toutes les 10 secondes on fait apparaitre de la nourriture
        InvokeRepeating("InitializeFood", 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
