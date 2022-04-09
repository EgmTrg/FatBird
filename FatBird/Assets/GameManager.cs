using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private void SingletonOperation()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
    #region Properties
    private Vector3 _birdSpawnOffset;
    private float _timeSittingAround;
    private string _sceneName;

    public float TimeSittingAround
    {
        get { return _timeSittingAround; }
        set { _timeSittingAround = value; }
    }
    public string SceneName
    {
        get { return SceneManager.GetActiveScene().name; }
        set { _sceneName = value; }
    }
    public Vector3 BirdSpawnOffset
    {
        get { return _birdSpawnOffset; }
        private set { _birdSpawnOffset = value; }
    }
    #endregion

    [SerializeField] private List<Bird> birdPrefabList;

    private void Awake()
    {
        SingletonOperation();

        _birdSpawnOffset = new Vector3(-3, -1);
        _sceneName = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        TimeSittingAround = 0f;
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log($"Current Scene Index -> {nextSceneIndex - 1} " +
                  $"Next Scene Index -> {nextSceneIndex}");

        SceneManager.LoadScene(nextSceneIndex);
    }
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        SpawnNewBird();
        _timeSittingAround = 0f;
    }

    public Bird SpawnNewBird()
    {
        if (birdPrefabList == null)
        {
            Debug.Log("PrefabList bos ya da oyunda bird var.");
            return null;
        }

        int random = Random.Range(0, birdPrefabList.Count);
        Bird newBird = Instantiate(birdPrefabList[random], _birdSpawnOffset, Quaternion.identity);
        LevelController.Instance.AddTargetToCamera(newBird);
        return newBird;
    }
}