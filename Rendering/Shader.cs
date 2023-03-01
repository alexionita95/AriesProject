using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Rendering
{
    public struct Material
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float Shininess { get; set; }
    }

    public struct Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
    }
    public class Shader
    {
        int handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Debug.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Debug.WriteLine(infoLog);
            }

            handle = GL.CreateProgram();

            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);

            GL.LinkProgram(handle);

            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(handle);
                Debug.WriteLine(infoLog);
            }


            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);


        }

        public void Use()
        {
            GL.UseProgram(handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(handle, attribName);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(handle, name);

            GL.Uniform1(location, value);
        }
        public void SetFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(handle, name);

            GL.Uniform1(location, value);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(handle, name);

            GL.UniformMatrix4(location, true, ref matrix);
        }
        public void SetVector3(string name, Vector3 vector)
        {
            int location = GL.GetUniformLocation(handle, name);

            GL.Uniform3(location, vector);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
