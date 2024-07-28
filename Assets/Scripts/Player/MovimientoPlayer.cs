using UnityEngine;
using Mirror;

public class MovimientoPlayer : NetworkBehaviour
{
    private BoxCollider2D bodyBox;
    private RaycastHit2D hit;
    private Vector2 movePlyr;
    public Animator animator;
    public float speed = 5f;
    public GameObject popUp; // Referencia al prefab popUp
    private ControladorPopUp controladorPopUp; // Referencia al componente ControladorPopUp

    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 syncPosition;

    private void Start()
    {
        bodyBox = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        
        // Obtener la referencia al ControladorPopUp
        controladorPopUp = FindObjectOfType<ControladorPopUp>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            // Detectar presionamiento de la tecla E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Llamar al método MostrarPopUp del ControladorPopUp y pasar la referencia del SyncVarPlayers
                if (controladorPopUp != null)
                {
                    controladorPopUp.MostrarPopUp(this.GetComponent<SyncVarPlayers>());
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");
            
            // Normalizar el vector de movimiento
            movePlyr = new Vector2(hor, ver).normalized * speed * Time.fixedDeltaTime;

            // Colisión en Y
            hit = Physics2D.BoxCast(transform.position, bodyBox.size, 0, new Vector2(0, movePlyr.y), Mathf.Abs(movePlyr.y), LayerMask.GetMask("Player", "Blocking"));
            if (hit.collider == null)
            {
                transform.Translate(0, movePlyr.y, 0);
            }

            // Colisión en X
            hit = Physics2D.BoxCast(transform.position, bodyBox.size, 0, new Vector2(movePlyr.x, 0), Mathf.Abs(movePlyr.x), LayerMask.GetMask("Player", "Blocking"));
            if (hit.collider == null)
            {
                transform.Translate(movePlyr.x, 0, 0);
            }

            // Actualizar animator directamente
            animator.SetFloat("Horizontal", hor);
            animator.SetFloat("Vertical", ver);
            animator.SetBool("IsMoving", movePlyr.magnitude > 0.1f);

            // Sincronizar posición
            CmdSyncPosition(transform.position);
        }
    }

    [Command]
    private void CmdSyncPosition(Vector3 newPosition)
    {
        syncPosition = newPosition;
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        if (!isLocalPlayer)
        {
            transform.position = newPosition;
        }
    }
}