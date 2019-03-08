using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MNISTread : MonoBehaviour {

	public GameObject cuboPrebaf;
	public GameObject[][] cubos;
	public float[] valores;
	public float[][] valoresDeEntrenamoento;
	public float[] lablesEntrenamiento;
	public float[][] valoresDePruebas;
	public float[] lablesPruebas;
	public int numeroActual;

	void Awake(){

		lablesEntrenamiento = new float[60000]; // guarda el numero de la imagen
		valoresDeEntrenamoento = new float[60000][]; // guarda la informacion de los pixeles 28*28

		lablesPruebas = new float[10000];
		valoresDePruebas = new float[10000][];

		for(int v = 0; v < valoresDeEntrenamoento.Length; v++) // inicializa en 0 los valores de entrenmaiento
			valoresDeEntrenamoento[v] = new float[28*28];

		for(int v = 0; v < valoresDePruebas.Length; v++)
			valoresDePruebas[v] = new float[28*28];

		InicializarCanvas();
		Inicializar("train-labels-idx1-ubyte","train-images-idx3-ubyte");
		InicializarPruebas("t10k-labels-idx1-ubyte", "t10k-images-idx3-ubyte");
	}

	void Inicializar (string label_nombre, string images_nombre) {

		FileStream ifsLabels = File.Open(Application.dataPath + "/" + label_nombre, FileMode.Open);
		FileStream ifsImages = File.Open(Application.dataPath + "/" + images_nombre, FileMode.Open);

		BinaryReader brLabels = new BinaryReader(ifsLabels);
		BinaryReader brImages = new BinaryReader(ifsImages);
		
		int magic1 = brImages.ReadInt32(); // discard
        int numImages = brImages.ReadInt32(); 
        int numRows = brImages.ReadInt32(); 
        int numCols = brImages.ReadInt32(); 

        int magic2 = brLabels.ReadInt32(); 
        int numLabels = brLabels.ReadInt32(); 

		byte[][] pixels = new byte[28][];
        for (int i = 0; i < pixels.Length; ++i)
          pixels[i] = new byte[28];

		for (int di = 0; di < 60000; ++di) 
        {
        	for (int i = 0; i < 28; ++i)
        	{
            	for (int j = 0; j < 28; ++j)
            	{
            		byte b = brImages.ReadByte();
            		pixels[i][j] = b;
					valoresDeEntrenamoento[di][(28*i)+j] = pixels[i][j];
            	}
        	}

        	byte lbl = brLabels.ReadByte();
			lablesEntrenamiento[di] = lbl;
    	} // each image
	}

	void InicializarPruebas (string label_nombre, string images_nombre) {

		FileStream ifsLabels = File.Open(Application.dataPath + "/" + label_nombre, FileMode.Open);
		FileStream ifsImages = File.Open(Application.dataPath + "/" + images_nombre, FileMode.Open);

		BinaryReader brLabels = new BinaryReader(ifsLabels);
		BinaryReader brImages = new BinaryReader(ifsImages);
		
		int magic1 = brImages.ReadInt32(); // discard
        int numImages = brImages.ReadInt32(); 
        int numRows = brImages.ReadInt32(); 
        int numCols = brImages.ReadInt32(); 

        int magic2 = brLabels.ReadInt32(); 
        int numLabels = brLabels.ReadInt32(); 

		byte[][] pixels = new byte[28][];
        for (int i = 0; i < pixels.Length; ++i)
          pixels[i] = new byte[28];

		for (int di = 0; di < 10000; ++di) 
        {
        	for (int i = 0; i < 28; ++i)
        	{
            	for (int j = 0; j < 28; ++j)
            	{
            		byte b = brImages.ReadByte();
            		pixels[i][j] = b;
					valoresDePruebas[di][(28*i)+j] = pixels[i][j];
            	}
        	}

        	byte lbl = brLabels.ReadByte();
			lablesPruebas[di] = lbl;
			VisualizarNumero(valoresDePruebas[10000-1], 10000-1);
    	} // each image
	}

	public void VisualizarNumero(float[] info, int labelId)
	{
		for (int i = 0; i < 28; ++i)
      	{
      		for (int j = 0; j < 28; ++j)
      	 	{
				Renderer rend = cubos[i][j].GetComponent<Renderer>();
				rend.material.color = new Color32((byte)info[(28*i)+j], (byte)info[(28*i)+j], (byte)info[(28*i)+j], 1);
        	}
      	}

		numeroActual = (int) lablesPruebas[labelId];
	}

	public void Borrar()
	{
		for (int i = 0; i < 28; ++i)
      	{
      		for (int j = 0; j < 28; ++j)
      	 	{
				Renderer rend = cubos[i][j].GetComponent<Renderer>();
				rend.material.color = Color.black;
        	}
      	}
	}

	public float[] leerInfo()
	{
		float[] val = new float[28*28];
		for (int i = 0; i < 28; ++i)
      	{
      		for (int j = 0; j < 28; ++j)
      	 	{
				val[(28*i)+j] = cubos[i][j].GetComponent<Renderer>().material.color.r;
        	}
      	}
		return val;
	}

	void InicializarCanvas()
	{
		// se encarga de spawnerar todos los cubos para visualizacion
		cubos = new GameObject[28][];
		for(int c = 0; c < cubos.Length; c++)
			cubos[c] = new GameObject[28];

		GameObject parent = new GameObject("Dibujo");

		for(int columna = 0; columna < 28; columna++)
		{
			for(int renglon = 0; renglon < 28; renglon++)
			{
				Vector3 pos = new Vector3(-14+renglon, 14-columna,0);
				cubos[columna][renglon] = (GameObject) Instantiate(cuboPrebaf,pos, Quaternion.identity);
				cubos[columna][renglon].transform.name = "id_" + ((columna*28)+renglon).ToString();
				cubos[columna][renglon].transform.parent = parent.transform;
			}
		}
	}
}
