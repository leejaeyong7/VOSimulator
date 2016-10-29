using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CielaSpike;
public class TransformationLoader
{
	public double scale = 1;	
	public Vector3 translation;
	public Quaternion rotation;


	public TransformationLoader ()
	{
		translation = new Vector3 ();
		rotation = new Quaternion ();
		scale = 1;
	}

	public bool loadAsync(System.IO.FileInfo fp){
		translation = new Vector3 ();
		rotation = new Quaternion ();

		System.IO.StreamReader reader = fp.OpenText();
		string line;
		line = reader.ReadLine ();
		string[] words = line.Split (' ');
		scale = double.Parse (words [0]);

		line = reader.ReadLine ();
		words = line.Split (' ');

		double r11 = double.Parse (words [0]);
		double r12 = double.Parse (words [1]);
		double r13 = double.Parse (words [2]);

		line = reader.ReadLine ();
		words = line.Split (' ');
		double r21 = double.Parse (words [0]);
		double r22 = double.Parse (words [1]);
		double r23 = double.Parse (words [2]);

		line = reader.ReadLine ();
		words = line.Split (' ');
		double r31 = double.Parse (words [0]);
		double r32 = double.Parse (words [1]);
		double r33 = double.Parse (words [2]);

		line = reader.ReadLine ();
		words = line.Split (' ');
		double t1 = double.Parse (words [0]);
		double t2 = double.Parse (words [1]);
		double t3 = double.Parse (words [2]);


		rotation = getQuaternion(
			(float)r11,(float)r12,(float)r13,
			(float)r21,(float)r22,(float)r23,
			(float)r31,(float)r32,(float)r33);

		translation = new Vector3(
			(float) t1,(float) t2,(float) t3);
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

