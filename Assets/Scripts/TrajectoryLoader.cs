using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using CielaSpike;
public class TrajectoryLoader 
{ 
	public List<Vector3> true_positions;	
	public List<Quaternion> true_rotations;
	public List<Vector3> estimated_positions;	
	public List<Quaternion> estimated_rotations;

	public double alignScale;
	public Quaternion alignRotation;
	public Vector3 alignTranslation;


	public TrajectoryLoader(){
		true_positions = new List<Vector3> ();
		true_rotations = new List<Quaternion> ();
		estimated_positions = new List<Vector3> ();
		estimated_rotations = new List<Quaternion> ();
	}


	public IEnumerator loadFile(System.IO.FileInfo filePath, bool isTrue){
		if (isTrue) {
			true_positions.Clear ();
			true_rotations.Clear ();
			if (true_TrajectoryLoader (filePath)) {
				yield break;
			} else {
				throw new Exception ("Parse Failed!");
			}
		} else {
			estimated_positions.Clear ();
			estimated_rotations.Clear ();
			UnityEngine.Debug.Log ("Camera.out parse start!");
			if (estimated_TrajectoryLoader (filePath)) {
				UnityEngine.Debug.Log ("Camera.out parse end");
				if (true_positions.Count > 0) {
					UnityEngine.Debug.Log ("Alignment start!");
					align ();	
					UnityEngine.Debug.Log ("Alignment end!");
				}
				yield break;
			} else {
				throw new Exception ("Parse Failed!");
			}
		}
	}
	public bool loadFileSync(System.IO.FileInfo filePath){
		estimated_positions.Clear ();
		estimated_rotations.Clear ();
		UnityEngine.Debug.Log ("Camera.out parse start!");
		if (estimated_TrajectoryLoader (filePath)) {
			UnityEngine.Debug.Log ("Camera.out parse end");
			if (true_positions.Count > 0) {
				UnityEngine.Debug.Log ("Alignment start!");
				align ();	
				UnityEngine.Debug.Log ("Alignment end!");
			}
			return true;
		}
		return false;
	}

	bool true_TrajectoryLoader(System.IO.FileInfo fp){
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
			true_positions.Add (new Vector3 (x, y, z));
			true_rotations.Add (new Quaternion(a, b, c, d));

			int num_markers = int.Parse(words [11]);
			if (num_markers <= 0) {
				return false;
			}
			for (int i = 0; i < num_markers; i++) {
				reader.ReadLine ();
			}
		}
		reader.Close ();
		return true;
	}


	bool estimated_TrajectoryLoader(System.IO.FileInfo fp){
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

			estimated_positions.Add (new Vector3 (
				-1 * (r11 * t1 + r21 * t2 + r31 * t3),
				-1 * (r12 * t1 + r22 * t2 + r32 * t3),
				-1 * (r13 * t1 + r23 * t2 + r33 * t3)));
			estimated_rotations.Add(getQuaternion(r11,r12,r13,r21,r22,r23,r31,r32,r33));
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



	void align(){
		// define dimensions
		int N = true_positions.Count; // s
		int interpolateNum = 1;
		int M = estimated_positions.Count*interpolateNum; // t
		int D = 3;

		// setup initialization constants
		double scale = 1;
		Matrix T = Matrix.Create (Matrix.CreateMatrixData (M, D));
		Matrix S = Matrix.Create (Matrix.CreateMatrixData (N, D));
//		Matrix R = Matrix.Identity (D, D);
		Matrix R = Matrix.Create (Matrix.CreateMatrixData (D, D));
		R [0, 2] = 1;
		R [1, 1] = -1;
		R [2, 0] = 1;
		Vector tr = Vector.Zeros (D);

		System.Random rand = new System.Random();
		double w = rand.NextDouble ();

		tr [0] = 0;
		tr [1] = 0;
		tr [2] = 0;

		for(int i = 0; i < M; i++){
			T [i, 0] = estimated_positions[i].x;
			T [i, 1] = estimated_positions[i].y;
			T [i, 2] = estimated_positions[i].z;
		}

		for (int j = 0; j < N; j++) {
			S [j, 0] = true_positions[j].x;
			S [j, 1] = true_positions[j].y;
			S [j, 2] = true_positions[j].z;
		}


		double std = (1.0 / (D * N * M));
		double sum = 0;
		for (int i = 0; i < M; i++) {
			for (int j = 0; j < N; j++) {
				sum += (S.GetRowVector (j) - T.GetRowVector (i)).SquaredNorm();
			}
		}
		std *= sum;


		// loop EM algorithm
		// while register
		int count = 0;

		while (std > 0.001  && count < 50) {
			// e etep
			UnityEngine.Debug.Log ("E step at iteration: " + count.ToString()); 

			Matrix P = Matrix.Create (Matrix.CreateMatrixData (M, N));
			for (int j = 0; j < N; j++) {
				double sum_k = 0;
				for (int k = 0; k < M; k++) {
					sum_k += Math.Exp ((-1.0 / (2 * std)) * ((S.GetRowVector (j) - transf (T.GetRowVector (k), scale, R, tr)).SquaredNorm ()));
				}
				sum_k +=  Math.Pow((2*Math.PI * std),((double)D)/2.0) * (w / (1 - w)) * ((double)M / (double)N);
				for (int i = 0; i < M; i++) {
					P[i,j] = Math.Exp ((-1.0 / (2*std))*((S.GetRowVector(j) - transf(T.GetRowVector(i),scale,R,tr)).SquaredNorm()))/sum_k;
				}
			}

			// M step:
			UnityEngine.Debug.Log ("M step at iteration: " + count.ToString());

			double Np = 0;
			for (int i = 0; i < M; i++) {
				for (int j = 0; j < N; j++) {
					Np += P [i, j];
				}
			}
			Matrix S_trans = S.Clone ();
			S_trans.Transpose ();
			Matrix P_trans = P.Clone ();
			P_trans.Transpose ();
			Matrix T_trans = T.Clone ();
			T_trans.Transpose ();
			Vector mu_s = ((1.0 / Np) * S_trans * P_trans * Vector.Ones(M).ToColumnMatrix()).GetColumnVector(0);
			Vector mu_t = ((1.0 / Np) * T_trans * P * Vector.Ones(N).ToColumnMatrix()).GetColumnVector(0);
			Matrix s_hat = S - (Vector.Ones(N).ToColumnMatrix() * mu_s.ToRowMatrix());
			Matrix t_hat = T - (Vector.Ones(M).ToColumnMatrix() * mu_t.ToRowMatrix());

			Matrix t_hat_trans = t_hat.Clone ();
			t_hat_trans.Transpose ();

			Matrix s_hat_trans = s_hat.Clone ();
			s_hat_trans.Transpose ();
			Matrix A = s_hat_trans * P_trans * t_hat;
			Matrix A_trans = A.Clone ();
			A_trans.Transpose ();
			SingularValueDecomposition f = A.SingularValueDecomposition;
			Matrix U = f.LeftSingularVectors;
			Matrix Vt = f.RightSingularVectors;
			Vt.Transpose ();

			double duv = (U * Vt).Determinant ();
			Matrix C = Matrix.Identity (D, D);
			C[D-1,D-1] = duv;


			Matrix diagP1 = Matrix.Identity(M,M);
			Matrix diagPT1 = Matrix.Identity(N,N);

			for (int t = 0; t < M; t++) {
				diagP1 [t, t] = P.GetRowVector (t).Sum ();
			}
			for (int t = 0; t < N; t++) {
				diagPT1 [t, t] = P.GetColumnVector(t).Sum ();
			}


			R = U*C*Vt;
			scale = ((A_trans * R).Trace ()) / 
				    ((t_hat_trans*diagP1*t_hat).Trace());

			tr = (mu_s - (scale * (R * mu_t.ToColumnMatrix())).GetColumnVector(0));
			std = (1.0 / (Np * D)) * ((((s_hat_trans * diagPT1 * s_hat)).Trace()) - scale * (A_trans * R).Trace());
			Debug.Log("std: " + std.ToString());
			count++;
		}
		Matrix R_trans = R.Clone();

		R_trans.Transpose ();

		T = scale * T * R_trans + Vector.Ones (M).ToColumnMatrix () * tr.ToRowMatrix ();

		alignScale = scale;

		alignRotation = getQuaternion (
			(float)R [0, 0], (float)R [0, 1], (float)R [0, 2],
			(float)R [1, 0], (float)R [1, 1], (float)R [1, 2],
			(float)R [2, 0], (float)R [2, 1], (float)R [2, 2]);

		alignTranslation = new Vector3 ((float)tr[0],(float)tr[1],(float)tr[2]);

		string output = "";
		output += scale.ToString () + '\n';

		output += R[0,0].ToString() + R[0,1].ToString() + R[0,2].ToString() + '\n'; 
		output += R[1,0].ToString() + R[1,1].ToString() + R[1,2].ToString() + '\n'; 
		output += R[2,0].ToString() + R[2,1].ToString() + R[2,2].ToString() + '\n'; 
		output += tr[0].ToString() + tr[1].ToString() + tr[2].ToString() + '\n'; 

		System.IO.File.WriteAllText ( "./transform.txt", output );


		estimated_positions.Clear ();
		for (int i = 0; i < M; i++) {
			estimated_positions.Add(new Vector3 ((float)T[i,0], (float)T[i,1], (float)T[i,2]));
		}
	}



	Vector transf(Vector v, double s, Matrix r, Vector t){
		return (s * r * v.ToColumnMatrix() + t.ToColumnMatrix()).GetColumnVector(0);
	}


}

