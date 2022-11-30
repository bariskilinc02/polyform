using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bools : MonoBehaviour
{
    public bool is_InMenu;
    public bool is_Playing;
    public bool is_OnTransition;
    public bool isLevelCompleted; 
    public bool isLevelTransition;
    public bool isDeleteLineMode;

    private void Awake()
    {
        is_InMenu = true;
    }
}
