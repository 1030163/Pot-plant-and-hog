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



    public void Parralaxer()
    {

        //Y VALUES ARE WIERD!!

        Vector2 Parralax = startPosition - new Vector2(transform.position.x, transform.position.y);

        ForeGround.transform.position = Parralax * 0.1f + ForeGroundf;
        MidGround.transform.position = Parralax * 0 + MidGroundf;
        BackGround.transform.position = Parralax * 0.05f + BackGroundf;
    }

    // Update is called once per frame
    void Update()
    {
        Parralaxer();
    }
}
