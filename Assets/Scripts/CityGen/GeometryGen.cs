using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class used to generate procedural geometry.
/// </summary>
public class GeometryGen
{
    private static GeometryGen mInstance = null;
    public static GeometryGen Instance
    {
        get
        {
            if (mInstance == null) mInstance = new GeometryGen();
            return mInstance;
        }
    }

    private Mesh mCubeMesh;

    void Start()
    {
        LoadMeshData();
    }

    /// <summary>
    /// Generates a building mesh.
    /// </summary>
    /// <param name="height"> The height of the building </param>
    /// <param name="width"> The width of the building</param>
    /// <returns> Returns the generated mesh </returns>
    public Mesh GenBuilding(float height,float width)
    {
        if (!mCubeMesh) LoadMeshData();

        float halfWidth = width * 0.5f;
        List<Vector3> vertices = new List<Vector3>();
        Mesh mesh = new Mesh();

        // Copy vertices
        List<Vector3> toCopyVertices = new List<Vector3>();
        foreach(Vector3 ele in mCubeMesh.vertices)
        {
            toCopyVertices.Add(ele);
        }
        mesh.SetVertices(toCopyVertices);
        
        // Copy normals
        List<Vector3> toCopyNormals = new List<Vector3>();
        foreach (Vector3 ele in mCubeMesh.normals)
        {
            toCopyNormals.Add(ele);
        }
        mesh.SetNormals(toCopyNormals);

        // Copy triangles
        mesh.SetTriangles(mCubeMesh.triangles,0,true);
        
        // TO-DO: Copy UVs

        // Adjust vertex 
        for(int i=0;i<mesh.vertexCount;i++)
        {
            Vector3 v = mesh.vertices[i];
            if (v.y < 0.0f) v.y = 0.0f;
            if (v.y > 0.0f) v.y = height;
            if (v.x < 0.0f) v.x = -halfWidth;
            if (v.x > 0.0f) v.x = halfWidth;
            if (v.z < 0.0f) v.z = -halfWidth;
            if (v.z > 0.0f) v.z = halfWidth;
            vertices.Add(v);
        }

        mesh.SetVertices(vertices);
        mesh.RecalculateBounds();
        return mesh;
    }

    void LoadMeshData()
    {
        // Load cube vertex data
        GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mCubeMesh = tmp.GetComponent<MeshFilter>().mesh;
        GameObject.Destroy(tmp);
    }
}
