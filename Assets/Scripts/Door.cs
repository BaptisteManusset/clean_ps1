using System;
using System.Collections;
using System.Collections.Generic;
using JSAM;
using Unity.AI.Navigation;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[SelectionBase]
public class Door : MonoBehaviour, IUsable
{
    public static Dictionary<NavMeshLink, Door> Table =  new();
    
    
    public enum DoorUseState
    {
        Fail,
        Locked,
        Success
    }

    public ItemData key;

    public Transform anchor;

    public DestinationGetter Getter;

    public NavMeshLink MeshLink;

    public SoundFileObject openSound;
    public SoundFileObject lockSound;  
    public event Action<DoorUseState> OnUseDoor;
    public event Action OnExitedDoor;
    public bool countdown = false;

    

    private void Awake()
    {
        if (Getter == null)
        {
            Debug.LogWarning($"Missing Getter in {gameObject.name}",gameObject);
            return;
        }
        MeshLink = GetComponent<NavMeshLink>();
        
        Table.Add(MeshLink,this);
    }

    public static Door GetDoor(NavMeshLink link)
    {
        Table.TryGetValue(link, out Door door);
        return door;
    }

    private void Start()
    {
        if (Getter == null) return;
        Door door = Getter.Get();
        if (door) MeshLink.endTransform = door.anchor;
    }

    public void Use()
    {
        if (countdown)
        {
            OnUseDoor?.Invoke(DoorUseState.Fail);
            return;
        }

        if (Getter.Get() == null)
        {
            CenterMessage.Instance.PublishMessage("La porte est bloquée");
            PlayLockSound();
            OnUseDoor?.Invoke(DoorUseState.Locked);
            return;
        }

        if ((key == null || !Inventory.Instance.Contains(key)) && key != null)
        {
            CenterMessage.Instance.PublishMessage("Une clé est nécessaire");
            PlayLockSound();
            OnUseDoor?.Invoke(DoorUseState.Locked);
            return;
        }

        Getter.Get().GoTo();
        SimulateUse();
        OnUseDoor?.Invoke(DoorUseState.Success);
    }

    public void PlayLockSound()
    {
        if (lockSound) lockSound.Play(transform.position);
    }

    public void SimulateUse()
    {
        if (openSound) openSound.Play(transform.position);
    }

    public void ExitUse()
    {
    }

    private void GoTo()
    {
        GameManager.Instance.player.Teleport(anchor);
        StartCoroutine(PlayingCountdown());
        OnExitedDoor?.Invoke();
    }

    IEnumerator PlayingCountdown()
    {
        countdown = true;
        yield return new WaitForSecondsRealtime(1f);
        countdown = false;
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(anchor.position, Vector3.one * .2f);
        Gizmos.DrawCube(anchor.position + anchor.forward, Vector3.one * .1f);
        Gizmos.DrawLine(anchor.position, anchor.position + anchor.forward);
    }

    [ContextMenu("Link doors")]
    private static void LinkDoors()
    {
        List<Door> doors = new();

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Door door = gameObject.GetComponentInChildren<Door>(true);
            if (door)
            {
                doors.Add(door);
            }
        }

        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].SetOtherDoor(doors[(i + 1) % doors.Count]);
            EditorUtility.SetDirty(doors[i]);
        }
    }

    public void SetOtherDoor(Door a_door, bool probability = false)
    {
        if (!Getter)
        {
            Undo.RecordObject(gameObject.transform, "Add reference to destination getter");
            Getter = GetComponent<DestinationGetter>();
        }
        
        if (probability)
        {
            Getter.AddDoorToRule(a_door);
        }
        else
        {
            Getter.AddDoor(a_door);
        }

        if (MeshLink == null)
        {
            MeshLink = GetComponent<NavMeshLink>();
        }
    }
#endif
}