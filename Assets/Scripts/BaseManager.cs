using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour, ISerializationCallbackReceiver

{
    [SerializeField]
    public GameObject place;
    [SerializeField]
    public int width, height;

    [SerializeField]
    public PlaceManager[,] places;
    
    [SerializeField]
    private PlaceManager[] placesTemp;
    [SerializeField]
    private int widthTemp, heightTemp;

    public static BaseManager singleton;


    private bool endCheck = true;
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
    public void Generate()
    {
        if (places != null)
        {
            for (int x = 0; x < places.GetLength(0); x++)
            {
                for (int y = 0; y < places.GetLength(1); y++)
                {
                    if (places[x, y] != null)
                    {
                        DestroyImmediate(places[x, y].gameObject);
                        places[x, y] = null;
                    }
                    
                }
            }
            
        }
        places = new PlaceManager[width, height];
        GameObject temp;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                temp = Instantiate(place, transform.position + new Vector3(0.5f +x,0.5f +y, 0), Quaternion.identity, transform);
                places[x, y] = temp.GetComponent<PlaceManager>();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        FullCheck();
        EndCheck();
    }
    public bool PlaceCheck(BlocksManager blocksManager)
    {
        bool check;
        bool once = true;
        int posx =0, posy=0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (places[x, y].Empty())
                {



                    check = true;
                    for (int i = 0; i < blocksManager.array.GetLength(0); i++)
                    {
                        for (int j = blocksManager.array.GetLength(1)-1; j >= 0; j--)
                        {
                            if (once)
                            {
                                if (blocksManager.array[i, j])
                                {
                                    posx = i;
                                    posy = j;
                                    once = false;
                                }
                            }
                            else{

                                if (blocksManager.array[i, j] && (x+i-posx<0 || x + i - posx >=width || y-j-posy<0 || y - j - posy >= height || !places[x + i - posx, y - j - posy].Empty()))
                                {
                                    check = false;
                                    j = -1;
                                    i = blocksManager.array.GetLength(0);
                                }

                            }
                        }
                    }
                    if (check)
                    {
                        return true;
                    }






                }
                
            }
        }
        return false;
    }
    public void EndCheck()
    {
        bool check = false;
        int count = 0;
        for(int i = 0; i < PackageManager.singleton.places.Length; i++)
        {
            if(PackageManager.singleton.places[i].blocksManager != null)
            {
                count++;
                if ( PlaceCheck(PackageManager.singleton.places[i].blocksManager))
                {
                    check = true;
                }
            }
            
        }
        if ( endCheck && count>0 && !check)
        {
            endCheck = false;
            UIManager.singleton.end.SetActive(true);
        }
    }

    public void FullCheck()
    {
        List<PlaceManager> fullList = new List<PlaceManager>();

        bool check = true;
        for(int x = 0; x < width; x++)
        {
            check = true;
            for (int y = 0; y < height; y++)
            {
                if (places[x, y].Empty())
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!fullList.Contains(places[x, y]))
                    {
                        fullList.Add(places[x, y]);

                    }
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            check = true;
            for (int x = 0; x < width; x++)
            {
                if (places[x, y].Empty())
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!fullList.Contains(places[x, y]))
                    {
                        fullList.Add(places[x, y]);

                    }
                }
            }
        }

        for (int x = 0; x < width/3; x++)
        {
            
            for (int y = 0; y < height/3; y++)
            {
                check = true;

                for (int i = 0; i < 3; i++)
                {

                    for (int j = 0; j < 3; j++)
                    {
                        if (places[x*3+i, y*3+j].Empty())
                        {
                            check = false;
                            break;
                        }
                    }

                }
                if (check)
                {
                    for (int i = 0; i < 3; i++)
                    {

                        for (int j = 0; j < 3; j++)
                        {
                            if (!fullList.Contains(places[x*3+i, y*3+j]))
                            {
                                fullList.Add(places[x*3+i, y*3+j]);

                            }
                        }

                    }
                }
                
            }
            
        }

        for(int i = 0; i < fullList.Count; i++)
        {
            fullList[i].Blast();
        }
    }


    public Vector3 BlockPositionToArrayPosition(BlockManager blockManager)
    {
        return new Vector3(Mathf.FloorToInt(blockManager.transform.position.x), Mathf.FloorToInt(blockManager.transform.position.y), 0);
    }

    public bool Check(BlockManager blockManager)
    {
        Vector3 pos = BlockPositionToArrayPosition(blockManager);
        if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height && places[(int)pos.x, (int)pos.y].blockManager == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public PlaceManager GetPlace(BlockManager blockManager)
    {
        Vector3 pos = BlockPositionToArrayPosition(blockManager);
        if(pos.x>=0 && pos.x<width && pos.y>=0 && pos.y<height && places[(int)pos.x,(int)pos.y].blockManager == null)
        {
            return places[(int)pos.x, (int)pos.y];
        }
        else
        {
            return null;
        }
    }
    public void OnBeforeSerialize()
    {
        widthTemp = places.GetLength(0);
        heightTemp = places.GetLength(1);
        placesTemp = new PlaceManager[widthTemp * heightTemp];
        for (int x = 0; x < widthTemp; x++)
        {
            for (int y = 0; y < heightTemp; y++)
            {
                placesTemp[x + y * widthTemp] = places[x, y];
                
            }
        }
    }

    public void OnAfterDeserialize()
    {
        places = new PlaceManager[widthTemp, heightTemp];
        for (int x = 0; x < widthTemp; x++)
        {
            for (int y = 0; y < heightTemp; y++)
            {
                places[x, y] = placesTemp[x + y * widthTemp];

            }
        }
    }
}
