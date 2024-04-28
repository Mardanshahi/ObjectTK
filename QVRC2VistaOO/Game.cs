using MINNOVAA.ObjectTK.Buffers;
using MINNOVAA.ObjectTK.Shaders;
using MINNOVAA.ObjectTK.Textures;
using MINNOVAA.ObjectTK.Tools.Cameras;
using MINNOVAA.ObjectTK.Tools.Shapes;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Graphics;

namespace Qvrc2VistaOO
{
    public class Game : GameWindow
    {

        private Camera camera;

        float initFov = MathHelper.PiOver4;
        Vector3 initialCamPos = Vector3.UnitZ * 2;

        #region Volume Image Variables
        string volume_monire = @"..\..\..\..\media\skull-monire.raw";
        int XDIM = 512;
        int YDIM = 512;
        int ZDIM = 438;

        float XScale = 1f;
        float YScale = 1f;
        float ZScale = 0.68f;

        //public int tfTexID;
        bool isViewRotated = false;

        /// //////////////////////////////////////////////////////////////////////
        Shader DistanceShader;
        Shader RaycastShader;

        Matrix4 rotModel;

        /// ////////////////////
        Texture1D TfTexture;
        Texture3D VolTexture;
        Texture2D TargetTexture ;
        private VertexArray VolumeVAO;
        private VolumeCube _cube;
        private Renderbuffer DBuffer;
        private Framebuffer FrameBufferObject;

        bool FastRendering;
        int Nsamples;

        OpenTK.Quaternion RotationMadeByUser;

        Vector2 mouseCurPos;
        Vector2 mouseLastPos;

        int ShadingMode;
        int CompositingMode;
        float[] BackgroundColor = { 0.4f, 0.2f, 0.2f, 1.0f };
        float[] LightColor = { 0.5f, 0.5f, 1.0f }; /* no alpha here */
        float AmbientReflectance = 0.05f;
        float DiffuseReflectance = 0.3f;
        float SpecularReflectance = 0.45f;

        //////////
        int NsamplesHigh = 1000;
        int NsamplesLow = 100;
        #endregion


        void SetFastRendering(bool s)
        {
            if (s != FastRendering)
            {
                FastRendering = s;
                Nsamples = s ? NsamplesLow : NsamplesHigh;
                //update();
            }
        }

        /* target texture for the first rendering pass */
        void InitTargetTexture(int w, int h)
        {
            if (TargetTexture != null)
                TargetTexture.Dispose();
            TargetTexture = new Texture2D(SizedInternalFormat.Rgba16f, w, h);
            TargetTexture.SetWrapMode(TextureWrapMode.ClampToEdge);
            TargetTexture.SetFilter(TextureMinFilter.Linear, TextureMagFilter.Linear);
        }

        /* framebuffer object for two pass rendering */
        void InitFbo(int w, int h)
        {
            if (DBuffer != null)
                DBuffer.Dispose();
            if (FrameBufferObject != null)
                FrameBufferObject.Dispose();
            DBuffer = new Renderbuffer();
            FrameBufferObject = new Framebuffer();
            DBuffer.Init(RenderbufferStorage.DepthComponent, w, h);       /* render buffer for face culling */
            FrameBufferObject.Bind(FramebufferTarget.Framebuffer);        /* frame buffer for rendering */
            FrameBufferObject.Attach(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TargetTexture);
            FrameBufferObject.Attach(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, DBuffer);
            Framebuffer.Unbind(FramebufferTarget.Framebuffer);
        }
  
        int LoadVolumeUShort(string path, int w, int h, int d)
        {
            VolumeTexture.CreateCompatible( out VolTexture, w, h, d);
            VolTexture.LoadData(path);           
            VolTexture.SetWrapMode(TextureWrapMode.ClampToEdge);
            VolTexture.SetFilter(TextureMinFilter.Linear, TextureMagFilter.Linear);          
            return VolTexture.Handle;
        }

        int LoadTransferFunction(float[] data, int sz)
        {

            var TFLength = GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.MaxTextureSize);
            Console.WriteLine("TFLength: " + TFLength);
            float[,] pData = new float[TFLength, 4];
            var involPoints = TransferFunction.Presets
                .SelectMany(x => x.Points)
                .ToList();


            var points = involPoints.Select(x => new TfPoint() { Intensity = (int)(x.Intensity/16 + (TFLength/2)  ), Opacity = x.Opacity, JetValue = x.JetValue }).ToList();
            points.Add(new TfPoint() { Intensity = TFLength - 1, JetValue = new Vector3(0.9f, 0.9f, 0.8f), Opacity = 0.8f });

            //fill the colour values at the place where the colour should be after interpolation
            // index must be below 256

            for (int i = 0; i < points.Count; i++)
            {
                pData[points[i].Intensity, 0] = points[i].JetValue.X;
                pData[points[i].Intensity, 1] = points[i].JetValue.Y;
                pData[points[i].Intensity, 2] = points[i].JetValue.Z;
                pData[points[i].Intensity, 3] = points[i].Opacity;
            }

            //for each adjacent pair of colours, find the difference in the rgba values and then interpolate
            for (int j = 0; j < points.Count - 1; j++)
            {
                float dDataR = (pData[points[j + 1].Intensity, 0] - pData[points[j].Intensity, 0]);
                float dDataG = (pData[points[j + 1].Intensity, 1] - pData[points[j].Intensity, 1]);
                float dDataB = (pData[points[j + 1].Intensity, 2] - pData[points[j].Intensity, 2]);
                float dDataA = (pData[points[j + 1].Intensity, 3] - pData[points[j].Intensity, 3]);
                int dIndex = points[j + 1].Intensity - points[j].Intensity;
                float dDataIncR = dDataR / dIndex;
                float dDataIncG = dDataG / dIndex;
                float dDataIncB = dDataB / dIndex;
                float dDataIncA = dDataA / dIndex;
                for (int i = points[j].Intensity + 1; i < points[j + 1].Intensity; i++)
                {
                    pData[i, 0] = (pData[i - 1, 0] + dDataIncR);
                    pData[i, 1] = (pData[i - 1, 1] + dDataIncG);
                    pData[i, 2] = (pData[i - 1, 2] + dDataIncB);
                    pData[i, 3] = (pData[i - 1, 3] + dDataIncA);
                }
            }

            VolumeTexture.CreateCompatible(out TfTexture, pData.GetLength(0));
            TfTexture.LoadData(pData);
            TfTexture.SetWrapMode(TextureWrapMode.ClampToEdge);
            TfTexture.SetFilter(TextureMinFilter.Linear, TextureMagFilter.Linear);

            return TfTexture.Handle;
        }


        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Default)
        {
        }
        // This function runs on every update frame.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            const float cameraSpeed = 1.5f;

            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape))
                Exit();
   
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (input.IsKeyDown(Key.W))
            {
                camera.State.Position += camera.State.Front * cameraSpeed * (float)e.Time; // Forward
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.S))
            {
                camera.State.Position -= camera.State.Front * cameraSpeed * (float)e.Time; // Backwards
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.A))
            {
                camera.State.Position -= camera.State.Right * cameraSpeed * (float)e.Time; // Left
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.D))
            {
                camera.State.Position += camera.State.Right * cameraSpeed * (float)e.Time; // Right
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.Up))
            {
                camera.State.Position += camera.State.Up * cameraSpeed * (float)e.Time; // Up
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.Down))
            {
                camera.State.Position -= camera.State.Up * cameraSpeed * (float)e.Time; // Down
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.U))
            {
                ShadingMode = 0;// simple phong
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.I))
            {
                ShadingMode = 1;// simple phong + Edges
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.O))
            {
                ShadingMode = 2;// simple phong + Edges + Toon
                isViewRotated = true;
            }
            if (input.IsKeyDown(Key.P))
            {
                ShadingMode = 3;// None
                isViewRotated = true;
            }
            base.OnUpdateFrame(e);
        }

    
        protected override void OnLoad(EventArgs e)
        {
            #region Volume Image Init
            Nsamples = NsamplesHigh;
            CompositingMode = 0;
            ShadingMode = 1;

            SetFastRendering(false);

            /* load textures */
            _ = LoadVolumeUShort(volume_monire, XDIM, YDIM, ZDIM);
            _ = LoadTransferFunction(null, 256);

            /* I should probably get initial rotation from DICOM orientation data */
            RotationMadeByUser = Quaternion.FromEulerAngles(0, 0, 0);
            rotModel = Matrix4.CreateFromQuaternion(RotationMadeByUser);
            /* draw in [0,1] because we want local coordinates easily mapped
             * to colors, translate in [-0.5,0.5] to make camera transforms
             * easier */
            rotModel = rotModel * Matrix4.CreateTranslation(-0.5f, -0.5f, -0.5f);

            // initialize demonstration geometry
            _cube = new VolumeCube();
            _cube.UpdateBuffers();


            /* enable alpha blending */
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            /* draw background */
            GL.ClearColor(BackgroundColor[0], BackgroundColor[1],
                         BackgroundColor[2], BackgroundColor[3]);

            /* initialize first pass shader */
            /* see shaders source code for details */
            /* this just shades our cube with color mapped to local position */
            DistanceShader = new Shader();
            DistanceShader.LoadFromFile(ShaderType.VertexShader, "shaders/firstpass.vert");
            DistanceShader.LoadFromFile(ShaderType.FragmentShader, "shaders/firstpass.frag");
            DistanceShader.CreateAndLinkProgram();

            /* and this is where the volume rendering really happens */
            RaycastShader = new Shader();
            RaycastShader.LoadFromFile(ShaderType.VertexShader, "shaders/raycast.vert");
            RaycastShader.LoadFromFile(ShaderType.FragmentShader, "shaders/raycast.frag");
            RaycastShader.CreateAndLinkProgram();

            ///////////////////////////////////////
            RaycastShader.Use();
            //add attributes and uniforms
            //raycast_shader.AddAttribute("vVertex");
            RaycastShader.AddUniform("projection");
            RaycastShader.AddUniform("model");
            RaycastShader.AddUniform("view");

            RaycastShader.AddUniform("backtex");
            RaycastShader.AddUniform("voltex");
            RaycastShader.AddUniform("tftex");
            RaycastShader.AddUniform("screen_width");
            RaycastShader.AddUniform("screen_height");
            RaycastShader.AddUniform("scale");

            RaycastShader.AddUniform("nsamples");
            RaycastShader.AddUniform("compositing_mode");
            RaycastShader.AddUniform("shading_mode");
            RaycastShader.AddUniform("light_color");
            RaycastShader.AddUniform("ka");
            RaycastShader.AddUniform("kd");
            RaycastShader.AddUniform("ks");

            RaycastShader.UnUse();
            
                
            DistanceShader.Use();
            //add attributes and uniforms
            //distance_shader.AddAttribute("vVertex");
            DistanceShader.AddUniform("projection");
            DistanceShader.AddUniform("model");
            DistanceShader.AddUniform("view");

            DistanceShader.AddAttribute("aPosition1");
            var vertexLocation1 = DistanceShader.getAttributeOperator("aPosition1");
            VolumeVAO = new VertexArray();
            VolumeVAO.Bind();
            VolumeVAO.BindAttribute(vertexLocation1, _cube.VertexBuffer, 3, VertexAttribPointerType.Float, 3 * sizeof(float), 0, false);       // (_textureProgram.InPosition, _cube.VertexBuffer);
            VolumeVAO.BindElementBuffer(_cube.IndexBuffer);
            //var err0 = GL.GetError();

            DistanceShader.UnUse();

            #endregion

            camera = new Camera(initialCamPos,  initFov);

            base.OnLoad(e);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (isViewRotated)
            {
                Matrix4 camProjMat;
                Matrix4 camMatrix;


                    Vector3 va = ArcBallVector(mouseCurPos, Width, Height);
                    Vector3 vb = ArcBallVector(mouseLastPos, Width, Height);
                    var angle = Math.Acos(Vector3.Dot(va, vb));  //angle is dot product between the two vectors on the sphere,
                    var axis = Vector3.Cross(vb, va);            //axis is the normal vector 
                    RotationMadeByUser = double.IsNaN(angle) ? RotationMadeByUser : Quaternion.FromAxisAngle(axis, (float)angle) * RotationMadeByUser;
             

                /* init model matrix */
                var rotationMadeByUserMatrix = Matrix4.CreateFromQuaternion(RotationMadeByUser);
                Matrix4 initialRotationMatrixScene = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), -MathHelper.PiOver2);
                


                #region Volume Image


                /* backup current fbo as Qt might be doing something there */
                int savedfbo;
                GL.GetInteger(GetPName.FramebufferBinding, out savedfbo);
                //Console.WriteLine("saved: "+savedfbo);
                GL.Viewport(0, 0, Width, Height);

                var scaleMatrix = Matrix4.CreateScale(XScale, YScale, ZScale);
                var translationMatrix = Matrix4.CreateTranslation(-0.5f, -0.5f, -0.5f);
                rotModel = translationMatrix * scaleMatrix * initialRotationMatrixScene * rotationMadeByUserMatrix;


                /* map framebuffer object for offscreen rendering */
                GL.Enable(EnableCap.DepthTest);
                FrameBufferObject.Bind(FramebufferTarget.Framebuffer);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                /* first pass: draw a colored cube with front face culling */
                /* the colors will be the coordinates of the back face we can use
                 * as the end points for our raycasting integral */
                DistanceShader.Use();
                ///////////render_cube(DistanceShader, CullFaceMode.Front);////////////////
                camMatrix = camera.GetViewMatrix();
                camProjMat = camera.GetProjectionMatrix();

                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.CullFace);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                /* scene transform happens in the shaders with modern GL */
                GL.UniformMatrix4(DistanceShader.getUniformOperator("projection"), false, ref camProjMat);
                GL.UniformMatrix4(DistanceShader.getUniformOperator("model"), false, ref rotModel);
                GL.UniformMatrix4(DistanceShader.getUniformOperator("view"), false, ref camMatrix);

                GL.CullFace(CullFaceMode.Front);

                VolumeVAO.Bind();
                VolumeVAO.DrawElements(PrimitiveType.Triangles, 36);
                /////////////////////////////////////////////////////////////

                DistanceShader.UnUse();

                /* restore previous framebuffer, we'll render to screen now */
                //Console.WriteLine("fbo: " + FrameBufferObject.FrameBufferID);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);// FrameBufferObject.FrameBufferID);// > 0 ? FrameBufferObject.FrameBufferID : 0);
                RaycastShader.Use();

                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                int tex_loc = RaycastShader.getUniformOperator("backtex");
                GL.ActiveTexture(TextureUnit.Texture0);
                TargetTexture.Bind();
                GL.Uniform1(tex_loc, 0);

                /* volume data */
                tex_loc = RaycastShader.getUniformOperator("voltex");
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture3D, VolTexture.Handle);
                GL.Uniform1(tex_loc, 1);

                /* transfer function */
                tex_loc = RaycastShader.getUniformOperator("tftex");
                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture1D, TfTexture.Handle);
                GL.Uniform1(tex_loc, 2);

                /* viewport size, needed to get normalized texture coordinates */
                int screen_width_loc = RaycastShader.getUniformOperator("screen_width");
                int screen_height_loc = RaycastShader.getUniformOperator("screen_height");
                GL.Uniform1(screen_width_loc, (float)Width);
                GL.Uniform1(screen_height_loc, (float)Height);

                /* viewport size, needed to get normalized texture coordinates */
                int scale_loc = RaycastShader.getUniformOperator("scale");

                float[] scale = { XScale, YScale, ZScale };
                GL.Uniform3(scale_loc, 1, scale);

                /* how many samples we want in our ray integral */
                int nsamples_loc = RaycastShader.getUniformOperator("nsamples");
                GL.Uniform1(nsamples_loc, (float)Nsamples);

                /* compositing mode (front to back, mip, mida), mida doesn't really work */
                int compositing_mode_loc = RaycastShader.getUniformOperator("compositing_mode");
                GL.Uniform1(compositing_mode_loc, (int)CompositingMode);

                /* shading mode (blinn phong, toon, none) */
                int shading_mode_loc = RaycastShader.getUniformOperator("shading_mode");
                GL.Uniform1(shading_mode_loc, (int)ShadingMode);

                /* shading parameters */
                int light_color_loc = RaycastShader.getUniformOperator("light_color");
                GL.Uniform3(light_color_loc, 1, LightColor);
                int ka_loc = RaycastShader.getUniformOperator("ka");
                GL.Uniform1(ka_loc, AmbientReflectance);
                int kd_loc = RaycastShader.getUniformOperator("kd");
                GL.Uniform1(kd_loc, DiffuseReflectance);
                int ks_loc = RaycastShader.getUniformOperator("ks");
                GL.Uniform1(ks_loc, SpecularReflectance);


                /* second pass: render the cube again with backface culling, now
                 * the color data stores the starting position for our raycasting
                 * computation */
                /////  render_cube(RaycastShader, CullFaceMode.Back); ////////////////
                //camMatrix = camera.GetViewMatrix();
                //camProjMat = camera.GetProjectionMatrix();

                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.CullFace);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                /* scene transform happens in the shaders with modern GL */
                GL.UniformMatrix4(RaycastShader.getUniformOperator("projection"), false, ref camProjMat);
                GL.UniformMatrix4(RaycastShader.getUniformOperator("model"), false, ref rotModel);
                GL.UniformMatrix4(RaycastShader.getUniformOperator("view"), false, ref camMatrix);

                GL.CullFace(CullFaceMode.Back);
 
                VolumeVAO.Bind();
                VolumeVAO.DrawElements(PrimitiveType.Triangles, 36);
                ///////////////////////////////
                RaycastShader.UnUse();
                #endregion
    
                isViewRotated = false;
                Context.SwapBuffers();
            }
            base.OnRenderFrame(e);
            mouseLastPos = mouseCurPos;

        }

        protected override void OnResize(EventArgs e)
        {
            camera.AspectRatio = Width / (float)Height;
            InitTargetTexture(Width, Height);
            InitFbo(Width, Height);
            GL.Viewport(0, 0, Width, Height);
            isViewRotated = true;

            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            
            DistanceShader.Dispose();
            RaycastShader.Dispose();

            base.OnUnload(e);
        }
        
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseCurPos = new Vector2(e.X, e.Y);

            if (e.Mouse.IsButtonDown(MouseButton.Left))
                isViewRotated = true;

            base.OnMouseMove(e);
        }
        Vector3 ArcBallVector(Vector2 v, int width, int height)
        {
            width = Math.Max(width, 10);
            height = Math.Max(height, 10);
            /* normalize in [-1, 1] (view space) */
            Vector2 norm_v = (2.0f * Vector2.Divide(v, new Vector2(width, height))) - new Vector2(1.0f, 1.0f);
            /* viewport y axis is top to bottom, view is cartesian */
            Vector3 P = new Vector3(norm_v.X, -norm_v.Y, 0);
            /* return a point on a sphere centered at 0 with radius 1 equal to
             * the viewport radius */
            float Psq = P.LengthSquared;
            if (Psq <= 1)
                P.Z = (float)Math.Sqrt(1.0 - Psq);
            else
                P.Normalize();
            return P;
        }


        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            camera.State.Fov -= e.DeltaPrecise * 0.05f;
            SetFastRendering(true);
            isViewRotated = true;

            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            SetFastRendering(true);
            isViewRotated = true;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            //Q_UNUSED(event);
            SetFastRendering(false);
            isViewRotated = true;

            base.OnMouseDown(e);
        }


    }
}
