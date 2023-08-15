using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    static GameObject obj;
    void Awake()
    {
            SetUpSingleton();
    }
    private void SetUpSingleton()
    {
         if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
