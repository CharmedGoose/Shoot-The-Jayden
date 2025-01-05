// guess

using System.Collections;
using UnityEngine;

public class IWonderWhatThisDoes : MonoBehaviour
{
    [Header("Refrences")]
    public Transform player;
    public Transform head;

    Vector3 playerDirection;
    Vector3 direction;
    Vector3 headDirection;

    float angleY;
    float angleX;
    bool inCameraView;

    RaycastHit hit;

    int layerMask;

    Transform playerHead;
    Rigidbody rb;
    BoxCollider boxCollider;
    GameObject meshRenderer;
    Transform playerChecker;
    GameObject eyes;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>().gameObject;
        playerChecker = head.GetChild(2);
        eyes = head.GetChild(1).gameObject;
        playerHead = Camera.main.transform.parent;

        layerMask = ~LayerMask.GetMask("???");

        StartCoroutine(ChangeLocation(0));
    }

    void Update()
    {
        playerDirection = (player.position - transform.position).normalized;

        head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(playerDirection), 1);
        head.localRotation = Quaternion.Euler(head.localRotation.eulerAngles.x, 0, 0);

        playerDirection.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection), 1);

        if (Vector3.Distance(transform.position, player.position) < 25f)
        {
            StartCoroutine(ChangeLocation(0.5f));
            return;
        }

        if (!meshRenderer.activeSelf) return;

        direction = (transform.position - player.position).normalized;
        headDirection = (transform.position - playerHead.position).normalized;

        angleY = Vector3.Angle(player.forward, direction);

        angleX = Vector3.Angle(playerHead.forward, headDirection);

        inCameraView = (angleY <= 50f) && (angleX <= 50f);

        if (inCameraView && Physics.Raycast(playerChecker.position, playerChecker.forward, out hit, 10000f, layerMask))
        {
            if (hit.transform.CompareTag("Player"))
            { 
                StartCoroutine(ChangeLocation(1f));
            }
        }

    }

    IEnumerator ChangeLocation(float time)
    {
        yield return new WaitForSeconds(time);

        rb.isKinematic = true;
        boxCollider.enabled = false;
        meshRenderer.SetActive(false);
        eyes.SetActive(false);

        float x = Random.Range(-100, 100);
        float z = Random.Range(-100, 100);

        float distanceX = x < 0 ? player.position.x - 100 + x : player.position.x + 100 + x;
        float distanceZ = z < 0 ? player.position.z - 100 + z : player.position.z + 100 + z;

        transform.position = new Vector3(distanceX, 50, distanceZ);

        yield return new WaitForSeconds(Random.Range(20f, 60f));

        rb.isKinematic = false;
        boxCollider.enabled = true;
        eyes.SetActive(true);
        meshRenderer.SetActive(true);
    }
}
