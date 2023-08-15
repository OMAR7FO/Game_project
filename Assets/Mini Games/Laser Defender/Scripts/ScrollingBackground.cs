using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] float ScrollingSpeed = 0.5f;
    Material myMaterial;
    Vector2 offSet;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0, ScrollingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
    }
}
