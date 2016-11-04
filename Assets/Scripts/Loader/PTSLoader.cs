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
	List<int> flipIndices;

	public PTSLoader ()
	{
		flipIndices = new List<int>();
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
		Quaternion prevRot = new Quaternion (); 
		int index = 0;
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

			Quaternion currRot = new Quaternion (a, b, c, d);
			if (index > 0) {
				float angle1 = Quaternion.Angle (currRot, prevRot);
				Quaternion flipped = new Quaternion (0, 1.0f, 0, 0);
				flipped = prevRot*flipped;
				float angle2 = Quaternion.Angle (currRot, flipped);

//				Quaternion prevRotInv = Quaternion.Inverse (prevRot);
//				float angle2 = Quaternion.Angle (currRot, prevRotInv);
				// is fliped
				if (Mathf.Abs(angle1) > Mathf.Abs(angle2)){
					// if last one was not flipped
					if (flipIndices.Count == 0 || 
						flipIndices [flipIndices.Count - 1] != index - 1) {
						flipIndices.Add (index);		
					}
				} else {
					// if last one was flipped
					if (flipIndices.Count > 1 &&
						flipIndices [flipIndices.Count - 1] == index - 1) {
						flipIndices.Add (index);		
					}
				}
			}
			prevRot = currRot;
			rotations.Add (currRot);
			index++;
		}
		reader.Close ();

		if (flipIndices.Count < rotations.Count / 2) {
			foreach (int i  in flipIndices) {
				Quaternion flipped =new Quaternion (0, 1.0f, 0, 0);
				rotations [i] = rotations[i] * flipped;
			}
		} else {
			bool[] flips = new bool[rotations.Count];
			for (int i = 0; i < rotations.Count; i++) {
				flips [i] = true;
			}
			foreach (int i  in flipIndices) {
				flips [i] = false;
			}
			for (int i = 0; i < rotations.Count; i++) {
				if (flips [i]) {
					Quaternion flipped = new Quaternion (0, 1.0f, 0, 0);
					rotations [i] = rotations[i] * flipped;
				}
			}
		}
		return true;
	}

}

