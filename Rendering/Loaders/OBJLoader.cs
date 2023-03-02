using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rendering.Loaders
{
    struct FaceElement
    {
        public int vIndex;
        public int tIndex;
        public int nIndex;
    }
    struct Face
    {
        public List<FaceElement> Elements=new List<FaceElement>();
        public Face()
        {

        }
    }
    public class OBJLoader
    {
        List<Vector3> objVertices;
        List<Vector3> objNormals;
        List<Vector2> objTexCoords;
        List<Face> faces;

        List<Vector3> vertices;
        List<Vector3> normals;
        List<Vector2> uvMap;
        List<uint> indices;
        

        string path;
        public OBJLoader(string _path)
        {
            objVertices = new List<Vector3>();
            objNormals = new List<Vector3>();
            objTexCoords = new List<Vector2>();
            faces = new List<Face>();

            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            uvMap = new List<Vector2>();
            indices = new List<uint>();
            path = _path;
        }

        public Mesh Load()
        {
            StreamReader sr = new StreamReader(path);
            while(! sr.EndOfStream)
            {
                string line = sr.ReadLine();
                ProcessLine(line);

            }
            return BuildMesh();
        }

        private void ProcessLine(string line)
        {
            var newLine = Regex.Replace(line, @"\s+", " ");
            var tokens = newLine.Split(" ");

            if (tokens.Length == 0)
                return;

            switch (tokens[0])
            {
                case "v":
                    objVertices.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                    break;
                case "vn":
                    objNormals.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                    break;
                case "vt":
                    objTexCoords.Add(new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2])));
                    break;
                case "f":
                    faces.Add(ProcessFace(line));
                    break;
            }
        }
        private Face ProcessFace(string line)
        {
            var tokens = line.Split(" ");
            Face face = new Face();
            for(int i=1;i<tokens.Length;++i)
            {
                if (tokens[i] == "")
                    continue;
                var elementArray = tokens[i].Split("/");
                FaceElement element = new FaceElement();
                element.vIndex = int.Parse(elementArray[0]);
                element.tIndex = elementArray[1] == "" ? -1 : int.Parse(elementArray[1]);
                element.nIndex = int.Parse(elementArray[2]);
                face.Elements.Add(element);
            }
            return face;

        }

        private Mesh BuildMesh()
        {
            //vertices = objVertices;
            for (int i = 0; i < faces.Count; ++i)
            {
                if (faces[i].Elements.Count == 3)
                {
                    for (int j = 0; j < faces[i].Elements.Count; ++j)
                    {
                        FaceElement element = faces[i].Elements[j];
                       ProcessElement(element);
                    }
                }
                else
                {

                    FaceElement e0 = faces[i].Elements[0];
                    FaceElement e1 = faces[i].Elements[1];
                    FaceElement e2 = faces[i].Elements[2];
                    FaceElement e3 = faces[i].Elements[3];
                    ProcessElement(e0);
                    ProcessElement(e1);
                    ProcessElement(e3);
                    ProcessElement(e1);
                    ProcessElement(e2);
                    ProcessElement(e3);

                }
                
            }
            return new Mesh(vertices, normals, uvMap, indices);
        }
        int IndexOfElement(FaceElement e)
        {
            bool hasTexture = e.tIndex != -1;
            Vector2 uv = Vector2.Zero;
            if(hasTexture)
            {
                uv = objTexCoords[e.tIndex - 1];
            }
            Vector3 vert = objVertices[e.vIndex - 1];
            Vector3 norm = objNormals[e.nIndex - 1];
            for(int i=0;i<vertices.Count;++i)
            {
                if (vertices[i] == vert && normals[i] == norm)
                {
                    if(hasTexture)
                    {
                        if (uvMap[i] == uv)
                        {
                            return i;
                        }    
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        private void ProcessElement(FaceElement element)
        {
            int index = IndexOfElement(element);
            if(index != -1)
            {
                indices.Add((uint)index);
            }
            else
            {
                vertices.Add(objVertices[element.vIndex - 1]);
                normals.Add(objNormals[element.nIndex - 1]);
                if (element.tIndex != -1)
                {
                    uvMap.Add(objTexCoords[element.tIndex - 1]);
                }
                else
                {
                    uvMap.Add(Vector2.Zero);
                }
                indices.Add((uint)(vertices.Count-1));
            }
        }

    }
}
