using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Osciilator : MonoBehaviour
{
    [SerializeField] float period = 2f;
    [SerializeField] Vector3 movementVector=new Vector3(10f,10f,10f);
    [Range(0,1)] [SerializeField] float movementFactor;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycle = Time.time / period; // grows continually from 0 
      
       const  float tau = Mathf.PI * 2f;//about 6.28
        float rawsineWave = Mathf.Sin(cycle*tau );//goes -1 to 1
        movementFactor = rawsineWave / 2f+0.5f; // goes -0.5 to 0.5 after dividing by2 and then goes 0 to 1 after adding 0.5

        Vector3 offset = movementVector * movementFactor;
        transform.position = offset + startPos; 
    }
}
