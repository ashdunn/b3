using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovingObstacle : MonoBehaviour
{
    public float speed;
    public float changeTime = 3f;
    private float aggrTime = 0f;
    private bool flagUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aggrTime += Time.deltaTime;
        if (aggrTime >= changeTime)
        {
            flagUp = !flagUp;
            aggrTime = 0f;
        }
        if (flagUp)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
    }
}
