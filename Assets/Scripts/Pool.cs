using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] private T sample;
    [SerializeField] private int objectsCount;
    public T[] ObjectsPool { get; private set; }

    private void Start() {
        ObjectsPool = new T[objectsCount];
        for (var i = 0; i < objectsCount; i++) {
            var o = Instantiate(sample, transform);
            o.transform.localPosition = Vector3.zero;
            o.gameObject.SetActive(false);
            ObjectsPool[i] = o;
        }
    }

    public T GetPooledObject() {
        var o = ObjectsPool.FirstOrDefault(t => !t.gameObject.activeSelf);

        return o;
    }
}