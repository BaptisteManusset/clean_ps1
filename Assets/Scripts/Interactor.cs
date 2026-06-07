using System;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Interactor : MonoBehaviour, IUsable
{
    public IntRef variable;

    public bool isAlreadyUsed = false;

    public GameObject defaultVisual;
    public GameObject usedVisual;


    public event Action Used; 
    
    private void Awake()
    {
        defaultVisual.SetActive(true);
        usedVisual.SetActive(false);
    }

    public void Use()
    {
        if (isAlreadyUsed) return;
        isAlreadyUsed = true;
        variable.Value++;
        
        defaultVisual.SetActive(false);
        usedVisual.SetActive(true);
        Used?.Invoke();
    }

    public void ExitUse()
    {
    }
}

public interface IUsable
{
    public void Use();
    public void ExitUse();
}