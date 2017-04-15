//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Microsoft;
//public class DungeonGenerator : MonoBehaviour {
//	public static double NextGaussianDouble()
//	//INEFFICIENT WITH SO MANY DOUBLES BUT CBA TO MAKE IT NICE AND EFFICIENT
//	//Value between -1 and 1 (see https://en.wikipedia.org/wiki/Marsaglia_polar_method)
//	{
//		double U, u, v, S;
//
//		do
//		{
//			u = 2.0 * Random.value - 1.0;
//			v = 2.0 * Random.value - 1.0;
//			S = u * u + v * v;
//		}
//		while (S >= 1.0);
//
//		double fac = Mathf.Sqrt(-2.0 * Mathf.Log(S) / S);
//		return u * fac;
//	}
//	//Constants
//	int TileSize = 4;
//	int NumCells = 150;
//	Dictionary<int, List<double>> Rooms = new Dictionary<int, List<double>>();
//	Dictionary<int, List<double>> RoomCoord = new Dictionary<int, List<double>> ();
//	//Working out a specific room's dimensions
//	public List<double> RoomSize ()
//	{
//		List<double> RoomDim = new List<double>();
//		RoomDim.Add (NextGaussianDouble () + 10);
//		RoomDim.Add (NextGaussianDouble () + 10);
//		return RoomDim;
//	}	
//	public double roundm (double x, double y)
//	//rounds x/y coord "size" to that of a whole number to have a tile attributed to it
//	{
//		double n = (Mathf.Floor ((x + y - 1) / y) * y);
//		return n;
//	}
//	public List<double> RandomPCircle(double radius)
//	//Random X and Y coord of a point in the circle radius defaultly set to 1 normally but edited to be a constant. Probs doesnt work then
//	{
//		Random.Range (0, radius);
//		double t = (Mathf.PI * Random.Range (0, radius));
//		double u = (Random.Range (0, radius) + Random.Range (0, radius));
//		double r = 0;
//		if (u > radius) {
//			r = 2 * radius - u;
//		} else {
//			r = u;
//		}
//		List<double> coord = new List<double> ();
//		coord.Add (roundm((r * radius * Mathf.Cos (t)),TileSize));
//		coord.Add (roundm((r * radius * Mathf.Sin (t)),TileSize));
//		return coord;
//	}
//	public void Generation ()
//	//generating all the coords for the squares IDK WHAT TO DO THOUGH
//	{
//
//		for (int i = 0; i < NumCells; i = i+1)
//		{
//			Rooms.Add(i,RoomSize());
//			RoomCoord.Add(i,RandomPCircle(1));
//		}
//	}
//
//}
