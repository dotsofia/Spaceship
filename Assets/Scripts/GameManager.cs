using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private Player player;
    private Invaders invaders;
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        Respawn();
    }

    private void Respawn()
    {
        player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
    }

    private void GameOver()
    {
        invaders.gameObject.SetActive(false);
        SceneManager.LoadScene(2);
    }

    private void SetScore(int newScore)
    {
        score = newScore;
        scoreText.text = score.ToString("D4");
    }

    private void SetLives(int newLives)
    {
        lives = Mathf.Max(newLives, 0);
        livesText.text = lives.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        SetScore(score + invader.score);
        Destroy(invader.gameObject);

        if (score >= 150)
        {
            SceneManager.LoadScene(1);
        }
    }

}
