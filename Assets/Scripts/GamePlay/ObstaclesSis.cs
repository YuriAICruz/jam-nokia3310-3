using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSis : MonoBehaviour
{

    public GameObject ObtPrefab;
    public List<GameObject> ObtScene;
    public Queue<GameObject> ActiveObt;
    public Vector3 CObtPos;
    public int _velocity=1;
    // Start is called before the first frame update
    void Start()
    {
        ActiveObt = new Queue<GameObject>();
        foreach (var obt in ObtScene)
        {
            ActiveObt.Enqueue(obt);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obt in ActiveObt)
        {
            obt.transform.Translate(Vector3.left*_velocity*Time.deltaTime);
        }

        if (ActiveObt.Peek().transform.localPosition.x<=-84)
        {
            ChangPos();
        }
    }

    private void ChangPos()
    {
        var obt = ActiveObt.Dequeue();
        obt.SetActive(false);
        var newobt = Instantiate(ObtPrefab, transform);
        newobt.transform.localPosition = CObtPos;
        ActiveObt.Enqueue(newobt);
    }
}
