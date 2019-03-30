using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int id;
    public delegate void Destroyed(int id);
    Destroyed onDestroyed;

    public void RegisterDestroyedCallback(Destroyed des)
    {
        onDestroyed += des;
    }
    public void DeRegisterDestroyedCallback(Destroyed des)
    {
        onDestroyed -= des;
    }

    private void OnEnable()
    {
        GetComponent<Health>().RegisterHealthLostListner(OnHealthUpdated);
    }
    private void OnDisable()
    {
        GetComponent<Health>().DeRegisterHealthLostListner(OnHealthUpdated);
    }
    void OnHealthUpdated(float health)
    {
        if(health<=0)
        {
            onDestroyed(id);
        }
    }

}
