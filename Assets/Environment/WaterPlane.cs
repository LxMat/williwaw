using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

[System.Obsolete]
public class WaterPlane : NetworkBehaviour
{
    public bool debugOnComputer = false;
    public int boundsX = 100;
    public int boundsZ = 100;
    public int resolutionX = 100;
    public int resolutionZ = 100;

    public List<Vector4> Waves;

    [Range(0f, 0.9f)]
    public float Steepness;
    public float WaveLength = 10;

    public Material material;
    private GameObject meshObject;
    public GameObject micObject;
    public GameObject gyroObject;
    private Canvas canvas;
    public Vector2 WaveDirection; 
    private Mesh _mesh;


    private float scaleX;
    private float scaleZ;

    private void Start()
    {
        Debug.Log(material.name);
        scaleX = boundsX / resolutionX;
        scaleZ = boundsZ / resolutionZ;

        Mesh mesh = generateGrid();
        meshObject = new GameObject("Water");
        meshObject.name = "waterWaves";

        meshObject.AddComponent<MeshFilter>();
        meshObject.AddComponent<MeshRenderer>();
        meshObject.AddComponent<MeshCollider>();

        meshObject.GetComponent<Renderer>().material = material;
        

        for(int i = 1; i<= 5; i++)
        {
            Waves.Add(material.GetVector("_Wave" + i));
        }


        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        _mesh = mesh;


        meshObject.transform.position = this.transform.position;
    }

    private Mesh generateGrid()
    {
        float scaleX = boundsX / resolutionX;
        float scaleZ = boundsZ / resolutionZ;
        float u = 1.0f / resolutionX;
        float v = 1.0f / resolutionZ;

        int sizeX = resolutionX + 1;
        int sizeY = resolutionZ + 1;

        Vector3[] verts = new Vector3[sizeX * sizeY];
        Vector3[] normals = new Vector3[sizeX * sizeY];
        Vector4[] tangents = new Vector4[sizeX * sizeY];
        Vector2[] texcoords = new Vector2[sizeX * sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            float posX = x * scaleX;
            for (int z = 0; z < sizeY; z++)
            {
                normals[x + z * sizeX] = new Vector3(0, 1, 0);
                tangents[x + z * sizeX] = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                Vector3 vertexPos = new Vector3(posX, 0.0f, z * scaleZ);
                verts[x + z * sizeX] = vertexPos;
                texcoords[x + z * sizeX] = new Vector2(x * u, z * v);
            }
        }

        int[] indices = new int[sizeX * sizeY * 6];

        int num = 0;
        for (int x = 0; x < sizeX - 1; x++)
        {
            for (int y = 0; y < sizeY - 1; y++)
            {
                indices[num + 0] = x + y * sizeX;
                indices[num + 1] = x + (y + 1) * sizeX;
                indices[num + 2] = (x + 1) + y * sizeX;
                indices[num + 3] = x + (y + 1) * sizeX;
                indices[num + 4] = (x + 1) + (y + 1) * sizeX;
                indices[num + 5] = (x + 1) + y * sizeX;
                num += 6;
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = verts;
        mesh.uv = texcoords;
        mesh.normals = normals;
        mesh.triangles = indices;
        mesh.tangents = tangents;

        
        return mesh;

    }
    public float getHeight(Vector3 pos)
    {
        float ypos = 0f;
        Vector4 time = Shader.GetGlobalVector("_Time");
        Vector2 posXZ = new Vector3(pos.x, pos.z);

        foreach (Vector4 wave in Waves){
            float k = 2 * Mathf.PI / wave.w;
            float c = Mathf.Sqrt(9.8f / k);
            Vector2 d = new Vector2(wave.x,wave.y).normalized;
            float a = wave.z / k;
            float f = k * (Vector2.Dot(d, posXZ) - c * time.y);
            ypos += a * Mathf.Sin(f);

            //Vector3 newPos = new Vector3(pos.x, 0, pos.z);
            //newPos += new Vector3(d.x * (a * Mathf.Cos(f)),
            //                    a * Mathf.Sin(f),
            //        d.y * (a * Mathf.Cos(f))
            //                    );

        }
        return ypos;
    }
    


    private void updateVerts()
    {

        float scaleX = boundsX / resolutionX;
        float scaleZ = boundsZ / resolutionZ;
        float u = 1.0f / resolutionX;
        float v = 1.0f / resolutionZ;

        int sizeX = resolutionX + 1;
        int sizeY = resolutionZ + 1;

        Vector3[] verts = new Vector3[sizeX * sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            float posX = x * scaleX;
            for (int z = 0; z < sizeY; z++)
            {
                Vector3 vertexPos = gertsnerOffset(new Vector3(posX, 0, z * scaleZ));
                verts[x + z * sizeX] = vertexPos;
            }
        }
        meshObject.GetComponent<MeshFilter>().mesh.vertices = verts;
        meshObject.GetComponent<MeshCollider>().sharedMesh = _mesh = meshObject.GetComponent<MeshFilter>().mesh;
        meshObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();

    }

    private Vector3 gertsnerOffset(Vector3 p)
    {


        float wavel = 10;
        //error handling
        if (WaveLength > 0)
        {
            wavel = WaveLength;
        }

        float steepness = Steepness;

        Vector2 dir = WaveDirection;
        float k = 2 * Mathf.PI / wavel;
        float c = Mathf.Sqrt(9.8f / k);
        Vector2 d = dir.normalized;
        float a = steepness / k;
        float f = k * (Vector2.Dot(d, new Vector2(p.x, p.z)) - c * Time.time);

        p.x += d.x * ((steepness / k) * Mathf.Cos(f));
        p.y += (steepness / k) * Mathf.Sin(f);
        p.z += d.y * ((steepness / k) * Mathf.Cos(f));
        return p;
    }




    //Displays the 100 first vertices and normals;
    private void OnDrawGizmos()
    {
        //run only when app is running
        if (!Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < 100; i++)
        {
            var vert = _mesh.vertices[i];
            var normal = _mesh.normals[i];
            Gizmos.DrawSphere(vert, 0.1f);
            Gizmos.DrawLine(vert, vert + normal);
        }
    }

    private void Update()

    {

        //updateVerts();

        //Steepness = micObject.GetComponent<MicrophoneInput>().waves;

        if (debugOnComputer)
        {
            Steepness = 0.6f;
            Mesh smesh = meshObject.GetComponent<MeshFilter>().sharedMesh;
            Vector3[] verts = smesh.vertices;
            // Debug.Log(verts[20].y);
        }
    }

}
