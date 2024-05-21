using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FieldPrefabObject
{
    private int _row;
    private int _colunm;
    private GameObject _instance;

    public FieldPrefabObject(GameObject instance, int row, int colunm)
    {
        _instance = instance;
        _row = row;
        _colunm = colunm;
    }

    public bool IsChangeAble = true;

    public void ChangeColorToGreen()
    {
         _instance.GetComponent<Image>().color = Color.green;
    }

    public void ChangeColorToRed()
    {
         _instance.GetComponent<Image>().color = Color.red;
    }

    public void ResetField()
    {
        IsChangeAble = true;
        _instance.GetComponent<Image>().color = Color.white;
        if (TryGetTextByName("Value", out Text text))
        {
            text.text = "";
        }
        for (int i = 1; i < 10; i++)
        {
            if (TryGetTextByName($"Number_{i}", out Text textNumber))
            {
                textNumber.text = "";
            }
        }
        Number = 0;
    }

    public bool TryGetTextByName(string name, out Text text)
    {
        text = null;
        Text[] texts = _instance.GetComponentsInChildren<Text>();
        foreach(var currentText in texts)
        {
            if (currentText.name.Equals(name))
            {
                text = currentText;
                return true;
            }
        }
        return false;
    }       

    public int Row {get => _row; set => _row = value;}
    public int Colunm {get => _colunm; set => _colunm = value;}

    public void SetHoverMode()
    {
        _instance.GetComponent<Image>().color = new Color(0.70f,0.99f,0.99f);
    }

    public void UnsetHoverMode()
    {
        _instance.GetComponent<Image>().color = new Color(1f,1f,1f);
    }

    public int Number;
    public void SetNumber(int number)
    {
        if(TryGetTextByName("Value", out Text text))
        {
            Number = number;
            text.text = number.ToString();
            for(int i = 1; i <10; i++)
            {
                if(TryGetTextByName($"Number_{i}",out Text textNumber))
                {
                    textNumber.text = "";
                }
            }
        }
    }

    public void SetSmallNumber(int number)
    {
        if(TryGetTextByName($"Number_{number}",out Text text))
        {
            text.text = number.ToString();
            if(TryGetTextByName("Value", out Text textValue))
            {
                textValue.text = "";
            }
        }
    }
}
