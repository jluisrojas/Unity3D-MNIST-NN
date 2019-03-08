using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork {

	int[] layer; // es el numero de capas y sus neuronas
	Layer[] layers; // las objetos de las capas

	Layer[] layers_Start;
	Layer[] layers_Delta;

	public NeuralNetwork (int[] layer) // inicializa todo
	{
		/*this.layer = new int[layer.Length]; // inicaliza el numero de capas y el tamaño de cada una
		for(int i = 0; i < layer.Length; i++)
			this.layer[i] = layer[i];*/
		this.layer = layer;

		layers = new Layer[layer.Length-1];	// inicializa la neurnas pasandole el numero de nueronas y conexiones que debe de tener
		
		layers_Start = new Layer[layer.Length-1];
		layers_Delta = new Layer[layer.Length-1];

		for(int i = 0; i < layers.Length; i++)
		{
			layers[i] = new Layer(layer[i], layer[i+1]);

			layers_Start[i] = new Layer(layer[i], layer[i+1]);
			layers_Delta[i] = new Layer(layer[i], layer[i+1]);
		}
	}

	public float[] FeedForward(float[] inputs)
	{
		layers[0].FeedForward(inputs);
		for(int i = 1; i < layers.Length; i ++)
		{
			layers[i].FeedForward(layers[i-1].outputs);
		}
		return layers[layers.Length-1].outputs;
	}

	public void BackProp(float[] expected)
	{
		for(int i = layers.Length-1; i >= 0; i--)
		{
			if(i == layers.Length-1)
			{
				layers[i].BackPropOutput(expected);
			}
			else
			{
				layers[i].BackPropHidden(layers[i+1].errorS, layers[i+1].weights);
			}
		}
	}

	public void UpdateNetwork()
	{
		for(int i = 0; i < layers.Length; i++)
		{
			layers[i].UpdateWeightsBias();
		}
	}

	
	public class Layer {

		public int numberOfInputs; // numero de neuronas en la capa anterior
		public int numberOfOuputs;	// numero de neuronas en la capa actual
		public int numberOfPasses;

		public float[] outputs;
		public float[] inputs;
		public float[] biases;
		public float[] biasesDelta;
		public float[,] weights;
		public float[,] weightsDelta;
		public float[] errorS;
		public float[] error;

		public Layer(int numberOfInputs, int numberOfOuputs)
		{
			this.numberOfInputs = numberOfInputs;
			this.numberOfOuputs = numberOfOuputs;

			outputs = new float[numberOfOuputs];
			inputs = new float[numberOfInputs];
			biases = new float[numberOfOuputs];
			biasesDelta = new float[numberOfOuputs];
			weights = new float[numberOfOuputs, numberOfInputs];
			weightsDelta = new float[numberOfOuputs, numberOfInputs];
			errorS = new float[numberOfOuputs];
			error = new float[numberOfOuputs];
			numberOfPasses = 0;

			InitilizeWeightsBias();
		}

		public void InitilizeWeightsBias()
		{
			for(int i = 0; i < numberOfOuputs; i ++)
			{
				for(int j = 0; j < numberOfInputs; j ++)
				{
					weights[i,j] = UnityEngine.Random.Range(-1.0f, 1.0f);
				}
				biases[i] = UnityEngine.Random.Range(-0.5f, 0.5f);
			}
		}


		public float[] FeedForward(float[] inputs) // hace el todas las operaciones con un input que se le inserta
		{
			this.inputs = inputs;

			for(int i = 0; i < numberOfOuputs; i ++)
			{
				outputs[i] = 0;
				for(int j = 0; j < numberOfInputs; j ++)
				{
					outputs[i] += inputs[j] * weights[i,j]; //+ biases[i];
				}

				outputs[i] = sigmoid(outputs[i]);
			}

			return outputs;
		}

		public void BackPropOutput(float[] expected)
		{
			for(int i = 0; i < numberOfOuputs; i++)
				error[i] = outputs[i] - expected[i];

			for(int i = 0; i < numberOfOuputs; i++)
				errorS[i] = error[i]*sigmoidDelta(outputs[i]);

			for(int i = 0; i < numberOfOuputs; i++)
			{
				for(int j = 0; j < numberOfInputs; j++)
				{
					weightsDelta[i,j] = errorS[i]*inputs[j];
				}
				biasesDelta[i] = errorS[i];
			}

			numberOfPasses++;
		}

		public void BackPropHidden(float[] errorS_forward, float[,] weights_forward)
		{
			for(int i = 0; i < numberOfOuputs; i++)
			{
				errorS[i] = 0;
				for(int j = 0; j < errorS_forward.Length; j++)
				{
					errorS[i] += errorS_forward[j]* weights_forward[j,i];
				}

				errorS[i] *= sigmoidDelta(outputs[i]);
			}
			for(int i = 0; i < numberOfOuputs; i++)
			{
				for(int j = 0; j < numberOfInputs; j++)
				{
					weightsDelta[i,j] = errorS[i]*inputs[j];
				}
				biasesDelta[i] = errorS[i];
			}

			numberOfPasses++;
		}

		public void UpdateWeightsBias()
		{
			for(int i = 0; i < numberOfOuputs; i++)
			{
				for(int j = 0; j < numberOfInputs; j++)
				{
					weights[i,j] -= (weightsDelta[i,j]/numberOfPasses)*1.0f;
				}
				biases[i] -= (biasesDelta[i]/numberOfPasses)*1.033f;
			}

			numberOfPasses = 0;
		}

		public float sigmoid(float x) // funcion sigmoid
		{
			return (1/(1+Mathf.Exp(-x)));
			//return (float)Math.Tanh(x);
		}

		public float sigmoidDelta(float x) // derivada de la funcion sigmoid
		{
			return sigmoid(x)*(1-sigmoid(x));
			//return 1 - (x*x);
		}
	}
} 
