using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymTrainDodge : MonoBehaviour
{
    // Le prefab de la nourriture
    public GameObject FoodPrefab;

    // Le prefab de l'obstacle
    public GameObject ObstaclePrefab;

    // Le nombre de nourriture à faire apparaitre sur le terrain
    public int FoodCount = 500;

    // Le nombre d'obstacle à faire apparaitre sur le terrain
    public int ObstacleCount = 150;

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

            // On recupère la taille X et Z 
            float x = transform.localScale.x;
            float z = transform.localScale.z;   

            // On lui donne une position aléatoire
            food.transform.position = new Vector3(Random.Range(-250, 250), 0.2f, Random.Range(-250, 250));

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

       for (int i = 0; i < ObstacleCount; i++)
        {
            // On instancie la nourriture
            GameObject obstacle = Instantiate(ObstaclePrefab);

            // On recupère la taille X et Z 
            float x = transform.localScale.x;
            float z = transform.localScale.z;   

            // On lui donne une position aléatoire
            obstacle.transform.position = new Vector3(Random.Range(-250, 250), 0.2f, Random.Range(-250, 250));

            // On fait une rotation aléatoire sur l'axe Y
            obstacle.transform.Rotate(0, Random.Range(0, 360), 0);

            // On ajoute un box collider
            obstacle.AddComponent<BoxCollider>();

            // On change la taille du box collider
            obstacle.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, 0.5f);

            // On l'ajoute à la liste des objets du parent
            obstacle.transform.parent = transform;
        }


        // Toutes les minutes on fait apparaitre de la nourriture
        InvokeRepeating("InitializeFood", 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
