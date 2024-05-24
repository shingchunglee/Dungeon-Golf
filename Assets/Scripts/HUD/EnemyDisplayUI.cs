using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class EnemyDisplayUI : MonoBehaviour
{
    public string parentFolderPath = "Final"; 
    public GameObject slotEnemy; 
    public Transform EnemyGrid; 

    private void Start()
    {
        LoadAllEnemies();
    }

    void LoadAllEnemies()
    {
        AccessSubFolder(parentFolderPath);
    }

    void AccessSubFolder(string folderPath)
    {
       
        Object[] prefabs = Resources.LoadAll(folderPath, typeof(GameObject));
        foreach (Object prefab in prefabs)
        {
            GameObject prefabObject = (GameObject)prefab;
            CreateSlot(prefabObject);
        }

        
        string fullPath = Path.Combine(Application.dataPath, "Resources", folderPath);
        string[] subfolders = Directory.GetDirectories(fullPath);
        foreach (string subfolder in subfolders)
        {
            string subfolderPath = Path.Combine(folderPath, Path.GetFileName(subfolder));
            AccessSubFolder(subfolderPath.Replace("\\", "/"));
        }
    }

    void CreateSlot(GameObject prefab)
    {
        GameObject slot = Instantiate(slotEnemy, EnemyGrid);

        
        EnemyUnit enemyUnit = prefab.GetComponent<EnemyUnit>();

       
        string enemyInfo = $"Damage: {enemyUnit.attackDamage}\n" +
                           $"Max HP: {enemyUnit.MaxHP}\n" +
                           $"Max Attacks/Turn: {enemyUnit.maxAttacksPerTurn}\n" +
                           $"Move Attacks/Turn: {enemyUnit.moveAttacksPerTurn}";

       
        slot.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = enemyUnit.enemyName;

        
        slot.transform.Find("Info").GetComponent<TextMeshProUGUI>().text = enemyInfo;

        
        Image enemyImage = slot.transform.Find("Image").GetComponent<Image>();
        SpriteRenderer spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            enemyImage.sprite = spriteRenderer.sprite;
        }
    }
}
