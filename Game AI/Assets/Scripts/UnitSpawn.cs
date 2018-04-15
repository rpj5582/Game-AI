using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawn : MonoBehaviour {

    [SerializeField]
    private GameObject unitPrefab;

    [SerializeField]
    private Text flockParametersText;

    //The maximum number of units we will allow on the field at once
    private float maxUnits;

    //Index 0 is red team, index 1 is green team
    private List<GameObject>[] unitTeams;

    //How many units are currently spawned
    private float unitCount;

    #region Properties

    public Text FlockParametersText {
        get {
            return flockParametersText;
        }

        set {
            flockParametersText = value;
        }
    }

    #endregion

    void Awake() {
        unitTeams = new List<GameObject>[2];

        unitTeams[0] = new List<GameObject>();
        unitTeams[1] = new List<GameObject>();
    }

    void Update() {
        CheckSpawn();
    }

    private void SpawnUnit(Vector3 _pos, Unit.TEAM _team) {
        GameObject newUnit = Instantiate(unitPrefab, _pos, Quaternion.identity);
        
        //Initializes the unit's visuals and properties
        newUnit.GetComponent<Unit>().InitUnit(_team, Unit.UNIT_LVL.LVL4);
        
        //Add the unit to the correct team 
        if(_team == Unit.TEAM.RED) {
            unitTeams[0].Add(newUnit);
        } else {
            unitTeams[1].Add(newUnit);
        }

        unitCount++;
    }

    //Waits for user input to create a new unit
    private void CheckSpawn() {
        if(Input.GetMouseButtonDown(0)) {

            //Find the correct location to place the unit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit)) {
                SpawnUnit(hit.point, Unit.TEAM.RED);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

            //Find the correct location to place the unit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SpawnUnit(hit.point, Unit.TEAM.GREEN);
            }
        }
    }
}
