using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
  public int gridSize = 40;

    public float scale = 10f;

    public float maxHeight = 10f;

    public float waterLevel = 1f;

    public float sandHeight = 1.1f;

    public float grassHeight = 3f;

    public float treeProbability = 0.05f;

    public float bushProbability = 0.05f;

    public float decorationProbability = 0.02f;

    public GameObject agentVegetarian;

    public GameObject agentCarnivorous;

    public int numberOfAgentsVegetarian = 10;

    public int numberOfAgentsCarnivorous = 10;

    public List<GameObject> trees = new List<GameObject>();

    public List<GameObject> bushes = new List<GameObject>();

    public List<GameObject> decorations = new List<GameObject>();

    private float[,] heightMap;

    private List<Color> sandColors = new List<Color>();

    private List<Color> grassColors = new List<Color>();

    private List<Color> waterColors = new List<Color>();

    private List<GameObject> waterTiles = new List<GameObject>();

    private void Start()
    {

        sandColors.Add(new Color(0.988f, 0.922f, 0.749f)); // Jaune sable clair
        sandColors.Add(new Color(0.941f, 0.859f, 0.616f)); // Jaune sable moyen
        sandColors.Add(new Color(0.824f, 0.706f, 0.443f)); // Jaune sable foncé

        grassColors.Add(new Color(0.749f, 0.988f, 0.749f)); // Vert clair
        grassColors.Add(new Color(0.616f, 0.941f, 0.616f)); // Vert moyen
        grassColors.Add(new Color(0.443f, 0.824f, 0.443f)); // Vert foncé

        waterColors.Add(new Color(0.302f, 0.490f, 0.604f)); // Bleu lac foncé
        waterColors.Add(new Color(0.388f, 0.584f, 0.694f)); // Bleu lac moyen
        waterColors.Add(new Color(0.553f, 0.710f, 0.686f)); // Bleu-vert lac clair
        waterColors.Add(new Color(0.729f, 0.831f, 0.780f)); // Vert lac clair        

        GenerateTerrain();

        InvokeRepeating("UpdateWaters", 0.0f, 1f);
   
    }

    private void UpdateWaters()
    {
        foreach (GameObject water in waterTiles)
        {
            water.GetComponent<Renderer>().material.color = waterColors[Random.Range(0, waterColors.Count)];
        }
    }

    private void GenerateTerrain()
    {
    
        heightMap = new float[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                float xCoord = (float)x / gridSize * scale;
                float zCoord = (float)z / gridSize * scale;

                heightMap[x, z] = Mathf.PerlinNoise(xCoord, zCoord) * maxHeight;

                if (heightMap[x, z] < waterLevel)
                {
                    heightMap[x, z] = waterLevel;
                }
            }
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {

                float height = heightMap[x, z];

                Vector3 position = new Vector3(x, 1, z);

                if (height <= waterLevel)
                {
                    GameObject water = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    water.tag = "Water";
                    water.transform.position = position;

                    water.transform.localScale = new Vector3(1, 0.20f, 1);

                    water.GetComponent<BoxCollider>().size = new Vector3(1, 1f, 1);

                    waterTiles.Add(water);

                    water.GetComponent<Renderer>().material.color = waterColors[Random.Range(0, waterColors.Count)];
                }
                else if (height <= waterLevel + sandHeight)
                {
                    GameObject sand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    sand.tag = "Sand";
                    sand.transform.position = position;
                    sand.transform.localScale = new Vector3(1, 0.30f, 1);
                    
                    sand.GetComponent<Renderer>().material.color = sandColors[Random.Range(0, sandColors.Count)];
                    sand.GetComponent<BoxCollider>().size = new Vector3(1, 1f, 1);
                }
                else
                {
                    GameObject grass = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    grass.tag = "Food";
                    grass.transform.position = position;
                    grass.transform.localScale = new Vector3(1, 0.30f, 1);
                    grass.GetComponent<BoxCollider>().size = new Vector3(1, 1f, 1);

                    grass.GetComponent<Renderer>().material.color = grassColors[Random.Range(0, grassColors.Count)];

                    if (Random.Range(0, 100) < treeProbability)
                    {
                        GameObject tree = Instantiate(trees[Random.Range(0, trees.Count)]);
                        tree.transform.position = new Vector3(x, 1f, z);
                        tree.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        tree.tag = "Wall";
                    }

                    if (Random.Range(0, 100) < bushProbability)
                    {
                        GameObject bush = Instantiate(bushes[Random.Range(0, bushes.Count)]);
                        bush.transform.position = new Vector3(x, 1.2f, z);
                        bush.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        bush.tag = "Food";
                        
                        bush.AddComponent<BoxCollider>();
                        
                        bush.GetComponent<BoxCollider>().isTrigger = true;
                        
                    }

                    if (Random.Range(0, 100) < decorationProbability)
                    {
                        GameObject decoration = Instantiate(decorations[Random.Range(0, decorations.Count)]);
                        decoration.transform.position = new Vector3(x, 1.2f, z);
                        decoration.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        decoration.transform.Rotate(0, Random.Range(0, 360), 0);
                        decoration.tag = "Wall";
                        
                        decoration.AddComponent<BoxCollider>();
                        
                        decoration.GetComponent<BoxCollider>().isTrigger = true;

                    }

                }
            }
        }

        // Faire apparaitre les agents dans 1/4 du centre de la map
        for (int i = 0; i < numberOfAgentsVegetarian; i++)
        {
            GameObject agent = Instantiate(agentVegetarian);
            agent.transform.position = new Vector3(Random.Range(gridSize / 4, gridSize / 2), 3f, Random.Range(gridSize / 4, gridSize / 2));
            agent.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        for (int i = 0; i < numberOfAgentsCarnivorous; i++)
        {
            GameObject agent = Instantiate(agentCarnivorous);
            agent.transform.position = new Vector3(Random.Range(gridSize / 4, gridSize / 2), 3f, Random.Range(gridSize / 4, gridSize / 2));
            agent.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

    }
    
    void Update()
    {

    }

}
