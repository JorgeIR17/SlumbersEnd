using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGenerator 
{
    public static void CreateSudokuObject(out SudokuObject finalObject, out SudokuObject gameObject)
    {
        _finalSudokuObject = null;
        SudokuObject sudokuObject = new SudokuObject();
        CreateRandomGroups(sudokuObject);
        if(TryToSolve(sudokuObject))
        {
            sudokuObject = _finalSudokuObject;
        }
        else
        {
            throw new System.Exception("Algo ha ido mal");
        }
        finalObject = sudokuObject;
        gameObject = RemoveSomeRandomNumbers(sudokuObject);
    }

    private static SudokuObject RemoveSomeRandomNumbers(SudokuObject sudokuObject)
    {
        SudokuObject newSudokuObject = new SudokuObject();
        newSudokuObject.Values = (int[,]) sudokuObject.Values.Clone();
        List<int>values = GetValues();
        bool isFinish = false;
        while (!isFinish)
        {
            int index = Random.Range(0, values.Count);
            int searchedIndex = values[index];

            for(int i = 1; i <10; i++)
            {
                for(int j = 1; j<10; j++)
                {
                    if(i*j==searchedIndex)
                    {
                        values.RemoveAt(index);
                        SudokuObject nextSudokuObject = new SudokuObject();
                        nextSudokuObject.Values = (int[,]) newSudokuObject.Values.Clone();
                        nextSudokuObject.Values[i-1,j-1] = 0;
                        if(TryToSolve(nextSudokuObject,true))
                        {
                            newSudokuObject = nextSudokuObject;
                        }
                    }
                }
            }

            if(values.Count < 30)
            {
                isFinish = true;
            }
        }

        return newSudokuObject;
         
    }

    private static List<int> GetValues()
    {
        List<int> values = new List<int>();
        for(int i=1; i<10 ; i++)
        {
            for(int j=0; j<10; j++)
            {
                values.Add(i*j);
            }
        }

        return values;
    }

    private static SudokuObject _finalSudokuObject;

    private static bool TryToSolve(SudokuObject sudokuObject, bool OnlyOne = false)
    {
        if(HasEmptyFieldsToFill(sudokuObject,out int row, out int colunm, OnlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudokuObject,row,colunm);
            foreach(var possibleValue in possibleValues)
            {
                SudokuObject nextSudokuObject = new SudokuObject();
                nextSudokuObject.Values =(int[,]) sudokuObject.Values.Clone();
                nextSudokuObject.Values[row,colunm] = possibleValue;
                if(TryToSolve(nextSudokuObject,OnlyOne))
                {
                    return true;
                }
            }
        }

        if(HasEmptyFields(sudokuObject))
        {
            return false;
        }
        _finalSudokuObject = sudokuObject;
        return true;
    }

    private static bool HasEmptyFields(SudokuObject sudokuObject)
    {
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if(sudokuObject.Values[i,j]==0)
                {
                   return true;
                }
            }
        }

        return false;
    }

    private static List<int>GetPossibleValues(SudokuObject sudokuObject, int row, int colunm)
    {
        List<int> PossibleValues = new List<int>();
        for(int value = 1; value < 10; value++)
        {
            if(sudokuObject.IsPossibleNumberInPosition(value,row,colunm))
            {
                PossibleValues.Add(value);
            }
        }

        return PossibleValues;
    }

    private static bool HasEmptyFieldsToFill(SudokuObject sudokuObject, out int row, out int colunm, bool OnlyOne = false)
    {
        row = 0;
        colunm = 0;
        int amountOfPossibleValues = 10;
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if(sudokuObject.Values[i,j]==0)
                {
                   int currentAmount = GetPossibleAmountOfValues(sudokuObject, i,j);
                   if(currentAmount != 0)
                   {
                        if(currentAmount<amountOfPossibleValues)
                        {
                            amountOfPossibleValues = currentAmount;
                            row = i;
                            colunm = j;
                        }
                   }
                    
                }
            }
        }
        if(OnlyOne)
        {
            if(amountOfPossibleValues==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if(amountOfPossibleValues == 10)
        {
            return false;
        }

        return true;
    }

    private static int GetPossibleAmountOfValues(SudokuObject sudokuObject, int row, int colunm)
    {
        int amount = 0;
        for(int value = 1; value < 10; value++)
        {
            if(sudokuObject.IsPossibleNumberInPosition(value,row,colunm))
            {
                amount++;             
            }
        }
        return amount;
    }

    public static void CreateRandomGroups(SudokuObject sudokuObject)
    {
        List<int>values = new List<int>() {0,1,2};
        int index = Random.Range(0,values.Count);
        InsertRandomGroup(sudokuObject, 1+values[index]);
        values.RemoveAt(index);

        index = Random.Range(0,values.Count);
        InsertRandomGroup(sudokuObject, 4+values[index]);
        values.RemoveAt(index);

        index = Random.Range(0,values.Count);
        InsertRandomGroup(sudokuObject, 7+values[index]);
        values.RemoveAt(index);
    }

    public static void InsertRandomGroup(SudokuObject sudokuObject, int group)
    {
        sudokuObject.GetGroupIndex(group, out int startRow, out int startColunm);
        List<int>values =new List<int>() {1,2,3,4,5,6,7,8,9};
        for(int row = startRow; row <startRow + 3; row++)
        {
            for(int colunm = startColunm; colunm < startColunm + 3; colunm++)
            {
                int index = Random.Range(0,values.Count);
                sudokuObject.Values[row,colunm] = values[index];
                values.RemoveAt(index);
            }
        }
    }
}
