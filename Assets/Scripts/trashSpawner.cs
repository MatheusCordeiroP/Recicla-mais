using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashSpawner : MonoBehaviour
{
    private bool spawnPlastic;
    private bool spawnMetal;
    private bool spawnGlass;
    private bool spawnPaper;
    private bool spawnOrganic;

    [SerializeField]
    private GameObject[] plasticToSpawn;
    [SerializeField]
    private GameObject[] metalToSpawn;
    [SerializeField]
    private GameObject[] glassToSpawn;
    [SerializeField]
    private GameObject[] paperToSpawn;
    [SerializeField]
    private GameObject[] organicToSpawn;

    //Mine
    [SerializeField] AudioClip[] itemAppears = null;

    [SerializeField]
    private float timeToSpawn;

    private bool isRunning;

    //Mine
    AudioSource audioSource;

    private GameManager gm;


    void Start()
    {
        spawnPlastic = true;
        spawnMetal = true;
        spawnGlass = true;
        spawnPaper = true;
        spawnOrganic = true;

        //Mine
        audioSource = GetComponent<AudioSource>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        InvokeRepeating("spawnRandomObject", 0, timeToSpawn);
    }

    void Update()
    {
        isRunning = gm.GetIsRunning();
    }

    void spawnRandomObject()
    {

        if (!isRunning)
            return;

        do
        {
            int n = Random.Range(0, 5);
            
            if (n == 0 && spawnPlastic)
            {
                int objNumber = Random.Range(0, plasticToSpawn.Length);
                GameObject.Instantiate(plasticToSpawn[objNumber], this.gameObject.transform);
                audioSource.clip = itemAppears[0];
                audioSource.Play();
                return;
            }
            if (n == 1 && spawnMetal)
            {
                int objNumber = Random.Range(0, metalToSpawn.Length);
                GameObject.Instantiate(metalToSpawn[objNumber], this.gameObject.transform);
                audioSource.clip = itemAppears[1];
                audioSource.Play();
                return;
            }
            if (n == 2 && spawnGlass)
            {
                int objNumber = Random.Range(0, glassToSpawn.Length);
                GameObject.Instantiate(glassToSpawn[objNumber], this.gameObject.transform);
                audioSource.clip = itemAppears[2];
                audioSource.Play();
                return;
            }
            if (n == 3 && spawnPaper)
            {
                int objNumber = Random.Range(0, paperToSpawn.Length);
                GameObject.Instantiate(paperToSpawn[objNumber], this.gameObject.transform);
                audioSource.clip = itemAppears[3];
                audioSource.Play();
                return;
            }
            if (n == 4 && spawnOrganic)
            {
                int objNumber = Random.Range(0, organicToSpawn.Length);
                GameObject.Instantiate(organicToSpawn[objNumber], this.gameObject.transform);
                audioSource.clip = itemAppears[4];
                audioSource.Play();
                return;
            }

        } while (true);
    }
    
}
