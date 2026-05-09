using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BowlController : MonoBehaviour
{
    [SerializeField]
    String[] stuff;

    [SerializeField]
    GameObject[] layers;
    Stack<String> ingridents = new Stack<string>();
    Stack<GameObject> gameLayers = new Stack<GameObject>();

    String _currentTarget;

    void Start()
    {
        foreach (var item in stuff)
        {
            ingridents.Push(item);
        }

        foreach (var item in layers)
        {
            gameLayers.Push(item);
        }

        if (ingridents.Count != 0)
        {
            _currentTarget = ingridents.Pop();
        }
    }

    public void CheckForIngridient(string name)
    {
        if (name == _currentTarget && ingridents.Count == 0)
        {
            GameOver();
        }
        else if (name == _currentTarget)
        {

            _currentTarget = ingridents.Pop();
            GameObject layer = gameLayers.Pop();
            Debug.Log(layer.name);
            layer.SetActive(true);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Done");
    }
}
