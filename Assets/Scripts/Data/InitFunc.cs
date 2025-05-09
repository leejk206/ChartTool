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
        lineRender.Init();
        noteEditor.Init();
    }

    public void PlayModeInit()
    {

    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
