using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState GameStates;

    [SerializeField]
    private List<GameObject> ListWithPlatforms;

    private int InputPlatformIndex = 0;
    
    private GameObject NowPlatform;
    private GameObject NextPlatform;
    public GameObject Player;
    public GameObject SpawnPoint;

    public int Level;

    private Color CachedColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Level = 1;
        GameStates = GameState.Default;
    }

    private void Update()
    {
        DevChangeLevel();
    }

    public GameObject GetRandomElement(List<GameObject> inputList)
    {
        GameObject randomPlatform;

        int index = Random.Range(0, inputList.Count);
        randomPlatform = inputList[index];

        return randomPlatform;
    }

    public IEnumerator ChangeColor()
    {
        ListWithPlatforms.Add(GetRandomElement(GridSpawner.Instance.Platforms));

        InputPlatformIndex = 0;
        NextPlatform = ListWithPlatforms[InputPlatformIndex];

        GameStates = GameState.ShowColors;

        foreach (var item in ListWithPlatforms)
        {
            CachedColor = item.GetComponentInChildren<MeshRenderer>().material.color;
            yield return new WaitForSeconds(1);
            item.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
            yield return new WaitForSeconds(2);
            item.GetComponentInChildren<MeshRenderer>().material.color = CachedColor;
        }

        GameStates = GameState.RepeatColors;
        PlayerMovement.Instance.LockMovement = false;
    }

    public IEnumerator EndLevel()
    {
        if (GameStates == GameState.LoseGame)
        {
            MainCanvas.Instance.WinLoseText.text = "You LOSE! Try again.";
            MainCanvas.Instance.WinLoseText.color = Color.red;
            MainCanvas.Instance.WinLoseText.gameObject.SetActive(true);

            InputPlatformIndex = 0;
            ListWithPlatforms.Clear();
        }

        else if (GameStates == GameState.WinGame)
        {
            MainCanvas.Instance.WinLoseText.text = "You WIN! Congratulations!";
            MainCanvas.Instance.WinLoseText.color = Color.green;
            MainCanvas.Instance.WinLoseText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(3);

        foreach (var item in GridSpawner.Instance.Platforms)
        {
            item.GetComponentInChildren<MeshRenderer>().material.color = CachedColor;
        }

        MainCanvas.Instance.WinLoseText.gameObject.SetActive(false);

        if (GameStates == GameState.WinGame)
        {
            Level++;
        }

        else if (GameStates == GameState.LoseGame)
        {
            Level = 1;
            PlayerClimbing.Instance.ClimbAvalialble = false;
            GunController.Instance.DownGuns();
        }

        RespawnPlayer();
    }

    public void LoseGame()
    {
        GameStates = GameState.LoseGame;
        PlayerMovement.Instance.LockMovement = true;
        NowPlatform = null;
        NextPlatform = null;
        InputPlatformIndex = 0;

        StartCoroutine(EndLevel());
    }

    public void WinGame()
    {
        GameStates = GameState.WinGame;
        PlayerMovement.Instance.LockMovement = true;
        NowPlatform = null;
        NextPlatform = null;
        InputPlatformIndex = 0;

        StartCoroutine(EndLevel());
    }

    public void CheckSelectPlatform(GameObject platform)
    {
        if (GameStates == GameState.RepeatColors && InputPlatformIndex <= ListWithPlatforms.Count)
        {

            NowPlatform = platform;

            InputPlatformIndex++;

            if (NowPlatform == NextPlatform)
            {
                platform.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

                if (InputPlatformIndex < ListWithPlatforms.Count)
                    NextPlatform = ListWithPlatforms[InputPlatformIndex];
                else
                    WinGame();
            }

            else
            {
                platform.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                LoseGame();
            }
        }
    }

    private void RespawnPlayer()
    {
        GameStates = GameState.Default;
        PlayerMovement.Instance.LockMovement = false;
        Player.transform.position = SpawnPoint.transform.position;
        Player.transform.rotation = SpawnPoint.transform.rotation;

        if (Level == 5)
            StartCoroutine(AddClimbingText());
    }

    private IEnumerator AddClimbingText()
    {
        MainCanvas.Instance.WinLoseText.text = "Now you can jump on the walls.";
        MainCanvas.Instance.WinLoseText.color = Color.white;
        MainCanvas.Instance.WinLoseText.color = Color.green;
        MainCanvas.Instance.WinLoseText.gameObject.SetActive(true);

        yield return new WaitForSeconds(4.0f);

        MainCanvas.Instance.WinLoseText.gameObject.SetActive(false);
    }

    private void DevChangeLevel()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && Level > 1)
        {
            ListWithPlatforms.Clear();

            for (int i = 0; i < Level; i++)
            {
                ListWithPlatforms.Add(GetRandomElement(GridSpawner.Instance.Platforms));
            }

            Level--;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) && Level < 10)
        {
            ListWithPlatforms.Clear();

            for (int i = 0; i < Level; i++)
            {
                ListWithPlatforms.Add(GetRandomElement(GridSpawner.Instance.Platforms));
            }

            Level++;
        }
    }
}

public enum GameState
{
    Default,
    ShowColors,
    RepeatColors,
    WinGame,
    LoseGame
}
