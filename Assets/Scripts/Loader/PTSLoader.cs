using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CielaSpike;

public class PTSLoader 
{
	// Public elements for saving pos / rot
	public List<Vector3> positions;	
	public List<Quaternion> rotations;

	public PTSLoader ()
	{
		positions = new List<Vector3> ();
		rotations = new List<Quaternion> ();
	}

	public bool loadAsync(System.IO.FileInfo fp){
		// clear previous vectors
		positions.Clear ();
		rotations.Clear ();

		System.IO.StreamReader reader = fp.OpenText();
		string line;
		line = reader.ReadLine ();
		if (line == "") {
			// Incorrect format!
			return false;
		}

		while ((line = reader.ReadLine ()) != null) {
			string[] words = line.Split (' ');
			float x = float.Parse(words [4]);
			float y = float.Parse(words [5]);
			float z = float.Parse(words [6]);
			float a = float.Parse(words [7]);
			float b = float.Parse(words [8]);
			float c = float.Parse(words [9]);
			float d = float.Parse(words [10]);
			positions.Add (new Vector3 (x, y, z));
			rotations.Add (new Quaternion(a, b, c, d));

			int num_markers = int.Parse(words [11]);
			if (num_markers <= 0) {
				return false;
			}
		}
		reader.Close ();
		return true;
	}

}

