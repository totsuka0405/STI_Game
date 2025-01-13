using System;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.ItemType itemType;
    [SerializeField] AudioClip clip;
    Item item;
    
    private void Start()
    {
        item = ItemGenerator.instance.Spawn(itemType);
        switch (itemType)
        {
            case Item.ItemType.hand_light:
                GameManager.instance.isHaveHandLight = false;
                break;
            case Item.ItemType.phone:
                GameManager.instance.isHavePhone = false;
                break;
            case Item.ItemType.whistle:
                GameManager.instance.isHaveWhistle = false;
                break;
            case Item.ItemType.gloves:
                GameManager.instance.isHaveGloves = false;
                break;
            case Item.ItemType.slippers:
                GameManager.instance.isHaveSlipper = false;
                break;
            case Item.ItemType.bottle:
                GameManager.instance.isHaveBottle = false;
                break;
            case Item.ItemType.radio:
                GameManager.instance.isHaveRadio = false;
                break;
            case Item.ItemType.firstaidkit:
                GameManager.instance.isHaveFirstAidKit = false;
                break;
            default:
                break;

        }
    }

    public void OnClickObj()
    {
        ItemBox.instance.SetItem(item);
        switch (itemType)
        {
            case Item.ItemType.hand_light:
                GameManager.instance.isHaveHandLight = true;
                break;
            case Item.ItemType.phone:
                GameManager.instance.isHavePhone = true;
                break;
            case Item.ItemType.whistle:
                GameManager.instance.isHaveWhistle = true;
                break;
            case Item.ItemType.gloves:
                GameManager.instance.isHaveGloves = true;
                break;
            case Item.ItemType.slippers:
                GameManager.instance.isHaveSlipper = true;
                break;
            case Item.ItemType.bottle:
                GameManager.instance.isHaveBottle = true;
                break;
            case Item.ItemType.radio:
                GameManager.instance.isHaveRadio = true;
                break;
            case Item.ItemType.firstaidkit:
                GameManager.instance.isHaveFirstAidKit = true;
                break;
            default:
                break;

        }
        SoundManager.instance.PlaySE(clip, transform.position, this.transform);
        gameObject.SetActive(false);
    }
}
