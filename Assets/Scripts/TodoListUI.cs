using TMPro;
using UnityEngine;

public class TodoListUI : MonoBehaviour
{

    public TMP_Text Trash;
    public TMP_Text Plant;
    private void Start()
    {
        TodoListManager.Instance.trashVariable.valueChanged += InteractorOnUsed;
        TodoListManager.Instance.plantVariable.valueChanged += InteractorOnUsed;
        InteractorOnUsed(0);
    }

    private void InteractorOnUsed(int i)
    {
        
        Trash.text = $"Trash: {TodoListManager.Instance.trashVariable.Value}/{TodoListManager.Instance.trash.Count}";
        Plant.text = $"Plants: {TodoListManager.Instance.plantVariable.Value}/{TodoListManager.Instance.plants.Count}";
    }
}