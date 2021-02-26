using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    int maxKill = 20;

    [SerializeField]
    Text killText;

    [SerializeField]
    Player player;

    [SerializeField]
    Shot shot;

    [SerializeField]
    Text centerText;

    [SerializeField]
    float waitTime = 2;

    [SerializeField]
    EnemySpawner[] spawners;

    int kill = 0;
    bool gameClear = false;

    Animator animator;

    public int Kill
    {
        set
        {
            kill = value;
            killText.text = kill.ToString() + "/" + maxKill.ToString();
            
            if(kill >= maxKill)
            {
                StartCoroutine(GameClear());
            }
        }

        get
        {
            return kill;
        }
    }

    public IEnumerator GameClear()
    {
        if(!gameClear)
        {
            yield return new WaitForSeconds(1.0f);
            gameClear = true;
            shot.shootEnabled = false;
            SetSpawners(false);
            centerText.gameObject.SetActive(true);
            centerText.text = "GameClear!!";
            StopEnemies();
            yield return new WaitForSeconds(waitTime);
            centerText.text = "";
            centerText.enabled = false;
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        else
        {
            yield return null;
        }
    }

    void StopEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            EnemyController controller = enemy.GetComponent<EnemyController>();
            controller.moveEnabled = false;
        }
    }

    void SetSpawners(bool isEnable)
    {
        foreach(EnemySpawner spawner in spawners)
        {
            spawner.spawnEnabled = isEnable;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Kill = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
