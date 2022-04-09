using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D), typeof(LineRenderer))]
public class Bird : MonoBehaviour
{
    #region Props
    public bool BirdWasLaunched { get => birdWasLaunched; private set => birdWasLaunched = value; }
    public Rigidbody2D Rigidbody { get => _rigidbody2D; set => _rigidbody2D = value; }
    #endregion

    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private bool birdWasLaunched;

    //private Vector3 _initialPosition;
    private float _launchPower = 250;

    #region DragAndDrop
    private void OnMouseDown()
    {
        _lineRenderer.enabled = true;
    }

    private void OnMouseUp()
    {
        Vector2 directionToInitialPosition = GameManager.Instance.BirdSpawnOffset - transform.position;
        Rigidbody.AddForce(directionToInitialPosition * _launchPower);
        Rigidbody.gravityScale = 1f;
        BirdWasLaunched = true;

        _lineRenderer.enabled = false;
        //virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>().m_ScreenY = 0.5f;
    }

    private void OnMouseDrag()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(newPosition.x, newPosition.y);
    }
    #endregion

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //virtualCamera = GameManager.Instance.VirtualCamera;

        //virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>().m_ScreenY = 0.8f;
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, GameManager.Instance.BirdSpawnOffset);
        
        BirdController();
    }

    private void BirdController()
    {
        if (birdWasLaunched && _rigidbody2D.velocity.magnitude <= 0.1)
            GameManager.Instance.TimeSittingAround += Time.deltaTime;

        if (GameManager.Instance.TimeSittingAround >= 3f)
        {
            if (LevelController.Instance.LevelIsOver())
                GameManager.Instance.LoadNextLevel();
            else
            {
                GameManager.Instance.TimeSittingAround = 0f;
                GameManager.Instance.SpawnNewBird();
                Destroy(gameObject);
            }
        }
    }
}
