using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PseudoRandom  
{
	private static int[] randListNormal = new int[]{0,1,2,3,4,5};

	/*
	 * 
	 * ,1,1,0,1,1,2,1,0,1,1,1,1,0,2,1,1,0,
		1,1,3,1,1,2,1,3,1,2,4,1,1,2,1,2,1,1,2,1,1,0,
		1,1,1,2,1,2,0,1,2,0,1,2,3,4,1,1,2,1,1,2,1,2
*/

	private static int[] randListExtreme = new int[]{
		1,21,21,17,10,1,1,0,1,1,209,1,0,1,11,1,1,0,2,1,1,0,
		1,1,3,1,1,21,1,3,1,2,4,1,1,2,1,22,1,1,2,1,1,0,
		1,1,1,2,71,2,2,1,2,0,1,2,3,34,1,1,2,1,1,2,1,2};

	private static int[] randListMoreExtreme = new int[]{
		1, 21, 209, 2342, 234, 23, 1, 1, 1, 1, 2, 2, 3, 4, 5, 1231,
		1, 2, 3, 4, 90, 99, 5, 3, 3, 4, 5, 6, 7, 988, 899, 898, 8, 9, -213};

	public static int index;

	/// <summary>
	/// Returns a random number in the range of 0-4; weighted toward 1
	/// </summary>
	/// <returns>The random normal.</returns>
	public static int getRandomNormal()
	{


		index++;
		return randListNormal [index % randListNormal.Length];
	}

	/// <summary>
	/// Returns a random number in the range of 0 to 209; weighted toward 1
	/// </summary>
	/// <returns>The random normal.</returns>

	public static int getRandomExtreme()
	{
		index++;
		return randListExtreme [index % randListExtreme.Length];
	}

	/// <summary>
	/// Returns a random number in the range of -213 to 988; weighted heavily toward 1
	/// </summary>
	/// <returns>The random normal.</returns>
	public static int getRandomMoreExtreme()
	{
		index++;
		return randListExtreme [index % randListMoreExtreme.Length];
	}
}
