using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Butterfly_Mover : MonoBehaviour
{
    // stuff for camera
    const int waitWidth = 16;
    public float threshold = 10f; 

    public int width = 640;
    public int height = 360;

    Color32[] pixels;
    Color32[] old_pixels;
    Color32[] flipped_pixels;
    static WebCamTexture cam;
    Texture2D tex;

    public GameObject butterfly;
    public float speed = 0.1f;
    public float damping = 0.5f;

    // how many points in a circle around center point
    public int num_points = 200;
    public float theta;
    public float radius = 16f;

    private float max_offset_x = 8f;
    private float offset_radius;
    private float max_offset_y = 4.5f;


    public int numlevels = 16;

    Action update = () => { };
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;

        theta = 2 * Mathf.PI / num_points;
        offset_radius = radius/25;

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

    void GetFrameOne()
    {
        if (cam.didUpdateThisFrame)
        {
            Flip_Camera(old_pixels);
            tex.SetPixels32(old_pixels);
            tex.Apply();
            
            update = CamIsOn;
            Debug.Log("YIPPEE");
        }
        else
        {
            Debug.Log("nope");
        }
    }

    void WaitingForCam()
    {
        if (cam.width > waitWidth)
        {
            width = cam.width;
            height = cam.height;
            Debug.Log($"Width = {width}, Height = {height}");
            pixels = new Color32[cam.width * cam.height];
            old_pixels = new Color32[cam.width * cam.height];
            flipped_pixels = new Color32[cam.width * cam.height];
            tex = new Texture2D(cam.width, cam.height, TextureFormat.RGBA32, false);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = tex;
            transform.localScale = new Vector3(width * 9f / height, 9, 1);
            update = GetFrameOne;
        }
        else
        {
            Debug.Log("Not Yet");
        }
    }

    void CamIsOn()
    {

        // reset the background every 100 frames
        Debug.Log(Time.time);
        if (((int)Time.time) % 5 == 0)
        {
            update = GetFrameOne;
        }

        if (cam.didUpdateThisFrame)
        {
            Flip_Camera(pixels);
            

            // for (int i = 0; i < pixels.Length; ++i)
            // {
            // //     // Don't be so negative.

            // //     // pixels[i].r = (byte)(255 - pixels[i].r);
            // //     // pixels[i].g = (byte)(255 - pixels[i].g);
            // //     // pixels[i].b = (byte)(255 - pixels[i].b);


            // //     // Peter Maxx is back!

            // //     // int numLevels = 3;
            // //     // float divisor = 255f / numLevels;

            // //     // pixels[i].r = (byte)((int)(pixels[i].r / divisor + 0.5f) * divisor + 0.5f);
            // //     // pixels[i].g = (byte)((int)(pixels[i].g / divisor + 0.5f) * divisor + 0.5f);
            // //     // pixels[i].b = (byte)((int)(pixels[i].b / divisor + 0.5f) * divisor + 0.5f);


            // //     // The '60s were a strange time...

            // //     // pixels[i].r = (byte)(pixels[i].r * 4 % 256);
            // //     // pixels[i].g = (byte)(pixels[i].g * 4 % 256);
            // //     // pixels[i].b = (byte)(pixels[i].b * 4 % 256);


            // //     // Zebra vision.

            //     int total = (old_pixels[i].r + old_pixels[i].g + old_pixels[i].b) / 3;
            //     int level = total / (256 / numlevels);

            //     if (level > brightness_threshold)
            //     {
            //         old_pixels[i].r = old_pixels[i].g = old_pixels[i].b = 255;
            //     }
            //     else
            //     {
            //         old_pixels[i].r = old_pixels[i].g = old_pixels[i].b = 0;
            //     }
            // }

            tex.SetPixels32(pixels);
            tex.Apply();

            // pixels = old_pixels;

            int cur_col = (int) ((butterfly.transform.position.x + max_offset_x) / (max_offset_x * 2) * width);
            int cur_row = (int) ((butterfly.transform.position.y + max_offset_y) / (max_offset_y * 2) * height);

            // Remember polar: x = r*cos(theta) ; y = r*sin(theta)  
            for (int pts = 0; pts < num_points; pts ++)
            {
                int check_col = (int) (radius * (float) Mathf.Cos(theta * pts)) + cur_col; 
                int check_row = (int) (radius * (float) Mathf.Sin(theta * pts)) + cur_row;

                if (check_col >= cam.width-1)
                {
                    // check_col = (int) (cam.width - (radius * 2) - 1);
                    check_col = cam.width-1;
                }
                if (check_col <= 0)
                {
                    // check_col = (int) (radius * 2) + 1;
                    check_col = 0;
                }
                if (check_row >= cam.height-1)
                {
                    // check_row = (int) (cam.height - radius - 1);
                    check_row = cam.height-1;
                }
                if(check_row <= 0)
                {
                    // check_row = (int) (radius + 1);
                    check_row = 0;
                }

                
                int index = check_row * width + check_col;
                // fix this, it's getting out of bounds right now
                // int total = (pixels[index].r + pixels[index].g + pixels[index].b) / 3;
                
                // int level = total / (256 / numlevels);
                int old_avg = (old_pixels[index].r + old_pixels[index].g + old_pixels[index].b)/ 3;
                // int old_level = old_avg / (256 / numlevels);
                int new_avg = (pixels[index].r + pixels[index].g + pixels[index].b) / 3;
                // int new_level = new_avg / (256 / numlevels);

                int difference = Math.Abs(old_avg - new_avg);

                int red_diff = Math.Abs(old_pixels[index].r - pixels[index].r);
                int green_diff = Math.Abs(old_pixels[index].g - pixels[index].g);
                int blue_diff = Math.Abs(old_pixels[index].b - pixels[index].b);

                // Debug.Log("old_level: " +old_avg + " new_level: " + new_avg + " difference: " +  difference);



                // int total = pixels[index].r + pixels[index].g + pixels[index].b;

                // draws a red circle just in case
                pixels[index].r = 255;
                pixels[index].b = 0;
                pixels[index].g = 0;
                // Debug.Log("has brightnes: " + new_total);

                if (red_diff > threshold || green_diff > threshold || blue_diff > threshold)
                {
                    // Debug.Log("has brightnes: " + pts + " index: " + index);
                    // Debug.Log("has brightnes: " + total);

                    // Push in the opposite direction
                    float new_x = (float)check_col/width * (max_offset_x *2) - max_offset_x;
                    float new_y = (float)check_row/height * (max_offset_y * 2) - max_offset_y;


                    Vector3 hand_pos = new Vector3(new_x / offset_radius, new_y / offset_radius, 0);
                    // Debug.Log("hand_pos:" + hand_pos.x + " " + hand_pos.y);
                    Vector3 direction = butterfly.transform.position - hand_pos;
                    Vector3 new_position = butterfly.transform.position + direction * speed * Time.deltaTime;
                    // if (new_position.x > (max_offset_x - offset_radius_x))
                    // {
                    //     new_position.x = max_offset_x - offset_radius_x;
                    // }
                    // if (new_position.x < ((max_offset_x - offset_radius_x) * -1))
                    // {
                    //     new_position.x = (max_offset_x - offset_radius_x) * -1;
                    // }
                    // if (new_position.y > max_offset_y - offset_radius_y)
                    // {
                    //     new_position.y = max_offset_y - offset_radius_y;
                    // }
                    // if (new_position.y < ((max_offset_y -offset_radius_y) * -1))
                    // {
                    //     new_position.y = (max_offset_y-offset_radius_y) * -1;
                    // }
                    if (new_position.x > (max_offset_x - offset_radius))
                    {
                        new_position.x = max_offset_x - offset_radius;
                    }
                    if (new_position.x < ((max_offset_x - offset_radius) * -1))
                    {
                        new_position.x = (max_offset_x - offset_radius) * -1;
                    }
                    if (new_position.y > max_offset_y - offset_radius)
                    {
                        new_position.y = max_offset_y - offset_radius;
                    }
                    if (new_position.y < ((max_offset_y - offset_radius) * -1))
                    {
                        new_position.y = (max_offset_y - offset_radius) * -1;
                    }
                    // Debug.Log("direction x:" + new_position.x);
                    // Debug.Log("direction y: " + new_position.y);
                    butterfly.transform.position = new_position;
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();

            // tex.SetPixels32(pixels);
            // tex.Apply();

            // pixels = old_pixels;
        }
        else
        {
            Vector3 new_position = butterfly.transform.position;
            if (new_position.x > (max_offset_x - offset_radius))
                    {
                        new_position.x = max_offset_x - offset_radius;
                    }
                    if (new_position.x < ((max_offset_x - offset_radius) * -1))
                    {
                        new_position.x = (max_offset_x - offset_radius) * -1;
                    }
                    if (new_position.y > max_offset_y - offset_radius)
                    {
                        new_position.y = max_offset_y - offset_radius;
                    }
                    if (new_position.y < ((max_offset_y - offset_radius) * -1))
                    {
                        new_position.y = (max_offset_y - offset_radius) * -1;
                    }
                    // Debug.Log("direction x:" + new_position.x);
                    // Debug.Log("direction y: " + new_position.y);
                    butterfly.transform.position = new_position;
        }
    }

    void Flip_Camera(Color32[] pixel)
    {
        for (int c = 0; c < cam.height; c++)
        {
            cam.GetPixels32(flipped_pixels);
            for(int i = 0; i < cam.width; i++)
            {
                // int index = 
                // pixels[(cam.width * (cam.height - 1 - c)) + i] = pixels[(c * cam.width) + i];
                pixel[(c * cam.width) + (cam.width - 1 - i)] = flipped_pixels[(c * cam.width) + i];
            } 
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        cam.Stop();
        cam = new WebCamTexture(WebCamTexture.devices[0].name, width, height, 30);
        cam.Play();
        int w = cam.width;
        Debug.Log($"Width = {width}, Height = {height}");

        update = WaitingForCam;

        
    }

}
