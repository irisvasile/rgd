using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour, IPunObservable
{
    PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        photonView.ObservedComponents.Add(this);
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }
    [Header("Stats")]
    public float attackDistance;
    public float attackRate;
    private float nextAttack;

    private NavMeshAgent navMeshAgent;
    private Animator anim; //TODO animators
    private bool walking;

    private Transform targetedEnemy;
    private bool clickedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); //for animators
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, 1000)) //max distance = 1000
            {
                if (hit.collider.tag == "Enemy")
                {
                    targetedEnemy = hit.transform;
                    clickedEnemy = true;
                    print("I WANNA TARGET THE ENEMY!");
                }
                else
                {
                    walking = true;
                    clickedEnemy = false;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = hit.point;
                    Debug.Log(navMeshAgent.destination.y);
                }
            }
        }

        if (clickedEnemy)
        {
            MoveAndAttack();
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            walking = false;
        }
        else
        {
            walking = true;
        }

       // anim.SetBool("isWalking", walking); //TODO animators
    }
    void MoveAndAttack()
    {
        if (targetedEnemy == null)
        {
            return;
        }

        navMeshAgent.destination = targetedEnemy.position;

        if (navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else
        {
            transform.LookAt(targetedEnemy);
            Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
            }
            navMeshAgent.isStopped = true;
            walking = false;
        }
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            // We own this player: send the others our data
            stream.SendNext(transform.position); //position of the character
            stream.SendNext(transform.rotation); //rotation of the character

        }
        else
        {
            // Network player, receive data
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            Quaternion syncRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
