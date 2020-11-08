using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Security;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEditor;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int m_SizeX;
    [SerializeField] private int m_SizeZ;
    [SerializeField] private float m_HeightMultiplyer;
    [SerializeField] [Range(1, 20)] private int m_Smoother;
    private int m_RealSizeX;
    private int m_RealSizeZ;
    public Vector3[] Vertics;
    public int[] Triangles;
    public Vector2[] UVS;
    public Mesh Mesh;

    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;

   

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            GenerateShape();
        }
        //GenerateShape();
    }


    private void GenerateShape()
    {
        MeshFilter = GetComponent<MeshFilter>();
        MeshCollider = GetComponent<MeshCollider>();
        m_RealSizeX = m_SizeX * m_Smoother;
        m_RealSizeZ = m_SizeZ * m_Smoother;

        Vector2 _seed = GetRandomSeed();
        Vertics = new Vector3[(m_RealSizeX + 1) * (m_RealSizeZ + 1)];
        int _index = 0;
        for (int z = 0; z <= m_RealSizeZ; z++)
        {
            for (int x = 0; x <= m_RealSizeX; x++)
            {
                float _Multiply = 0.3f;
                float Y = Mathf.PerlinNoise((x / (float)m_Smoother + _seed.x) * _Multiply, (z / (float)m_Smoother + _seed.y) * _Multiply) * m_HeightMultiplyer;
                Vertics[_index] = new Vector3(x / (float)m_Smoother, Y, z / (float)m_Smoother);
                _index++;

            }
        }

        int vert = 0;
        int tris = 0;
        Triangles = new int[(m_RealSizeX) * (m_RealSizeZ) * 6];
        for (int z = 0; z < m_RealSizeZ; z++)
        {
            for (int x = 0; x < m_RealSizeX; x++)
            {
                Triangles[tris + 0] = vert + 0;
                Triangles[tris + 1] = vert + m_RealSizeX + 1;
                Triangles[tris + 2] = vert + 1;
                Triangles[tris + 3] = vert + 1;
                Triangles[tris + 4] = vert + m_RealSizeX + 1;
                Triangles[tris + 5] = vert + m_RealSizeX + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        UVS = new Vector2[Vertics.Length];
        _index = 0;
        for (int z = 0; z <= m_RealSizeZ; z++)
        {
            for (int x = 0; x <= m_RealSizeX; x++)
            {
                UVS[_index] = new Vector2((float)x / (float)m_RealSizeX, (float)z / (float)m_RealSizeZ);
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

    private Vector2 GetRandomSeed()
    {
        return new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 item in Vertics)
        {
            Gizmos.DrawSphere(item, 0.01f);
        }
    }

}
