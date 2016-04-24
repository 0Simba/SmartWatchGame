using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    Transform _transform;
    enum state { direction, power, movement };

    delegate void StateAction();
    StateAction currAction;

    void Start ()
    {
        _transform = transform;

	}
	

	void Update ()
    {
        currAction();
    }

    void ChangeState(state selection)
    {
        switch (selection)
        {
            case state.direction:
                currAction = ChooseDirection;
                break;
            case state.power:
                currAction = ChoosePower;
                break;
            case state.movement:
                currAction = ExecuteMovement;
                break;
        }
    }

    void ChooseDirection()
    {

    }

    void ChoosePower()
    {

    }

    void ExecuteMovement()
    {

    }
}
