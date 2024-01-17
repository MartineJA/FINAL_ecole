using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cliquersurleboss : MonoBehaviour
{

    
    private Material m_Material;
    [SerializeField] private GameObject[] m_pattern;
    int m_Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponentInChildren<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject);
        
        m_Material.color = Color.red;
        Destroy(m_pattern[m_Count]);
        m_Count++;
        
        
        if (m_pattern.Length == m_Count)
        {
            Destroy(gameObject);
        }

        Debug.Log("pattern length   " + m_pattern.Length);
        Debug.Log("pattern    " + m_pattern); 
        Debug.Log("count   " + m_Count);
    }

    private void OnTriggerExit(Collider other)
    {
        m_Material.color = Color.black;
    }
}
