using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;

    private ParticleSystem m_ParticleSystem;
    public List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                {
                    m_Instances[i].transform.position = m_Particles[i].position;
                    m_Instances[i].transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
                }
                else
                {
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                    m_Instances[i].transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
                }

                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
