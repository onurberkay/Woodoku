using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class BlocksManager : MonoBehaviour, ISerializationCallbackReceiver
{
    
    public static Vector3 offset = new Vector3(0,2,-0.4f); 
    
    [SerializeField]
    public GameObject block;
    [SerializeField]
    public int width, height;
    [SerializeField]
    public bool[,] array;
    [SerializeField]
    private bool[] arrayTemp;
    [SerializeField]
    private int widthTemp, heightTemp;
    [SerializeField]
    private List<BlockManager> blocks;
    public PackagePlaceManager placeManager;
    public void Generate()
    {
        
        if (blocks != null)
        {
            for(int i = 0; i < blocks.Count; i++)
            {
                
                DestroyImmediate(blocks[i].gameObject,true);
            }
            blocks.Clear();
        }
        else
        {
            blocks = new List<BlockManager>();
        }
        GameObject temp;
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (array[x, y])
                {
                    
                    temp = Instantiate(block, transform.position + new Vector3(x - width / 2f+0.5f, -y + height / 2f-0.5f, 0), Quaternion.identity, transform);
                    blocks.Add(temp.GetComponent<BlockManager>());
                }
                
            }
        }

        

    }


    private void OnMouseDown()
    {
        int layerMask = LayerMask.GetMask("BG");
        

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            transform.position += 0.2f*((offset + hit.point)-transform.position);

            transform.DOScale(1, 0.25f);
        }

       
    }
    private void OnMouseDrag()
    {


        int layerMask = LayerMask.GetMask("BG");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            transform.position += 0.2f * ((offset + hit.point) - transform.position);

        }
    }
    private void OnMouseUp()
    {
        bool check = true;
        for(int i = 0; i < blocks.Count; i++)
        {
            if (!BaseManager.singleton.Check(blocks[i]))
            {
                check = false;
            }
        }
        if (check)
        {
            placeManager.blocksManager = null;
            placeManager = null;
            for (int i = 0; i < blocks.Count; i++)
            {
                BaseManager.singleton.GetPlace(blocks[i]).Fill(blocks[i]);
            }
            Destroy(gameObject);
        }
        else
        {
            transform.DOMove(placeManager.transform.position, 0.3f);
            transform.DOScale(0.33f, 0.3f);
        }


        
    }


    public void OnBeforeSerialize()
    {
        if (array == null)
        {
            widthTemp = 0;
        }
        else
        {
            widthTemp = array.GetLength(0);
            heightTemp = array.GetLength(1);
            arrayTemp = new bool[widthTemp * heightTemp];
            for (int x = 0; x < widthTemp; x++)
            {
                for (int y = 0; y < heightTemp; y++)
                {
                    arrayTemp[x + y * widthTemp] = array[x, y];

                }
            }
        }
        
    }

    public void OnAfterDeserialize()
    {
        if(widthTemp != 0)
        {
            array = new bool[widthTemp, heightTemp];
            for (int x = 0; x < widthTemp; x++)
            {
                for (int y = 0; y < heightTemp; y++)
                {
                    array[x, y] = arrayTemp[x + y * widthTemp];

                }
            }
        }
        
    }
}
