using Sirenix.OdinInspector;
using UnityEngine;

public class VisualDamageNbrSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabVisuel;
    [SerializeField] SOEventVisualNumber eventVisualNumber;

    private void OnEnable()
    {
        eventVisualNumber.CreateVisualNumber += CreateVisual;
    }

    private void OnDisable()
    {
        eventVisualNumber.CreateVisualNumber -= CreateVisual;
    }

    public void CreateVisual(EventVisualNbrData data)
    {

        Vector2 pos = new Vector2(data.spawnPoint.x , data.spawnPoint.y );

        GameObject newSingle = Instantiate(prefabVisuel, data.spawnPoint, transform.rotation, transform);


        newSingle.transform.position = pos;


        DataUIVisuel dataUIVisuel = new DataUIVisuel();
        dataUIVisuel.textColor = data.color;
        dataUIVisuel.text = data.isPositive ? data.nbr.ToString() : "-" + data.nbr.ToString();



        newSingle.GetComponent<SingleNbrDamageVisuel>().Initialise(dataUIVisuel);    
    }


}
