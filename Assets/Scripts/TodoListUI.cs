using System;
using TMPro;
using UnityEngine;

public class TodoListUI : MonoBehaviour
{

    public TMP_Text Trash;
    public TMP_Text Plant;
    private void Start()
    {
        foreach (Interactor interactor in TodoListManager.Instance.interactors)
        {
            interactor.Used += InteractorOnUsed;
        }

        InteractorOnUsed();
    }

    private void InteractorOnUsed()
    {
        
        Trash.text = $"Trash: {TodoListManager.Instance.trashVariable.Value}/{TodoListManager.Instance.trash.Count}";
        Plant.text = $"Plants: {TodoListManager.Instance.plantVariable.Value}/{TodoListManager.Instance.plants.Count}";
    }
}