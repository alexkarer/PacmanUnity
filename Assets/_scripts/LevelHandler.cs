using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{ 
    [SerializeField]
    private GameObject points = null;

    private PointRegulate pr = null;
    private TextMeshPro tmp = null;

    private int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshPro>();

        pr = points.GetComponent<PointRegulate>();

        pr.allPointsConsumed += Pr_allPointsConsumed;

        tmp.text = "00";
    }

    private void Pr_allPointsConsumed(object sender, PointRegulate.allPointsConsumedEventArgs e)
    {
        level++;
        tmp.text = level.ToString("00");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
