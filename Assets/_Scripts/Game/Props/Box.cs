using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int X, Y;
    public int Length;
    public bool isAffected;

    private Material _boxMaterial;

    private float _currentBoxLength;

    private void Init()
    {
        _boxMaterial = GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (isAffected == false)
        {
            UpdateBoxSize();
        }
        else
        {

        }
        
    }

    private void UpdateBoxSize()
    {
        _currentBoxLength = _boxMaterial.GetFloat("_Up_Fit");
        _boxMaterial.SetFloat("_Up_Fit", Mathf.Lerp(_currentBoxLength, Length, 0.1f));
    }

    public void Effect(int value)
    {
        Length = value;
    }
}
