using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	[SerializeField]
	GameObject prefab;

    public static Board board;

    public const int NUMBER_OF_COLUMNS = 8;

    public const int NUMBER_OF_ROWS = 8;


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
		Debug.Log("Number of flips for this move: " + numberOfFlips(tile, turn));
		Debug.Log("Is this a valid move? " + isValidMove(tile, turn));
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (tile.CurrentState == Tile.State.FREE)
            {
                // tile.CurrentState = turn;
				// Debug.Log(tile.CurrentState);

                if (check(tile, turn))
                {
					FlipBoard();
                    listBoardStates.Add(board);
					tile.CurrentState = turn;
                    switchTurn();
                }
                // else
                // {
                //     tile.CurrentState = Tile.State.FREE;
                // }
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Reset();
        }
	}

    void FlipBoard()
    {
        if (tileToFlip.Count > 0)
        {
            foreach (Tile tile in tileToFlip)
            {
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
	private bool isOnBoard(int x, int y) {
		// Returns true if the coordinates are on the board
		return x >= 0 && x <= 7 && y >= 0 && y <= 7;
	}
	bool tilesToBeFlippedUp(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going up
		int x = xStart;
		int y = yStart - 1;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					y--;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							y++;
						}
					}
				}
				if (validMove == true) {
					while (y < yStart) {
						tileToFlip.Add(squares[x, y]);
						y++;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedDown(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart;
		int y = yStart + 1;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					y++;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							y--;
						}
					}
				}
				if (validMove == true) {
					while (y > yStart) {
						tileToFlip.Add(squares[x, y]);
						y--;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedLeft(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart - 1;
		int y = yStart;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					x--;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							x++;
						}
					}
				}
				if (validMove == true) {
					while (x < xStart) {
						tileToFlip.Add(squares[x, y]);
						x++;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedRight(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart + 1;
		int y = yStart;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					x++;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							x--;
						}
					}
				}
				if (validMove == true) {
					while (x > xStart) {
						tileToFlip.Add(squares[x, y]);
						x--;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedDiagonalUpRight(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart + 1;
		int y = yStart - 1;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					x++;
					y--;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							x--;
							y++;
						}
					}
				}
				if (validMove == true) {
					while (x > xStart && y < yStart) {
						tileToFlip.Add(squares[x, y]);
						x--;
						y++;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedDiagonalDownRight(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart + 1;
		int y = yStart + 1;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					x++;
					y++;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							x--;
							y--;
						}
					}
				}
				if (validMove == true) {
					while (x > xStart && y > yStart) {
						tileToFlip.Add(squares[x, y]);
						x--;
						y--;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedDiagonalDownLeft(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart - 1;
		int y = yStart + 1;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					x--;
					y++;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							x++;
							y--;
						}
					}
				}
				if (validMove == true) {
					while (x < xStart && y > yStart) {
						tileToFlip.Add(squares[x, y]);
						x++;
						y--;
					}
				}
			}
		}
		return validMove;
	}
	bool tilesToBeFlippedDiagonalUpLeft(int xStart, int yStart, Tile.State turnState) {
		bool validMove = false;
		// Returns a list of Tiles to be flipped; empty list if move is not valid.
		if (!isOnBoard(xStart, yStart)) {
			return validMove;
		}
		// Check if any tiles to flip going right
		int x = xStart - 1;
		int y = yStart - 1;
		if (isOnBoard(x, y)) {
			Tile tileUp = squares[x, y];
			if (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE) {
				// There is a piece of opposite color above ours
				while (tileUp.CurrentState != turnState && tileUp.CurrentState != Tile.State.FREE && validMove == false) {
					x--;
					y--;
					if (isOnBoard(x, y)) {
						tileUp = squares[x, y];
						if (tileUp.CurrentState == turnState) {
							validMove = true;
							x++;
							y++;
						}
					}
				}
				if (validMove == true) {
					while (x < xStart && y < yStart) {
						tileToFlip.Add(squares[x, y]);
						x++;
						y++;
					}
				}
			}
		}
		return validMove;
	}

    private bool check(Tile tile, Tile.State turn)
    {
        return tilesToBeFlippedUp(tile.x, tile.y, turn) | tilesToBeFlippedDown(tile.x, tile.y, turn) | tilesToBeFlippedLeft(tile.x, tile.y, turn) | tilesToBeFlippedRight(tile.x, tile.y, turn) | tilesToBeFlippedDiagonalUpRight(tile.x, tile.y, turn) | tilesToBeFlippedDiagonalDownRight(tile.x, tile.y, turn) | tilesToBeFlippedDiagonalDownLeft(tile.x, tile.y, turn) | tilesToBeFlippedDiagonalUpLeft(tile.x, tile.y, turn);
    }
	private int numberOfFlips(Tile tile, Tile.State turnState) {
		check(tile, turnState);
		int numberOfFlips = tileToFlip.Count;
		tileToFlip.Clear();
		return numberOfFlips;
	}
	private bool isValidMove(Tile tile, Tile.State turnState) {
		// If number of flips > 0, return true
		return numberOfFlips(tile, turnState) > 0 ? true:false;
	}
	private bool skipTurn(Tile tile, Tile.State turnState) {
		foreach (Tile tile in squares) {
			if (numberOfFlips(tile, turnState) > 0) {
				return false;
			}
		}
		switchTurn();
		return true;
	}

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
