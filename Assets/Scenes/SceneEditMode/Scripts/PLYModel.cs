using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


//
public class property{
	public string name;
}

public class property<T> :property{
	public List<T> data = new List<T>();
}

public class element{
	public element(){
		properties = new List<property>();
	}

	public string name;
	public int count;
	public List<property> properties;
}

public class PLYModel
{

	public List<element> elements;
	public List<Mesh> meshes;
	public bool isPointcloud = false;
	public PLYModel ()
	{
		elements = new List<element>();
		meshes = new List<Mesh> ();
	}
	// loads file from local path and stores it in elements array
	public bool loadFile(System.IO.FileInfo filePath){
		System.IO.StreamReader reader = filePath.OpenText();

		string line;
		element currElement = null;
		property currProp = null;

		line = reader.ReadLine ();
		if (line != "ply") {
			// Incorrect format!
			return false;
		}

		// read headers
		while((line = reader.ReadLine()) != null)
		{
			if (line == "end_header") {
				break;
			}
			string[] words = line.Split (' ');
			// parse header lines
			switch (words [0]) {
			case "element":
				// if element, increment element counter
				currElement = new element();
				elements.Add(currElement);
				currElement.name = words [1];
				currElement.count = int.Parse(words [2]);
				break;
			case "property":
				switch (words [1]) {

				case "char":
					currProp = new property<char> ();
					currProp.name = words [2];
					break;
				case "uchar":
					currProp = new property<byte> ();
					currProp.name = words [2];
					break;
				case "short":
					currProp = new property<short> ();
					currProp.name = words [2];
					break;
				case "ushort":
					currProp = new property<ushort> ();
					currProp.name = words [2];
					break;
				case "int":
					currProp = new property<int> ();
					currProp.name = words [2];
					break;
				case "uint":
					currProp = new property<uint> ();
					currProp.name = words [2];
					break;
				case "float":
					currProp = new property<float> ();
					currProp.name = words [2];
					break;
				case "double":
					currProp = new property<double> ();
					currProp.name = words [2];
					break;
				case "list":
					int listType;
					if (words.Length == 4) {
						listType = 2;
					} else {
						listType = 3;
					}
					switch (words [listType]) {
					case "char":
						currProp = new property<List<char>> ();
						currProp.name = words [listType+1];
						break;
					case "uchar":
						currProp = new property<List<byte>> ();
						currProp.name = words [listType+1];
						break;
					case "short":
						currProp = new property<List<short>> ();
						currProp.name = words [listType+1];
						break;
					case "ushort":
						currProp = new property<List<ushort>> ();
						currProp.name = words [listType+1];
						break;
					case "int":
						currProp = new property<List<int>> ();
						currProp.name = words [listType+1];
						break;
					case "uint":
						currProp = new property<List<uint>> ();
						currProp.name = words [listType+1];
						break;
					case "float":
						currProp = new property<List<float>> ();
						currProp.name = words [listType+1];
						break;
					case "double":
						currProp = new property<List<double>> ();
						currProp.name = words [listType+1];
						break;
					default:
						break;
					}
					break;
				default:
					break;
				}
				if (currElement != null) {
					currElement.properties.Add (currProp);
				} else {
					return false;
				}
				break;
			default:
				break;
			}
		}



		// start parsing data and store them in property array per element.
		for (int e_i = 0; e_i < elements.Count; e_i++) {
			element e = elements [e_i];
			for (int e_n = 0; e_n < e.count; e_n++) {
				line = reader.ReadLine ();	
				if (line == null) {
					return false;
				}
				string[] words = line.Split (' ');
				for (int p_i = 0; p_i < e.properties.Count; p_i++) {
					Type t = e.properties [p_i].GetType ();
					property p = e.properties [p_i];
					if(t == typeof(property<char>)){
						((property<char>)p).data.Add(char.Parse(words[p_i]));
					}
					else if(t == typeof(property<byte>)){
						((property<byte>)p).data.Add(byte.Parse(words[p_i]));
					}
					else if(t == typeof(property<short>)){
						((property<short>)p).data.Add(short.Parse(words[p_i]));
					}
					else if(t == typeof(property<ushort>)){
						((property<ushort>)p).data.Add(ushort.Parse(words[p_i]));
					}
					else if(t == typeof(property<int>)){
						((property<int>)p).data.Add(int.Parse(words[p_i]));
					}
					else if(t == typeof(property<uint>)){
						((property<uint>)p).data.Add(uint.Parse(words[p_i]));
					}
					else if(t == typeof(property<float>)){
						((property<float>)p).data.Add(float.Parse(words[p_i]));
					}
					else if(t == typeof(property<double>)){
						((property<double>)p).data.Add(double.Parse(words[p_i]));
					}


					else if(t ==  typeof(property<List<char>>)){
						List<char> l = new List<char>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(char.Parse(words[i]));
						}
						((property<List<char>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<byte>>)){
						List<byte> l = new List<byte>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(byte.Parse(words[i]));
						}
						((property<List<byte>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<short>>)){
						List<short> l = new List<short>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(short.Parse(words[i]));
						}
						((property<List<short>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<ushort>>)){
						List<ushort> l = new List<ushort>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(ushort.Parse(words[i]));
						}
						((property<List<ushort>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<int>>)){
						List<int> l = new List<int>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(int.Parse(words[i]));
						}
						((property<List<int>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<uint>>)){
						List<uint> l = new List<uint>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(uint.Parse(words[i]));
						}
						((property<List<uint>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<float>>)){
						List<float> l = new List<float>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(float.Parse(words[i]));
						}
						((property<List<float>>)p).data.Add(l);
						break;
					}
					else if(t ==  typeof(property<List<double>>)){
						List<double> l = new List<double>();
						int numList = int.Parse(words[0]);
						for(int i = 1; i < numList; i++){
							l.Add(double.Parse(words[i]));
						}
						((property<List<double>>)p).data.Add(l);
						break;
					}
				}

			}
		}
		reader.Close();

		//create mesh from elements


		return true;
	}



	// parse elements and return vectors
	public bool createMesh(){
		element vertex = elements.Find(item=>item.name == "vertex");
		element face = elements.Find(item=>item.name == "face");

		if (vertex == null) {
			return false;
		}

		if (face  == null || face.count == 0) {
			isPointcloud = true;
		} 
		if (isPointcloud) {
			return createPointCloud ();
		} else {
			return createObject ();
		}
	}

	public bool createPointCloud(){
		element vertex = elements.Find(item=>item.name == "vertex");

		int max_point = 65000;

		// parse vertex
		property<float> x = (property<float>)(vertex.properties.Find (p=>p.name == "x"));
		property<float> y = (property<float>)vertex.properties.Find (p=>p.name == "y");
		property<float> z = (property<float>)vertex.properties.Find (p=>p.name == "z");

		property<float> nx = (property<float>)vertex.properties.Find (p=>p.name == "nx");
		property<float> ny = (property<float>)vertex.properties.Find (p=>p.name == "ny");
		property<float> nz = (property<float>)vertex.properties.Find (p=>p.name == "nz");

		property<byte> r = (property<byte>)vertex.properties.Find (p=>p.name == "red");
		property<byte> g = (property<byte>)vertex.properties.Find (p=>p.name == "green");
		property<byte> b = (property<byte>)vertex.properties.Find (p=>p.name == "blue");
		property<byte> a = (property<byte>)vertex.properties.Find (p=>p.name == "alpha");

		if (!(x!=null && y!=null && z!=null)) {
			return false;
		}
		bool hasNormal = false;
		bool hasColor = false;
		bool hasAlpha = false;
		if (nx != null && nx.data.Count == x.data.Count) {
			hasNormal = true;
		}
		if (r != null && r.data.Count == x.data.Count) {
			hasColor = true;
		}
		if (a != null) {
			hasAlpha = true;
		}

		int numVertex = x.data.Count;
		int offset = 0;
		while (numVertex > max_point) {
			Mesh mesh_r = new Mesh ();
			List<Vector3> vertices_r = new List<Vector3>();		
			List<Vector3> normals_r = new List<Vector3>();		
			List<Color> colors_r = new List<Color>();		
			int[] indices_r = new int[max_point];
			for (int n = 0; n < max_point; n++) {
				indices_r [n] = n;
				vertices_r.Add (new Vector3 (x.data [n+offset], y.data [n+offset], z.data [n+offset]));
				if (hasNormal) {
					normals_r.Add (new Vector3 (nx.data [n+offset], ny.data [n+offset], nz.data [n+offset]));
				}
				if (hasColor) {
					if (hasAlpha) {
						colors_r.Add (new Color (r.data [n+offset]/255, g.data [n+offset]/255, b.data [n+offset]/255,a.data[n+offset]/255));
					} else {
						colors_r.Add (new Color (r.data [n+offset]/255, g.data [n+offset]/255, b.data [n+offset]/255,1.0f));
					}
				}
			}
			mesh_r.vertices = vertices_r.ToArray ();
			if (hasNormal) {
				mesh_r.normals = normals_r.ToArray ();
			}
			if (hasColor) {
				mesh_r.colors = colors_r.ToArray ();
			}
			mesh_r.SetIndices (indices_r, MeshTopology.Points, 0);
			meshes.Add (mesh_r);
			numVertex -= max_point;
			offset += max_point;
		}

		List<Vector3> vertices = new List<Vector3>();		
		List<Vector3> normals = new List<Vector3>();		
		List<Color> colors = new List<Color>();		

		int[] indices = new int[numVertex];
		for (int n = 0; n < numVertex; n++) {
			indices [n] = n;
			vertices.Add (new Vector3 (x.data [n+offset], y.data [n+offset], z.data [n+offset]));
			if (hasNormal) {
				normals.Add (new Vector3 (nx.data [n+offset], ny.data [n+offset], nz.data [n+offset]));
			}
			if (hasColor) {
				if (hasAlpha) {
					colors.Add (new Color (r.data [n+offset]/255, g.data [n+offset]/255, b.data [n+offset]/255,a.data[n+offset]/255));
				} else {
					colors.Add (new Color (r.data [n+offset]/255, g.data [n+offset]/255, b.data [n+offset]/255,1.0f));
				}
			}
		}

		Mesh mesh = new Mesh ();
		mesh.vertices = vertices.ToArray ();
		if (hasNormal) {
			mesh.normals = normals.ToArray ();
		}
		if (hasColor) {
			mesh.colors = colors.ToArray ();
		}
		mesh.SetIndices (indices, MeshTopology.Points, 0);
		meshes.Add (mesh);

		return true;
	}


	// create 3d object instead
	public bool createObject(){

		element vertex = elements.Find(item=>item.name == "vertex");
		element face = elements.Find(item=>item.name == "face");

		List<Vector3> vertices = new List<Vector3>();		
		List<Vector3> normals = new List<Vector3>();		
		List<Color> colors = new List<Color>();		
		List<int> triangles = new List<int> ();

		// parse vertex
		property<float> x = (property<float>)(vertex.properties.Find (p=>p.name == "x"));
		property<float> y = (property<float>)vertex.properties.Find (p=>p.name == "y");
		property<float> z = (property<float>)vertex.properties.Find (p=>p.name == "z");

		if (!(x!=null && y!=null && z!=null)) {
			return false;
		}
		int numVertex = x.data.Count;
		// currently object should not have more than 65k points
		if (numVertex > 65535) {
			return false;
		}
		for (int n = 0; n < numVertex; n++) {
			vertices.Add (new Vector3 (x.data [n], y.data [n], z.data [n]));
		}

		property<float> nx = (property<float>)vertex.properties.Find (p=>p.name == "nx");
		property<float> ny = (property<float>)vertex.properties.Find (p=>p.name == "ny");
		property<float> nz = (property<float>)vertex.properties.Find (p=>p.name == "nz");

		int numNormals = x.data.Count;
		for (int n = 0; n < numNormals; n++) {
			normals.Add (new Vector3 (nx.data [n], ny.data [n], nz.data [n]));
		}

		property<byte> r = (property<byte>)vertex.properties.Find (p=>p.name == "red");
		property<byte> g = (property<byte>)vertex.properties.Find (p=>p.name == "green");
		property<byte> b = (property<byte>)vertex.properties.Find (p=>p.name == "blue");
		property<byte> a = (property<byte>)vertex.properties.Find (p=>p.name == "alpha");

		if (a != null) {
			int numColors = r.data.Count;
			for (int n = 0; n < numColors; n++) {
				colors.Add (new Color (r.data [n]/255, g.data [n]/255, b.data [n]/255));
			}
		} else {
			int numColors = r.data.Count;
			for (int n = 0; n < numColors; n++) {
				colors.Add (new Color (r.data [n]/255, g.data [n]/255, b.data [n]/255,a.data[n]/255));
			}
		}

		// not implemented yet!
		property<int> material_index = (property<int>)vertex.properties.Find (p=>p.name == "material_index");


		// parse face if exists

		if (!isPointcloud) {
			property<List<int>> vertex_indices = (property<List<int>>)face.properties.Find (p => p.name == "vertex_indices");
			int numFaces = vertex_indices.data.Count;
			for (int n = 0; n < numFaces; n++) {
				List<int> il = vertex_indices.data [n];
				if (il.Count == 3) {
					triangles.Add (il [0]);
					triangles.Add (il [1]);
					triangles.Add (il [2]);
				} else if (il.Count == 4) {
					triangles.Add (il [0]);
					triangles.Add (il [1]);
					triangles.Add (il [2]);
					triangles.Add (il [2]);
					triangles.Add (il [3]);
					triangles.Add (il [4]);
				} else {
					return false;
				}
			}
		}
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray ();
		mesh.colors = colors.ToArray ();
		mesh.triangles = triangles.ToArray ();
		meshes.Add (mesh);
		return true;
	}
}
