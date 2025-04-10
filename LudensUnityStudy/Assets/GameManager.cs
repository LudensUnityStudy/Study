using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    // UI
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject RestartButton;

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                // Player Reposition
                PlayerReposition();
            }

            // Health Down
            HeathDown();

        }
    }

    public void HeathDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
            
        else
        {
            // All Health UI Off
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);

            // Player Die Effect
            player.OnDie();
            // Player UI

            // Retry Button UI
            RestartButton.SetActive(true);
        }
    }

    void PlayerReposition()
    {
        player.VelocityZero();
        player.transform.position = new Vector3(-3, 1.4f, -1);
    }

    public void NextStage()
    {
        // Change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else // Game Clear
        {
            // Player Control Lock
            Time.timeScale = 0;
            // Reset UI

            // Restart Button UI
            Text text = RestartButton.GetComponentInChildren<Text>();
            text.text = "Clear";
            RestartButton.SetActive(true);
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
