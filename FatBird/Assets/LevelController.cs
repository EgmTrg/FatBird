using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region Singleton
    private static LevelController instance;
    public static LevelController Instance { get { return instance; } }
    private void SingletonOperation()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    [SerializeField] private Cinemachine.CinemachineTargetGroup _targetGroup;
    [SerializeField] private Enemy[] _enemies;

    private void Awake()
    {
        SingletonOperation();
    }

    private void Start()
    {
        _enemies = FindObjectsOfType<Enemy>();

        /*foreach (Enemy enemy in _enemies)
        {
            Debug.Log(enemy.transform.name);
        }*/

        foreach (Enemy enemy in _enemies)
        {
            _targetGroup.AddMember(enemy.transform, 1, 3);
        }
    }

    public void AddTargetToCamera(Bird bird)
    {
        _targetGroup.AddMember(bird.transform, 1, 3);
    }

    public bool LevelIsOver()
    {
        foreach (Enemy enemy in _enemies)
        {
            // Is any enemy alive? If find any enemy then returns.
            if (enemy != null)
                return false;
        }

        // Enemies are not alive.
        // Debug.Log("You killed all enemies.");
        //GameManager.Instance.LoadNextLevel();
        return true;
    }
}
