using System.Collections.Generic;
using UnityEngine;

public class TodoListManager : MonoSingleton<TodoListManager>
{
    public IntRef plantVariable;
    public IntRef trashVariable;

    public List<Interactor> plants = new();
    public List<Interactor> trash = new();

    public Interactor[] interactors;

    protected override void Awake()
    {
        base.Awake();
        plantVariable.Value = 0;
        trashVariable.Value = 0;

        interactors = FindObjectsByType<Interactor>(FindObjectsSortMode.None);

        foreach (Interactor interactor in interactors)
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