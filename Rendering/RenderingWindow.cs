using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Rendering
{
    public class RenderingWindow : GameWindow
    {
        Shader shader;
        Texture texture;
        Texture specular;
        Camera camera;
        int VertexBufferObject;
        int NormalBufferObject;
        int uvBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        uint[] indices;

        Mesh mesh;
        double time = 0;
        private bool firstMove = true;

        private Vector2 lastPos;

        Color4 toyColor = new Color4(1.0f, 0.5f, 0.31f, 1.0f);


        public RenderingWindow(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {

            camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f), Size.X / (float)Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);


            shader = new Shader(@"Resources\Shaders\default.vert", @"Resources\Shaders\material.frag");
            shader.Use();


            List<Vector3> vertices = new List<Vector3>() {
    // Positions    
new Vector3(-0.5f, -0.5f, -0.5f),
new Vector3( 0.5f, -0.5f, -0.5f),
new Vector3( 0.5f,  0.5f, -0.5f),
new Vector3( 0.5f,  0.5f, -0.5f),
new Vector3(-0.5f,  0.5f, -0.5f),
new Vector3(-0.5f, -0.5f, -0.5f),

new Vector3(-0.5f, -0.5f,  0.5f),
new Vector3( 0.5f, -0.5f,  0.5f),
new Vector3( 0.5f,  0.5f,  0.5f),
new Vector3( 0.5f,  0.5f,  0.5f),
new Vector3(-0.5f,  0.5f,  0.5f),
new Vector3(-0.5f, -0.5f,  0.5f),

new Vector3(-0.5f,  0.5f,  0.5f),
new Vector3(-0.5f,  0.5f, -0.5f),
new Vector3(-0.5f, -0.5f, -0.5f),
new Vector3(-0.5f, -0.5f, -0.5f),
new Vector3(-0.5f, -0.5f,  0.5f),
new Vector3(-0.5f,  0.5f,  0.5f),

new Vector3( 0.5f,  0.5f,  0.5f),
new Vector3( 0.5f,  0.5f, -0.5f),
new Vector3( 0.5f, -0.5f, -0.5f),
new Vector3( 0.5f, -0.5f, -0.5f),
new Vector3( 0.5f, -0.5f,  0.5f),
new Vector3( 0.5f,  0.5f,  0.5f),

new Vector3(-0.5f, -0.5f, -0.5f),
new Vector3( 0.5f, -0.5f, -0.5f),
new Vector3( 0.5f, -0.5f,  0.5f),
new Vector3( 0.5f, -0.5f,  0.5f),
new Vector3(-0.5f, -0.5f,  0.5f),
new Vector3(-0.5f, -0.5f, -0.5f),

new Vector3(-0.5f,  0.5f, -0.5f),
new Vector3( 0.5f,  0.5f, -0.5f),
new Vector3( 0.5f,  0.5f,  0.5f),
new Vector3( 0.5f,  0.5f,  0.5f),
new Vector3(-0.5f,  0.5f,  0.5f),
new Vector3(-0.5f,  0.5f, -0.5f)
        };

            List<Vector3> normals = new List<Vector3>{	
//     Normals        
 new Vector3(0.0f,  0.0f, -1.0f),
 new Vector3(0.0f,  0.0f, -1.0f),
 new Vector3(0.0f,  0.0f, -1.0f),
 new Vector3(0.0f,  0.0f, -1.0f),
 new Vector3(0.0f,  0.0f, -1.0f),
 new Vector3(0.0f,  0.0f, -1.0f),

 new Vector3(0.0f,  0.0f,  1.0f),
 new Vector3(0.0f,  0.0f,  1.0f),
 new Vector3(0.0f,  0.0f,  1.0f),
 new Vector3(0.0f,  0.0f,  1.0f),
 new Vector3(0.0f,  0.0f,  1.0f),
 new Vector3(0.0f,  0.0f,  1.0f),

-new Vector3(1.0f,  0.0f,  0.0f),
-new Vector3(1.0f,  0.0f,  0.0f),
-new Vector3(1.0f,  0.0f,  0.0f),
-new Vector3(1.0f,  0.0f,  0.0f),
-new Vector3(1.0f,  0.0f,  0.0f),
-new Vector3(1.0f,  0.0f,  0.0f),

 new Vector3(1.0f,  0.0f,  0.0f),
 new Vector3(1.0f,  0.0f,  0.0f),
 new Vector3(1.0f,  0.0f,  0.0f),
 new Vector3(1.0f,  0.0f,  0.0f),
 new Vector3(1.0f,  0.0f,  0.0f),
 new Vector3(1.0f,  0.0f,  0.0f),

 new Vector3(0.0f, -1.0f,  0.0f),
 new Vector3(0.0f, -1.0f,  0.0f),
 new Vector3(0.0f, -1.0f,  0.0f),
 new Vector3(0.0f, -1.0f,  0.0f),
 new Vector3(0.0f, -1.0f,  0.0f),
 new Vector3(0.0f, -1.0f,  0.0f),

 new Vector3(0.0f,  1.0f,  0.0f),
 new Vector3(0.0f,  1.0f,  0.0f),
 new Vector3(0.0f,  1.0f,  0.0f),
 new Vector3(0.0f,  1.0f,  0.0f),
 new Vector3(0.0f,  1.0f,  0.0f),
 new Vector3(0.0f,  1.0f,  0.0f)
 };

            List<Vector2> uvMap = new List<Vector2>() {
//      Texture coords
  new Vector2(0.0f, 0.0f),
  new Vector2(1.0f, 0.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(0.0f, 1.0f),
  new Vector2(0.0f, 0.0f),

  new Vector2(0.0f, 0.0f),
  new Vector2(1.0f, 0.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(0.0f, 1.0f),
  new Vector2(0.0f, 0.0f),

  new Vector2(1.0f, 0.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(0.0f, 1.0f),
  new Vector2(0.0f, 1.0f),
  new Vector2(0.0f, 0.0f),
  new Vector2(1.0f, 0.0f),

  new Vector2(1.0f, 0.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(0.0f, 1.0f),
  new Vector2(0.0f, 1.0f),
  new Vector2(0.0f, 0.0f),
  new Vector2(1.0f, 0.0f),

  new Vector2(0.0f, 1.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(1.0f, 0.0f),
  new Vector2(1.0f, 0.0f),
  new Vector2(0.0f, 0.0f),
  new Vector2(0.0f, 1.0f),

  new Vector2(0.0f, 1.0f),
  new Vector2(1.0f, 1.0f),
  new Vector2(1.0f, 0.0f),
  new Vector2(1.0f, 0.0f),
  new Vector2(0.0f, 0.0f),
  new Vector2(0.0f, 1.0f)
};
            mesh = new Mesh(vertices,normals,uvMap,new List<uint>());
            texture = Texture.LoadFromFile(@"Resources\Textures\Character.png");
            specular = Texture.LoadFromFile(@"Resources\Textures\Character_specular.png");

            //Code goes here
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            camera.AspectRatio = Size.X / (float)Size.Y;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.BindVertexArray(mesh.VertexArrayObject);
            texture.Use();
            specular.Use(TextureUnit.Texture1);
            shader.Use();

            Matrix4 model = Matrix4.Identity;//Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time));
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();
            //shader.SetVector3("objectColor", new Vector3(toyColor.R, toyColor.G, toyColor.B));

            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular",1);
            shader.SetFloat("material.shininess", 32.0f);

            shader.SetVector3("light.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            shader.SetVector3("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f)); // darken the light a bit to fit the scene
            shader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetVector3("light.position", new Vector3(0f, 2f, 10f));


            shader.SetVector3("viewPos", camera.Position);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            model = Matrix4.CreateScale(new Vector3(0.5f,0.5f,0.5f))*Matrix4.CreateTranslation(new Vector3(0f, 0f, 1.5f));
            shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            Context.SwapBuffers();

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;
            base.OnUpdateFrame(e);

            if (!IsFocused) 
            {
                return;
            }

            KeyboardState input = KeyboardState;


            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= Vector3.Normalize(Vector3.Cross(camera.Front, camera.Up)) * cameraSpeed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += Vector3.Normalize(Vector3.Cross(camera.Front, camera.Up)) * cameraSpeed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)e.Time; //camera.Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)e.Time; //Down
            }


            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }


            var mouse = MouseState;
            if (firstMove) 
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
  
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity; 
            }

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.Fov -= e.OffsetY;
        }
    }
}
