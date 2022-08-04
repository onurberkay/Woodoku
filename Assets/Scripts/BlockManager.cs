using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlockManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Blast()
    {
        StartCoroutine(BlastStream());
    }
    IEnumerator BlastStream()
    {
        transform.DORotate(new Vector3(720, 0, 0), 1f, RotateMode.WorldAxisAdd);
        transform.DOMoveZ(transform.position.z - 1, 1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
