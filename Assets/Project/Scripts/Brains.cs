using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brains : MonoBehaviour {

    public int miniMax(Node node, int depth, Tile.State turn) {
        if (depth == 0 || !node.board.movePossible(turn)) {
            return evaluate(node.board);
        }
        if (turn == Tile.State.BLACK) {
            // MAX
            int bestValue = -10000000;


        }
        else if (turn == Tile.State.WHITE) {
            // MIN
        }

    }

    // Heuristic evaluation function
    public int evaluate(Board board) {
        int score = 0;
        return score;
    }

    public List<Node> generateSuccessors(Node currentBoard, Tile.State.Turn) {
        Node b = new Node();

        foreach(Tile tile in currentBoard.board) {
            if (tile.CurrentState == Tile.State.FREE && currentBoard.board.check(tile, turn)) {
                Board board = currentBoard.board;
                b.setParent(currentBoard);
                b.setBoard(board);

            }

        }
    }
}
