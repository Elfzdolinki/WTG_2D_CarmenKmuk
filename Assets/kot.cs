using UnityEngine;
using UnityEngine.SceneManagement;

public class Kot : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _dragOffset;
    private bool _kotWasLaunched;
    private float _timeSittingAround;

    [SerializeField] private float _launchPower = 500;
    [SerializeField] private Kot[] _birds; // Tablica ptaków
    private int _currentBirdIndex = 0; // Aktualnie u¿ywany ptak
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        // Inicjalizujemy wszystkie ptaki i dezaktywujemy je, poza pierwszym
        for (int i = 0; i < _birds.Length; i++)
        {
            _birds[i].gameObject.SetActive(i == 0); // Aktywny tylko pierwszy ptak
            _birds[i]._initialPosition = _birds[i].transform.position; // Zapisz ich pozycjê pocz¹tkow¹
            _birds[i]._rigidbody2D = _birds[i].GetComponent<Rigidbody2D>();
            _birds[i]._spriteRenderer = _birds[i].GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (_currentBirdIndex >= _birds.Length) return;

        var currentBird = _birds[_currentBirdIndex];

        currentBird.GetComponent<LineRenderer>().SetPosition(0, currentBird._initialPosition);
        currentBird.GetComponent<LineRenderer>().SetPosition(1, currentBird.transform.position);

        if (currentBird._kotWasLaunched && currentBird._rigidbody2D.velocity.magnitude <= 0.1f)
        {
            currentBird._timeSittingAround += Time.deltaTime;
        }

        if (currentBird.transform.position.y > 6
            || currentBird.transform.position.y < -6
            || currentBird.transform.position.x > 20
            || currentBird.transform.position.x < -5
            || currentBird._timeSittingAround > 2)
        {
            NextBirdOrRestartLevel();
        }
    }

    private void OnMouseDown()
    {
        if (_kotWasLaunched || _currentBirdIndex >= _birds.Length) return;

        var currentBird = _birds[_currentBirdIndex];

        currentBird.GetComponent<LineRenderer>().enabled = true;
        currentBird._spriteRenderer.color = Color.red;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentBird._dragOffset = currentBird.transform.position - new Vector3(mousePosition.x, mousePosition.y, currentBird.transform.position.z);
    }

    private void OnMouseUp()
    {
        if (_kotWasLaunched || _currentBirdIndex >= _birds.Length) return;

        var currentBird = _birds[_currentBirdIndex];

        currentBird._spriteRenderer.color = Color.white;
        Vector2 directionToInitialPosition = currentBird._initialPosition - currentBird.transform.position;
        currentBird._rigidbody2D.AddForce(directionToInitialPosition * _launchPower);
        currentBird._rigidbody2D.gravityScale = 1;
        currentBird._kotWasLaunched = true;

        currentBird.GetComponent<LineRenderer>().enabled = false;
    }

    private void OnMouseDrag()
    {
        if (_kotWasLaunched || _currentBirdIndex >= _birds.Length) return;

        var currentBird = _birds[_currentBirdIndex];

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentBird.transform.position = new Vector3(mousePosition.x, mousePosition.y, currentBird.transform.position.z) + currentBird._dragOffset;
    }

    private void NextBirdOrRestartLevel()
    {
        _birds[_currentBirdIndex].gameObject.SetActive(false); // Dezaktywuj obecnego ptaka
        _currentBirdIndex++;

        if (_currentBirdIndex < _birds.Length)
        {
            _birds[_currentBirdIndex].gameObject.SetActive(true); // Aktywuj kolejnego ptaka
        }
        else
        {
            // Jeœli wszystkie ptaki zosta³y u¿yte, restartujemy poziom
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}