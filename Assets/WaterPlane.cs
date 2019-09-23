using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlane : MonoBehaviour
{
    public bool debugOnComputer = false;
    public int boundsX;
    public int boundsZ;
    public int resolutionX = 1;
    public int resolutionY = 1;
    public int width = 100;
    public int depth = 100;


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

    private Vector4[] Waves2Shader;

    void Start()
    {
        Mesh mesh = generateGrid();
        MeshCollider meshcol = gameObject.AddComponent<MeshCollider>();
        meshObject = new GameObject("Water");
        meshObject.name = "waterWaves";

        meshObject.AddComponent<MeshFilter>();
        meshObject.AddComponent<MeshRenderer>();
        meshObject.AddComponent<MeshCollider>();

        //update shader properties
        Waves2Shader = new Vector4[4];
        Waves2Shader[0] = new Vector4(Wave1.direction.x, Wave1.direction.y, Wave1.Steepness, Wave1.WaveLength);
        Waves2Shader[1] = new Vector4(Wave2.direction.x, Wave2.direction.y, Wave2.Steepness, Wave2.WaveLength);
        Waves2Shader[2] = new Vector4(Wave3.direction.x, Wave3.direction.y, Wave3.Steepness, Wave3.WaveLength);
        Waves2Shader[3] = new Vector4(Wave4.direction.x, Wave4.direction.y, Wave4.Steepness, Wave4.WaveLength);

        material.SetVector("_Wave1", Waves2Shader[0]);
        material.SetVector("_Wave2", Waves2Shader[1]);
        material.SetVector("_Wave3", Waves2Shader[2]);
        material.SetVector("_Wave4", Waves2Shader[3]);



        meshObject.GetComponent<Renderer>().material = material;

        

        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        _mesh = mesh;


        
        meshcol.sharedMesh = mesh;
        meshObject.transform.position = this.transform.position;

    }


    Mesh generateGrid()
    {
        float scaleX = boundsX / resolutionX;
        float scaleZ = boundsZ / resolutionY;
        float u = 1.0f / resolutionX;
        float v = 1.0f / resolutionY;

        int sizeX = resolutionX + 1;
        int sizeY = resolutionY + 1;

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

    void updateVerts()
    {

        float scaleX = boundsX / resolutionX;
        float scaleZ = boundsZ / resolutionY;
        float u = 1.0f / resolutionX;
        float v = 1.0f / resolutionY;

        int sizeX = resolutionX + 1;
        int sizeY = resolutionY + 1;

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


    Vector3 gertsnerOffset(Vector3 p)
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

  /*
  * HLSL code... delete comment whenever
  * 
    half3 p = v.vertex;

  //Gerstner Wave offset
  float k = 2 * UNITY_PI / _Wavelength;
  float c = sqrt(9.8 / k);
  float2 d = normalize(_Direction);
  float f = k * (dot(d, p.xz) - c * _Time.y);
  float a = _Steepness / k;

  p.x += d.x * ((_Steepness / k) * cos(f));
  p.y += (_Steepness / k) * sin(f);
  p.z += d.y * ((_Steepness / k) * cos(f));

      */



    //Displays the 100 first vertices and normals;
  void OnDrawGizmos()
  {
    
    for (int i = 0; i<100; i++)
    {
      var vert = _mesh.vertices[i];
      var normal = _mesh.normals[i];
      Gizmos.DrawSphere(vert, 0.1f);
      Gizmos.DrawLine(vert, vert + normal);
    }
  }
    void Update()

    {

        //updateVerts();

        Steepness = micObject.GetComponent<MicrophoneInput>().waves;


      if (debugOnComputer)
        {
            Steepness = 0.6f;
            Mesh smesh = meshObject.GetComponent<MeshFilter>().sharedMesh;
            Vector3[] verts = smesh.vertices;
            Debug.Log(verts[20].y);
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            meshObject.GetComponent<Renderer>().material.SetVector("_Direction", new Vector4(-1f, 0f, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            meshObject.GetComponent<Renderer>().material.SetVector("_Direction", new Vector4(0f, 1f, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            meshObject.GetComponent<Renderer>().material.SetVector("_Direction", new Vector4(1f, 0f, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            meshObject.GetComponent<Renderer>().material.SetVector("_Direction", new Vector4(0f, -1f, 0, 0));
        }
    }


    [System.Serializable]
    public struct WaveProperty
    {
        public Vector2 direction;
        public float Steepness;
        public float WaveLength;
    }
    public WaveProperty Wave1;
    public WaveProperty Wave2;
    public WaveProperty Wave3;
    public WaveProperty Wave4;

}
