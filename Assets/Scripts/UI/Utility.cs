using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
   public static int WrapAround(int max, int current, int increment, int min=0)
    {
        int temp = current + increment;

        if(temp >= max)
        {
            temp = min;
        }
        else if(temp < min)
        {
            temp = max - 1;
        }

        return temp;

    }
}
