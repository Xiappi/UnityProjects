using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlatformController : MonoBehaviour
{

    public enum PathType
    {
        LINEAR,
        CONTINUOUS,
    }

    [SerializeField] PathType pathType;
    [SerializeField] GameObject PathParent;
    [SerializeField] float MoveSpeed;

    private List<Transform> pathNodes;
    private Vector3 currentPositionHolder, startPosition;
    private int currentNode = 0;
    private int moveDirection = 1;
    private float timer = 0f;
    private bool returning = false;

    private GameObject currentObjectOnPlatform;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (PathParent == null)
            return;

        pathNodes = PathParent.GetComponentsInChildren<Transform>().ToList();
        pathNodes.RemoveAt(0);
        CheckNode();
    }

    private void CheckNode()
    {
        timer = 0f;
        startPosition = gameObject.transform.position;
        currentPositionHolder = pathNodes[currentNode].position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentObjectOnPlatform = other.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        currentObjectOnPlatform.GetComponent<Rigidbody2D>().velocity += rb.velocity;
        currentObjectOnPlatform = null;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAlongPath();
    }

    void LateUpdate()
    {
        if (currentObjectOnPlatform == null)
            return;
        var otherRb = currentObjectOnPlatform.GetComponent<Rigidbody2D>();
        otherRb.velocity += new Vector2(rb.velocity.x, 0);
    }

    void MoveObjectOnPlatform()
    {
        MoveObjectOnPlatform();
    }

    private void MoveAlongPath()
    {
        if (PathParent == null)
            return;

        timer += Time.deltaTime * MoveSpeed;

        var distance = Vector3.Distance(gameObject.transform.position, currentPositionHolder);
        if (distance > 0.5f && distance != 0)
        {
            var dir_x = currentPositionHolder.x - startPosition.x;
            var dir_y = currentPositionHolder.y - startPosition.y;

            var factor = MoveSpeed / Mathf.Sqrt(Mathf.Pow(dir_x, 2) + Mathf.Pow(dir_y, 2));

            var vel_x = dir_x * factor;
            var vel_y = dir_y * factor;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(vel_x, vel_y);
        }
        else
        {
            if (currentNode == pathNodes.Count - 1 || (returning && currentNode == 0))
            {
                switch (pathType)
                {
                    case PathType.LINEAR:
                        moveDirection = -moveDirection;
                        returning = true;
                        break;
                    case PathType.CONTINUOUS:
                        currentNode = -1;
                        break;
                }
            }
            currentNode += moveDirection;
            CheckNode();
        }
    }
}
