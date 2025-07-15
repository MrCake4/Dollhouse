using UnityEngine;
using UnityEditor;
using System.Linq;

public class TriangleCountScanner : EditorWindow
{
    [MenuItem("Tools/Triangle Count Scanner")]
    public static void ShowWindow()
    {
        GetWindow<TriangleCountScanner>("Triangle Count Scanner");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Scan Scene"))
        {
            int totalTriangles = 0;

            var meshFilters = FindObjectsOfType<MeshFilter>();
            var sorted = meshFilters.Select(mf =>
            {
                var mesh = mf.sharedMesh;
                int tris = (mesh != null) ? mesh.triangles.Length / 3 : 0;
                return new { mf.name, tris, obj = mf.gameObject };
            })
            .OrderByDescending(x => x.tris)
            .ToList();

            foreach (var entry in sorted.Take(10))
            {
                Debug.Log($"{entry.name} has {entry.tris} triangles", entry.obj);
                totalTriangles += entry.tris;
            }

            Debug.Log($"Total scene triangle count (visible MeshFilters): {totalTriangles}");
        }
    }
}