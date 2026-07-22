using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using JSAM;
using Unity.AI.Navigation;
using UnityEditor.Sprites;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[SelectionBase]
public class Door : MonoBehaviour, IUsable
{
    public static Dictionary<NavMeshLink, Door> Table = new();


    public Room parentRoom;

    public enum DoorUseState
    {
        Fail,
        Locked,
        Success
    }

    private const float DelayBeforeReuse = 1f;
    public ItemData key;

    public Transform anchor;

    public DestinationGetter Getter;

    public NavMeshLink MeshLink;

    public SoundFileObject openSound;
    public SoundFileObject lockSound;

    public event Action<DoorUseData> OnUseDoor;

    // public event Action OnExitedDoor;
    public bool countdown = false;


    private void Awake()
    {
        parentRoom = GetComponentInParent<Room>();
        if (Getter == null)
        {
            Debug.LogWarning($"Missing Getter in {gameObject.name}", gameObject);
            return;
        }

        MeshLink = GetComponent<NavMeshLink>();

        Table.Add(MeshLink, this);
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
        DoorUseData useInfo = new(a_originRoom: parentRoom);


        if (countdown)
        {
            useInfo.State = DoorUseState.Fail;
            OnUseDoor?.Invoke(useInfo);
            return;
        }

        if (Getter.Get() == null)
        {
            CenterMessage.Instance.PublishMessage("La porte est bloquée");
            PlayLockSound(transform.position);

            useInfo.State = DoorUseState.Locked;
            OnUseDoor?.Invoke(useInfo);
            return;
        }

        if ((key == null || !Inventory.Instance.Contains(key)) && key != null)
        {
            CenterMessage.Instance.PublishMessage("Une clé est nécessaire");
            PlayLockSound(transform.position);
            useInfo.State = DoorUseState.Locked;
            OnUseDoor?.Invoke(useInfo);
            return;
        }

        useInfo.State = DoorUseState.Success;
        Door destination = Getter.Get().GoTo();
        useInfo.destinationRoom = destination.parentRoom;

        PlaySound(destination.transform.position);
        OnUseDoor?.Invoke(useInfo);
    }


    public struct DoorUseData
    {
        public Room originRoom;
        public Room destinationRoom;

        public DoorUseState State;

        public DoorUseData(Room a_originRoom) : this()
        {
            originRoom = a_originRoom;
            destinationRoom = null;
            State = DoorUseState.Locked;
        }
    }

    public void PlayLockSound(Vector3 position)
    {
        if (lockSound) lockSound.Play(position);
    }

    public void SimulateUse()
    {
        PlaySound(transform.position);
    }

    private void PlaySound(Vector3 position)
    {
        if (openSound) openSound.Play(position);
    }

    public void ExitUse()
    {
    }

    private Door GoTo()
    {
        StartCoroutine(PreventingInstantReuse());
        GameManager.Instance.player.Teleport(anchor);
        GameManager.Instance.player.CurrentRoom = parentRoom;

        // OnExitedDoor?.Invoke();
        return this;
    }

    private IEnumerator PreventingInstantReuse()
    {
        countdown = true;
        yield return new WaitForSecondsRealtime(DelayBeforeReuse);
        countdown = false;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        string msg = "";
        Gizmos.DrawCube(anchor.position, Vector3.one * .2f);
        Gizmos.DrawCube(anchor.position + anchor.forward, Vector3.one * .1f);
        Gizmos.DrawLine(anchor.position, anchor.position + anchor.forward);

        if (Getter.Destination != null)
        {
            msg += $"▶️ {Getter.Destination.gameObject.name}\n";
            foreach (KeyValuePair<DayCompareGroup, SerializedDictionary<Door, float>> ruledDestination in Getter
                         .RuledDestinations)
            {
                foreach (KeyValuePair<Door, float> keyValuePair in ruledDestination.Value)
                {
                    msg += $"⚙️➡️ {keyValuePair.Key.gameObject.name}\n";
                }
            }
        }

        if (key) msg += $"\n🔒{key.name}\n";

        Handles.Label(transform.position + transform.up, msg);
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