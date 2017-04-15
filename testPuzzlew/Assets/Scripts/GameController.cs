using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : TSingleton<GameController> {

    int countGen = 0;
    int RowCuurentBlank;
    int ColCurrentBlank;
    int countListImages = 0;

    public int row, col;
    public List<GameObject> ImageOfPictureList;
    public GameObject[,] MatrixPicture;

    public Dictionary<string, bool> Visited = new Dictionary<string, bool>();
    int[] dx = { 0, 1, 0, -1 };
    int[] dy = { 1, 0, -1, 0 };
    
    public int[,] MatrixImages;
    void Start()
    {
        MatrixImages = new int[row, col];
        MatrixPicture = new GameObject[row - 1, col - 1];
        LoadPritesPicture();
        InitObjectImages();
        InitImages();
        GenPicture();
    }
    public void LoadPritesPicture()
    {
        for (int i = 0; i < ImageOfPictureList.Count; i++)
        {
            ImageOfPictureList[i].GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.LoadSprites(i + 1);
        }
    }
    void InitObjectImages(){
        for(int i = 1 ; i < row - 1 ; i++){
            for (int j = 1; j < col - 1; j++)
            {
                MatrixPicture[i, j] = ImageOfPictureList[countListImages];
                countListImages++;
            }
        }
    }
    void InitImages()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
                {
                    MatrixImages[i, j] = -1;
                }
                else if (i == row - 2 && j == row - 2)
                {
                    MatrixImages[i, j] = 0;
                    MatrixPicture[i, j].GetComponent<StepByStepController>().row = i;
                    MatrixPicture[i, j].GetComponent<StepByStepController>().col = j;
                    RowCuurentBlank = i;
                    ColCurrentBlank = j;
                }
                else
                {
                    MatrixImages[i, j] = MatrixPicture[i, j].GetComponent<StepByStepController>().index;
                    MatrixPicture[i, j].GetComponent<StepByStepController>().row = i;
                    MatrixPicture[i, j].GetComponent<StepByStepController>().col = j;
                }
            }
        }
    }
    public void CheckMove(int rowClick, int colClick)
    {
        for (int i = 0; i <= 3; i++)
        {
            if (MatrixImages[rowClick + dx[i], colClick + dy[i]] == 0)
            {
                SwapImage(rowClick, colClick, rowClick + dx[i], colClick + dy[i]);
                Move(rowClick, colClick, rowClick + dx[i], colClick + dy[i]);
                return;
            }
        }
    }

    public bool CheckMove(int rowCurrent , int colCurrent , int RowNext , int ColNext)
    {
        if (MatrixImages[RowNext, ColNext] != -1)
        {
            SwapImage(rowCurrent, colCurrent, RowNext, ColNext);
            string str = "";
            for (int i = 1; i <= row - 2; i++)
            {
                for (int j = 1; j <= col - 2; j++)
                {
                    str += (char)(MatrixImages[i, j] + 48);
                }
            }
            Debug.Log(str);
            if (Visited.ContainsKey(str) == false)
            {
                Visited.Add(str, true);
                return true;
            }
            else
            {
                SwapImage(rowCurrent, colCurrent, RowNext, ColNext);
                return false;
            }
        }
        return false;
    }
    void Move(int rowCurrent , int colCurret , int rowSwap , int colSwap)
    {
        Vector3 cuurentPos = MatrixPicture[rowCurrent, colCurret].transform.position;
        MatrixPicture[rowCurrent, colCurret].transform.position = MatrixPicture[rowSwap, colSwap].transform.position;
        MatrixPicture[rowSwap, colSwap].transform.position = cuurentPos;

        SwapObjectImages(ref MatrixPicture[rowCurrent, colCurret], ref MatrixPicture[rowSwap, colSwap]);

        RowCuurentBlank = rowSwap;
        ColCurrentBlank = colSwap;

        MatrixPicture[rowCurrent, colCurret].GetComponent<StepByStepController>().row = rowCurrent;
        MatrixPicture[rowCurrent, colCurret].GetComponent<StepByStepController>().col = colCurret;

        MatrixPicture[rowSwap, colSwap].GetComponent<StepByStepController>().row = rowSwap;
        MatrixPicture[rowSwap, colSwap].GetComponent<StepByStepController>().col = colSwap;
    }
    public void SwapObjectImages(ref GameObject a, ref GameObject b)
    {
        GameObject c = a;
        a = b;
        b = c;
    }

    public void SwapImage(int rowCurrent, int colCurret, int rowSwap, int colSwap)
    {
        int c = MatrixImages[rowCurrent, colCurret];
        MatrixImages[rowCurrent, colCurret] = MatrixImages[rowSwap, colSwap];
        MatrixImages[rowSwap, colSwap] = c;
    }
    
    public void GenPicture()
    {
        StartCoroutine(WaitGenPicture());
    }

    int RowNext;
    int ColNext;

    IEnumerator WaitGenPicture()
    {
        countGen = 0;
        while (true)
        {
            if (countGen >= 50 && ColCurrentBlank == 3 && RowCuurentBlank == 3) break;
            int index = Random.Range(0, 4);
            for (int i = 1; i <= 4; i++)
            {
                index = (index+1) % 4;
                RowNext = RowCuurentBlank + dx[index];
                ColNext = ColCurrentBlank + dy[index];
                if (CheckMove(RowCuurentBlank, ColCurrentBlank, RowNext, ColNext))
                {
                    Move(RowCuurentBlank, ColCurrentBlank, RowNext, ColNext);
                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);
            countGen++;
        }
     }

    public bool CheckWin()
    {
        int index = 1;
        for (int i = 1; i < row - 1; i++)
        {
            for (int j = 1; j < col - 1; j++)
            {
                if (index == 9) index = 0;
                if (MatrixPicture[i, j].GetComponent<StepByStepController>().index != index)
                {
                    return false;
                }
                index++;
            }
        }
        return true;
    }
 }

