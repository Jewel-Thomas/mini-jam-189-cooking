using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Vector3 originPosition = Vector3.zero;
    public Grid grid;

    private Vector2Int? startCell = null;
    private Vector2Int? endCell = null;
    private List<Vector2Int> currentPath = null;

    void Start()
    {
        grid = new Grid(width, height, cellSize, originPosition);
        startCell = new Vector2Int(0, 0);
    }

    void Update()
    {
        // Left click: set end and find path
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            grid.GetXY(mouseWorld, out int x, out int y);
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                endCell = new Vector2Int(x, y);
                currentPath = FindPath(startCell.Value, endCell.Value);
            }
        }

        // Right mouse button held: paint obstacles
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            grid.GetXY(mouseWorld, out int x, out int y);
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                grid.SetValue(x, y, 1); // 1 = obstacle
                // Set debug text color to black
                var debugText = GetDebugText(x, y);
                if (debugText != null) debugText.color = Color.black;
            }
        }

        // Draw path
        if (currentPath != null && currentPath.Count > 1)
        {
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Vector3 a = gridToWorld(currentPath[i]);
                Vector3 b = gridToWorld(currentPath[i + 1]);
                Debug.DrawLine(a, b, Color.green);
            }
        }
    }

    // Helper to get debug text for a cell
    private TextMesh GetDebugText(int x, int y)
    {
        var field = typeof(Grid).GetField("debugTextArray", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            var arr = field.GetValue(grid) as TextMesh[,];
            if (arr != null && x >= 0 && x < arr.GetLength(0) && y >= 0 && y < arr.GetLength(1))
                return arr[x, y];
        }
        return null;
    }

    public Vector3 gridToWorld(Vector2Int cell)
    {
        return originPosition + new Vector3(cell.x, cell.y) * cellSize + new Vector3(cellSize, cellSize) * 0.5f;
    }

    class Node
    {
        public Vector2Int pos;
        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;
        public Node parent;
        public Node(Vector2Int pos) { this.pos = pos; }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();
        Node GetOrCreate(Vector2Int p) { if (!allNodes.ContainsKey(p)) allNodes[p] = new Node(p); return allNodes[p]; }

        var open = new SimplePriorityQueue<Node>();
        var closed = new HashSet<Vector2Int>();
        Node startNode = GetOrCreate(start);
        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, end);
        open.Enqueue(startNode, startNode.fCost);

        while (open.Count > 0)
        {
            Node current = open.Dequeue();
            if (current.pos == end)
            {
                // Reconstruct path
                List<Vector2Int> path = new List<Vector2Int>();
                Node n = current;
                while (n != null)
                {
                    path.Add(n.pos);
                    n = n.parent;
                }
                path.Reverse();
                return path;
            }
            closed.Add(current.pos);

            foreach (var (neighborPos, moveCost) in GetNeighborsWithCostNoCornerCut(current.pos))
            {
                if (closed.Contains(neighborPos)) continue;
                // Skip if not walkable (obstacle)
                if (grid.GetValue(neighborPos.x, neighborPos.y) != 0) continue;
                Node neighborNode = GetOrCreate(neighborPos);
                int tentativeG = current.gCost + moveCost;
                if (tentativeG < neighborNode.gCost || !open.Contains(neighborNode))
                {
                    neighborNode.parent = current;
                    neighborNode.gCost = tentativeG;
                    neighborNode.hCost = Heuristic(neighborPos, end);
                    if (!open.Contains(neighborNode))
                        open.Enqueue(neighborNode, neighborNode.fCost);
                    else
                        open.UpdatePriority(neighborNode, neighborNode.fCost);
                }
            }
        }
        return null;
    }
    // Prevents diagonal corner cutting
    public IEnumerable<(Vector2Int pos, int cost)> GetNeighborsWithCostNoCornerCut(Vector2Int pos)
    {
        int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
        int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };
        for (int i = 0; i < 8; i++)
        {
            int nx = pos.x + dx[i];
            int ny = pos.y + dy[i];
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                // For diagonal, check both adjacent sides are walkable
                if (i >= 4)
                {
                    int adj1x = pos.x + dx[i];
                    int adj1y = pos.y;
                    int adj2x = pos.x;
                    int adj2y = pos.y + dy[i];
                    if (grid.GetValue(adj1x, adj1y) != 0 || grid.GetValue(adj2x, adj2y) != 0)
                        continue;
                }
                int cost = (i < 4) ? 10 : 14;
                yield return (new Vector2Int(nx, ny), cost);
            }
        }
    }
// SimplePriorityQueue implementation (min-heap for A*)
public class SimplePriorityQueue<T>
{
    private List<(T item, float priority)> elements = new List<(T, float)>();
    public int Count => elements.Count;
    public void Enqueue(T item, float priority) => elements.Add((item, priority));
    public T Dequeue()
    {
        int bestIndex = 0;
        for (int i = 1; i < elements.Count; i++)
            if (elements[i].priority < elements[bestIndex].priority) bestIndex = i;
        T bestItem = elements[bestIndex].item;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
    public bool Contains(T item) => elements.Exists(e => EqualityComparer<T>.Default.Equals(e.item, item));
    public void UpdatePriority(T item, float newPriority)
    {
        for (int i = 0; i < elements.Count; i++)
            if (EqualityComparer<T>.Default.Equals(elements[i].item, item))
                elements[i] = (item, newPriority);
    }
}

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        // Octile distance for diagonal movement
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return 10 * (dx + dy) + (4 - 2 * 10) * Mathf.Min(dx, dy); // 10 for straight, 14 for diagonal
    }

    // Returns both neighbor position and movement cost
    public IEnumerable<(Vector2Int pos, int cost)> GetNeighborsWithCost(Vector2Int pos)
    {
        int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
        int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };
        for (int i = 0; i < 8; i++)
        {
            int nx = pos.x + dx[i];
            int ny = pos.y + dy[i];
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                int cost = (i < 4) ? 10 : 14; // 10 for straight, 14 for diagonal
                yield return (new Vector2Int(nx, ny), cost);
            }
        }
    }
}
