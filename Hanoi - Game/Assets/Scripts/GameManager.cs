using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Disc[] m_Dics = new Disc[5];
    [SerializeField] private GameObject m_Middle;

    [SerializeField] private Stack<GameObject> m_Pole1 = new Stack<GameObject>();
    [SerializeField] private Stack<GameObject> m_Pole2 = new Stack<GameObject>();
    [SerializeField] private Stack<GameObject> m_Pole3 = new Stack<GameObject>();

    private Vector3 m_Pole1X;
    private float m_Pole2X;
    private float m_Pole3X;

    private void Awake()
    {
        Pole1Load();
    }

    private void Start()
    {
        m_Dics[0].SetPosition(m_Middle.transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void Pole1Load()
    {
        GameObject pole1 = GameObject.Find("Pole0");
        GameObject pole2 = GameObject.Find("Pole1");
        GameObject pole3 = GameObject.Find("Pole2");
        m_Pole1X = pole1.transform.position;
        m_Pole2X = pole2.transform.position.x;
        m_Pole3X = pole3.transform.position.x;
        for (int i = m_Dics.Length - 1; i > -1; i--)
        {
            Vector3 pos = m_Dics[i].gameObject.transform.position;
            m_Dics[i].gameObject.transform.position = new Vector3(m_Pole1X.x, pos.y, pos.z);
            m_Pole1.Push(m_Dics[i].gameObject);
        }
    }
}