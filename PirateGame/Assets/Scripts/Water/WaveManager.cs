using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] private float m_FloatPower;

    [SerializeField] private int m_SizeX;
    [SerializeField] private int m_SizeZ;
    [SerializeField] private float m_Spacing;
    [SerializeField] private float m_HeightMultiplyer;
    [SerializeField] private float m_WaveSpeed;
    public Vector3[] Vertics;
    public int[] Triangles;
    public Vector2[] UVS;
    public Mesh Mesh;

    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;

    private float m_WaterX;
    private float m_WaterZ;

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
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
        Mesh = new Mesh();
        GenerateShape();
        StartCoroutine(MoveWater());
    }

    private IEnumerator MoveWater()
    {
        while (true)
        {
            m_WaterX += m_WaveSpeed * Time.deltaTime;
            m_WaterZ += m_WaveSpeed * Time.deltaTime;
            if (m_WaterX > 100f && m_WaterZ > 100f)
            {
                m_WaterX = 0f;
                m_WaterZ = 0f;
            }
            UpdateWaterMesh();
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void GenerateShape()
    {
        Vertics = new Vector3[(m_SizeX + 1) * (m_SizeZ + 1)];
        int _index = 0;
        for (int z = 0; z < m_SizeZ; z++)
        {
            for (int x = 0; x < m_SizeX; x++)
            {
                float _Multiply = 0.3f;
                float Y = Mathf.PerlinNoise((x + m_WaterX) * _Multiply, (z + m_WaterZ) * _Multiply) * m_HeightMultiplyer;
                Vertics[_index] = new Vector3(x * m_Spacing, Y, z * m_Spacing);
                _index++;
            }
        }

        int vert = 0;
        int tris = 0;
        Triangles = new int[(m_SizeX) * (m_SizeZ) * 6];
        for (int z = 0; z < m_SizeZ; z++)
        {
            for (int x = 0; x < m_SizeX; x++)
            {
                Triangles[tris + 0] = vert + 0;
                Triangles[tris + 1] = vert + m_SizeX + 1;
                Triangles[tris + 2] = vert + 1;
                Triangles[tris + 3] = vert + 1;
                Triangles[tris + 4] = vert + m_SizeX + 1;
                Triangles[tris + 5] = vert + m_SizeX + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        UVS = new Vector2[Vertics.Length];
        _index = 0;
        for (int z = 0; z <= m_SizeZ; z++)
        {
            for (int x = 0; x <= m_SizeX; x++)
            {
                UVS[_index] = new Vector2((float)x / (float)m_SizeX, (float)z / (float)m_SizeZ);
                _index++;
            }
        }
        Mesh.Clear();
        Mesh.vertices = Vertics;
        Mesh.triangles = Triangles;
        Mesh.uv = UVS;
        Mesh.RecalculateNormals();
        MeshFilter.mesh = Mesh;
        MeshCollider.sharedMesh = MeshFilter.mesh;
    }

    private void UpdateWaterMesh()
    {
        int _index = 0;
        for (int z = 0; z <= m_SizeZ; z++)
        {
            for (int x = 0; x <= m_SizeX; x++)
            {
                float Y = Mathf.PerlinNoise(x + m_WaterX, z + m_WaterZ) * m_HeightMultiplyer;
                Vertics[_index] = new Vector3(x * m_Spacing, Y, z * m_Spacing);
                _index++;
            }
        }
        Mesh.Clear();
        Mesh.vertices = Vertics;
        Mesh.triangles = Triangles;
        Mesh.uv = UVS;
        Mesh.RecalculateNormals();
        MeshFilter.mesh = Mesh;
        MeshCollider.sharedMesh = MeshFilter.mesh;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    for (int i = 0; i < Vertics.Length; i++)
    //    {
    //        if (Vertics[i].y > 0.5f * m_HeightMultiplyer)
    //        {
    //            Gizmos.color = Color.red;
    //        }
    //        else
    //        {
    //            Gizmos.color = Color.blue;
    //        }
    //        Gizmos.DrawSphere(Vertics[i] + transform.position, 10f);
    //    }
    //}

    public float GetFloatPower()
    {
        return m_FloatPower;
    }

    public float WaveHeight(Vector3 _pos)
    {
        
        return (Mathf.PerlinNoise(_pos.x + m_WaterX, _pos.y + m_WaterX) * m_HeightMultiplyer) + transform.position.y;
    }
}