using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
 
    private Vector3 initialPosition;


    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Start()
    {
        InvokeRepeating(nameof(CreateInvaderGrid), 3f, 3f);
    }

    private void CreateInvaderGrid()
    {
        float upperBound = Camera.main.ViewportToWorldPoint(Vector3.up).y;
        float lowerBound = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        float randy = Random.Range(lowerBound, upperBound);
        Invader invader = Instantiate(prefabs[0], transform);
        invader.transform.position = new Vector3(transform.position.x, randy, transform.position.z);
    }

    public void ResetInvaders()
    {
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;
        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) count++;
        }
        return count;
    }
}
