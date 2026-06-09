using System;
using JSAM;
using UnityEngine;

[SelectionBase]
public class InteractorItem : MonoBehaviour, IUsable
{
    public IntRef variable;

    public bool isAlreadyUsed = false;

    public GameObject defaultVisual;
    public GameObject usedVisual;
    
    public SoundFileObject useSound;



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
        useSound?.Play();
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