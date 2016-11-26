using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brains : MonoBehaviour {

    public int miniMax(Board board, int depth, Tile.State turn) {
        if (depth == 0 || !board.movePossible(board, turn)) {
            return evaluate(board);
        }
        if (board.turn == Tile.State.BLACK) {
            // MAX

        }
        else if (board.turn == Tile.State.WHITE) {
            // MIN
        }

    }
    public int evaluate(Board board) {
        int score = 0;
        return score;
    }
}
