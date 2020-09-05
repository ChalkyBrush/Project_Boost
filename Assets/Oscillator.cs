using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(2f, 2f, 0);

    [Range(0,1)] [SerializeField] float movementFactor = 0;
    [SerializeField] float period = 2f;
    Vector3 startPos;
// Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = 1;
        if (period > 0) {
            cycles = Time.time / period;
        }
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        Vector3 offset = movementVector * rawSinWave;
        transform.position = startPos + offset;
    }
}
