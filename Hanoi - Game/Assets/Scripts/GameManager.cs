using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Disc[] m_Dics = new Disc[5];
    [SerializeField] public GameObject m_Middle;
    public Spot spot;

    public GameObject m_Pole0;
    public GameObject m_Pole1;
    public GameObject m_Pole2;

    private void Awake()
    {
        PoleLoader();
        ValueGiver();
        Pole1Load();
    }

    private void Start()
    {
        spot = FindObjectOfType<Spot>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void Pole1Load()
    {
        Pole pole0 = m_Pole0.GetComponent<Pole>();
        for (int i = m_Dics.Length - 1; i > -1; i--)
        {
            pole0.m_Dics.Push(m_Dics[i].gameObject);
            Vector3 pos = m_Dics[i].gameObject.transform.position;
            m_Dics[i].gameObject.transform.position = new Vector3(m_Pole0.gameObject.transform.position.x, pos.y, pos.z);
        }
        print(pole0.m_Dics.Count);
    }

    private void PoleLoader()
    {
        m_Pole0 = GameObject.Find("Pole0");
        m_Pole1 = GameObject.Find("Pole1");
        m_Pole2 = GameObject.Find("Pole2");
    }

    private void ValueGiver()
    {
        for (int i = 0; i < m_Dics.Length; i++)
        {
            Disc disc = m_Dics[i].GetComponent<Disc>();
            disc.Num = i;
        }
    }
}