using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Configuration;

namespace Raymarcher
{
    public class     Scene
    {
        private SDF sdfList;
        private Camera camera;
        private Vector3 lightDir;
        private static int maxStep;
        private int calls = 0;

        public Scene(string inputFile)
        {
            sdfList = SceneParser.ParseFile(inputFile);
            camera = new Camera(SceneParser.camPos, SceneParser.camFwd, SceneParser.camFov);
            maxStep = 500;
            //lightDir = new Vector3(0.5f, -1f, -1f).Normalized();
            lightDir = SceneParser.lightDir.Normalized();
        }

        public static bool FloatEq(float a, float b)
        {
            float epsilon = 0.001f;
            return Math.Abs(a - b) <= epsilon;
        }

        /*
         * March along the given ray from origin.
         * Returns the last tested position
         * put the number of steps in nbSteps and the last distance to the sdf in dist
         */
        public Vector3 RayMarch(SDF sdf, Vector3 ray, Vector3 origin, out int nbSteps, out float dist)
        {
            Vector3 current = origin;
            dist = sdf.Evaluate(current);
            nbSteps = 0;
            for (; nbSteps < maxStep && dist > 0 && dist < 100; nbSteps++)
            {
                current = current + ray * dist;
                dist = sdf.Evaluate(current);
            }

            return current;
        }

        /*
         * Render the loaded scene with the raymarching algorithm.
         * Filename is the name of the file that has to be created at the end of the render.
         * w id the width of the image you have to create
         * h is the height of the image you have to create
         */
        public void RenderScene(string fileName, int w, int h)
        {
            camera.SetScreenData(w, h);
            Bitmap image = new Bitmap(w, h);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Vector3 ray = camera.GetRay(i, j);
                    ray = ray - camera.origin;
                    ray = ray.Normalized();
                    
                    Vector3 point = RayMarch(sdfList, ray, camera.origin, out _, out float dist);
                    Color color;
                    if (dist <= 0 || FloatEq(0, dist))
                    {
                        point = point - (Math.Abs(dist) + 0.001f) * ray;
                        RayMarch(sdfList, -lightDir, point, out _, out dist);
                        
                        if (dist <= 0 || FloatEq(0, dist))
                            color = Color.FromArgb(50, 50, 50);
                        else
                            color = Shading.Shade(lightDir, point, sdfList, 0);
                    }
                    else
                        color = Color.Black;
                    image.SetPixel(i, j, color);
                }
                Console.WriteLine(i);
            }
            
            image.Save(fileName);
        }
    }
}