using System;
using OpenTK;
//using ReactiveUI;
//using ReactiveUI.Fody.Helpers;

namespace Qvrc2VistaOO
{
    // This is the camera class as it could be set up after the tutorials on the website
    // It is important to note there are a few ways you could have set up this camera, for example
    // you could have also managed the player input inside the camera class, and a lot of the properties could have
    // been made into functions.

    // TL;DR: This is just one of many ways in which we could have set up the camera
    // Check out the web version if you don't know why we are doing a specific thing or want to know more about the code
    public class Camera //: ReactiveObject
    {
        // Rotation around the X axis (radians)
        private float _pitch;
        public float Pitch
        {
            get => _pitch; set { _pitch = value;}
        }
        // Rotation around the Y axis (radians)
        private float _yaw = -MathHelper.PiOver2; // Without this you would be started rotated 90 degrees right
        public float Yaw
        {
            get => _yaw; set { _yaw = value; }
        }
        public float DepthFar = 100f;
        public float DepthNear = 0.01f;

        public Camera(Vector3 position, float initFov)
        {
            State = new CameraState()
            {
                Position = position,
                Fov = initFov,
            };
        }
        public Camera() { }

       // [Reactive]
        public CameraState State { get; set; }

        // This is simply the aspect ratio of the viewport, used for the projection matrix
        public float AspectRatio { private get; set; } 

        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(State.Position, State.Position + State.Front, State.Up);
        }
        public Matrix4 GetOricubeViewMatrix()
        {
            return Matrix4.LookAt((Vector3.UnitZ * 8), (Vector3.UnitZ * 0), State.Up);
        }
        // Get the projection matrix using the same method we have used up until this point
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(State.Fov, AspectRatio, DepthNear, DepthFar);
        }
        public Matrix4 GetOricubeProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver6, 1, DepthNear, DepthFar);
        }
        // Get the Orthogonal projection matrix using the same method we have used up until this point
        public Matrix4 GetOricubeOrthoProjectionMatrix()
        {
            return Matrix4.CreateOrthographicOffCenter(-2, 2, -2, 2, DepthNear, DepthFar);
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials
        private void UpdateVectors()
        {
            // First the front matrix is calculated using some basic trigonometry
            State.Front = new Vector3()
            {
                X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw),
                Y = (float)Math.Sin(_pitch),
                Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw),
            };

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results
            State.Front = Vector3.Normalize(State.Front);

            // Calculate both the right and the up vector using cross product
            // Note that we are calculating the right from the global up, this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera
            State.Right = Vector3.Normalize(Vector3.Cross(State.Front, Vector3.UnitY));
            State.Up = Vector3.Normalize(Vector3.Cross(State.Right, State.Front));
        }
    }

    public class CameraState// : ReactiveObject
    {
        public static CameraState InitialState => new CameraState() { Position = Vector3.UnitZ * 2, Fov = (float)(Math.Atan(0.6f)) };
        // The field of view of the camera (radians)
        private float _fov;

        // The field of view (FOV) is the vertical angle of the camera view, this has been discussed more in depth in a
        // previous tutorial, but in this tutorial you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance
        public float Fov
        {
            get => _fov;
            set => _fov = MathHelper.Clamp(value, 0.001f, MathHelper.PiOver2);  //reactive: put raised
        }

        //[Reactive]
        public Vector3 Position { get; set; }

        // Those vectors are directions pointing outwards from the camera to define how it rotated
        //[Reactive]
        public Vector3 Up { get; set; } = Vector3.UnitY;
        //[Reactive]
        public Vector3 Right { get; set; } = Vector3.UnitX;
        //[Reactive]
        public Vector3 Front { get; set; } = -Vector3.UnitZ;
    }

}