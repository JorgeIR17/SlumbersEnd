using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Game : MonoBehaviour
{

    public GameObject MainPanel;
    public GameObject SudokuFieldPanel;
    public GameObject FieldPrefab;
    public GameObject ControlPanel;
    public GameObject ControlPrefab;

    public GameObject canva;
    public GameObject winnerMessage;
    public UnityEvent OnCanvasDestroyed;

    public Button InformationButton;
    // Start is called before the first frame update
    void Start()
    {
       Cursor.lockState = CursorLockMode.None;
       CreateFieldPrefab();
       CreateControlPrefab();
       CreateSudokuObject();
       winnerMessage.SetActive(false);
    }

    public void ClickOn_Finish()
    {
        bool allCorrect = true;

        for(int row=0; row<9;row++)
        {
            for(int colunm=0; colunm<9; colunm++)
            {
                FieldPrefabObject fieldObject = _fieldPrefabObjectDic[new Tuple<int, int>(row,colunm)];

                if(fieldObject.IsChangeAble)
                {
                    if(_finalObject.Values[row,colunm] == fieldObject.Number)
                    {
                        fieldObject.ChangeColorToGreen();
                    }
                    else
                    {
                        fieldObject.ChangeColorToRed();
                        allCorrect = false;
                    }
                }
            }
            
            if(allCorrect)
            {
                winnerMessage.SetActive(true);
            }
        }   
    }

    public void DestroyCanva()
    {
        Cursor.lockState = CursorLockMode.None;
        Destroy(canva.gameObject);
        OnCanvasDestroyed.Invoke();
    }

    private SudokuObject _gameObject;
    private SudokuObject _finalObject;

    public void RestartGame()
    {
        // Resetear el mensaje de ganador
        winnerMessage.SetActive(false);

        // Resetear los colores y valores de las celdas
        foreach (var fieldObject in _fieldPrefabObjectDic.Values)
        {
            fieldObject.ResetField();
        }

        // Generar un nuevo Sudoku
        CreateSudokuObject();
    }

    public void SalirGame()
    {
        canva.gameObject.SetActive(false);
    }
    
    private void CreateSudokuObject()
    {
        SudokuGenerator.CreateSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject);
        _gameObject = gameObject;
        _finalObject = finalObject;
        for(int row = 0; row < 9; row++)
        {
            for(int colunm = 0; colunm < 9; colunm++)
            {
                var currentValue = _gameObject.Values[row, colunm];
                if(currentValue != 0)
                {
                    FieldPrefabObject fieldObject = _fieldPrefabObjectDic[new Tuple<int, int>(row,colunm)];
                    fieldObject.SetNumber(currentValue);
                    fieldObject.IsChangeAble = false;
                }
                
            }
        }
    }
    private bool IsInformationButtonActive = false;
    public void ClickOn_InformationButton()
    {
        if(IsInformationButtonActive)
        {
            IsInformationButtonActive = false;
            InformationButton.GetComponent<Image>().color = new Color(1f,1f,1f);
        }
        else
        {
            IsInformationButtonActive = true;
            InformationButton.GetComponent<Image>().color = new Color(0.70f,0.99f,0.99f);
        }
    }
    private Dictionary<Tuple<int, int>, FieldPrefabObject> _fieldPrefabObjectDic = new Dictionary<Tuple<int, int>, FieldPrefabObject>();

    private void CreateFieldPrefab()
    {
         for(int row = 0; row < 9; row++)
        {
            for(int colunm = 0; colunm < 9; colunm++)
            {
                GameObject instance = GameObject.Instantiate(FieldPrefab, SudokuFieldPanel.transform);
                
                FieldPrefabObject fieldPrefabObject = new FieldPrefabObject(instance, row, colunm);
                _fieldPrefabObjectDic.Add(new Tuple<int, int>(row, colunm), fieldPrefabObject);

                instance.GetComponent<Button>().onClick.AddListener( () => onClick_FieldPrefab(fieldPrefabObject));
            }
        }
    }

    private void CreateControlPrefab()
    {
         for(int i = 1; i < 10; i++)
        {

            GameObject instance = GameObject.Instantiate(ControlPrefab, ControlPanel.transform);
            instance.GetComponentInChildren<Text>().text = i.ToString();
            ControlPrefabObject controlPrefabObject = new ControlPrefabObject();
            controlPrefabObject.Number = i;
            instance.GetComponent<Button>().onClick.AddListener( () => ClickOn_ControlPrefab(controlPrefabObject));
            
        }
    }

    private void ClickOn_ControlPrefab(ControlPrefabObject controlPrefabObject)
    {
        if(_currentHoveredFieldPrefab != null)
        {
            if(IsInformationButtonActive)
            {
                _currentHoveredFieldPrefab.SetSmallNumber(controlPrefabObject.Number);
            }
            else
            {
                int currentNumber = controlPrefabObject.Number;
                int row = _currentHoveredFieldPrefab.Row;
                int colunm = _currentHoveredFieldPrefab.Colunm;

                if(_gameObject.IsPossibleNumberInPosition(currentNumber,row,colunm))
                {
                _currentHoveredFieldPrefab.SetNumber(controlPrefabObject.Number);
                }
            }
            
        }
    }
    private FieldPrefabObject _currentHoveredFieldPrefab;
    private void onClick_FieldPrefab(FieldPrefabObject fieldPrefabObject)
    {
        if(fieldPrefabObject.IsChangeAble)
        {
            if(_currentHoveredFieldPrefab != null)
            {
                _currentHoveredFieldPrefab.UnsetHoverMode();
            }
            _currentHoveredFieldPrefab = fieldPrefabObject;
            fieldPrefabObject.SetHoverMode();
        }
          
    }

}
