
using UnityEngine;
using TMPro;

public class HexagonBomb : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private int counter;

    private void OnEnable()
    {
        counter = 9;
        UpdateText();
    }

    private void OnDisable()
    {
        HexagonManager.Instance.bombs.Remove(gameObject);
        Destroy(gameObject);
    }


    public void UpdateText()
    {
        counter--;
        text.text = counter.ToString();
        if (counter == 0)
        {
            GameManager.Instance.RestartLevel();
        }
    }
    
}
