using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarSelect : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_cars = new List<GameObject>();

    [SerializeField]
    private GameObject m_placeholderRender = null;

    void Start()
    {
        int carIndex = PlayerInfo.Instance.SelectedCar;
        if(carIndex < m_cars.Count)
        {
            GameObject render = Instantiate(m_cars[carIndex], transform);
            render.transform.position = m_placeholderRender.transform.position;
            render.transform.rotation = m_placeholderRender.transform.rotation;
            Destroy(m_placeholderRender);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
