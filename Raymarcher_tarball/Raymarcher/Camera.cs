using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Raymarcher
{
    public class Camera
    {
        public Vector3 origin;
        private Vector3 forward;
        private Vector3 up;
        private Vector3 right;
        private Vector3 screenOrigin;
        private float stepY;
        private float stepX;
        private float sizeX;
        private float sizeY;
        private float stepMin;

        public Camera()
        {
            origin = new Vector3(0f, 0f, 0f);
            forward = new Vector3(1f, 0f, 0f);
            up = new Vector3(0f, 1f, 0f);
            right = Vector3.Cross(up, forward).Normalized();
            ComputeFOV(60);
        }

        public Camera(Vector3 origin, Vector3 forward,  float fov)
        {
            this.origin = origin;
            this.forward = forward.Normalized();
            right = new Vector3(-forward.z, 0, forward.x).Normalized();
            up = Vector3.Cross(right ,forward).Normalized();
            ComputeFOV(fov);
        }

        public void ComputeFOV(float fov)
        {
            sizeX = (float) Math.Abs(Math.Tan(fov * Math.PI / 360)) * 2;
            sizeY = sizeX;
        }

        public void SetScreenData(int width, int height)
        {
            sizeY = sizeX * height / width;
            stepX = sizeX / width;
            stepY = sizeY / height;

            screenOrigin = origin + forward + up * sizeY * 0.5f - right * sizeX * 0.5f;
        }

        public Vector3 GetRay(int x, int y)
        {
            return screenOrigin + x * right * stepX - y * up * stepY;
        }
    }
}