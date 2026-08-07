using System.Collections;
using Unity.AI.Navigation;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour
{
    private AgentLinkMover linkMover;

    public Lock passDoor = new();
    [Range(0.1f, 100)] public float doorDelay = 1;

    public CinemachineCamera camera;

    private void Awake()
    {
        linkMover = GetComponent<AgentLinkMover>();
    }

    private void OnEnable()
    {
        linkMover.agent.SetDestination(GameManager.Instance.player.transform.position);
        linkMover.OnEnterMeshLink += EnterMeshLink;
    }

    private void OnDisable()
    {
        linkMover.OnEnterMeshLink -= EnterMeshLink;
    }

    private void Update()
    {
        if (linkMover.agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            linkMover.agent.SetDestination(GameManager.Instance.player.transform.position);
            camera.enabled = true;
        }
        else
        {
            camera.enabled = false;
        }
    }

    private void EnterMeshLink()
    {
        passDoor.AddExternalLock(linkMover);
        StartCoroutine(DelayingMove());
    }

    private IEnumerator DelayingMove()
    {
        yield return new WaitForSeconds(doorDelay);
        Door door = Door.GetDoor((NavMeshLink)linkMover.linkData.owner);
        door.SimulateUse();

        transform.position = linkMover.linkData.endPos;
        linkMover.agent.CompleteOffMeshLink();
        passDoor.RemoveExternalLock(linkMover);
    }
}