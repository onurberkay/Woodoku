using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlaceManager : MonoBehaviour
{
    private Material mat;
    Color color;

    public BlockManager blockManager;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponentInChildren<Renderer>().material;
    }

    public void Fill(BlockManager blockManager)
    {
        this.blockManager = blockManager;
        blockManager.transform.parent = null;
        blockManager.transform.DOMove(transform.position, 0.5f);
    }
    public bool Empty()
    {
        if (blockManager == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void Blast()
    {
        blockManager.Blast();
        blockManager = null;
    }
    public void SetColor(Color color)
    {
        mat.color = color;
        this.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
