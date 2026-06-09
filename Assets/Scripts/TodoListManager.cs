using System.Collections.Generic;
using UnityEngine;

public class TodoListManager : MonoSingleton<TodoListManager>
{
    public IntRef plantVariable;
    public IntRef trashVariable;

    public List<InteractorItem> plants = new();
    public List<InteractorItem> trash = new();

    public InteractorItem[] interactors;

    protected override void Awake()
    {
        base.Awake();
        plantVariable.Value = 0;
        trashVariable.Value = 0;

        interactors = FindObjectsByType<InteractorItem>(FindObjectsSortMode.None);

        foreach (InteractorItem interactor in interactors)
        {
            if (interactor.variable == plantVariable)
            {
                plants.Add(interactor);
            }
            else if (interactor.variable == trashVariable)
            {
                trash.Add(interactor);
            }
        }
    }

    public void Clear(ItemData itemData)
    {
        trashVariable.Value = 0;
    }
}