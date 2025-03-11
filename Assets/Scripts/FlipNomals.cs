using UnityEngine;
using System.Linq;

public class FlipNormals : MonoBehaviour
{
    public MeshCollider meshCollider;

    private void Awake()
    {
        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                Debug.LogError("MeshCollider not found on this GameObject or not assigned in the inspector.");
                return;
            }
        }

        // Create new mesh to invert normals

        Mesh originalMesh = meshCollider.sharedMesh;
        Mesh mesh = Instantiate(originalMesh);
        
        // Reversing each triangle
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int temp = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = temp;
        }
        mesh.triangles = triangles;
        
        // Invert normals
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        mesh.normals = normals;
        
        // Recalculate bounds and tangents after modification
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        
        // Assign the modified mesh back to the MeshCollider
        meshCollider.sharedMesh = mesh;
        
        // If there's a MeshFilter, update it too
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
    }
}

