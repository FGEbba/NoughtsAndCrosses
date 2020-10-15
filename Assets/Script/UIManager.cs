using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    [SerializeField] public GameObject SquarePrefab;
    [SerializeField] public GameObject VerticalLinePrefab;
    [SerializeField] public GameObject HorizontalLinePrefab;


    [SerializeField] private Text WinText;
    [SerializeField] private Text LoseText;
    [SerializeField] private Text TieText;
    [SerializeField] private Button ResetButton;

    private GameManager GameManager;
    public void SetGameManager(GameManager value) { GameManager = value; }


    void Start()
    {
        CheckFields();

        WinText.gameObject.SetActive(false);
        LoseText.gameObject.SetActive(false);
        TieText.gameObject.SetActive(false);
        ResetButton.gameObject.SetActive(false);

    }

    public void UIGridSpawn(GridManager grid)
    {
        grid.SpawnGrid(SquarePrefab, VerticalLinePrefab, HorizontalLinePrefab, canvas);
    }
    public void OnWin()
    {
        WinText.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);

        ResetButton.onClick.AddListener(delegate { OnResetClick(); }); ;
    }

    public void OnLose()
    {
        LoseText.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);

        ResetButton.onClick.AddListener(delegate { OnResetClick(); }); ;
    }

    public void OnTie()
    {
        TieText.gameObject.SetActive(true);
        ResetButton.gameObject.SetActive(true);

        ResetButton.onClick.AddListener(delegate { OnResetClick(); }); ;
    }

    private void CheckFields()
    {
        if (SquarePrefab == null)
            Debug.LogError("No prefab for the Square is set!");

        if (VerticalLinePrefab == null)
            Debug.LogError("No prefab for the Vertical Line is set!");

        if (HorizontalLinePrefab == null)
            Debug.LogError("No prefab for the Horizontal Line is set!");

        if (WinText == null)
            Debug.LogError("No Win Text is set!");

        if (TieText == null)
            Debug.LogError("No Tie Text is set!");

        if (LoseText == null)
            Debug.LogError("No Lose Text is set!");

        if (ResetButton == null)
            Debug.LogError("No reset button is set!");


    }


    private void OnResetClick()
    {
        WinText.gameObject.SetActive(false);
        LoseText.gameObject.SetActive(false);
        TieText.gameObject.SetActive(false);
        ResetButton.gameObject.SetActive(false);
        ResetButton.onClick.RemoveAllListeners();
        GameManager.ResetBoard();
    }

}
