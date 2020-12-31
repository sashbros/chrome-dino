using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NN_Apply : MonoBehaviour
{
    // public NN nn = new NN(2, 4);

    //public NN child = new NN(2, 4);

    // private double[][] input = new double[2][];

    public GameObject player;
    public int startPopulation;

    public List<NN> nn_list = new List<NN>();
    // [HideInInspector]
    // public List<float> score_list = new List<float>();

    public int generation;
    public Text generationText;
    public Text maxFitnessText;
    public float prevFitness;

    public float currentScore = 0f;
    public Text currentScoreText;
    
    void Start()
    {
        Application.targetFrameRate = 300;
        
        for (int i = 0; i < startPopulation; i++)
        {
            GameObject instance = Instantiate(player, this.transform.position, Quaternion.identity) as GameObject;
            // Debug.Log(instance.GetComponent<DinoController>().nn.weight_hi[0][0]);

            // nn_list.Add(instance.GetComponent<DinoController>().nn);
            // score_list.Add(instance.GetComponent<DinoController>().totalScore);
        }
        generation+=1;
    /*
        input[0] = new double[1];
        input[1] = new double[1];
        input[0][0] = 4d;
        input[1][0] = 7d;

        double[][] output = nn.feedForward(input);
        
        double output_ans = output[0][0];

        if (output_ans < 0.5d)
            Debug.Log("Do not Jump");
        else
            Debug.Log("Jump");

        NN child = nn.copyNN();
        Debug.Log(child.hidNodes);
    */
    }

    
    void Update()
    {
        if (nn_list.Count == 10)
        {
            Sort_NNList(nn_list);
            // find top 5 nn (sort)
            // copy top 5 to rest 5

            for (int i = 5; i < 10; i++)
            {
                nn_list[i] = nn_list[i-5];
            }

            // mutate the copied 5
            for (int i = 5; i < 10; i++)
            {
                nn_list[i].mutateNN(0.05f);
            }

            // destroy all gameobjects with tag "Obstacle"
            GameObject[] obstacle_list = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject item in obstacle_list)
            {
                if (item.name != "Obstacle01" && 
                    item.name != "Obstacle02" && 
                    item.name != "Obstacle03" && 
                    item.name != "Obstacle04")
                    {
                        Destroy(item);
                    }
            }

            // instantiate 10 dinos
            // instance.nn = nn_list[i].copyNN()
            for (int i = 0; i < startPopulation; i++)
            {
                GameObject instance = Instantiate(player, this.transform.position, Quaternion.identity) as GameObject;
                instance.GetComponent<DinoController>().nn = nn_list[i].copyNN();
            }

            
            if (prevFitness == 0f)
                prevFitness = nn_list[0].fitness;
            // generation++
            generation++;
            generationText.text = "Generation - " + generation.ToString();
            maxFitnessText.text = "Max. Fitness - " + System.Math.Max(prevFitness, nn_list[0].fitness).ToString("0.00");
            // Debug.Log(nn_list[0].fitness);
            // Debug.Log(nn_list[5].fitness);

            prevFitness = System.Math.Max(prevFitness, nn_list[0].fitness);
            // nn_list.Clear
            nn_list.Clear();

            // current score
            currentScore = 0f;
        }

        currentScore += Time.deltaTime;
        currentScoreText.text = "Current Fitness - " + currentScore.ToString("0.00");
    }

    public void Sort_NNList(List<NN> nn_list)
    {
        for (int i = 0; i < nn_list.Count - 1; i++)
        {
            for (int j = i+1; j < nn_list.Count; j++)
            {
                if (nn_list[i].fitness < nn_list[j].fitness)
                {
                    NN temp = nn_list[i];
                    nn_list[i] = nn_list[j];
                    nn_list[j] = temp;
                }
            }
        }
    }
}
