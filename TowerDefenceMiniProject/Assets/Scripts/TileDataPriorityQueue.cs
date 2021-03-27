using System.Collections.Generic;

public class TileDataPriorityQueue
{
	int count = 0;
	int minimum = int.MaxValue;

	public int Count { get { return count; } }

	List<TileData> list = new List<TileData>();

	public void Enqueue(TileData cell)
	{
		count += 1;
		int priority = cell.SearchPriority;
		if (priority < minimum)
		{
			minimum = priority;
		}

		while (priority >= list.Count)
		{
			list.Add(null);
		}
		cell.NextWithSamePriority = list[priority];
		list[priority] = cell;
	}

	public TileData Dequeue()
	{
		count -= 1;

		for (; minimum < list.Count; minimum++)
		{
			TileData cell = list[minimum];
			if (cell != null)
			{
				list[minimum] = cell.NextWithSamePriority;
				return cell;
			}
		}

		return null;
	}

	public void Change(TileData cell, int oldPriority)
	{
		TileData current = list[oldPriority];
		TileData next = current.NextWithSamePriority;

		if (current == cell)
		{
			list[oldPriority] = next;
		}
		else
		{
			while (next != cell)
			{
				current = next;
				next = current.NextWithSamePriority;
			}
			current.NextWithSamePriority = cell.NextWithSamePriority;
		}

		Enqueue(cell);
		count -= 1;
	}

	public void Clear()
	{
		list.Clear();
		count = 0;
		minimum = int.MaxValue;
	}
}
