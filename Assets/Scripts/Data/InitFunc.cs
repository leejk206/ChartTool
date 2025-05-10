using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFunc
{
    LineRender lineRender;
    NoteEditor noteEditor;

    public void EditModeInit()
    {
        lineRender = GameObject.Find("LineRender").GetComponent<LineRender>();
        noteEditor = GameObject.Find("NoteEditor").GetComponent<NoteEditor>();
        Managers.Chart.Init();
        lineRender.Init();
        noteEditor.Init();
    }

    public void PlayModeInit()
    {
        Managers.Chart.Init();
        lineRender.Init();
        noteEditor.Init();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
