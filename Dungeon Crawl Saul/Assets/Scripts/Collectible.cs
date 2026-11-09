using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Player player;
    public int direction = 1;
    private float height = 2;
    private float timePassed = 0;
    void Update()
    {
        Vector2 position = transform.position;
        position.y += (height * Time.deltaTime * direction);
        transform.position = position;

        timePassed += Time.deltaTime;
        if (timePassed > 2)
        {
            timePassed = 0;
            direction *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.tag == "Red Gem" && collision.gameObject.tag == "Player")
        {
            player.AddRedGemCollectible();
            Destroy(this.gameObject);
        }
        if (this.tag == "Green Gem" && collision.gameObject.tag == "Player")
        {
            player.AddGreenGemCollectible();
            Destroy(this.gameObject);
        }
        if (this.tag == "Purple Gem" && collision.gameObject.tag == "Player")
        {
            player.AddPurpleGemCollectible();
            Destroy(this.gameObject);
        }
        if (this.tag == "Turquoise Gem" && collision.gameObject.tag == "Player")
        {
            player.AddTurquoiseGemCollectible();
            Destroy(this.gameObject);
        }

    }
}