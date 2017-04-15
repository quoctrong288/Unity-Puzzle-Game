using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepByStepController : MonoBehaviour {
    public int row, col;
    public int index;
    void OnMouseDown()
    {
        GameController.Instance.CheckMove(row, col);
        if (GameController.Instance.CheckWin())
        {
            Debug.Log("WIN");
        }
    }
}
