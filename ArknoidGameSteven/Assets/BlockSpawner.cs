using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public static BlockSpawner Instance;

    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private GameObject m_BlockPrefab;
    [SerializeField] private Color[] m_Colors;

    private Block[] m_Blocks;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SpawnBlocks();
    }

    private void SpawnBlocks()
    {
        float _colX = m_BlockPrefab.GetComponent<Collider2D>().bounds.size.x;
        float _colY = m_BlockPrefab.GetComponent<Collider2D>().bounds.size.y;
        Vector2 Pos = new Vector2(transform.position.x - (Width / 2f), transform.position.y - (Height / 2f));
        m_Blocks = new Block[Height * Width];
        int index = 0;
        for (int i = 0; i < Height; i++)
        {
            Vector3 offset = Vector3.zero;
            for (int j = 0; j < Width; j++)
            {
                offset = new Vector3(0f, -0.5f, 0f);

                Vector3 newpos = new Vector3(Pos.x + j + _colX, Pos.y + (i * 0.5f) + _colY, 0f);
                GameObject gameObject = Instantiate(m_BlockPrefab, newpos + offset, Quaternion.identity);

                SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
                sprite.color = m_Colors[GetRandomIndex(m_Colors.Length)];

                m_Blocks[index] = gameObject.GetComponent<Block>();
                index++;
            }
        }
    }

    public void ResetAllBlocks()
    {
        for (int i = 0; i < m_Blocks.Length; i++)
        {
            m_Blocks[i].ResetBlock();
        }
    }

    private int GetRandomIndex(int length)
    {
        return Random.Range(0, length);
    }
}