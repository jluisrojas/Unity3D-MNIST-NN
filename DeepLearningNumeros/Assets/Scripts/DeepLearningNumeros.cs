using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLearningNumeros : MonoBehaviour {

	public int TamañoBatch = 100;
	public float[] valores;

	private MNISTread mnist;
	NeuralNetwork net;

	void Start()
	{
		mnist = this.GetComponent<MNISTread>();
		net = new NeuralNetwork(new int[] {784, 30, 30, 10});
	}
	public void Calcular () {

		float[][] entrada;
		entrada = new float[TamañoBatch][];

		float[][] esperado;
		esperado = new float[TamañoBatch][];

		for(int i = 0; i < entrada.Length; i++)
		{
			entrada[i] = new float[28*28];
			esperado[i] = new float[10];
		}

		for(int i = 0; i < 60000/TamañoBatch; i++)// 60 000 -> 59 900
		{
			for(int j = 0; j < TamañoBatch; j++) // pasa la informacion de entrada
			{
				entrada[j] = mnist.valoresDeEntrenamoento[(i*TamañoBatch)+j];
				float[] lab = new float[10];
				lab[(int)mnist.lablesEntrenamiento[(i*TamañoBatch)+j]] = 1;
				esperado[j] = lab;
				net.FeedForward(entrada[j]);
				net.BackProp(esperado[j]);
				net.UpdateNetwork();

			}
			//net.UpdateNetwork();
		}
		Probar();
	}

	public void Probar()
	{
		int alazar = Random.Range(1, 10000);
		valores = net.FeedForward(mnist.valoresDePruebas[alazar]);
		//Debug.Log("Label: " + mnist.lablesEntrenamiento[alazar]);
		mnist.VisualizarNumero(mnist.valoresDePruebas[alazar], alazar);
		UnityEngine.Debug.Log("Probability that it is a 0: " + net.FeedForward(mnist.valoresDePruebas[alazar])[0]);
		UnityEngine.Debug.Log("Probability that it is a 1: " + net.FeedForward(mnist.valoresDePruebas[alazar])[1]);
		UnityEngine.Debug.Log("Probability that it is a 2: " + net.FeedForward(mnist.valoresDePruebas[alazar])[2]);
		UnityEngine.Debug.Log("Probability that it is a 3: " + net.FeedForward(mnist.valoresDePruebas[alazar])[3]);
		UnityEngine.Debug.Log("Probability that it is a 4: " + net.FeedForward(mnist.valoresDePruebas[alazar])[4]);
		UnityEngine.Debug.Log("Probability that it is a 5: " + net.FeedForward(mnist.valoresDePruebas[alazar])[5]);
		UnityEngine.Debug.Log("Probability that it is a 6: " + net.FeedForward(mnist.valoresDePruebas[alazar])[6]);
		UnityEngine.Debug.Log("Probability that it is a 7: " + net.FeedForward(mnist.valoresDePruebas[alazar])[7]);
		UnityEngine.Debug.Log("Probability that it is a 8: " + net.FeedForward(mnist.valoresDePruebas[alazar])[8]);
		UnityEngine.Debug.Log("Probability that it is a 9: " + net.FeedForward(mnist.valoresDePruebas[alazar])[9]);

	}

	public void leer()
	{
		valores = net.FeedForward(mnist.leerInfo());
	}
}
