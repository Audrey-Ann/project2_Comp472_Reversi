using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	[SerializeField]
	GameObject prefab;

    public static Board board;

    public Tile[,] squares = new Tile[8,8];

    Dictionary<Tile, List<Tile>> tilesPlusNeighbours;

    public static List<Board> listBoardStates = new List<Board>();

    List<Tile> tileToFlip;

    Tile.State turn;

    void Start () {

    tilesPlusNeighbours = new Dictionary<Tile, List<Tile>>();

    tileToFlip = new List<Tile>();

    turn = Tile.State.BLACK;

    board = this;

        for (int i = 0; i < 8; i++)
        {
			for (int j = 0; j < 8; j++)
            {
                squares[j, i] = Instantiate(prefab).GetComponent<Tile>();
				squares [j, i].transform.parent = this.transform;
                squares[j, i].x = j;
                squares[j, i].y = i;
			}
		}

        collectNeighbours(squares); 
        Reset();

    }

    public void OnClickTile ( Tile tile )
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log(tile.x + " " + tile.y);

            if (tile.CurrentState == Tile.State.FREE)
            {

                tile.CurrentState = turn;

                if (checkValidMove(tile, turn))
                {
                    listBoardStates.Add(board);
                    switchTurn(); 
                }
                else
                {
                    tile.CurrentState = Tile.State.FREE;
                }
            }
        }
        FlipBoard();

        // Debug.Log("click " + tile.x + ", " + tile.y);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Reset(); 
        }
	}

    bool checkValidMove(Tile tile, Tile.State clickedColour)
    {
        List<Tile> tileNeighbours = new List<Tile>(); 
            tileNeighbours = tilesPlusNeighbours[tile]; 
        //Check for neighbours: 
        foreach(Tile neighbour in tileNeighbours)
        {   //Check if we can flip and check neighbours of the clicked tile:  
            if (check(turn) && neighbour.CurrentState != Tile.State.FREE && neighbour.CurrentState != clickedColour){return true;}
        }

        return false; 
    }

    void FlipBoard()
    {
        if (tileToFlip.Count > 0)
        {
            foreach (Tile tile in tileToFlip)
            {
               // Debug.Log(squares[tile.y, tile.x].y + " " +  squares[tile.y, tile.x].x);

                if (squares[tile.x, tile.y].CurrentState == Tile.State.WHITE)
                {
                    squares[tile.x, tile.y].CurrentState = Tile.State.BLACK;
                }
                else if (squares[tile.x, tile.y].CurrentState == Tile.State.BLACK)
                {
                    squares[tile.x, tile.y].CurrentState = Tile.State.WHITE;
                }
            }

            tileToFlip.Clear();
        }
        else
        {
            Debug.Log("Nothing to be flipped"); 
        }
    }

    private bool check(Tile.State turn)
    {
        for (int row = 0; row < 8; row++)
        {
            if (checkRow(row, turn))
            {
                return true;
            }
        }
        for (int col = 0; col < 8; col++)
        {
            if (checkColumn(col, turn))
            {
                return true;
            }
        }
        return false;
    }

    private bool checkRow(int row, Tile.State turnState)
    {
        bool valid = false;
        int col = 0;
        int playerColorTile = 0;
        int opponentColorTile = 0;
        bool nextToEachOther = false;

        while (col < 8)
        {
            if (squares[row, col].CurrentState == turnState)
            {
                playerColorTile++;
                //Make sure that the player's tiles are not next to each other: 
                if (playerColorTile > 1 && squares[row, col].CurrentState == squares[row, col - 1].CurrentState)
                {
                    nextToEachOther = true;
                }
            }
            if (playerColorTile > 0 && squares[row, col].CurrentState != turnState && squares[row, col].CurrentState != Tile.State.FREE)
            {
                //check if the tile before the oppenent one is not a free tile: 
                if (squares[row, col - 1].CurrentState != Tile.State.FREE)
                {
                    tileToFlip.Add(squares[row, col]);
                    opponentColorTile++;
                }
            }
            col++;
        }

        //check constraints
        if (playerColorTile >= 2 && opponentColorTile > 0 && nextToEachOther == false)
        {
            valid = true;
        }

        if(valid == false)
        {
            //purge the list of tiles to be flipped: 
            tileToFlip.Clear();
        }

        return valid;
    }
   
    private bool checkColumn(int col, Tile.State turnState)
    {
        bool valid = false;
        int row = 0;
        int playerColorTile = 0;
        int opponentColorTile = 0;
        bool nextToEachOther = false;

        // count consecutive black tiles
        while (row < 8)
        {
            if (squares[row, col].CurrentState == turnState)
            {
                playerColorTile++;

                if (playerColorTile > 1 && squares[row, col].CurrentState == squares[row - 1, col].CurrentState)
                {
                 //   nextToEachOther = true;
                }
            }
            if (playerColorTile > 0 && squares[row, col].CurrentState != turnState && squares[row, col].CurrentState != Tile.State.FREE)
            {
                //check if the tile before the oppenent one is not a free tile: 
                if (squares[row - 1, col].CurrentState != Tile.State.FREE)
                {
                    tileToFlip.Add(squares[row, col]);
                    opponentColorTile++;
                }
            }
            row++;
        }

        // check constraint
        if (playerColorTile >= 2 && opponentColorTile > 0 && nextToEachOther == false)
        {
            valid = true;
        }

        if (valid == false)
        {
            //purge the list of tiles to be flipped: 
            tileToFlip.Clear();
        }

        return valid;
    }

    /*
    private bool checkDiagonals(Tile.State turnState)
    {
        bool valid = false;
        int playerColorTile = 0;
        int opponentColorTile = 0;
        bool nextToEachOther = false;

        int other = 0; 
        for (int row = 0; row < 8; row++)
        {
            for(int i = 0; i < 8; i++)
            {

            if (squares[row + i, other + i].CurrentState == turnState)
            {
                playerColorTile++;

                if (playerColorTile > 1 && squares[row + i, other + i].CurrentState == squares[row, other].CurrentState)
                {
                    //   nextToEachOther = true;
                }
            }
            if (playerColorTile > 0 && squares[row + i, other + i].CurrentState != turnState && squares[row + i, other + i].CurrentState != Tile.State.FREE)
            {
                //check if the tile before the oppenent one is not a free tile: 
                if (squares[row, other].CurrentState != Tile.State.FREE)
                {
                    tileToFlip.Add(squares[row, other]);
                    opponentColorTile++;
                }
            }
            row++;
        }
    }
        // check constraint
        if (playerColorTile >= 2 && opponentColorTile > 0 && nextToEachOther == false)
        {
            valid = true;
        }

        if (valid == false)
        {
            //purge the list of tiles to be flipped: 
            tileToFlip.Clear();
        }

        return valid;
    }*/

    void switchTurn()
    {
        if (turn == Tile.State.BLACK)
        {
            turn = Tile.State.WHITE;
        }
        else if (turn == Tile.State.WHITE)
        {
            turn = Tile.State.BLACK;
        }
    }

    void Reset()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squares[j, i].CurrentState = Tile.State.FREE;
            }
        }

        squares[3, 3].CurrentState = Tile.State.WHITE;
        squares[4, 4].CurrentState = Tile.State.WHITE;
        squares[3, 4].CurrentState = Tile.State.BLACK;
        squares[4, 3].CurrentState = Tile.State.BLACK;
    }

    void collectNeighbours(Tile[,] list)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                List<Tile> neighbours = new List<Tile>();

                //corners
                if (i % 7 == 0 && j % 7 == 0)
                {
                    if (i == 0 && j == 0)
                    {
                        neighbours.Add(squares[j, i + 1]);
                        neighbours.Add(squares[j + 1, i]);
                        neighbours.Add(squares[j + 1, i + 1]);
                    }
                    else if (i == 7 && j == 7)
                    {
                        neighbours.Add(squares[j - 1, i - 1]);
                        neighbours.Add(squares[j - 1, i]);
                        neighbours.Add(squares[j, j - 1]);
                    }
                    else if (i == 0 && j == 7)
                    {
                        neighbours.Add(squares[j - 1, i]);
                        neighbours.Add(squares[j - 1, i + 1]);
                        neighbours.Add(squares[j, i + 1]);
                    }
                    else
                    {
                        neighbours.Add(squares[j, i - 1]);
                        neighbours.Add(squares[j + 1, i - 1]);
                        neighbours.Add(squares[j + 1, i]);
                    }

                }

                //first row and column: 
                else if (i > 0 && i < 7 && j == 7)
                {
                    neighbours.Add(squares[j - 1, i - 1]);
                    neighbours.Add(squares[j - 1, i]);
                    neighbours.Add(squares[j - 1, i + 1]);

                    neighbours.Add(squares[j, i - 1]);
                    neighbours.Add(squares[j, i + 1]);
                }
                //first row and column: 
                else if (i > 0 && i < 7 && j == 0)
                {

                    neighbours.Add(squares[j, i - 1]);
                    neighbours.Add(squares[j, i + 1]);

                    neighbours.Add(squares[j + 1, i - 1]);
                    neighbours.Add(squares[j + 1, i]);
                    neighbours.Add(squares[j + 1, i + 1]);
                }
                //first row and column: 
                else if (j > 0 && j < 7 && i == 0)
                {
                    neighbours.Add(squares[j - 1, i]);
                    neighbours.Add(squares[j - 1, i + 1]);

                    neighbours.Add(squares[j, i + 1]);

                    neighbours.Add(squares[j + 1, i]);
                    neighbours.Add(squares[j + 1, i + 1]);
                }
                //first and last row and column: 
                else if (j > 0 && j < 7 && i == 7)
                {
                    neighbours.Add(squares[j - 1, i - 1]);
                    neighbours.Add(squares[j - 1, i]);

                    neighbours.Add(squares[j, i - 1]);

                    neighbours.Add(squares[j + 1, i - 1]);
                    neighbours.Add(squares[j + 1, i]);
                }


                //inside
                else if (i != 0 && i != 7 && j != 0 && j != 7)
                {
                    neighbours.Add(squares[j - 1, i - 1]);
                    neighbours.Add(squares[j - 1, i]);
                    neighbours.Add(squares[j - 1, i + 1]);

                    neighbours.Add(squares[j, i - 1]);
                    neighbours.Add(squares[j, i + 1]);

                    neighbours.Add(squares[j + 1, i - 1]);
                    neighbours.Add(squares[j + 1, i]);
                    neighbours.Add(squares[j + 1, i + 1]);
                }

                tilesPlusNeighbours.Add(squares[j, i], neighbours);
            }
        }
    }


}
