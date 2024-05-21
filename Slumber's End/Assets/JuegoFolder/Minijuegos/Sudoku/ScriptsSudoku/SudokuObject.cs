using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuObject
{
    public int[,] Values = new int[9, 9];

    public void GetGroupIndex(int group, out int startRow, out int startColunm)
    {
        startRow = 0;
        startColunm = 0;

        switch(group)
        {
            case 1: 
                startRow = 0;
                startColunm = 0;
                break;
            
            case 2: 
                startRow = 0;
                startColunm = 3;
                break;
            
            case 3: 
                startRow = 0;
                startColunm = 6;
                break;
            
            case 4: 
                startRow = 3;
                startColunm = 0;
                break;
            
            case 5: 
                startRow = 3;
                startColunm = 3;
                break;
            
            case 6: 
                startRow = 3;
                startColunm = 6;
                break;
            
            case 7: 
                startRow = 6;
                startColunm = 0;
                break;
            
            case 8: 
                startRow = 6;
                startColunm = 3;
                break;
            
            case 9: 
                startRow = 6;
                startColunm = 6;
                break;
        }
    }

    public bool IsPossibleNumberInPosition(int number, int row, int colunm)
    {
        if(IsPossibleNumberInRow(number, row) && IsPossibleNumberInColunm(number, colunm))
        {
            if(IsPossibleNumberInGroup(number, GetGroup(row,colunm)))
            {
                return true;
            }
        }
        return false;
    }

    private int GetGroup(int row, int colunm)
    {
        if(row < 3)
        {
            if(colunm < 3)
            {
                return 1;
            }
            if(colunm < 6)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        if(row < 6)
        {
            if(colunm < 3)
            {
                return 4;
            }
            if(colunm < 6)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }
        else
        {
            if(colunm < 3)
            {
                return 7;
            }
            if(colunm < 6)
            {
                return 8;
            }
            else
            {
                return 9;
            }
        }
    }
    private bool IsPossibleNumberInRow(int number, int row)
    {
        for(int i = 0; i < 9; i++)
        {
           if( Values[row, i]==number )
           {
                return false;
           }
        }
        return true;
    }

    private bool IsPossibleNumberInColunm(int number, int colunm)
    {
        for(int i = 0; i < 9; i++)
        {
           if( Values[i, colunm]==number )
           {
                return false;
           }
        }
        return true;
    }

    private bool IsPossibleNumberInGroup(int number, int group)
    {
        GetGroupIndex(group, out int startRow, out int startColunm);
        for(int row = startRow; row < startRow + 3; row++)
        {
            for(int colunm = startColunm;colunm < startColunm + 3; colunm++)
            {
                if(Values[row,colunm]==number)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
