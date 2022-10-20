using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterAssets
{
    public class TrashHeap : MonoBehaviour
    {

        public GameObject[] trashPrefabs = new GameObject[10];
        public GameObject[] spawnPos = new GameObject[0];
        public int amount;
        public int minimum;
        public int maximum;

        private OnLevelStartLoad onLevelStart;


        public bool trashSpawned = false;



        // Start is called before the first frame update
        void Start()
        {

            amount = Random.Range(minimum, maximum);
            onLevelStart = GameObject.Find("OnLevelStart").GetComponent<OnLevelStartLoad>();

            for (int i = 0; i < amount; i++)
            {


                GameObject _gameObject = Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Length)], spawnPos[Random.Range(0, spawnPos.Length)].transform.position, Quaternion.identity);
                onLevelStart.AddTrashToList(_gameObject);
                Debug.Log("Adding " + _gameObject + " to list");

            }

        }

        // Update is called once per frame
        void Update()
        {


        }
        public void CreateEnemies()
        {
            Debug.Log("Spawn");
            for (int i = 0; i < amount; i++)
            {


                Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Length)], spawnPos[Random.Range(0, spawnPos.Length)].transform.position, Quaternion.identity);


            }
        }
    }
}
