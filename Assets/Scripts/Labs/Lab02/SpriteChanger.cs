using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite playerSprite;
    public GameObject playerGameObject;

    void OnMouseDown()
    {
        playerGameObject.GetComponent<SpriteRenderer>().sprite = playerSprite;
        playerGameObject.transform.localScale = new Vector3(3, 3, 0);
    }
}
