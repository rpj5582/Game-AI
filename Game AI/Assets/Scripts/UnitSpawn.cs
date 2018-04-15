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
    private List<Unit> units;

    //How many units are currently spawned
    private float unitCount;

    //GO to hold UI elements for each unit in the canvas
    [SerializeField]
    private GameObject unitUIContainer;

    [SerializeField]
    private GameObject unitUIElement;

    private Unit.UNIT_LVL selectedLevel = Unit.UNIT_LVL.WHITE;

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
        units = new List<Unit>();
    }

    void Update() {
        CheckSpawn();
        UpdateUnitsUI();
    }

    //Initializes the unit's visuals and properties
    private void SpawnUnit(Vector3 _pos, Unit.TEAM _team) {
        GameObject newUnit = Instantiate(unitPrefab, _pos, Quaternion.identity);

        GameObject newUnitUI = Instantiate(unitUIElement, unitUIContainer.transform);
        newUnitUI.GetComponent<Image>().color = _team == Unit.TEAM.RED ? Color.red : Color.green;
        
        newUnit.GetComponent<Unit>().UiElement = newUnitUI.GetComponent<RectTransform>();
        newUnit.GetComponent<Unit>().InitUnit(_team, selectedLevel);

        units.Add(newUnit.GetComponent<Unit>());

        unitCount++;
    }

    //Sets the position of all UI elements associated with each unit
    private void UpdateUnitsUI() {
        for(int i = 0; i < units.Count; i++) {
            units[i].UiElement.position = Camera.main.WorldToScreenPoint(units[i].transform.position);
        }
    }

    //Waits for user input to create a new unit
    private void CheckSpawn() {
        //Decide what strenght level the new unit should be
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedLevel = Unit.UNIT_LVL.WHITE;
        }else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedLevel = Unit.UNIT_LVL.BLUE;
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedLevel = Unit.UNIT_LVL.YELLOW;
        } else if(Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedLevel = Unit.UNIT_LVL.BLACK;
        }

        //Find the correct location to place the unit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (Input.GetMouseButtonDown(0)) {   
                SpawnUnit(hit.point, Unit.TEAM.RED);
            } else if (Input.GetMouseButtonDown(1)){
                SpawnUnit(hit.point, Unit.TEAM.GREEN);
            }
        }
    }
}
