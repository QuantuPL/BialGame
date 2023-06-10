using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public bool isDepleted;
    public GameObject spawned;   
    
    public void Deplete()
    {
        if (!isDepleted)
        {
            var s = Instantiate(spawned);
            s.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }

    public void Respawn()
    {
        if (isDepleted)
        {
            var s = Instantiate(spawned);
            s.transform.position = transform.position;
            Destroy(this.gameObject);

        }
    }
}
