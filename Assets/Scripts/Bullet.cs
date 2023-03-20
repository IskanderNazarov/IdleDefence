using UnityEngine;

public class Bullet : MonoBehaviour {
    public int Damage { get; private set; }

    public void Activate(int damage) {
        Damage = damage;
        gameObject.SetActive(true);
    }
}