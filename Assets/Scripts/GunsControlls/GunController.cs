using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static GunController Instance;

    public List<GameObject> Guns;

    public bool GunsInUp;

    public Transform[] BulletSpawnPoints;

    public GameObject BulletPrefab;

    public float BulletSpeed;

    public Vector3 GunsTransformStart;

    private void Awake()
    {
        Instance = this;
    }

    private bool _isBusy = false;
    private void Update()
    {
        if (GameManager.Instance.Level >= 5 && GameManager.Instance.GameStates == GameState.ShowColors)
        {
            UpGuns();
        }

        if (GameManager.Instance.Level >= 5 && GameManager.Instance.GameStates == GameState.RepeatColors && !_isBusy)
        {
            StartCoroutine(Shooting());
        }
    }

    public void UpGuns()
    {
        foreach (var item in Guns)
        {
            GunsTransformStart = item.transform.position;

            item.transform.position = new Vector3(GunsTransformStart.x, Mathf.Lerp(GunsTransformStart.y, 3.5f, 3.0f * Time.deltaTime),
                GunsTransformStart.z);
        }

        PlayerClimbing.Instance.ClimbAvalialble = true;
    }

    public void DownGuns()
    {
        foreach (var item in Guns)
        {
            GunsTransformStart = item.transform.position;

            item.transform.position = new Vector3(GunsTransformStart.x, GunsTransformStart.y = -3,
                GunsTransformStart.z);
        }
    }

    private Color _cachedColor;
    public IEnumerator Shooting()
    {
        _isBusy = true;

        var randomHole = UnityEngine.Random.Range(0, BulletSpawnPoints.Length);

        yield return new WaitForSeconds(0.5f);

        _cachedColor = BulletSpawnPoints[randomHole].GetComponentInParent<MeshRenderer>().material.color;
        BulletSpawnPoints[randomHole].GetComponentInParent<MeshRenderer>().material.color = Color.red;
        
        yield return new WaitForSeconds(1.0f);
        
        BulletSpawnPoints[randomHole].GetComponentInParent<MeshRenderer>().material.color = _cachedColor;

        var bullet = Instantiate(BulletPrefab, BulletSpawnPoints[randomHole].position, BulletSpawnPoints[randomHole].rotation);
        bullet.GetComponentInChildren<Rigidbody>().velocity = BulletSpawnPoints[randomHole].forward * 180f;


        _isBusy = false;
    }
}
