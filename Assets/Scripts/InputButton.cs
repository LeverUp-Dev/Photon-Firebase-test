using UnityEngine;

public class InputButton : MonoBehaviour
{
    public static float VerticalInput;

    public enum State
    {
        None,
        Down,
        Up
    }

    private State state = State.None;

    private void Update()
    {
        // Player Control
        if (state == State.None)
        {
            VerticalInput = 0f;
        }
        else if (state == State.Up)
        {
            VerticalInput = 1f;
        }
        else if (state == State.Down)
        {
            VerticalInput = -1f;
        }
    }

    public void OnMoveUpButtonPressed()
    {
        state = State.Up;
    }

    public void OnMoveUpButtonUp()
    {
        // 동시 입력을 사용할 것이기 때문에 조건문을 걸어 주어야 한다
        if(state == State.Up)
        {
            state = State.None;
        }
    }

    public void OnMoveDownButtonPressed()
    {
        state = State.Down;
    }

    public void OnMoveDownButtonUp()
    {
        // 동시 입력을 사용할 것이기 때문에 조건문을 걸어 주어야 한다
        if (state == State.Down)
        {
            state = State.None;
        }
    }
}