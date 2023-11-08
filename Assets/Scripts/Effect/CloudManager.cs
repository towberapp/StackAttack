using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [Range(1f, 10f)]
    [SerializeField] private float speedCloud = 1f;

    [Range(1f, 10f)]
    [SerializeField] private float cloudFrequinsy = 2f;

    [SerializeField] private GameObject cloudBase;
    [SerializeField] private List<Sprite> sprite;

    private IEnumerator coroutine;

    private void Awake()
    {
        coroutine = GenerateCloud();

        EventsController.PreStartEvent.AddListener(OnPreStartGame);
        EventsController.GameOverEvent.AddListener(OnGameOver);        
    }

    private void OnDestroy()
    {
        EventsController.PreStartEvent.RemoveListener(OnPreStartGame);
        EventsController.GameOverEvent.RemoveListener(OnGameOver);
    }

    private void OnGameOver()
    {
        StopCoroutine(coroutine);
        //
    }

    private void OnPreStartGame(int row)
    {
        StartCoroutine(coroutine);
    }

    private IEnumerator GenerateCloud()
    {
        while(true)
        {
            GameObject cloud = Instantiate(cloudBase);
                       cloud.transform.position = new Vector3(-5f, SystemStatic.level + 5f + Random.Range(0f, 5f), 0);

            Cloud component = cloud.GetComponent<Cloud>();
                  component.moveSpeed = Random.Range(0.1f, 0.2f) * speedCloud;            
                  component.SetSprite(GetRandomSprite());

            new WaitForSeconds(Random.Range(1f,2.5f) * cloudFrequinsy);
        }
        
    }


    Sprite GetRandomSprite()
    {
        if (sprite.Count == 0)
        {
            Debug.LogWarning("Список sprite пуст.");
            return null;
        }

        int randomIndex = Random.Range(0, sprite.Count);
        return sprite[randomIndex];
    }
}
