using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    BasicInkExample inkManager;

    private void Start()
    {
        inkManager = gameObject.AddComponent<BasicInkExample>();
    }

    void StartDialogue()
    {
        //Pauses game
        Time.timeScale = 0;
        inkManager.RefreshView();
    }
}
