using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text text;

    private int _score = 0;
    
    
    #region Singleton

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("UI Manager is Null");
            return _instance;
        }
    }

    #endregion
    
    

    private void Awake()
    {
        if(_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this;
        }
        UpdateText(0);
    }


    public void UpdateText(int i)
    {
        _score += i;
        if(text != null)
            text.text = _score.ToString();
    }

}

