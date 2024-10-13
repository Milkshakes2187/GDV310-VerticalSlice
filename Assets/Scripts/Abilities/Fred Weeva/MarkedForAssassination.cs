using UnityEngine;

public class MarkedForAssassination : MonoBehaviour
{
    // TODO: make sure to rework this ability to work with georges ability system
    // TODO: Comment once reworked

    public GameObject target;
    public GameObject indicatorPF;
    public GameObject echoPF;

    GameObject indicator;

    public float castTime = 5f;
    public float elapsedTime = 0f;

    private void Start()
    {
        indicator = Instantiate(indicatorPF, target.transform.position, Quaternion.identity);
        Destroy(indicator, castTime);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > castTime)
        {
            ActivateAbility();
            Destroy(gameObject);
        }

        if (indicator)
        {
            indicator.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 4, target.transform.position.z);
        }
    }

    void ActivateAbility()
    {
        Instantiate(echoPF, target.transform.position, Quaternion.identity);
    }
}
