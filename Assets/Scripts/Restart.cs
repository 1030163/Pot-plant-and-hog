using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private float resetTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) {
            resetTime = 0;
        }
        if (resetTime >= 10)
        {
            SceneManager.LoadScene("Part1", LoadSceneMode.Single);
        }
        resetTime += Time.fixedDeltaTime;
    }
}
