using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterUnit : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject RaycastPrefab;

    [Space]
    [Header("Components")]
    public List<GameObject> Rays;

    public List<GameObject> HittingBoxs;

    public List<GameObject> BoxsOnRow;
    public List<int> HeightOfBoxsOnRow;


    [Space]
    [Header("Fields")]
    public bool isActive;
    public int id;
    private int RayCount;
    public  bool RaycasterDirection; //X or Y

    public int HitCount;


    private void Init()
    {

    }

    public void CreateRaycasts(int rayCount)
    {
        this.RayCount = rayCount;


        Vector3 rayPosition = new Vector3(0,0,0);
        for (int i = 0; i < this.RayCount; i++)
        {
            GameObject newRay = InstantiateBehaviour.InstantiateGameObjectLocal(RaycastPrefab, transform, new Vector3(0, i + 0.5f, 0));
            //GameObject NewRaycaster = Instantiate(RaycastPrefab, gameObject.transform);
            //NewRaycaster.transform.localPosition = new Vector3(NewRaycaster.transform.localPosition.x, i + 0.5f, NewRaycaster.transform.localPosition.z);
            newRay.name = i.ToString();
            Rays.Add(newRay);
        }

    }

    public void SetRaycastProperties(bool isXY, int id, bool isActive)//if is X = false, Y = true
    {
        RaycasterDirection = isXY;
        this.id = id;
        this.isActive = isActive;
    }

    public void StartSendingRays()
    {
        StartCoroutine(RoutineSendingRays());
    }

    public IEnumerator RoutineSendingRays()
    {
        while (true)
        {
            SendRays();
            yield return new WaitForSeconds(0.2f);
        }

    }
    public void SendRays()
    {
        HittingBoxs.Clear();
        for (int i = 0; i < Rays.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(Rays[i].transform.position, RaycasterDirection ? Vector3.back : Vector3.right, out hit, 100f))
            {
                if (HittingBoxs.Find(x => x == hit.transform.gameObject) == null)
                {
                    HittingBoxs.Add(hit.transform.gameObject);
                }
            }

            if (i == 0)
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(transform.position, RaycasterDirection ? Vector3.back : Vector3.right, 100.0f);
                BoxsOnRow.Clear();
                HeightOfBoxsOnRow.Clear();
                for (int a = 0; a < hits.Length; a++)
                {
                    BoxsOnRow.Add(hits[a].transform.gameObject);
                    HeightOfBoxsOnRow.Add((int)Mathf.Round(hits[a].transform.gameObject.GetComponent<MeshRenderer>().material.GetFloat("_Up_Fit")));//Mathf.Round(SelectedBox.transform.GetComponent<MeshRenderer>().material.GetFloat("_Up_Fit"))  / 5.0f

                }
            }
        }

        HitCount = HittingBoxs.Count;
        if (!RaycasterDirection)
        {
            //BuilderController.BC.Level.GetRays[id].BuildCount = new Vector2(HitCount, BuilderController.BC.Level.GetRays[id].BuildCount.y);
        }
        else
        {
           // BuilderController.BC.Level.GetRays[id].BuildCount = new Vector2(BuilderController.BC.Level.GetRays[id].BuildCount.x, HitCount);
        }

    }
}
