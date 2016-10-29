using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CielaSpike;

public class OUTLoader 
{
	// Public elements for saving pos / rot
	public List<Vector3> positions;	
	public List<Quaternion> rotations;


	public OUTLoader ()
	{
		positions = new List<Vector3> ();
		rotations = new List<Quaternion> ();
	}

	public bool loadAsync(System.IO.FileInfo fp){
		// clear previous vectors
		positions.Clear();
		rotations.Clear ();

		System.IO.StreamReader reader = fp.OpenText();
		string line;
		line = reader.ReadLine ();
		if (line == "") {
			// Incorrect format!
			return false;
		}
		line = reader.ReadLine ();
		string[] words = line.Split (' ');
		int numCam = int.Parse(words [0]);
		for (int i = 0; i < numCam; i++) {
			// first line = fl kx ky
			line = reader.ReadLine ();
			if (line == null) {
				return false;
			}
			words = line.Split (' ');

			// r11 r12 r13
			line = reader.ReadLine ();
			if (line == null) {
				return false;
			}
			words = line.Split (' ');
			float r11 = (float)decimal.Parse(words[0],System.Globalization.NumberStyles.Float);
			float r12 = (float)decimal.Parse(words[1],System.Globalization.NumberStyles.Float);
			float r13 = (float)decimal.Parse(words[2],System.Globalization.NumberStyles.Float);


			// r21 r22 r23
			line = reader.ReadLine ();
			if (line == null) {
				return false;
			}
			words = line.Split (' ');
			float r21 = (float)decimal.Parse(words[0],System.Globalization.NumberStyles.Float);
			float r22 = (float)decimal.Parse(words[1],System.Globalization.NumberStyles.Float);
			float r23 = (float)decimal.Parse(words[2],System.Globalization.NumberStyles.Float);

			// r31 r32 r33
			line = reader.ReadLine ();
			if (line == null) {
				return false;
			}
			words = line.Split (' ');
			float r31 = (float)decimal.Parse(words[0],System.Globalization.NumberStyles.Float);
			float r32 = (float)decimal.Parse(words[1],System.Globalization.NumberStyles.Float);
			float r33 = (float)decimal.Parse(words[2],System.Globalization.NumberStyles.Float);

			// t1 t2 t3
			line = reader.ReadLine ();
			if (line == null) {
				return false;
			}
			words = line.Split (' ');
			float t1 = (float)decimal.Parse(words[0],System.Globalization.NumberStyles.Float);
			float t2 = (float)decimal.Parse(words[1],System.Globalization.NumberStyles.Float);
			float t3 = (float)decimal.Parse(words[2],System.Globalization.NumberStyles.Float);

			positions.Add (new Vector3 (
				-1 * (r11 * t1 + r21 * t2 + r31 * t3),
				-1 * (r12 * t1 + r22 * t2 + r32 * t3),
				-1 * (r13 * t1 + r23 * t2 + r33 * t3)));
			rotations.Add(getQuaternion(r11,r12,r13,r21,r22,r23,r31,r32,r33));
		}
		reader.Close ();
		return true;
	}

	Quaternion getQuaternion(
		float r11, float r12, float r13,
		float r21, float r22, float r23,
		float r31, float r32, float r33){
		float w = Mathf.Sqrt (1 + r11 + r22 + r33);
		float x = (r32 - r23) / (4 * w);
		float y = (r13 - r31) / (4 * w);
		float z = (r21 - r12) / (4 * w);
		return new Quaternion (x, y, z, w);
	}
}

