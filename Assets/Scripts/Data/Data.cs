using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoteData
{
    public float time;
    public int startLine;
    public int endLine;
    public Define.NoteType noteType;
}

[Serializable]
public class ChartData
{
    public float bpm;
    public List<NoteData> notes = new List<NoteData>();
}