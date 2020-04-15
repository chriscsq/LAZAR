using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DifficultyToggle : MonoBehaviour
{
    ToggleGroup difficultyToggleGroup;

    // Start is called before the first frame update
    void Start()
    {
        difficultyToggleGroup = GetComponent<ToggleGroup>();
        Debug.Log("Selected" + CurrentSelection.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Toggle CurrentSelection
    {
        get
        {
            return difficultyToggleGroup.ActiveToggles().FirstOrDefault();
        }
    }

}
