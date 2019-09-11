using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlane : MonoBehaviour
{

  public bool createGrid = false;
  public int boundsX;
  public int boundsZ;
  public int resolutionX = 1;
  public int resolutionY = 1;
  public int width = 100;
  public int depth = 100;

  public Material material;
  private GameObject meshObject;
  void Start()
  {
    Mesh mesh = generateGrid();


    meshObject = new GameObject("Water");
    meshObject.name = "waterWaves";
    meshObject.AddComponent<MeshFilter>();
    meshObject.AddComponent<MeshRenderer>();
    meshObject.AddComponent<MeshCollider>();
    meshObject.GetComponent<Renderer>().material = material;
    meshObject.GetComponent<MeshFilter>().mesh = mesh;

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


  void Update()

  {
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
}
