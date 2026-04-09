using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum State {Patrolling, Chasing, Waiting, Assault }; //estados 
    public State currenteState = State.Patrolling; //estado inicial de patrulla

    [Header("Configuration")]
    [SerializeField]private Transform[] waypoints; //puntos de patrullaje
    [SerializeField]private Transform player; 
    [SerializeField]private float _speed=10f; 
    [SerializeField]private float _detectionRange=5f; //rango de detección
    [SerializeField] private float _assaultDistance = 2f;
    [SerializeField] private float _assaultSpeed = 2f;

    [SerializeField]private float _waitTime=2f; //tiempo de espera al llegar a los puntos de patrullaje


    private int _currentWaypointIndex = 0; 
    //private bool _isWaiting=false  ;
    private Renderer _ballRenderer; //para cambiar el color al cambiar el estado de la bola




    // Start is called before the first frame update
    void Start()
    {
        _ballRenderer = GetComponent<Renderer>(); //cogemos el color de la bola
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer= Vector3.Distance(transform.position,player.position);

        //hacemos la logica de la transicion de patrullaje a persecucion

        if (distanceToPlayer < _detectionRange)
        {
            //paramos las corrutinas
            StopAllCoroutines();

            //cambiamos el estado a perseguir
            currenteState= State.Chasing;

            if (distanceToPlayer <= _assaultDistance)
            {
                currenteState=State.Assault;
            }
        }
        else if (currenteState == State.Chasing && distanceToPlayer >=_detectionRange)
        {
            currenteState = State.Patrolling;
        }

        switch (currenteState) { 
            
            case State.Patrolling:
                Patrol();
                _ballRenderer.material.color= Color.green;
                break;  

            case State.Chasing:
                Chase();
                _ballRenderer.material.color = Color.red;
                break;

            case State.Assault:
                Assault();
                _ballRenderer.material.color= Color.blue;
                break;

            case State.Waiting:
                _ballRenderer.material.color = Color.yellow;
                break;        
        
        
        }
    }

    #region estados

    void Patrol()
    {
       
        Transform target = waypoints[_currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            StartCoroutine(WaitWaypoint());
        }
    }

    IEnumerator WaitWaypoint()
    {
        currenteState = State.Waiting;
        yield return new WaitForSeconds(_waitTime);

        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;

        currenteState = State.Patrolling;
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, _speed * Time.deltaTime * 1.2f);
    }

    void Assault()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, _speed * Time.deltaTime * _assaultSpeed);
    }


    #endregion
}
