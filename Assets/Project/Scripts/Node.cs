using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
    private Node parent;
    private Board board;
    private bool visited;
    private int cost = 0;

    public Node() {
        this.parent = null;
        this.board = null;
        this.visited = false;
        this.cost = 0;
    }
    public Node(Board board, Node parent, bool visited, int cost) {
        this.board = board;
        this.parent = parent;
        this.visited = visited;
        this.cost = cost;
    }
    public void setParent(Node parent) {
        this.parent = parent;
    }
    public Node getParent() {
        return this.parent;
    }
    public void setBoard(Board board) {
        this.board = board;
    }
    public Board getBoard() {
        return this.board;
    }
    public void setCost(int cost) {
        this.cost = cost;
    }
    public int getCost() {
        return this.cost;
    }
}
