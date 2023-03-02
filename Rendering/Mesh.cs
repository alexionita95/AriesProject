using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Rendering
{
    public class Mesh
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVMap { get; set; }
        public List<uint> Indices { get; set; }

        public int VertexArrayObject;
        private int VertexBufferObject;
        private int NormalBufferObject;
        private int UVBufferObject;
        public int ElementBufferObject;


        public Mesh(List<Vector3> vertices, List<Vector3> normals, List<Vector2> uVMap, List<uint> indices)
        {
            Vertices = vertices;
            Normals = normals;
            UVMap = uVMap;
            Indices = indices;
            Initialize();
            
        }

        private void Initialize()
        {
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            VertexBufferObject = InitBuffer(BufferTarget.ArrayBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * 3 * sizeof(float), Vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);
            NormalBufferObject = InitBuffer(BufferTarget.ArrayBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Normals.Count * 3 * sizeof(float), Normals.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            UVBufferObject = InitBuffer(BufferTarget.ArrayBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, UVMap.Count * 2 * sizeof(float), UVMap.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            

        }

        private int InitBuffer(BufferTarget target)
        {
            int buffer = GL.GenBuffer();
            GL.BindBuffer(target, buffer);
            return buffer;
        }

    }
}
