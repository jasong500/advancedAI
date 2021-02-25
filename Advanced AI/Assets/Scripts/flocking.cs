using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class flocking : MonoBehaviour
{
    public int mNumberOfEntities = 200;
    public TextMeshProUGUI hitCountT;
    public GameObject prefab = null;

    public float separationWeight = 0.8f;
    public float alignmentWeight = 0.5f;
    public float cohesionWeight = 0.7f;

    public UISlidersWidget sliderWidget = null;

    public static flocking instance = null;

    List<GameObject> mTheFlock = new List<GameObject>();
    bool needNewPos = false;

    void Start()
    {
        instance = this;

        sliderWidget.Setup();

        InstantiateFlock();
    }

    private void Update()
    {
        updateHitCount();

        for(int i = 0; i < mTheFlock.Count; i++)
        {
            if (mTheFlock[i].GetComponent<raycastChecking>().reachedDestination && !needNewPos)
            {
                needNewPos = true;
                break;
            }
        }
    }

    public void flockToRandom()
    {
        if (needNewPos)
        {
            needNewPos = false;
            Vector3 newPos = mTheFlock[0].GetComponent<raycastChecking>().randomPos();

            for (int i = 0; i < mTheFlock.Count; i++)
            {
                mTheFlock[i].GetComponent<raycastChecking>().setDestination(newPos);
            }
        }
    }

    public void updateHitCount()
    {
        int hitCount = 0;

        for (int i = 0; i < theFlock.Count; i++)
        {
            hitCount += theFlock[i].GetComponent<raycastChecking>().hitCount;
            hitCountT.text = hitCount.ToString();
        }
    }

    private void InstantiateFlock()
    {
        for (int i = 0; i < mNumberOfEntities; i++)
        {
            GameObject flockEntity = Instantiate(prefab);

            flockEntity.transform.rotation = Random.rotation;

            flockEntity.GetComponent<raycastChecking>().SetID(i);

            mTheFlock.Add(flockEntity);
        }
    }

    public List<GameObject> theFlock
    {
        get { return mTheFlock; }
    }
}
