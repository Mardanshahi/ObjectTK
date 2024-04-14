using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.MacOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qvrc2VistaOO
{
    public class Shader
    {
        private enum ShaderTypee { VertexShader, FragmentShader, GeometryShader };
        private int _program;
        private readonly Dictionary<string, int> _attributeList = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _uniformLocationList = new Dictionary<string, int>();
        int _totalShaders;
        int[] _shaders = new int[3];//0->vertexshader, 1->fragmentshader, 2->geometryshader

        public Shader()
        {
            _totalShaders = 0;
            _shaders[(int)ShaderTypee.VertexShader] = 0;
            _shaders[(int)ShaderTypee.FragmentShader] = 0;
            _shaders[(int)ShaderTypee.GeometryShader] = 0;
            _attributeList.Clear();
            _uniformLocationList.Clear();
        }

        public void LoadFromString(ShaderType type, string source) {
            var shaderID = GL.CreateShader(type);
            string vsName = $"qvrccc- Shader {_totalShaders}";
            GL.ObjectLabel(ObjectLabelIdentifier.Shader, shaderID, vsName.Length, vsName);

            GL.ShaderSource(shaderID, source);

            CompileShader(shaderID);
            _shaders[_totalShaders++] = shaderID;
         }
        public void CreateAndLinkProgram()
        {
            _program = GL.CreateProgram();
            if (_shaders[(int)ShaderTypee.VertexShader] != 0)
            {
                GL.AttachShader(_program, _shaders[(int)ShaderTypee.VertexShader]);
            }
            if (_shaders[(int)ShaderTypee.FragmentShader] != 0)
            {
                GL.AttachShader(_program, _shaders[(int)ShaderTypee.FragmentShader]);
            }
            if (_shaders[(int)ShaderTypee.GeometryShader] != 0)
            {
                GL.AttachShader(_program, _shaders[(int)ShaderType.GeometryShader]);
            }

            GL.LinkProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_program);
                Console.WriteLine(infoLog);
            }

            GL.DeleteShader(_shaders[(int)ShaderTypee.VertexShader]);
            GL.DeleteShader(_shaders[(int)ShaderTypee.FragmentShader]);
            GL.DeleteShader(_shaders[(int)ShaderTypee.GeometryShader]);


        }

        //public int GetAttribLocation(string attribName)
        //{
        //    return GL.GetAttribLocation(_program, attribName);
        //}

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine(infoLog);
            }
        }

        public void Use()
        {
            GL.UseProgram(_program);
        }
        public void UnUse()
        {
            GL.UseProgram(0);
        }
        
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(_program);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            //GL.DeleteProgram(_program);
            _attributeList.Clear();
            _uniformLocationList.Clear();
        }

        public void DeleteShaderProgram()
        {
            GL.DeleteProgram(_program);
        }
        public void Dispose()
        {
            Dispose(true);
            //GL.DeleteProgram(_program);
            GC.SuppressFinalize(this);
        }

        public void AddAttribute(string attribute)
        {
            _attributeList[attribute] = GL.GetAttribLocation(_program, attribute);
        }
        public int getAttributeOperator(string attribute)
        {
            return _attributeList[attribute];
        }
        public void AddUniform(string uniform)
        {
            _uniformLocationList[uniform] = GL.GetUniformLocation(_program, uniform);
        }
        public int getUniformOperator(string uniform)
        {
            return _uniformLocationList[uniform];
        }
        public void LoadFromFile(ShaderType whichShader, string sourceFile)
        {
            string ptmp = File.ReadAllText(sourceFile);

            LoadFromString(whichShader, ptmp);

        }

    }
}
