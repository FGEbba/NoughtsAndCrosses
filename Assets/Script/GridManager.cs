using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridManager : MonoBehaviour
{
    //Able to add more squares if we want to later
    [HideInInspector] public int NumberOfSquares = 3;
    [SerializeField] float squareMargin = 20f;

    private int TotalLineCount;
    private float vertical, horizontal;

    Vector3 posOne = Vector3.zero, posTwo = Vector3.zero;

    private Dictionary<(int, int), GameObject> Grid = new Dictionary<(int, int), GameObject>();

    public Dictionary<(int, int), GameObject> GetGrid() { return Grid; }

    public void SpawnGrid(GameObject Square, GameObject VLine, GameObject HLine, Canvas canvas)
    {
        TotalLineCount = NumberOfSquares - 1;

        vertical = canvas.pixelRect.height;
        horizontal = canvas.pixelRect.width;

        for (int i = 0; i < NumberOfSquares; i++)
        {
            for (int j = 0; j < NumberOfSquares; j++)
            {
                GameObject ButtonGO = Instantiate(Square);
                ButtonGO.name = $"Square x: {i} y: {j}";
                ButtonGO.transform.SetParent(canvas.transform);

                RectTransform ButtonRect = ButtonGO.GetComponent<RectTransform>();

                ButtonGO.transform.position = new Vector3((horizontal * 0.5f) - (i - 1) * (squareMargin + ButtonRect.sizeDelta.x), (vertical * 0.5f) - (j - 1) * (squareMargin + ButtonRect.sizeDelta.y));
                Grid.Add((i, j), ButtonGO);
            }
            if (i <= TotalLineCount && i != 0)
            {
                SpawnLine(VLine, i, NumberOfSquares - 3, true, canvas);
                SpawnLine(HLine, i, NumberOfSquares - 3, false, canvas);
            }
        }
    }


    public void SpawnLine(GameObject Line, int currentRow, int LineMultiplier, bool Vertical, Canvas canvas)
    {

        GameObject line = Instantiate(Line);
        RectTransform LineRect = line.GetComponent<RectTransform>();
        line.transform.SetParent(canvas.transform);

        if (Vertical)
        {
            RectTransform squareTransform = Grid[(currentRow, 0)].GetComponent<RectTransform>();
            posOne = squareTransform.position;
            posTwo = Grid[(currentRow - 1, 0)].GetComponent<RectTransform>().position;

            line.name = $"Vertical {currentRow}";

            LineRect.sizeDelta = new Vector2(LineRect.sizeDelta.x, ((squareTransform.sizeDelta.x + squareMargin) * NumberOfSquares));
            line.transform.position = new Vector3((posOne.x + (posTwo.x - posOne.x) * 0.5f), (vertical * 0.5f) - ((squareTransform.sizeDelta.x) * 0.5f * LineMultiplier + squareMargin * 0.5f * LineMultiplier));
        }
        else
        {
            RectTransform squareTransform = Grid[(0, currentRow)].GetComponent<RectTransform>();
            posOne = squareTransform.position;
            posTwo = Grid[(0, currentRow - 1)].GetComponent<RectTransform>().position;

            line.name = $"Horizontal {currentRow}";

            LineRect.sizeDelta = new Vector2(((squareTransform.sizeDelta.x + squareMargin) * NumberOfSquares), LineRect.sizeDelta.y);
            line.transform.position = new Vector3((horizontal * 0.5f) - ((squareTransform.sizeDelta.x) * 0.5f * LineMultiplier + squareMargin * 0.5f * LineMultiplier), (posOne.y + (posTwo.y - posOne.y) * 0.5f));
        }

    }
}
