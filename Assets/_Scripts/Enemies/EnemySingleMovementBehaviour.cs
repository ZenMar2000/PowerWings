using UnityEngine;
using static SharedLogics;

[RequireComponent(typeof(MovementDirection))]
public class EnemySingleMovementBehaviour : MonoBehaviour
{
    #region inspector variables
    [SerializeField] 
    #endregion

    #region private internal variables
    private Rigidbody2D rb;
    #endregion

    #region Unity functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
   
    }
    #endregion


    #region Utility private functions
   

    #endregion
}
