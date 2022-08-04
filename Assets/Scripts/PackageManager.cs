using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    public GameObject[] blocks;
    public PackagePlaceManager[] places;
    public static PackageManager singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FillPlaces();
    }

    public bool Empty()
    {
        bool check = true;
        for(int i = 0; i < places.Length; i++)
        {
            if (!places[i].Empty())
            {
                check = false;
            }
        }
        return check;
    }
    public void FillPlaces()
    {
        GameObject temp;
        for(int i = 0; i < places.Length; i++)
        {
            if (places[i].Empty())
            {
                temp = Instantiate(blocks[Random.Range(0, blocks.Length)], places[i].transform.position, Quaternion.identity, null);
                places[i].Fill(temp.GetComponent<BlocksManager>());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Empty())
        {
            FillPlaces();
        }
    }
}
