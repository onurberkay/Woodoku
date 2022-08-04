using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagePlaceManager : MonoBehaviour
{
    public BlocksManager blocksManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool Empty()
    {
        if (blocksManager == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Fill(BlocksManager blocksManager)
    {
        this.blocksManager = blocksManager;
        blocksManager.placeManager = this;
        blocksManager.transform.localScale = Vector3.one * 0.33f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
