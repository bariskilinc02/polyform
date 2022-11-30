using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUnit : MonoBehaviour
{
    public int X;
    public int Y;

    public int Height;

    public void SetPositionIndex(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int GetMinAccesableHeight(LevelController levelController, GroundUnit groundUnit)
    {
        int whichXUnit = groundUnit.X;
        int whichYUnit = groundUnit.Y;

        RaycasterUnit raycasterUnitX = levelController.RaycasterUnitsX.Find(x => x.RaycasterDirection == false && x.id == whichXUnit);
        RaycasterUnit raycasterUnitY = levelController.RaycasterUnitsY.Find(x => x.RaycasterDirection == true && x.id == whichYUnit);

        int accesableHeight = 0;
        for (int i = 0; i < levelController.CurrentGridLength; i++)
        {
            if (raycasterUnitX.HeightOfBoxsOnRow.Exists(x => x == accesableHeight) || raycasterUnitY.HeightOfBoxsOnRow.Exists(x => x == accesableHeight))
            {
                accesableHeight += 1;
            }
            else
            {
                break;
            }
        }

        if (accesableHeight > levelController.CurrentGridLength)
        {
            accesableHeight = -1; 
        }

        return accesableHeight;
          
    }

    public bool IsAccesableHeight(LevelController levelController, GroundUnit groundUnit, int value)
    {
        bool isAccesable = false;

        int whichXUnit = groundUnit.X;
        int whichYUnit = groundUnit.Y;

        RaycasterUnit raycasterUnitX = levelController.RaycasterUnitsX.Find(x => x.RaycasterDirection == false && x.id == whichXUnit);
        RaycasterUnit raycasterUnitY = levelController.RaycasterUnitsY.Find(x => x.RaycasterDirection == true && x.id == whichYUnit);

        if (!raycasterUnitX.HeightOfBoxsOnRow.Exists(x => x == value) && !raycasterUnitY.HeightOfBoxsOnRow.Exists(x => x == value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
