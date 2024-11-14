using UnityEngine;
using UnityEngine.SceneManagement;

public class Kot : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _dragOffset;
    private bool _kotWasLaunched;
    private float _timeSittingAround;

    [SerializeField] private float _launchPower = 500;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _initialPosition = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetComponent<LineRenderer>().SetPosition(0, _initialPosition);
        GetComponent<LineRenderer>().SetPosition(1, transform.position);

        if (_kotWasLaunched && _rigidbody2D.velocity.magnitude <= 0.1f)
        {
            _timeSittingAround += Time.deltaTime;
        }

        if (transform.position.y > 6 || 
            transform.position.y < -6 || 
            transform.position.x > 20 ||
            transform.position.x < -5 || 
            _timeSittingAround > 2)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
    private void OnMouseDown()
    {
        // Only allow interaction if the bird hasn't been launched yet
        if (_kotWasLaunched) return;
        GetComponent<LineRenderer>().enabled = true;

        _spriteRenderer.color = Color.red;

        // Calculate the offset between the bird's position and the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _dragOffset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    private void OnMouseUp()
    {
        // Only allow launching if the bird hasn't been launched yet
        if (_kotWasLaunched) return;

        _spriteRenderer.color = Color.white;

        Vector2 directionToInitialPosition = _initialPosition - transform.position;
        _rigidbody2D.AddForce(directionToInitialPosition * _launchPower);
        _rigidbody2D.gravityScale = 1;
        _kotWasLaunched = true; // Set this to true to prevent future launches

         GetComponent<LineRenderer>().enabled = false;
    }

    private void OnMouseDrag()
    {
        // Only allow dragging if the bird hasn't been launched yet
        if (_kotWasLaunched) return;

        // Maintain the offset during dragging to prevent teleporting
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z) + _dragOffset;
    }
}