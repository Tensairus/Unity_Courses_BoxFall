using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _spawnHeigth;
    [SerializeField] private int _cubesPerSecond;

    [SerializeField] private MeshRenderer _arenaFloor;
    [SerializeField] private int _spawnAreaPadding = 1;

    [SerializeField] private bool _isSpawning = true;

    private ObjectPool<Cube> _cubesPool;

    private float _spawnInterval;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(
            createFunc: () => CreateNewCube(),
            actionOnDestroy: (cube) => DestroyPooledCube(cube),
            defaultCapacity: 50,
            maxSize: 500
            );
    }

    private void OnEnable()
    {
        //_isSpawning = true;
        //_spawnInterval = 1f / _cubesPerSecond;
    }

    private void Start()
    {

        //_isSpawning = true;
        //_spawnInterval = 1f / _cubesPerSecond;

        StartSpawningCubes();

        //StartCoroutine(SpawnCubesOverTime(_spawnInterval));
    }

    private Cube CreateNewCube()
    {
        Cube newCube = Instantiate(_cubePrefab);
        newCube.ReadyToBePooled += OnCubeReadyToBePooled;
        return newCube;
    }

    private void DestroyPooledCube(Cube cube)
    {
        cube.ReadyToBePooled -= OnCubeReadyToBePooled;
        Destroy(cube);
    }

    private void OnCubeReadyToBePooled(Cube cube)
    {
        _cubesPool.Release(cube);
    }

    private void SpawnCube()
    {
        Cube cube = _cubesPool.Get();
        cube.Activate(PickSpawnPosition());
    }

    private Vector3 PickSpawnPosition()
    {
        int safetyCounterLimit = 20;
        int safetyCounter = 0;

        float spawnAreaMinX = _arenaFloor.bounds.min.x + _spawnAreaPadding;
        float spawnAreaMaxX = _arenaFloor.bounds.max.x - _spawnAreaPadding;
        float spawnAreaMinZ = _arenaFloor.bounds.min.z + _spawnAreaPadding;
        float spawnAreaMaxZ = _arenaFloor.bounds.max.z - _spawnAreaPadding;

        float spawnCheckRadius = 0.7f;

        bool isEmpty = false;

        Vector3 newPosition = new Vector3();

        while (isEmpty == false && safetyCounter < safetyCounterLimit)
        {
            safetyCounter++;

            newPosition = new Vector3(Random.Range(spawnAreaMinX, spawnAreaMaxX), _spawnHeigth, Random.Range(spawnAreaMinZ, spawnAreaMaxZ));

            Collider[] hitColliders = Physics.OverlapSphere(newPosition, spawnCheckRadius);

            if (hitColliders.Length == 0)
            {
                isEmpty = true;
            }
        }

        return newPosition;
    }

    private void StartSpawningCubes()
    {
        _isSpawning = true;
        _spawnInterval = 1f / _cubesPerSecond;

        StartCoroutine(SpawnCubesOverTime());
    }

    private IEnumerator SpawnCubesOverTime()
    {
        while (_isSpawning)
        {
            SpawnCube();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
