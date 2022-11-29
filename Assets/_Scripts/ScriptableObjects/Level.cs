using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level: ScriptableObject
{
    public int GridLength;
    public List<Vector2> Grids = new List<Vector2>();
    public string SolutionBinary;
}
