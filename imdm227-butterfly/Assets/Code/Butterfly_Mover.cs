using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class Butterfly_Mover : MonoBehaviour
{
    // stuff for camera
    const int waitWidth = 16;
    private float brightness_threshold = 10f; 

    public int width = 640;
    public int height = 360;

    Color32[] pixels;
    WebCamTexture cam;
    Texture2D tex;

    public GameObject butterfly;
    public float speed = 0.1f;
    public float damping = 0.5f;

    // how many points in a circle around center point
    public int num_points = 200;
    public float theta;
    public float radius = 16f;

    Action update = () => { };
    void Start()
    {
        theta = 2 * Mathf.PI / num_points;

        update = WaitingForCam;

        cam = new WebCamTexture(WebCamTexture.devices[0].name, width, height, 30);
        cam.Play();
        int w = cam.width;
        Debug.Log($"Width = {width}, Height = {height}");
    }

    // Update is called once per frame
    private void Update()
    {
        update();
    }

    void WaitingForCam()
    {
        if (cam.width > waitWidth)
        {
            width = cam.width;
            height = cam.height;
            Debug.Log($"Width = {width}, Height = {height}");
            pixels = new Color32[cam.width * cam.height];
            tex = new Texture2D(cam.width, cam.height, TextureFormat.RGBA32, false);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = tex;
            transform.localScale = new Vector3(width * 9f / height, 9, 1);
            update = CamIsOn;
        }
        else
        {
            Debug.Log("Not Yet");
        }
    }

    void CamIsOn()
    {
        if (cam.didUpdateThisFrame)
        {
            cam.GetPixels32(pixels);

            // for (int i = 0; i < pixels.Length; ++i)
            // {
            //     // Don't be so negative.

            //     // pixels[i].r = (byte)(255 - pixels[i].r);
            //     // pixels[i].g = (byte)(255 - pixels[i].g);
            //     // pixels[i].b = (byte)(255 - pixels[i].b);


            //     // Peter Maxx is back!

            //     // int numLevels = 3;
            //     // float divisor = 255f / numLevels;

            //     // pixels[i].r = (byte)((int)(pixels[i].r / divisor + 0.5f) * divisor + 0.5f);
            //     // pixels[i].g = (byte)((int)(pixels[i].g / divisor + 0.5f) * divisor + 0.5f);
            //     // pixels[i].b = (byte)((int)(pixels[i].b / divisor + 0.5f) * divisor + 0.5f);


            //     // The '60s were a strange time...

            //     // pixels[i].r = (byte)(pixels[i].r * 4 % 256);
            //     // pixels[i].g = (byte)(pixels[i].g * 4 % 256);
            //     // pixels[i].b = (byte)(pixels[i].b * 4 % 256);


            //     // Zebra vision.

            //     int total = (pixels[i].r + pixels[i].g + pixels[i].b) / 3;
            //     int numlevels = 16;
            //     int level = total / (256 / numlevels);

            //     if (level > brightness_threshold)
            //     {
            //         pixels[i].r = pixels[i].g = pixels[i].b = 255;
            //     }
            //     else
            //     {
            //         pixels[i].r = pixels[i].g = pixels[i].b = 0;
            //     }
            // }

            // tex.SetPixels32(pixels);
            // tex.Apply();

            // pixels = old_pixels;

            int cur_col = (int) ((butterfly.transform.position.x + 8) / 16 * width);
            int cur_row = (int) ((butterfly.transform.position.y + 4) / 8 * height);

            // Remember polar: x = r*cos(theta) ; y = r*sin(theta)  
            for (int pts = 0; pts < num_points; pts ++)
            {
                int check_col = (int) (radius * (float) Mathf.Cos(theta * pts)) + cur_col; 
                int check_row = (int) (radius * (float) Mathf.Sin(theta * pts)) + cur_row;

                if (check_col >= cam.width)
                {
                    check_col = cam.width - 1;
                }
                if (check_row >= cam.height)
                {
                    check_row = cam.height - 1;
                }

                
                int index = check_row * width + check_col;
                // fix this, it's getting out of bounds right now
                int total = (pixels[index].r + pixels[index].g + pixels[index].b) / 3;
                int numlevels = 16;
                int level = total / (256 / numlevels);

                // draws a red circle just in case
                pixels[index].r = 255;
                pixels[index].b = 0;
                pixels[index].g = 0;

                if (level > brightness_threshold)
                {
                    // Debug.Log("has brightnes: " + pts + " index: " + index);

                    // Push in the opposite direction
                    float new_x = (float)check_col/width * 16 - 8;
                    float new_y = (float)check_row/height * 8 - 4;

                    Debug.Log("direction x:" + new_x);
                    Debug.Log("direction y: " + new_y);

                    Vector3 hand_pos = new Vector3(new_x, new_y, 0);
                    Vector3 direction = butterfly.transform.position - hand_pos;
                    butterfly.transform.position += direction * speed * Time.deltaTime;
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();

            // pixels = old_pixels;
        }
    }

}
