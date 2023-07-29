using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        ForeGroundf = ForeGround.transform.position;
        MidGroundf = MidGround.transform.position;
        BackGroundf = BackGround.transform.position;
    }


    private Vector2 startPosition;
    public GameObject ForeGround, MidGround, BackGround, ReallyFarBackground;
    private Vector2 ForeGroundf, MidGroundf, BackGroundf, ReallyFarBackgroundf;
    public float foreGroundMult, backGroundMult, midGroundMult;


    public void Parralaxer()
    {

        

        Vector2 Parralax = startPosition - new Vector2(transform.position.x, transform.position.y);

        ForeGround.transform.position = foreGroundMult * Parralax * new Vector2(0.05f,0.01f) + ForeGroundf;
        MidGround.transform.position = midGroundMult * Parralax * new Vector2(0.02f, 0.01f) + MidGroundf;
        BackGround.transform.position = backGroundMult * Parralax * new Vector2(0.01f, 0.01f) + BackGroundf;
    }

    // Update is called once per frame
    void Update()
    {
        Parralaxer();
    }
}
