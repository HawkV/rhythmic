using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Note;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject despawner;

    public void Init(GameObject despawner) {
        this.despawner = despawner;
        StartCoroutine(EndlessSpawn());
    }

    public GameObject Spawn() {
        var note = Instantiate(Note, transform, worldPositionStays:false);
        note.GetComponent<NoteController>().Init(transform.position, despawner.transform.position, speed);
        note.transform.position = transform.position;

        return note;
    }

    public IEnumerator EndlessSpawn() {
        while (true) {
            Spawn();
            yield return new WaitForSeconds(1f);
        }
    }
}
