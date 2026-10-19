using UnityEngine;
using System.Collections;
using Firebase.Database;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class db : MonoBehaviour
{
    public DatabaseReference dbRef;
    private string _currentUser;
    [SerializeField] private Transform TodoParent;
    [SerializeField] private GameObject Todo;

    [SerializeField] TMP_Text textUnvalid;

    [SerializeField] private TMP_InputField RegistrLogin;
    [SerializeField] private TMP_InputField RegistrPassword;
    [SerializeField] private TMP_InputField RegistrRepeatPassword;

    [SerializeField] private TMP_InputField Login;
    [SerializeField] private TMP_InputField Password;
    [SerializeField] private GameObject Todolist;
    [SerializeField] private GameObject Autorization;
    [SerializeField] private GameObject Registration;
    DataSnapshot snapshot;

    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        if (PlayerPrefs.HasKey("Auth"))
        {
            _currentUser = PlayerPrefs.GetString("Auth");
            OpenMenu("Todolist");
            StartCoroutine(LoadData());

        }

    }

    public void Registr()
    {
        
        StartCoroutine(LoadDataUsers());
        if(snapshot.ChildrenCount != 0)
        {
            foreach (var item in snapshot.Children)
            {
                if (RegistrLogin.text == item.Key.ToString())
                {
                    textUnvalid.text = "придумайте другой логин, этот уже занят";
                }
                else if (RegistrLogin.text.Length > 8 && RegistrPassword.text.Length > 8 && RegistrRepeatPassword.text == RegistrPassword.text)
                {

                    dbRef.Child("Users").Child(RegistrLogin.text).SetValueAsync(RegistrPassword.text);
                    Todolist.SetActive(true);

                    _currentUser = RegistrLogin.text;
                    Registration.SetActive(false);
                    StartCoroutine(LoadData());
                    PlayerPrefs.SetString("Auth", _currentUser);
                }
            }
        }
        else {
            if (RegistrLogin.text.Length > 8 && RegistrPassword.text.Length > 8 && RegistrRepeatPassword.text == RegistrPassword.text)
            {

                dbRef.Child("Users").Child(RegistrLogin.text).SetValueAsync(RegistrPassword.text);
                Todolist.SetActive(true);

                _currentUser = RegistrLogin.text;
                Registration.SetActive(false);
                StartCoroutine(LoadData());
                PlayerPrefs.SetString("Auth", _currentUser);
            }
        }   
        
            
           
    }
    public void LoginGo()
    {


        StartCoroutine(LoadDataUsers());

        foreach (var item in snapshot.Children)
        {
            if (Login.text == item.Key.ToString() && Password.text == item.Value.ToString())
            {
                Todolist.SetActive(true);
                _currentUser = Login.text;
                Autorization.SetActive(false);  
                StartCoroutine(LoadData());
                PlayerPrefs.SetString("Auth", _currentUser);
            }
            else
            {
                print("no");
            }
        }


    }

    public void SaveData(string str)
    {

        dbRef.Child("UsersTodos").Child(_currentUser).Child("Todos").Child(str).SetValueAsync(str);

        GameObject select = Instantiate(Todo, TodoParent);
        select.GetComponentInChildren<TMP_Text>().text = str;
        select.GetComponentInChildren<Button>().onClick.AddListener(() => {
            dbRef.Child("UsersTodos").Child(_currentUser).Child("Todos").Child(select.GetComponentInChildren<TMP_Text>().text).RemoveValueAsync();
            Destroy(select);
        });
    }
    public IEnumerator LoadData()
    {
        var data = dbRef.Child("UsersTodos").Child(_currentUser).Child("Todos").GetValueAsync();
        yield return new WaitUntil(predicate: () => data.IsCompleted);
        if (data.Exception != null)
        {
            Debug.LogError(data.Exception);
        }
        else if (data.Result == null)
        {
            Debug.Log("Null");
        }
        else
        {
            snapshot = data.Result;
            foreach (var item in snapshot.Children)
            {
                GameObject select = Instantiate(Todo, TodoParent);
                select.GetComponentInChildren<TMP_Text>().text = item.Value.ToString();

                select.GetComponentInChildren<Button>().onClick.AddListener(() => {
                    dbRef.Child("UsersTodos").Child(_currentUser).Child("Todos").Child(select.GetComponentInChildren<TMP_Text>().text).RemoveValueAsync();
                    Destroy(select);

                });
            }
        }
    }

    public IEnumerator LoadDataUsers() 
    {
        var data = dbRef.Child("Users").GetValueAsync();
        yield return new WaitUntil(predicate: () => data.IsCompleted);
        if(data.Exception != null)
        {
            Debug.LogError(data.Exception);
        }
        else if (data.Result == null)
        {
            Debug.Log("Null");
        }
        else
        {
            snapshot = data.Result;
            
        }
    }
    public void OpenMenu(string Menu)
    {
        if(Menu == "Todolist")
        {
            Autorization.SetActive(false);
            Registration.SetActive(false);
            Todolist.SetActive(true);
        }
        else if(Menu == "Auth")
        {
            Autorization.SetActive(true);
            Registration.SetActive(false);
            Todolist.SetActive(false);
        }
        else if(Menu == "Registr")
        {
            Autorization.SetActive(false);
            Registration.SetActive(true);
            Todolist.SetActive(false);
        }
    }
    

}
