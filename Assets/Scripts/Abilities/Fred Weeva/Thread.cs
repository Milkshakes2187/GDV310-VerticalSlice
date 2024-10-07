using UnityEngine;
using UnityEngine.TextCore.Text;
using VInspector;

public class Thread : MonoBehaviour
{
    public GameObject mesh;
    MeshRenderer meshRenderer;
    public Material activeMat;
    public Material inactiveMat;

    public GameObject statusPF;

    public bool isThreadActive = false;

    private void Start()
    {
        mesh = transform.GetChild(0).gameObject;
        meshRenderer = mesh.GetComponent<MeshRenderer>();
    }

    [Button]
    public void ChangeThreadState(bool _isActive)
    {
        isThreadActive = _isActive;
        
        if (isThreadActive )
        {
            meshRenderer.material = activeMat;
        }
        else
        {
            meshRenderer.material = inactiveMat;
        }
    }

    public void CharacterCollided(Character _character)
    {
        _character.TakeDamage(50);

        var slowStatus = Instantiate(statusPF, _character.transform);
    }
}
