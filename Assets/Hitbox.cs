using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Hitbox))]
public class DemoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); //显示默认所有参数

        Hitbox demo = (Hitbox)target;

        demo.frames = (int)GUILayout.HorizontalSlider(demo.frames, 0, demo.bounds.Length);

        if (GUILayout.Button("AutoPlay"))
        {
            demo.AutoPlay();
        }
        if (GUILayout.Button("Previous"))
        {
            demo.frames--;
            demo.frames = Mathf.Clamp(demo.frames, 0, demo.bounds.Length);
            demo.Play(demo.frames);
        }
        if (GUILayout.Button("Next"))
        {
            demo.frames++;
            demo.frames = Mathf.Clamp(demo.frames, 0, demo.bounds.Length);
            demo.Play(demo.frames);
        }
    }
}

/// <summary>
/// 技能碰撞体
/// </summary>
public class Hitbox : MonoBehaviour
{
    public Bounds[] bounds;//手动配
    public int frames;

    void Awake()
    {

    }

    void Update()
    {
        transform.parent.transform.Translate(Vector3.right * Time.deltaTime);
    }

    public void Play(int frame)
    {
        transform.localPosition = bounds[frame - 1].center;
        transform.localScale = bounds[frame - 1].size;
    }

    public async void AutoPlay()
    {
        while (frames < bounds.Length)
        {
            frames++;
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
            Play(frames);
        }
    }
}
