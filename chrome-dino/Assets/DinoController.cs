using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour
{
    public float totalScore = 0f;
    private float y_min;

    public float jumpForce;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;

    public List<GameObject> near_player = new List<GameObject>();
    private float nearest_x_dis;
    private float nearest_y_dis;
    private double[][] input = new double[2][];

    public NN nn = new NN(2, 4);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        y_min = transform.position.y;

        input[0] = new double[1];
        input[1] = new double[1];

        // Debug.Log(nn.weight_hi[0][0]);
        // nn.mutateNN(0.1f);
        // Debug.Log(nn.weight_hi[0][0]);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    void Update()
    {
        if (isGrounded)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;

            #region NEURAL NETWORK CODE

            near_player.Clear();
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                near_player.Add(g);
            }

            float oldDistance = 999999f;
            // GameObject oldg = new GameObject();
            foreach (GameObject g in near_player)
            {
                if (this.gameObject.transform.position.x < g.transform.position.x)
                {
                    float dist = Vector3.Distance(this.gameObject.transform.position, g.transform.position);
                    if (dist < oldDistance)
                    {
                        nearest_x_dis = g.transform.position.x - this.gameObject.transform.position.x;
                        nearest_y_dis = g.transform.position.y - this.gameObject.transform.position.y;
                        // Debug.Log(g.transform.position.x);
                        // oldg = g;
                        oldDistance = dist;
                    }
                }
                
            }

            input[0][0] = nearest_x_dis;
            input[1][0] = nearest_y_dis;

            double[][] output = nn.feedForward(input);
            
            double output_ans = output[0][0];
            // Debug.Log(output_ans);

            if (output_ans > 0.5d && isGrounded)
                rb.velocity = Vector2.up * jumpForce;

            #endregion
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 5f;
        }

        if (transform.position.y < y_min)
        {
            transform.position = new Vector2(transform.position.x, y_min);
        }
        
        // if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        // {
        //     rb.velocity = Vector2.up * jumpForce;
        // }

        totalScore += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            // Debug.Log(totalScore);

            nn.fitness = totalScore;
            NN_Apply NN_Unity = GameObject.Find("NN").GetComponent<NN_Apply>();
            NN_Unity.nn_list.Add(this.nn);
            // NN_Unity.score_list.Add(this.totalScore);
            Destroy(gameObject);
            // Debug.Log("Died");
        }    
    }
}
