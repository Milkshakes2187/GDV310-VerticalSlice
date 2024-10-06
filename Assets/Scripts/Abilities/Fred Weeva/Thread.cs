using UnityEngine;
using VInspector;

public class Thread : MonoBehaviour
{
<<<<<<< Updated upstream
    bool isThreadActive = false;

    [Tab("Data")]
    public Material activeMaterial;
    public Material inactiveMaterial;

    [Tab("References")]
    MeshRenderer threadMeshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeThreadState(bool _isActive)
    {
        isThreadActive = _isActive;

        if (isThreadActive)
        {
            threadMeshRenderer.materials[0] = activeMaterial;
        }
        else
        {
            threadMeshRenderer.materials[0] = inactiveMaterial;
=======
    public GameObject mesh;
    MeshRenderer meshRenderer;
    public Material activeMat;
    public Material inactiveMat;

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
>>>>>>> Stashed changes
        }
    }
}
