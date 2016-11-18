using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum State { FREE, WHITE, BLACK };

    public int x;
    public int y;

    private State state;

    public UnityEngine.UI.Image image;

    public State CurrentState
    {
        get
        {
            return state;
        }

        set
        {
            switch (value)
            {
                case State.FREE:
                    state = value;
                    image.color = new Color(0, 0, 0, 0);
                    break;
                case State.WHITE:
                    state = value;
                    image.color = Color.white;
                    break;
                case State.BLACK:
                    state = value;
                    image.color = Color.black;
                    break;
            }
        }
    }

	// Use this for initialization
	void Awake ()
    {
        image.color = new Color(0, 0, 0, 0);
        state = State.FREE;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void setState(State s)
    {
        state = s;
    }

    public void OnClick ()
    {
        Board.board.OnClickTile(this);
    }
}
