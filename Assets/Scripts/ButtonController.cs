using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public TMP_InputField text;
    private db db;
    private void Start()
    {
        db = GetComponent<db>();
        StartCoroutine(db.LoadDataUsers());
    }
    public void Button()
    {
        db.SaveData(text.text);
        text.text = ""; 
    }
}
