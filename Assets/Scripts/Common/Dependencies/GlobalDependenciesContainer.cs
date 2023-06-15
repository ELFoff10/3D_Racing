using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalDependenciesContainer : Dependencies
{
    [SerializeField] private Pauser pauser;

    private static GlobalDependenciesContainer instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected override void BindAll(MonoBehaviour monoBehaviorInScene)
    {
        Bind<Pauser>(pauser, monoBehaviorInScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        FindAllObjectToBind();
    }
}
