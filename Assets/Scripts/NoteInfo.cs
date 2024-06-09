using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInfo : MonoBehaviour
{
    [Serializable]
    public struct NoteDescription {
        public string name;
        public Color color;
    }

    public NoteDescription[] noteDescription;
}
