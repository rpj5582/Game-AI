using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public enum UNIT_LVL { WHITE = 1, BLUE = 2, YELLOW = 3, BLACK = 4 }
    private UNIT_LVL level;

    public enum TEAM { RED = 0, GREEN = 1 }
    private TEAM team;

    [SerializeField]
    private Material[] strengthMats;

    [SerializeField]
    private MeshRenderer teamRenderer, strengthRenderer;

    private RectTransform uiElement;

    #region Properties
    public TEAM Team {
        get {
            return team;
        }

        set {
            team = value;
        }
    }

    public UNIT_LVL Level {
        get {
            return level;
        }

        set {
            level = value;
        }
    }

    public RectTransform UiElement {
        get {
            return uiElement;
        }

        set {
            uiElement = value;
        }
    }
    #endregion

    private void Awake() {

    }

    public void InitUnit(TEAM _team, UNIT_LVL _lvl) {
        team = _team;
        level = _lvl;
            
        //Sets the unit's materil to reflect its team. Will chnage this to an overlay or icon later
        teamRenderer.material = strengthMats[(int)_lvl - 1];
    }
}
