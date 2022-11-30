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
        UpdateBoxColliderSize();

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

    private void UpdateBoxColliderSize()
    {
        transform.GetComponent<BoxCollider>().center = new Vector3(0, Length / 2.0f, 0);
        transform.GetComponent<BoxCollider>().size = new Vector3(1, 1 + Length, 1);
    }

    public void Effect(int value)
    {
        Length = value;
    }
}
