using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace MINNOVAA.ObjectTK.Textures
{
    /// <summary>
    /// Contains extension methods for texture types.
    /// </summary>
    public static class VolumeTexture
    {
        private static void CheckError()
        {
            GL.Finish();
            Utility.Assert("Error while uploading data to texture.");
        }

        public static void CreateCompatible(out Texture3D texture, int width, int height, int depth, int levels = 0)
        {
            texture = new Texture3D(SizedInternalFormat.R16, width, height, depth, levels);
        }
        public static void CreateCompatible(out Texture1D texture, int width, int levels = 0)
        {
            texture = new Texture1D(SizedInternalFormat.Rgba16f, width, levels);
        }

        /// <summary>
        /// Uploads the contents of a bitmap to the given texture level.<br/>
        /// Will result in an OpenGL error if the given bitmap is incompatible with the textures storage.
        /// </summary>
        public static void LoadData(this Texture1D texture, float[,] pData, int level = 0)
        {
            texture.Bind();
            try
            {
                GL.TexSubImage1D(texture.TextureTarget, level, 0, texture.Width,
                    PixelFormat.Rgba, PixelType.Float, pData);
            }
            finally
            {
            }
            CheckError();
        }
        /// <summary>
        /// Uploads the contents of a bitmap to the given texture level.<br/>
        /// Will result in an OpenGL error if the given bitmap is incompatible with the textures storage.
        /// </summary>
        public static void LoadData(this Texture3D texture, string path, int level = 0)
        {
            texture.Bind();

            var byteArray = File.ReadAllBytes(path);
            GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr pData = pinnedArray.AddrOfPinnedObject();
            try
            {
                GL.TexSubImage3D(texture.TextureTarget, level, 0, 0, 0, texture.Width, texture.Height, texture.Depth,
                    PixelFormat.Red, PixelType.UnsignedShort, pData);
            }
            finally
            {
                pinnedArray.Free(); 
            }
            CheckError();
        }

        /// <summary>
        /// Retrieves the texture data.
        /// </summary>
        public static T[,,] GetContent<T>(this Texture3D texture, PixelFormat pixelFormat, PixelType pixelType, int level = 0)
            where T : struct
        {
            var data = new T[texture.Width, texture.Height, texture.Depth];
            texture.Bind();
            GL.GetTexImage(texture.TextureTarget, level, pixelFormat, pixelType, data);
            return data;
        }
        /// <summary>
        /// Retrieves the texture data.
        /// </summary>
        public static T[] GetContent<T>(this Texture1D texture, PixelFormat pixelFormat, PixelType pixelType, int level = 0)
            where T : struct
        {
            var data = new T[texture.Width];
            texture.Bind();
            GL.GetTexImage(texture.TextureTarget, level, pixelFormat, pixelType, data);
            return data;
        }
    }
}